using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Services;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private class LayerEntry
        {
            public Map TileMap { get; set; }

            public int Priority { get; set; }

            public List<LayerTilemap> Sublayers { get; set; }

            public int MapWidth { get; set; }

            public int MapHeight { get; set; }

            public ISurface Surface { get; set; }

            public Stream TilemapStream { get; set; }
        }
        private class TilesetMemory : IDisposable
        {
            [DllImport("kernel32.dll")]
            static extern void CopyMemory(IntPtr destination, IntPtr source, int length);

            [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
            static extern int memcmp(IntPtr b1, IntPtr b2, int count);

            const int MaximumTilesetWidth = 2048;
            const int MaximumTilesetHeight = 1024;
            const int BitDepth = 32 / 8;
            const int Size = MaximumTilesetWidth * MaximumTilesetHeight * BitDepth;
            const int MaximumTiles = Size;

            private int _tileWidth, _tileHeight, _stride;
            private int _currentIndex, _maxCount;
            private IntPtr Ptr;

            public IntPtr Memory { get; }
            public Dictionary<ulong, (int, IntPtr)> Tileset { get; } =
                new Dictionary<ulong, (int, IntPtr)>(MaximumTiles);

            public TilesetMemory(int tileWidth, int tileHeight)
            {
                _tileWidth = tileWidth;
                _tileHeight = tileHeight;
                _stride = _tileWidth * BitDepth;
                _currentIndex = 0;
                _maxCount = Size / (_tileWidth * _tileHeight * BitDepth);

                Memory = Marshal.AllocHGlobal(MaximumTilesetWidth *
                    MaximumTilesetHeight * BitDepth);
                Ptr = Memory;
                Check(Memory, _stride);
            }

            public unsafe int Check(IntPtr data, int stride)
            {
                for (int i = 0; i < _tileHeight; i++)
                    MemoryCopy(Ptr, _stride * i, data, i * stride, _stride);
                var hash = Security.Crc64.CalculateDigest((byte*)Ptr, 0, (uint)(_stride * _tileHeight));
                if (!Tileset.TryGetValue(hash, out var value))
                {
                    if (_currentIndex < _maxCount)
                    {
                        Tileset.Add(hash, value = (_currentIndex++, Ptr));
                        Ptr += _stride * _tileHeight;
                    }
                }
                return value.Item1;
            }

            public void Save(string fileName)
            {
                const int BPP = 32 / 8;
                var size = GetSizeFromTilesCount(_currentIndex);
                var image = new Bitmap(size.Width, size.Height);
                var bmpData = image.LockBits(new Rectangle(0, 0, size.Width, size.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var tilePerRow = size.Width / _tileWidth;
                foreach (var pair in Tileset)
                {
                    var index = pair.Value.Item1;
                    var srcData = pair.Value.Item2;

                    var ptr = bmpData.Scan0 + (index % tilePerRow) * _tileWidth * BPP +
                        (pair.Value.Item1 / tilePerRow) * _tileHeight * bmpData.Stride;
                    for (int i = 0; i < _tileHeight; i++)
                    {
                        MemoryCopy(ptr, i * bmpData.Stride, srcData, i * _stride, _stride);
                    }
                }
                image.UnlockBits(bmpData);
                image.Save(fileName);
            }

            private Size GetSizeFromTilesCount(int count)
            {
                var fff = System.Math.Sqrt(count);
                var tilesPerRow = CeilingPot(fff);
                var rows = CeilingPot((double)count / tilesPerRow);
                return new Size(tilesPerRow * _tileWidth, rows * _tileHeight);
            }
            private static int CeilingPot(double value)
            {
                int i = 1;
                while (true)
                {
                    if (i >= value)
                        return i;
                    i <<= 1;
                }
            }

            private static void MemoryCopy(IntPtr dst, int dstOff, IntPtr src, int srcOff, int length)
            {
                CopyMemory(dst + dstOff, src + srcOff, length);
            }
            private static bool MemoryCompare(IntPtr mem1, int off1, IntPtr mem2, int off2, int length)
            {
                return memcmp(mem1 + off1, mem2 + off2, length) == 0;
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(Memory);
            }
        }


        private string WriteTilemapChunk(Map tileMap, BinaryWriter w)
        {
            var dc = new DrawingContext(tileMap);
            var layers = tileMap.Layers
                .Where(x => x is LayerTilemap)
                .Select(x => (LayerTilemap)x)
                .GroupBy(x => x.Priority)
                .Where(x => x.Any(layer => layer.Visible))
                .Select(x => new LayerEntry
                {
                    TileMap = tileMap,
                    Priority = x.Key,
                    Sublayers = x.Where(layer => layer.Visible).ToList(),
                    Surface = null,
                    TilemapStream = null
                })
                .OrderBy(x => x.Priority)
                .ToList();

            // Rendering
            foreach (var layer in layers)
            {
                layer.MapWidth = layer.Sublayers.Max(x => x.Width);
                layer.MapHeight = layer.Sublayers.Max(x => x.Height);
                layer.Surface = RenderTilemapLayers(tileMap, dc, layer);
            }

            // Tileset creation
            using (var tileset = new TilesetMemory(tileMap.TileSize.Width, tileMap.TileSize.Height))
            {
                foreach (var layer in layers)
                {
                    layer.TilemapStream = new MemoryStream(4096 * 4096 / 16 / 16 * sizeof(ushort));
                    using (var writable = new BinaryWriter(layer.TilemapStream))
                    {
                        ProcessLayerAndTileset(layer, tileset, writable);
                    }
                }
                tileset.Save(_outputFileNameTilesetImage);
            }

            // Header writing
            var totalSize = layers.Sum(x => x.TilemapStream.Length);
            w.Write((byte)layers.Count);
            w.Write((byte)0);
            w.Write((byte)0);
            w.Write((byte)0);
            w.Write((uint)0);
            foreach (var layer in layers)
            {
                w.Write((short)layer.MapWidth);
                w.Write((short)layer.MapHeight);
                w.Write((uint)0);
                layer.TilemapStream.Position = 0;
                layer.TilemapStream.CopyTo(w.BaseStream);
            }
            return "TLV\x01";
        }

        /// <summary>
        /// Write a layer into the specified DrawingContext.
        /// </summary>
        /// <param name="tileMap"></param>
        /// <param name="dc"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        private static ISurface RenderTilemapLayers(Map tileMap, DrawingContext dc, LayerEntry layer)
        {
            var tileSize = tileMap.TileSize;
            ISurface surface = dc.Drawing.CreateSurface(
                layer.MapWidth * tileSize.Width, 
                layer.MapHeight * tileSize.Height, 
                PixelFormat.Undefined);
            dc.Drawing.Surface = surface;
            foreach (var sublayer in layer.Sublayers)
            {
                RenderTilemapLayer(tileMap, dc, surface, sublayer);
            }
            return surface;
        }

        /// <summary>
        /// Write a single sub-layer into the specified DrawingContext.
        /// </summary>
        /// <param name="tileMap"></param>
        /// <param name="dc"></param>
        /// <param name="surface"></param>
        /// <param name="layer"></param>
        private static void RenderTilemapLayer(Map tileMap, DrawingContext dc, ISurface surface, LayerTilemap layer)
        {
            var tileSize = tileMap.TileSize;
            int width = layer.Width;
            int height = layer.Height;
            var rect = new Rectangle
            {
                Width = tileSize.Width,
                Height = tileSize.Height
            };
            for (int y = 0; y < height; y++)
            {
                rect.Y = y * rect.Width;
                for (int x = 0; x < width; x++)
                {
                    var tile = layer.Tiles[x, y];
                    if (tile.Index > 0)
                    {
                        rect.X = x * rect.Height;
                        var imgTile = dc[tile.Index];
                        Drawing.Flip flip = Drawing.Flip.None;
                        if (tile.IsFlippedX)
                            flip |= Drawing.Flip.FlipHorizontal;
                        if (tile.IsFlippedY)
                            flip |= Drawing.Flip.FlipVertical;
                        dc.Drawing.DrawSurface(imgTile.Surface, imgTile.Rectangle, rect, flip);
                    }
                }
            }
        }

        /// <summary>
        /// Write a layer into a binary writer and add unique tiles into the tileset
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="tileSet"></param>
        /// <param name="writer"></param>
        private static void ProcessLayerAndTileset(LayerEntry layer, TilesetMemory tileSet, BinaryWriter writer)
        {
            const int BPP = 32 / 8;

            var tileSize = layer.TileMap.TileSize;
            using (var map = layer.Surface.Map())
            {
                var data = map.Data;
                unsafe
                {
                    int tileWidth = tileSize.Width;
                    int tileHeight = tileSize.Height;
                    int mapWidth = layer.MapWidth;
                    int mapHeight = layer.MapHeight;
                    var ptr = data;
                    for (int i = 0; i < mapHeight; i++)
                    {
                        for (int j = 0; j < mapWidth; j++)
                        {
                            var index = tileSet.Check(ptr + j * tileWidth * BPP, map.Stride);
                            writer.Write((ushort)index);
                        }
                        ptr += mapWidth * tileWidth * tileHeight * BPP;
                    }
                }
            }
        }
    }
}
