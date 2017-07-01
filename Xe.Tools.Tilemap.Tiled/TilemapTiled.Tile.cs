using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Xe.Tools.Tilemap
{
    public partial class TilemapTiled
    {
        internal class Tile : ITile
        {
            private TmxLayerTile _tile;

            public int Tileset { get; set; }
            public int Index { get; set; }
            public int PaletteIndex { get; set; }
            public bool IsFlippedX => _tile.HorizontalFlip;
            public bool IsFlippedY => _tile.VerticalFlip;
            public bool IsPriority => false;

            internal Tile(TmxLayerTile tile)
            {
                _tile = tile;
            }
        }
    }
}
