namespace Xe.Game.Tilemaps
{
    public interface ILayerTilemap : ILayerEntry
    {
        int Width { get; }

        int Height { get; }

        ITile GetTile(int x, int y);
    }
}
