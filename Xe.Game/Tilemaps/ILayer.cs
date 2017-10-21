namespace Xe.Game.Tilemaps
{
    public interface ILayer
    {
        string Name { get; }

        bool Visible { get; }

        int Width { get; }

        int Height { get; }

        int Index { get; }

        ITile GetTile(int x, int y);
    }
}
