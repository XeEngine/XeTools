using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CTileset : ITileset
        {
            //internal TmxTileset Tileset { get; private set; }
            public string ImagePath { get; private set; }
            /*public int TileWidth => Tileset.TileWidth;
            public int TileHeight => Tileset.TileHeight;
            public int TilesPerRow => Tileset.Columns ?? 0;
            public int TilesCount => Tileset.TileCount ?? 0;*/
            public int TileWidth => 16;
            public int TileHeight => 16;
            public int TilesPerRow => 0;
            public int TilesCount => 0;

            internal CTileset(/*TmxTileset tileset*/)
            {
                /*Tileset = tileset;
                ImagePath = Tileset?.Image?.Source;*/
            }
        }
    }
}
