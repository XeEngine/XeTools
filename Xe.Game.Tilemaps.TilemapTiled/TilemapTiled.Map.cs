using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled : ITileMap
    {
        private Tiled.Map Map { get; }

        public string BasePath => Map.BasePath;

        public Size Size { get; private set; }
        public Size TileSize { get; private set; }

        public Color BackgroundColor
        {
            get
            {
                var c = Map.BackgroundColor;
                return Color.FromArgb(c.a, c.r, c.g, c.b);
            }
        }

        public string BgmField
        {
            get => GetPropertyValue<string>(Map.Properties);
            set => SetPropertyValue(Map.Properties, value);
        }

        public string BgmBattle
        {
            get => GetPropertyValue<string>(Map.Properties);
            set => SetPropertyValue(Map.Properties, value);
        }

        public List<ITileset> Tilesets { get; private set; }
        public List<ILayerEntry> Layers { get; private set; }

        public TilemapTiled(Tiled.Map map)
        {
            Map = map;
            Size = new Size(Map.Width, Map.Height);
            TileSize = new Size(Map.TileWidth, Map.TileHeight);

            Tilesets = Map.Tileset
                .Select(x => (ITileset)new Tileset(x))
                .ToList();

            Layers = GetLayers(map.Entries)
                .Select(x =>
                {
                    if (x is Tiled.Layer tileMap)
                        return new CLayerTilemap(this, tileMap);
                    if (x is Tiled.ObjectGroup objectGroup)
                        return new CLayerObjects(objectGroup);
                    return (ILayerEntry)null;
                })
                .Where(x => x != null)
                .ToList();
        }
        

        private IEnumerable<Tiled.ILayerEntry> GetLayers(IEnumerable<Tiled.ILayerEntry> entries)
        {
            var list = new List<Tiled.ILayerEntry>();
            foreach (var entry in entries)
            {
                if (entry is Tiled.Layer layer)
                {
                    list.Add(layer);
                }
                if (entry is Tiled.ObjectGroup objectGroup)
                {
                    list.Add(objectGroup);
                }
                else if (entry is Tiled.Group group)
                {
                    list.AddRange(GetLayers(group.Entries));
                }
            }
            return list;
        }
    }
}
