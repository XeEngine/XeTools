namespace Xe.Game.Tilemaps
{
    public enum LayerProcessingMode
    {
        Tilemap,
        Collision,
        Depth
    }

    public class LayerTilemap : LayerEntry
    {
        public int Width => Tiles?.GetLength(0) ?? 0;

        public int Height => Tiles?.GetLength(1) ?? 0;

        public double Opacity { get; set; }

        public LayerProcessingMode ProcessingMode { get; set; }

        public Tile[,] Tiles { get; set; }
    }
}
