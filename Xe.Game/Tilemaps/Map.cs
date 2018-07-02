using System;
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

        public Guid BgmField { get; set; }

        public Guid BgmBattle { get; set; }

        public List<Tileset> Tilesets { get; set; }

        public List<LayerBase> Layers { get; set; }

        public List<LayerDefinition> LayersDefinition { get; set; }

        public List<EventDefinition> EventDefinitions { get; set; }
    }
}
