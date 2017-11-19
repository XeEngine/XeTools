using System;
using System.Drawing;
using System.Linq;
using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Services;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private class TileEntry
        {
            public ISurface Surface { get; }
            public Rectangle Rectangle { get; }

            public TileEntry(ISurface surface, Rectangle rectangle)
            {
                Surface = surface;
                Rectangle = rectangle;
            }
        }

        private class DrawingContext : IDisposable
        {
            public ITileMap TileMap { get; }
            public IDrawing Drawing { get; } = Factory.Resolve<IDrawing>();
            private ResourceService<string, ISurface> ResourceTileset { get; }
            private ResourceService<int, TileEntry> ResourceTiles { get; }

            public DrawingContext(ITileMap tileMap)
            {
                TileMap = tileMap;
                ResourceTileset = new ResourceService<string, ISurface>(
                    OnResourceTilesetLoad, OnResourceTilesetUnload);


                ResourceTiles = new ResourceService<int, TileEntry>(
                    OnResourceTileLoad, OnResourceTileUnload);
            }

            public TileEntry this[int index] => ResourceTiles[index];

            public void Dispose()
            {
                ResourceTiles.RemoveAll();
                ResourceTileset.RemoveAll();
            }

            private bool OnResourceTilesetLoad(string fileName, out ISurface surface)
            {
                surface = Drawing.CreateSurface(fileName,
                    new System.Drawing.Color[]
                    {
                        System.Drawing.Color.FromArgb(255, 255, 0, 255)
                    });
                return surface != null;
            }

            private void OnResourceTilesetUnload(string fileName, ISurface surface)
            {
                surface?.Dispose();
            }

            private bool OnResourceTileLoad(int index, out TileEntry tileEntry)
            {
                var tileset = TileMap.Tilesets
                    .LastOrDefault(x => index >= x.StartId);
                tileEntry = null;
                if (tileset != null)
                {
                    var surface = ResourceTileset[tileset.ImagePath];
                    if (surface != null)
                    {
                        var realIndex = index - tileset.StartId;
                        var width = TileMap.TileSize.Width;
                        var height = TileMap.TileSize.Height;
                        var rectangle = new Rectangle()
                        {
                            X = (realIndex % tileset.TilesPerRow) * width,
                            Y = (realIndex / tileset.TilesPerRow) * height,
                            Width = width,
                            Height = height
                        };
                        tileEntry = new TileEntry(surface, rectangle);
                    }
                }
                return tileEntry != null;
            }

            private void OnResourceTileUnload(int index, TileEntry tileEntry)
            {

            }
        }
    }
}
