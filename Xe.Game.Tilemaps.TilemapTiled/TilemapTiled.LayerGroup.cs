using System.Collections.Generic;
using System.Linq;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayerGroup : CLayerBase, ILayersGroup
        {
            private Tiled.Group _group;

            public List<ILayerBase> Layers { get; }

            public CLayerGroup(TilemapTiled map, Tiled.Group group) :
                base(group)
            {
                _group = group;
                Layers = _group.Entries
                    .Select(x =>
                    {
                        if (x is Tiled.Group __group)
                            return new CLayerGroup(map, __group);
                        if (x is Tiled.Layer layer)
                            return new CLayerTilemap(map, layer);
                        if (x is Tiled.ObjectGroup objGroup)
                            return new CLayerObjects(objGroup);
                        return (ILayerBase)null;
                    })
                    .Where(x => x != null)
                    .ToList();
            }
        }
    }
}
