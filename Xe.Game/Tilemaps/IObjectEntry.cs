namespace Xe.Game.Tilemaps
{
    public interface IObjectEntry
    {
        string Name { get; set; }

        string Type { get; set; }

        bool Visible { get; set; }

        double X { get; set; }

        double Y { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        Flip Flip { get; set; }

        Direction Direction { get; set; }
    }
}
