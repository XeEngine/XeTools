using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Tilemap
{
    public class TilemapDrawer : IDisposable
    {
        private IDrawing Drawing;
        private ITileMap Map;
        private ISurface[] Surfaces;

        public TilemapDrawer(IDrawing drawing, ITileMap map)
        {
            Drawing = drawing;
            Map = map;
            Surfaces = new ISurface[map.Tilesets.Count];
        }

        public void DrawLayerIndex(IDrawing drawing, int index)
        {
            foreach (var layer in Map.Layers.Where(x => x is ILayerTilemap).Select(x => x as ILayerTilemap))
            {
                Rectangle src = new Rectangle(0, 0, 0, 0);
                for (int j = 0; j < layer.Height; j++)
                {
                    for (int i = 0; i < layer.Width; i++)
                    {
                        var tile = layer.GetTile(i, j);
                        if (tile.Tileset < 0)
                            continue;
                        var tileset = Map.Tilesets[tile.Tileset];
                        if (tile.Index >= tileset.TilesCount)
                            continue;
                        var surface = GetSurface(tile.Tileset);
                        if (surface == null)
                            continue;
                        src.Width = tileset.TileWidth;
                        src.Height = tileset.TileHeight;
                        src.X = (tile.Index % tileset.TilesPerRow) * src.Width;
                        src.Y = (tile.Index / tileset.TilesPerRow) * src.Height;
                        Flip flip = tile.IsFlippedX ? Flip.FlipHorizontal : Flip.None;
                        if (tile.IsFlippedY)
                            flip |= Flip.FlipVertical;
                        drawing.DrawSurface(surface, src, i * src.Width, j * src.Height, flip);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var surface in Surfaces)
                surface?.Dispose();
        }

        private ISurface GetSurface(int index)
        {
            return Surfaces[index] = Surfaces[index] ??
                Drawing.CreateSurface(Map.Tilesets[index].ImagePath);
        }

    }
}
