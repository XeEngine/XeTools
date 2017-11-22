namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayerBase : ILayerBase
        {
            protected Tiled.ILayerEntry _entry;

            public string Name
            {
                get => _entry.Name;
                set => _entry.Name = value;
            }

            public bool Visible
            {
                get => _entry.Visible;
                set => _entry.Visible = value;
            }

            public CLayerBase(Tiled.ILayerEntry entry)
            {
                _entry = entry;
            }
        }

        internal class CLayerEntry : CLayerBase, ILayerEntry
        {
            public int Priority
            {
                get => GetPropertyValue<int>(_entry.Properties);
                set => SetPropertyValue(_entry.Properties, value);
            }

            public CLayerEntry(Tiled.ILayerEntry entry) :
                base(entry)
            {
                _entry = entry;
            }
        }
    }
}
