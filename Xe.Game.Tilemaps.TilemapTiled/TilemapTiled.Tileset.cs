namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        public class Tileset : ITileset
        {
            private Tiled.Tileset _tileset;

            public string ExternalTileset => _tileset.Source;

            public string ImageSource => _tileset.Image?.Source ?? null;

            public string ImagePath => _tileset.FullImagePath;

            public int StartId => _tileset.FirstGid;

            public int TileWidth => _tileset.TileWidth;

            public int TileHeight => _tileset.TileHeight;

            public int TilesPerRow => _tileset.Columns;

            public int TilesCount => _tileset.TileCount;

            internal Tileset(Tiled.Tileset tileset)
            {
                _tileset = tileset;
            }
        }
    }
}
