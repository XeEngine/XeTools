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

            #region Basic properties

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

            #endregion

            #region Appearance

            public string AnimationData
            {
                get => GetPropertyValue<string>(_objectEntry.Properties);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }

            public string AnimationName
            {
                get => GetPropertyValue<string>(_objectEntry.Properties);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }

            public Direction Direction
            {
                get => GetPropertyValue(_objectEntry.Properties, Direction.Undefined);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }

            public bool Visible
            {
                get => _objectEntry.Visible;
                set => _objectEntry.Visible = value;
            }

            public bool HasShadow
            {
                get => GetPropertyValue(_objectEntry.Properties, false);
                set => SetPropertyValue(_objectEntry.Properties, value);
            }

            #endregion

            #region Layout

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

            public double Z
            {
                get => GetPropertyValue<double>(_objectEntry.Properties);
                set => SetPropertyValue(_objectEntry.Properties, value);
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

            #endregion

            internal ObjectEntry(Tiled.Object objectEntry)
            {
                _objectEntry = objectEntry;
            }
        }
    }
}
