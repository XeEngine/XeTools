using System;
using System.Collections.Generic;
using System.Drawing;
using TiledSharp;
using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled : ITileMap
    {
        internal TmxMap Map { get; private set; }

        public Size Size { get; private set; }
        public Size TileSize { get; private set; }

        public List<ITileset> Tilesets { get; private set; }
        public List<ILayer> Layers { get; private set; }

        public TilemapTiled(TmxMap map)
        {
            Map = map;
            Size = new Size(Map.Width, Map.Height);
            TileSize = new Size(Map.TileWidth, Map.TileHeight);
            Tilesets = new List<ITileset>();
            foreach (var item in map.Tilesets)
                Tilesets.Add(new CTileset(item));
            Layers = new List<ILayer>();
            foreach (var item in map.Layers)
                Layers.Add(new CLayer(this, item));
        }
    }
}
