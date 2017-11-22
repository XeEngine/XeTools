using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public interface ILayerBase
    {
        string Name { get; set; }

        bool Visible { get; set; }
    }
}
