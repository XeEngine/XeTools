using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayerObjects : CLayerEntry, ILayerObjects
        {
            private Tiled.ObjectGroup _objectGroup;

            public List<IObjectEntry> Objects { get; }

            public CLayerObjects(Tiled.ObjectGroup objectGroup) :
                base(objectGroup)
            {
                _objectGroup = objectGroup;
                Objects = _objectGroup.Objects
                    .Select(x => (IObjectEntry)new ObjectEntry(x))
                    .ToList();
            }
        }
    }
}
