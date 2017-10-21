namespace Xe.Game.Tilemaps
{
    public interface ITileset
    {
        string ImagePath { get; }

        int StartId { get; }

        int TileWidth { get; }

        int TileHeight { get; }

        int TilesPerRow { get; }

        int TilesCount { get; }
    }
}
