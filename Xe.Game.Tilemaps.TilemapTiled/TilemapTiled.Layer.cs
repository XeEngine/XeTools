using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayer : ILayer
        {
            internal ITile[,] Tiles { get; private set; }

            internal Tiled.Layer Layer { get; private set; }

            public string Name => Layer.Name;
            public bool Visible => Layer.Visible;
            public int Width => Layer.Width;
            public int Height => Layer.Height;
            public int Index { get; private set; }

            public ITile GetTile(int x, int y)
            {
                return Tiles[x, y];
            }

            internal CLayer(TilemapTiled map, Tiled.Layer layer)
            {
                Layer = layer;
                Index = GetIndexFromName(layer.Name);

                Tiles = new Tile[Width, Height];
                var srcData = layer.Data;
                var dstData = Tiles;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var gid = srcData[x, y];
                        dstData[x, y] = new Tile(0, gid);
                    }
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
