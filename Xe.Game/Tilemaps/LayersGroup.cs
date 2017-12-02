using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public class LayersGroup : LayerBase
    {
        public List<LayerBase> Layers { get; set; }
    }
}
