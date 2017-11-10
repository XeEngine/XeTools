namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayerTilemap : ILayerTilemap
        {
            internal ITile[,] Tiles { get; private set; }

            internal Tiled.Layer Layer { get; private set; }

            public string Name
            {
                get => Layer.Name;
                set => Layer.Name = value;
            }

            public bool Visible
            {
                get => Layer.Visible;
                set => Layer.Visible = value;
            }

            public int Priority
            {
                get => GetPropertyValue<int>(Layer.Properties);
                set => SetPropertyValue(Layer.Properties, value);
            }

            public int Width => Layer.Width;
            public int Height => Layer.Height;

            public double Opacity
            {
                get => Layer.Opacity;
                set => Layer.Opacity = value;
            }

            public ITile GetTile(int x, int y)
            {
                return Tiles[x, y];
            }

            internal CLayerTilemap(TilemapTiled map, Tiled.Layer layer)
            {
                Layer = layer;
                Priority = GetPriorityFromName(layer.Name);

                Tiles = new Tile[Width, Height];
                var srcData = layer.Data;
                var dstData = Tiles;
                var width = Width;
                var height = Height;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var gid = srcData[x, y];
                        dstData[x, y] = new Tile(0, gid);
                    }
                }
            }

            private int GetPriorityFromName(string name)
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
