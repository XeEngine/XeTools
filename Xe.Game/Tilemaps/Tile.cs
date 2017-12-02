namespace Xe.Game.Tilemaps
{
    public struct Tile
    {
        /// <summary>
        /// Tileset index.
        /// </summary>
        public int Tileset { get; set; }

        /// <summary>
        /// Tile index inside the specified tileset index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Palette index.
        /// </summary>
        public int PaletteIndex { get; set; }

        /// <summary>
        /// If the tile is filipped horizontally
        /// </summary>
        public bool IsFlippedX { get; set; }

        /// <summary>
        /// If the tile is flipped vertically
        /// </summary>
        public bool IsFlippedY { get; set; }

        /// <summary>
        /// If the tile is draw on top.
        /// </summary>
        public bool IsPriority { get; set; }
    }
}
