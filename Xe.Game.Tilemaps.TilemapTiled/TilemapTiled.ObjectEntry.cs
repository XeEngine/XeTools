using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        private class ObjectEntry : IObjectEntry
        {
            private Tiled.Object _objectEntry;

            public string Name
            {
                get => _objectEntry.Name;
                set => _objectEntry.Name = value;
            }

            public string Type
            {
                get => _objectEntry.Type;
                set => _objectEntry.Type = value;
            }

            public bool Visible
            {
                get => _objectEntry.Visible;
                set => _objectEntry.Visible = value;
            }

            public double X
            {
                get => _objectEntry.X;
                set => _objectEntry.X = value;
            }

            public double Y
            {
                get => _objectEntry.Y;
                set => _objectEntry.Y = value;
            }

            public double Width
            {
                get => _objectEntry.Width;
                set => _objectEntry.Width = value;
            }

            public double Height
            {
                get => _objectEntry.Height;
                set => _objectEntry.Height = value;
            }

            public Flip Flip
            {
                get => GetPropertyValue(_objectEntry.Properties, Flip.None);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }

            public Direction Direction
            {
                get => GetPropertyValue(_objectEntry.Properties, Direction.Undefined);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }


            internal ObjectEntry(Tiled.Object objectEntry)
            {
                _objectEntry = objectEntry;
            }
        }
    }
}
