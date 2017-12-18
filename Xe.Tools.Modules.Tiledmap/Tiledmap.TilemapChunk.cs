using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Services;
using Xe.Tools.Tilemap;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private class LayerEntry
        {
            public Map TileMap { get; set; }

            public List<LayerTilemap> Sublayers { get; set; }

            public int MapWidth { get; set; }

            public int MapHeight { get; set; }

            public LayerDefinition Definition { get; set; }

            public LayerName Name { get; set; }

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
            // LayersDefinition is a strict requirement.
            if (tileMap.LayersDefinition == null)
                return null;

            var layers = tileMap.Layers
                .FlatterLayers<LayerTilemap>()
                .Where(x => x.ProcessingMode == LayerProcessingMode.Tilemap)
                .Where(x => x.Visible)
                .GroupBy(x => x.DefinitionId)
                .Join(tileMap.LayersDefinition,
                    group => group.Key,
                    definition => definition.Id,
                    (group, definition) => new
                    {
                        Group = group,
                        Definition = definition,
                    })
                .Join(TilemapSettings.LayerNames,
                    l => l.Definition.Id,
                    name => name.Id,
                    (layer, name) => new LayerEntry
                    {
                        TileMap = tileMap,
                        Sublayers = layer.Group.Where(x => x.Visible).ToList(),
                        Definition = layer.Definition,
                        Name = name,
                        Surface = null,
                        TilemapStream = null
                    })
                .Where(x => x.Definition.IsEnabled)
                .OrderBy(x => x.Name.Order)
                .ToList();

            using (var drawing = Factory.Resolve<IDrawing>())
            {
                using (var dc = new TilemapDrawer(drawing))
                {
                    dc.Map = tileMap;
                    // Rendering
                    var rect = new RectangleF(0, 0, float.MaxValue, float.MaxValue);
                    var tileSize = tileMap.TileSize;
                    foreach (var layer in layers)
                    {
                        layer.MapWidth = layer.Sublayers.Max(x => x.Width);
                        layer.MapHeight = layer.Sublayers.Max(x => x.Height);

                        ISurface surface = dc.Drawing.CreateSurface(
                             layer.MapWidth * tileSize.Width,
                             layer.MapHeight * tileSize.Height,
                             PixelFormat.Undefined, SurfaceType.Output);

                        dc.Drawing.Surface = surface;
                        foreach (var sublayer in layer.Sublayers)
                            dc.DrawLayer(sublayer, rect);
                        layer.Surface = drawing.Surface;
                    }
                }

                // Tileset creation
                using (var tileset = new TilesetMemory(tileMap.TileSize.Width, tileMap.TileSize.Height))
                {
                    foreach (var layer in layers)
                    {
                        layer.TilemapStream = new MemoryStream(4096 * 4096 / 16 / 16 * sizeof(ushort));
                        var writable = new BinaryWriter(layer.TilemapStream);
                        ProcessLayerAndTileset(layer, tileset, writable);
                        layer.Surface?.Dispose();
                    }
                    tileset.Save(_outputFileNameTilesetImage);
                }
            }

            // Header writing
            var totalSize = layers.Sum(x => x.TilemapStream.Length);
            w.Write((byte)layers.Count);
            w.Write(tileMap.BackgroundColor?.R ?? 0xFF);
            w.Write(tileMap.BackgroundColor?.G ?? 0x00);
            w.Write(tileMap.BackgroundColor?.B ?? 0xFF);
            w.Write((ushort)(tileMap.Size.Width));
            w.Write((ushort)(tileMap.Size.Height));
            foreach (var layer in layers)
            {
                w.Write((short)layer.MapWidth);
                w.Write((short)layer.MapHeight);
                w.Write((byte)layer.Name.Order);
                w.Write((byte)0); // RESERVED
                w.Write((byte)0); // RESERVED
                w.Write((byte)0); // RESERVED
                w.Write((uint)0); // RESERVED
                w.Write((uint)0); // RESERVED
                w.Write(layer.Definition.ParallaxHorizontalMultiplier);
                w.Write(layer.Definition.ParallaxVerticalMultiplier);
                w.Write(layer.Definition.ParallaxHorizontalSpeed);
                w.Write(layer.Definition.ParallaxVerticalSpeed);
                layer.TilemapStream.Position = 0;
                layer.TilemapStream.CopyTo(w.BaseStream);
            }
            return "TLV\x01";
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
