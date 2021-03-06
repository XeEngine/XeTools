﻿using System;
using System.Drawing;
using System.Linq;
using Xe.Drawing;
using Xe.Game.Tilemaps;
using Xe.Tools.Services;

namespace Xe.Tools.Tilemap
{
    public class TilemapDrawer : IDisposable
    {
        private class TileEntry
        {
            public ISurface Surface { get; }
            public Rectangle Rectangle { get; }

            public TileEntry(ISurface surface, Rectangle rectangle)
            {
                Surface = surface;
                Rectangle = rectangle;
            }
        }

        private Map _map;
        private ResourceService<int, TileEntry> _resTile;

        public IDrawing Drawing { get; }

        public Map Map
        {
            get => _map;
			set => _map = value;
        }

        public ResourceService<string, ISurface> ResourceTileset { get; }

        public Action<ObjectEntry, float, float, float> ActionDrawObject { get; set; }

        public TilemapDrawer(IDrawing drawing)
        {
            Drawing = drawing;
            ResourceTileset = new ResourceService<string, ISurface>(
                OnResourceTilesetLoad, OnResourceTilesetUnload);
            _resTile = new ResourceService<int, TileEntry>(
                OnResourceTileLoad, OnResourceTileUnload);
        }

        public void DrawBackground(RectangleF rect)
        {
            var backColor = Map?.BackgroundColor ?? System.Drawing.Color.Fuchsia;
            if (backColor != null)
            {
                var color = System.Drawing.Color.FromArgb(backColor.A, backColor.R, backColor.G, backColor.B);
                Drawing.Clear(color);
            }
        }

        public void DrawMap(RectangleF rect, bool drawInvisibleObjects = false)
        {
			if (Map == null)
				return;

            foreach (var priority in Map.Layers
                .FlatterLayers()
                .GroupBy(l => l.DefinitionId)
                .Join(Map.LayersDefinition,
                    sublayers => sublayers.Key,
                    definition => definition.Id,
                    (sublayers, definition) => new
                    {
                        Sublayers = sublayers,
                        Definition = definition
                    })
                .Where(x => x.Definition.IsVisible)
                .OrderBy(l => TilemapSettings.LayerNames.
                    FirstOrDefault(x => x.Id == l.Definition.Id)?.Order ?? -1))
            {
                foreach (var layer in priority.Sublayers)
                {
                    DrawLayer(layer, rect);
                }
            }

            if (drawInvisibleObjects)
            {
                foreach (var entities in Map.Layers
                    .FlatterLayers<LayerObjects>()
                    .GroupBy(l => l.DefinitionId)
                    .Join(Map.LayersDefinition,
                        sublayers => sublayers.Key,
                        definition => definition.Id,
                        (sublayers, definition) => new
                        {
                            Sublayers = sublayers,
                            Definition = definition
                        })
                    .Where(x => x.Definition.IsVisible)
                    .OrderBy(l => TilemapSettings.LayerNames.
                        FirstOrDefault(x => x.Id == l.Definition.Id)?.Order ?? -1))
                {
                    foreach (var layer in entities.Sublayers)
                    {
                        DrawLayer(layer, rect, 0.333333f);
                    }
                }
            }
        }
        
        public void DrawLayer(LayerBase layerBase, RectangleF rect)
        {
            if (layerBase is LayersGroup group)
            {
                foreach (var item in group.Layers)
                    DrawLayer(item, rect);
            }
            else if (layerBase is LayerEntry entry)
                DrawLayer(entry, rect);
        }

        public void DrawLayer(LayerEntry layer, RectangleF rect)
        {
            if (layer is LayerTilemap tilemap) DrawLayer(tilemap, rect.X, rect.Y, rect.Width, rect.Height);
            else if (layer is LayerObjects objects) DrawLayer(objects, rect);
        }

        public void DrawLayer(LayerTilemap layer, float x, float y, float panelWidth, float panelHeight)
        {
            if (!layer.Visible)
                return;
            var tileSize = Map.TileSize;
            int width = (int)Math.Min((panelWidth + tileSize.Width * 2 - 1) / tileSize.Width, layer.Width);
            int height = (int)Math.Min((panelHeight + tileSize.Height * 2 - 1) / tileSize.Height, layer.Height);
            var rect = new RectangleF
            {
                Width = Map.TileSize.Width,
                Height = Map.TileSize.Height
            };

            var smallX = x % tileSize.Width;
            var smallY = y % tileSize.Height;
            var tileX = (int)x / tileSize.Width;
            var tileY = (int)y / tileSize.Height;
			var alpha = (float)layer.Opacity;

            width = Math.Min(width, layer.Width - tileX);
            height = Math.Min(height, layer.Height - tileY);
            
            for (int iy = Math.Max(0, -tileY); iy < height; iy++)
            {
                rect.Y = iy * rect.Height - smallY;
                for (int ix = Math.Max(0, -tileX); ix < width; ix++)
                {
                    var tile = layer.Tiles[tileX + ix, tileY + iy];
                    if (tile.Index > 0)
                    {
                        rect.X = ix * rect.Width - smallX;
                        var imgTile = _resTile[tile.Index];
                        if (imgTile != null)
                        {
                            Flip flip = Flip.None;
                            if (tile.IsFlippedX)
                                flip |= Flip.FlipHorizontal;
                            if (tile.IsFlippedY)
                                flip |= Flip.FlipVertical;
                            Drawing.DrawSurface(imgTile.Surface, imgTile.Rectangle, rect, alpha, flip);
                        }
                    }
                }
            }
        }

        public void DrawLayer(LayerObjects layer, RectangleF rect, float alpha = 1.0f)
        {
            const float Margin = 128.0f;
            if (ActionDrawObject == null)
                return;
			if (!layer.Visible)
				return;
            foreach (var entry in layer.Objects)
            {
                float acutalX = (float)(entry.X - rect.X);
                float acutalY = (float)(entry.Y - rect.Y);
                if (acutalX >= -Margin || acutalX + rect.Width < Margin ||
                    acutalY >= -Margin || acutalY + rect.Height < Margin)
                {
                    ActionDrawObject(entry, acutalX, acutalY, alpha);
                }
            }
        }

        public void Dispose()
        {
            ResourceTileset.RemoveAll();
        }

        private bool OnResourceTilesetLoad(string fileName, out ISurface surface)
        {
            surface = Drawing.CreateSurface(fileName,
                new System.Drawing.Color[]
                {
                    System.Drawing.Color.FromArgb(255, 255, 0, 255)
                });
            return surface != null;
        }

        private void OnResourceTilesetUnload(string fileName, ISurface surface)
        {
            surface?.Dispose();
        }

        private bool OnResourceTileLoad(int index, out TileEntry tileEntry)
        {
            var tileset = Map.Tilesets
                .LastOrDefault(x => index >= x.StartId);
            tileEntry = null;
            if (tileset != null)
            {
                var surface = ResourceTileset[tileset.ImagePath];
                if (surface != null)
                {
                    var realIndex = index - tileset.StartId;
                    var width = Map.TileSize.Width;
                    var height = Map.TileSize.Height;
                    var rectangle = new Rectangle()
                    {
                        X = (realIndex % tileset.TilesPerRow) * width,
                        Y = (realIndex / tileset.TilesPerRow) * height,
                        Width = width,
                        Height = height
                    };
                    tileEntry = new TileEntry(surface, rectangle);
                }
            }
            return tileEntry != null;
        }

        private void OnResourceTileUnload(int index, TileEntry tileEntry)
        {

        }
    }
}
