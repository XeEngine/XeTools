namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class Tile : ITile
        {
            public int Tileset { get; set; }
            public int Index { get; set; }
            public int PaletteIndex { get; set; }
            public bool IsFlippedX { get; }
            public bool IsFlippedY { get; }
            public bool IsPriority => false;

            internal Tile(int tileset, uint tile)
            {
                Tileset = tileset;
                Index = (int)(tile & Tiled.Layer.INDEX_FLAG);
                PaletteIndex = 0;
                IsFlippedX = (tile & Tiled.Layer.FLIPPED_HORIZONTALLY_FLAG) != 0;
                IsFlippedY = (tile & Tiled.Layer.FLIPPED_VERTICALLY_FLAG) != 0;
            }
        }
    }
}
