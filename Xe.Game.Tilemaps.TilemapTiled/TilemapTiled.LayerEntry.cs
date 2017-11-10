namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled
    {
        internal class CLayerEntry : ILayerEntry
        {
            private Tiled.ILayerEntry _entry;

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

            public int Priority
            {
                get => GetPropertyValue<int>(_entry.Properties);
                set => SetPropertyValue(_entry.Properties, value);
            }

            public CLayerEntry(Tiled.ILayerEntry entry)
            {
                _entry = entry;
            }
        }
    }
}
