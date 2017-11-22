using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public interface ILayersGroup : ILayerBase
    {
        List<ILayerBase> Layers { get; }
    }
}
