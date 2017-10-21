using System.Collections.Generic;

namespace Xe.Game.Tilemaps
{
    public interface ILayerObjects : ILayerEntry
    {
        List<IObjectEntry> Objects { get; }
    }
}
