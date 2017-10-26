using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Components.MapEditor.Utility;
using Xe.Tools.Components.MapEditor.ViewModels;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor.Controls
{
    public class Tilemap : FrameworkElement
    {
        private class AnimKeyEntry : IEquatable<AnimKeyEntry>
        {
            public string AnimationData { get; }
            public string Animation { get; }
            public Direction Direction { get; }

            internal AnimKeyEntry(string animData, string anim, Direction direction)
            {
                AnimationData = animData;
                Animation = anim;
                Direction = direction;
            }

            public bool Equals(AnimKeyEntry other)
            {
                return AnimationData == other.AnimationData &&
                    Animation == other.Animation &&
                    Direction == other.Direction;
            }
        }

        #region Delegates and events

        public delegate void SelectedEntity(object sender, IObjectEntry objectEntry);
        public delegate void MoveEntry(object sender, IObjectEntry objectEntry, double newX, double newY);

        public SelectedEntity OnSelectedEntity;
        public MoveEntry OnMoveEntry;

        #endregion

        #region Services

        public ProjectService ProjectService => AnimationService?.ProjectService;
        public AnimationService AnimationService => MapEditorViewModel.Instance.AnimationService;

        #endregion

        #region Resources

        private VisualCollection _children;
        private DrawingVisual _visual;
        private ITileMap _tileMap => MapEditorViewModel.Instance.TileMap;
        private ResourceService<string, AnimationDataEntry> _resAnimationData;
        private ResourceService<AnimKeyEntry, FramesGroup> _resAnimations;
        private ResourceService<string, BitmapSource> _resTileset;
        private ResourceService<int, CroppedBitmap> _resTile;

        #endregion

        public ITileMap TileMap
        {
            get => _tileMap;
        }

        private int _scrollX, _scrollY;
        public int ScrollX
        {
            get => _scrollX;
            set
            {
                _scrollX = value;
                Render();
            }
        }
        public int ScrollY
        {
            get => _scrollY;
            set
            {
                _scrollY = value;
                Render();
            }
        }

        public Tilemap()
        {
            #region Resources initialization

            _resAnimationData = new ResourceService<string, AnimationDataEntry>(
                OnResourceAnimationDataLoad,
                OnResourceAnimationDataUnload);
            _resAnimations = new ResourceService<AnimKeyEntry, FramesGroup>(
                OnResourceAnimationsLoad,
                OnResourceAnimationsUnload);
            _resTileset = new ResourceService<string, BitmapSource>(
                OnResourceTilesetLoad,
                OnResourceTilesetUnload);
            _resTile = new ResourceService<int, CroppedBitmap>(
                OnResourceTileLoad,
                OnResourceTileUnload);

            foreach (var item in TileMap.Tilesets)
                _resTileset.Add(item.ImagePath);
            for (int i = 0; i < 65536; i++)
                _resTile.Add(i);

            #endregion

            _children = new VisualCollection(this);

            _visual = new DrawingVisual();
            _children.Add(_visual);

            Render();
        }

        #region Public methods

        public void Render()
        {
            using (var dc = _visual.RenderOpen())
            {
                Render(dc);
            }
        }

        #endregion

        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        #region Rendering

        private void Render(DrawingContext dc)
        {
            if (TileMap != null)
            {
                var color = TileMap.BackgroundColor;
                var size = TileMap.Size;
                var tileSize = TileMap.TileSize;
                var brush = new SolidColorBrush(new System.Windows.Media.Color()
                {
                    A = color.A,
                    R = color.R,
                    G = color.G,
                    B = color.B
                });
                dc.DrawRectangle(brush, new Pen(brush, 1.0), new Rect()
                {
                    X = 0, Y = 0,
                    Width = size.Width * tileSize.Width,
                    Height = size.Height * tileSize.Height
                });
                foreach (var layer in TileMap.Layers)
                    RenderLayer(dc, layer);
            }
        }

        private void RenderLayer(DrawingContext dc, ILayerEntry layer)
        {
            if (layer is ILayerTilemap tilemap) RenderLayer(dc, tilemap);
            else if (layer is ILayerObjects objects) RenderLayer(dc, objects);
        }

        private void RenderLayer(DrawingContext dc, ILayerTilemap layer)
        {
            if (!layer.Visible)
                return;
            /*int width = Math.Min((int)ActualWidth / TileMap.TileSize.Width, layer.Width);
            int height = Math.Min((int)ActualHeight / TileMap.TileSize.Height, layer.Height);
            var rect = new Rect
            {
                Width = TileMap.TileSize.Width,
                Height = TileMap.TileSize.Height
            };
            for (int y = 0; y < height; y++)
            {
                rect.Y = y * rect.Width;
                for (int x = 0; x < width; x++)
                {
                    var tile = layer.GetTile(x, y);
                    if (tile.Index > 0)
                    {
                        rect.X = x * rect.Height;
                        var imgTile = _resTile[tile.Index];
                        dc.DrawImage(imgTile, rect);
                    }
                }
            }*/
        }

        private void RenderLayer(DrawingContext dc, ILayerObjects layer)
        {
            foreach (var entry in layer.Objects)
                RenderObject(dc, entry);
        }

        private void RenderObject(DrawingContext dc, IObjectEntry entry)
        {
            var framesGroup = GetFramesGroup(entry.Name, "Stand", entry.Direction);
            if (framesGroup != null)
            {
                var realX = entry.X + entry.Width / 2;
                var realY = entry.Y + entry.Height / 2;
                if (realX > ActualWidth ||
                    realY > ActualHeight)
                    return;
                dc.DrawAnimation(framesGroup, realX, realY);
            }
            else
            {

            }
        }

        #endregion

        #region Event handler

        private bool _isMouseDown;
        private IObjectEntry _objEntrySelected;
        private Point _dragMousePosition;
        private double _dragObjEntryX, _dragObjEntryY;
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            _objEntrySelected = GetObjectEntry(ScrollX + position.X, ScrollY + position.Y);
            if (_objEntrySelected != null)
            {
                _isMouseDown = true;
                _dragMousePosition = position;
                _dragObjEntryX = _objEntrySelected.X;
                _dragObjEntryY = _objEntrySelected.Y;
                OnSelectedEntity?.Invoke(this, _objEntrySelected);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isMouseDown && _objEntrySelected != null)
            {
                var position = e.GetPosition(this);
                var diff = position - _dragMousePosition;
                var newX = _dragObjEntryX + diff.X;
                var newY = _dragObjEntryY + diff.Y;
                OnMoveEntry?.Invoke(this, _objEntrySelected, newX, newY);
            }
            else
                base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            _isMouseDown = false;
            base.OnMouseUp(e);
        }

        #endregion
        
        #region Resource events and utilities

        private IObjectEntry GetObjectEntry(double x, double y)
        {
            foreach (var layer in TileMap.Layers
                .Where(o => o is ILayerObjects)
                .Select(o => o as ILayerObjects)
                .Reverse())
            {
                foreach (var o in layer.Objects)
                {
                    var ox = o.X;
                    var oy = o.Y;
                    if (x >= ox && x < ox + o.Width &&
                        y >= oy && y < oy + o.Height)
                        return o;
                }
            }
            return null;
        }

        private FramesGroup GetFramesGroup(string name, string animation, Direction direction = Direction.Undefined)
        {
            switch (name)
            {
                case "onipolla":
                    name = "onip";
                    break;
                case "player":
                    name = "lehior";
                    break;
            }

            var desc = new AnimKeyEntry(name, animation, direction);
            if (!_resAnimationData.Exists(name))
                _resAnimationData.Add(name);
            if (!_resAnimations.Exists(desc))
                _resAnimations.Add(desc);
            return _resAnimations[desc];
        }

        private bool OnResourceAnimationDataLoad(string name, out AnimationDataEntry entry)
        {
            var fileName = $"{name}.anim.json";
            entry = AnimationDataEntry.Create(AnimationService, fileName);
            return entry != null;
        }

        private void OnResourceAnimationDataUnload(string name, AnimationDataEntry entry)
        {

        }

        private bool OnResourceAnimationsLoad(AnimKeyEntry entry, out FramesGroup framesGroup)
        {
            var animationData = _resAnimationData[entry.AnimationData];
            if (animationData != null)
            {
                framesGroup = animationData.GetAnimation(entry.Animation, entry.Direction);
            }
            else
            {
                framesGroup = null;
            }
            return framesGroup != null;
        }

        private void OnResourceAnimationsUnload(AnimKeyEntry entry, FramesGroup framesGroup)
        {

        }

        private bool OnResourceTilesetLoad(string fileName, out BitmapSource bitmap)
        {
            bitmap = ImageService.Open(fileName);
            bitmap = ImageService.MakeTransparent(bitmap, new Tools.Services.Color[]
            {
                new Tools.Services.Color() { r = 255, g = 0, b = 255, a = 0 }
            });
            return bitmap != null;
        }

        private void OnResourceTilesetUnload(string fileName, BitmapSource bitmap)
        {

        }

        private bool OnResourceTileLoad(int index, out CroppedBitmap bitmap)
        {
            var tileset = TileMap.Tilesets
                .LastOrDefault(x => index >= x.StartId);
            bitmap = null;
            if (tileset != null)
            {
                var texture = _resTileset[tileset.ImagePath];
                if (texture != null)
                {
                    var realIndex = index - tileset.StartId;
                    var width = TileMap.TileSize.Width;
                    var height = TileMap.TileSize.Height;
                    bitmap = new CroppedBitmap(texture,
                        new Int32Rect()
                        {
                            X = (realIndex % tileset.TilesPerRow) * width,
                            Y = (realIndex / tileset.TilesPerRow) * height,
                            Width = width,
                            Height = height
                        });
                }
            }
            return bitmap != null;
        }

        private void OnResourceTileUnload(int index, CroppedBitmap bitmap)
        {

        }

        #endregion
    }
}
