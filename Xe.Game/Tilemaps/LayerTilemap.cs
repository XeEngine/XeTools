namespace Xe.Game.Tilemaps
{
    public class LayerTilemap : LayerEntry
    {
        public int Type { get; set; }
        
        public int Width => Tiles?.GetLength(0) ?? 0;

        public int Height => Tiles?.GetLength(1) ?? 0;

        public double Opacity { get; set; }

        public Tile[,] Tiles { get; set; }
    }
}
