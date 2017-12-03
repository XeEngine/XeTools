using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Tilemaps
{
    public static class Extensions
    {
        public static IEnumerable<LayerEntry> FlatterLayers(this IEnumerable<LayerBase> entries)
        {
            var list = new List<LayerEntry>();
            foreach (var entry in entries)
            {
                if (entry is LayerEntry layer)
                {
                    list.Add(layer);
                }
                else if (entry is LayersGroup group)
                {
                    list.AddRange(FlatterLayers(group.Layers));
                }
            }
            return list;
        }
    }
}
