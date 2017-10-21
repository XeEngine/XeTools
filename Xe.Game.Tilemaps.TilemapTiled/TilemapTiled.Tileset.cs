namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CTileset : ITileset
        {
            private Tiled.Tileset _tileset;

            public string ImagePath => _tileset.FullImagePath;

            public int StartId => _tileset.FirstGid;

            public int TileWidth => _tileset.TileWidth;

            public int TileHeight => _tileset.TileHeight;

            public int TilesPerRow => _tileset.Columns;

            public int TilesCount => _tileset.TileCount;

            internal CTileset(Tiled.Tileset tileset)
            {
                _tileset = tileset;
            }
        }
    }
}
