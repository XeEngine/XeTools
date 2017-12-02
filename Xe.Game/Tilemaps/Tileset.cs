namespace Xe.Game.Tilemaps
{
    public class Tileset
    {
        public string ExternalTileset { get; set; }

        public string ImageSource { get; set; }

        public string ImagePath { get; set; }

        public int StartId { get; set; }

        public int TileWidth { get; set; }

        public int TileHeight { get; set; }

        public int TilesPerRow { get; set; }

        public int TilesCount { get; set; }
    }
}
