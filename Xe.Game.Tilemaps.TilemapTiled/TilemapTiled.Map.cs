using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled : ITileMap
    {
        internal Tiled.Map Map { get; }

        public Size Size { get; private set; }
        public Size TileSize { get; private set; }

        public List<ITileset> Tilesets { get; private set; }
        public List<ILayer> Layers { get; private set; }

        public TilemapTiled(Tiled.Map map)
        {
            Map = map;
            Size = new Size(Map.Width, Map.Height);
            TileSize = new Size(Map.TileWidth, Map.TileHeight);
            Tilesets = new List<ITileset>();
            /*foreach (var item in map.Tilesets)
                Tilesets.Add(new CTileset(item));*/

            Layers = GetLayers(map.Entries)
                .Select(x => new CLayer(this, x) as ILayer)
                .ToList();
        }
        

        private IEnumerable<Tiled.Layer> GetLayers(IEnumerable<Tiled.IEntry> entries)
        {
            var list = new List<Tiled.Layer>();
            foreach (var entry in entries)
            {
                if (entry is Tiled.Layer layer)
                {
                    list.Add(layer);
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
