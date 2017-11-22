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

            public int Type
            {
                get => GetPropertyValue<int>(Layer.Properties);
                set => SetPropertyValue(Layer.Properties, value);
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
                /*var defaultPriority = GetPriorityFromName(layer.Name);
                if (defaultPriority >= 0)
                    Priority = defaultPriority;*/

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
                var split = name.Split(' ');
                if (split.Length > 1)
                {
                    var pre = split[0];
                    switch (pre)
                    {
                        case "B": return 1;
                        case "B1": return 1;
                        case "B2": return 2;
                        case "L": return 3;
                        case "S": return 4;
                        case "SL": return 4;
                        case "H": return 5;
                        case "SH": return 6;
                        case "X": return 7;
                    }
                }
                return -1;
            }
        }
    }
}
