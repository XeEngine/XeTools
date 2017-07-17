using TiledSharp;
using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayer : ILayer
        {
            internal ITile[,] Tiles { get; private set; }

            internal TmxLayer Layer { get; private set; }

            public string Name => Layer.Name;
            public bool Visible => Layer.Visible;
            public int Width { get; private set; }
            public int Height { get; private set; }
            public int Index { get; private set; }

            public ITile GetTile(int x, int y)
            {
                return Tiles[x, y];
            }

            internal CLayer(TilemapTiled map, TmxLayer layer)
            {
                Layer = layer;
                Width = map.Size.Width;
                Height = map.Size.Height;
                Index = GetIndexFromName(layer.Name);

                Tiles = new Tile[Width, Height];
                foreach (var tile in Layer.Tiles)
                {
                    int index = -1;
                    int gid = 0;
                    var myTile = new Tile(tile);
                    foreach (var tileset in map.Map.Tilesets)
                    {
                        if (tileset.FirstGid >= tile.Gid)
                            break;
                        index++;
                        gid = tileset.FirstGid;
                    }
                    myTile.Index = tile.Gid - gid;
                    myTile.Tileset = index;
                    Tiles[tile.X, tile.Y] = myTile;
                }
            }

            private int GetIndexFromName(string name)
            {
                if (name.Length >= 0)
                {
                    switch (name[0])
                    {
                        case 'B': return 0;
                        case 'L': return 1;
                        case 'H': return 3;
                        case 'X': return 5;
                        case 'S':
                            if (name.Length > 1)
                            {
                                switch (name[1])
                                {
                                    case 'L': return 2;
                                    case 'H': return 4;
                                }
                            }
                            return 2;
                    }
                }
                return -1;
            }
        }
    }
}
