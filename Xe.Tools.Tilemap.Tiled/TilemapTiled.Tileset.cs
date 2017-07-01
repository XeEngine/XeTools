using System;
using System.IO;
using TiledSharp;

namespace Xe.Tools.Tilemap
{
    public partial class TilemapTiled
    {
        internal class CTileset : ITileset
        {
            internal TmxTileset Tileset { get; private set; }
            public string ImagePath { get; private set; }
            public int TileWidth => Tileset.TileWidth;
            public int TileHeight => Tileset.TileHeight;
            public int TilesPerRow => Tileset.Columns ?? 0;
            public int TilesCount => Tileset.TileCount ?? 0;

			internal CTileset(TmxTileset tileset)
            {
                Tileset = tileset;
                ImagePath = Tileset?.Image?.Source;
            }
        }
    }
}
