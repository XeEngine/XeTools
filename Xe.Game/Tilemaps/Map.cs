using System.Collections.Generic;
using System.Drawing;

namespace Xe.Game.Tilemaps
{
    public class Map
    {
        public string FileName { get; set; }

        public Size Size { get; set; }

        public Size TileSize { get; set; }

        public Color? BackgroundColor { get; set; }

        public string BgmField { get; set; }

        public string BgmBattle { get; set; }

        public List<Tileset> Tilesets { get; set; }

        public List<LayerBase> Layers { get; set; }
    }
}
