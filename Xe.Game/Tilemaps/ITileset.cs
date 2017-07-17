namespace Xe.Tools.Tilemap
{
    public interface ITileset
    {
        string ImagePath { get; }

        int TileWidth { get; }

        int TileHeight { get; }

        int TilesPerRow { get; }

        int TilesCount { get; }
    }
}
