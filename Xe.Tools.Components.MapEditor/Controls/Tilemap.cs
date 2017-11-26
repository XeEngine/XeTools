using System;
using System.ComponentModel;
using drawing = System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Drawing;
using Xe.Game;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Components.MapEditor.Utility;
using Xe.Tools.Components.MapEditor.ViewModels;
using Xe.Tools.Services;
using System.Runtime.InteropServices;
using Xe.Tools.Components.MapEditor.Services;

namespace Xe.Tools.Components.MapEditor.Controls
{
    public class Tilemap : FrameworkElement
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

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

        private class TileEntry
        {
            public ISurface Surface { get; }
            public drawing.Rectangle Rectangle { get; }

            public TileEntry(ISurface surface, drawing.Rectangle rectangle)
            {
                Surface = surface;
                Rectangle = rectangle;
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
        private IDrawing _drawingService;

        #endregion

        #region Resources

        private VisualCollection _children;
        private DrawingVisual _visual;
        private ITileMap _tileMap => MapEditorViewModel.Instance.TileMap;
        private ResourceService<string, AnimationDataEntry> _resAnimationData;
        private ResourceService<AnimKeyEntry, FramesGroup> _resAnimations;
        private ResourceService<string, ISurface> _resTileset;
        private ResourceService<int, TileEntry> _resTile;
        private WriteableBitmap _writeableBitmap;

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
            _resTileset = new ResourceService<string, ISurface>(
                OnResourceTilesetLoad,
                OnResourceTilesetUnload);
            _resTile = new ResourceService<int, TileEntry>(
                OnResourceTileLoad,
                OnResourceTileUnload);

            #endregion

            _children = new VisualCollection(this);

            _visual = new DrawingVisual();
            _children.Add(_visual);
        }

        #region Public methods

        public void Render()
        {
            if (TileMap != null)
            {
                RenderMap(TileMap, ScrollX, ScrollY);
                using (var dc = _visual.RenderOpen())
                {
                    Flush(dc, _drawingService.Surface);
                }
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

        private void ResizeRenderingEngine(int width, int height)
        {
            if (_drawingService == null)
            {
                _drawingService = DrawingDirectX.Factory(width, height, Drawing.PixelFormat.Format32bppArgb);
            }
            else if (_drawingService is DrawingDirectX dx)
            {
                dx.ResizeRenderTarget(width, height);
            }
            Render();
        }

        private void RenderMap(ITileMap tileMap, int x, int y)
        {
            var size = tileMap.Size;
            var tileSize = tileMap.TileSize;

            var backColor = tileMap.BackgroundColor;
            var color = drawing.Color.FromArgb(backColor.A, backColor.R, backColor.G, backColor.B);
            _drawingService.Clear(backColor);

            foreach (var priority in tileMap.Layers
                .FlatteredLayers()
                .GroupBy(l => l.Priority)
                .OrderBy(l => l.Key))
            {
                foreach (var layer in priority)
                {
                    RenderLayer(layer, x, y);
                }
            }
        }

        private void Flush(DrawingContext dc, ISurface surface)
        {
            using (var map = surface.Map())
            {
                if (_writeableBitmap == null ||
                    surface.Width != _writeableBitmap.Width ||
                    surface.Height != _writeableBitmap.Height ||
                    map.Stride / 4 != _writeableBitmap.Width)
                {
                    _writeableBitmap = new WriteableBitmap(map.Stride / 4, surface.Height, 96.0, 96.0, PixelFormats.Bgra32, null);
                }

                _writeableBitmap.Lock();
                CopyMemory(_writeableBitmap.BackBuffer, map.Data, map.Length);
                _writeableBitmap.AddDirtyRect(new Int32Rect()
                {
                    X = 0,
                    Y = 0,
                    Width = surface.Width,
                    Height = surface.Height
                });
                _writeableBitmap.Unlock();
            }

            dc.DrawImage(_writeableBitmap, new Rect()
            {
                X = 0,
                Y = 0,
                Width = _writeableBitmap.Width,
                Height = _writeableBitmap.Height
            });
        }

        private void RenderLayer(ILayerBase layerBase, int x, int y)
        {
            if (layerBase is ILayersGroup group)
            {
                foreach (var item in group.Layers)
                    RenderLayer(item, x, y);
            }
            else if (layerBase is ILayerEntry entry)
                RenderLayer(entry, x, y);
        }

        private void RenderLayer(ILayerEntry layer, int x, int y)
        {
            if (layer is ILayerTilemap tilemap) RenderLayer(tilemap, x, y);
            else if (layer is ILayerObjects objects) RenderLayer(objects, x, y);
        }

        private void RenderLayer(ILayerTilemap layer, int x, int y)
        {
            if (!layer.Visible)
                return;
            var tileSize = TileMap.TileSize;
            int width = Math.Min((int)(ActualWidth + tileSize.Width - 1) / tileSize.Width, layer.Width);
            int height = Math.Min((int)(ActualHeight + tileSize.Height - 1) / tileSize.Height, layer.Height);
            var rect = new drawing.Rectangle
            {
                Width = TileMap.TileSize.Width,
                Height = TileMap.TileSize.Height
            };

            var smallX = x % tileSize.Width;
            var smallY = y % tileSize.Height;
            var tileX = x / tileSize.Width;
            var tileY = y / tileSize.Height;

            width = Math.Min(width, layer.Width - tileX);
            height = Math.Min(height, layer.Height - tileY);

            for (int iy = Math.Max(0, -tileY); iy < height; iy++)
            {
                rect.Y = iy * rect.Width - smallY;
                for (int ix = Math.Max(0, -tileX); ix < width; ix++)
                {
                    var tile = layer.GetTile(tileX + ix, tileY + iy);
                    if (tile.Index > 0)
                    {
                        rect.X = ix * rect.Height - smallX;
                        var imgTile = _resTile[tile.Index];
                        Drawing.Flip flip = Drawing.Flip.None;
                        if (tile.IsFlippedX)
                            flip |= Drawing.Flip.FlipHorizontal;
                        if (tile.IsFlippedY)
                            flip |= Drawing.Flip.FlipVertical;
                        _drawingService.DrawSurface(imgTile.Surface, imgTile.Rectangle, rect, flip);
                    }
                }
            }
        }

        private void RenderLayer(ILayerObjects layer, int x, int y)
        {
            foreach (var entry in layer.Objects)
                RenderObject(entry, x, y);
        }

        private void RenderObject(IObjectEntry entry, int x, int y)
        {
            var strAnimData = entry.AnimationData ?? "data/sprite/editor.anim.json";
            var framesGroup = GetFramesGroup(strAnimData, entry.AnimationName, entry.Direction);
            if (framesGroup != null)
            {
                var realX = entry.X + entry.Width / 2;
                var realY = entry.Y + entry.Height / 2;
                if (realX > ActualWidth ||
                    realY > ActualHeight)
                    return;
                _drawingService.DrawAnimation(framesGroup, realX - x, realY - y);
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

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            var size = sizeInfo.NewSize;
            base.OnRenderSizeChanged(sizeInfo);
            ResizeRenderingEngine((int)size.Width, (int)size.Height);
        }

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

        private FramesGroup GetFramesGroup(string filePath, string animation, Direction direction = Direction.Undefined)
        {
            var desc = new AnimKeyEntry(filePath, animation, direction);
            if (!_resAnimationData.Exists(filePath))
                _resAnimationData.Add(filePath);
            if (!_resAnimations.Exists(desc))
                _resAnimations.Add(desc);
            return _resAnimations[desc];
        }

        private bool OnResourceAnimationDataLoad(string filePath, out AnimationDataEntry entry)
        {
            entry = AnimationDataEntry.Create(AnimationService, _drawingService, filePath);
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

        private bool OnResourceTilesetLoad(string fileName, out ISurface surface)
        {
            surface = _drawingService.CreateSurface(fileName,
                new drawing.Color[]
                {
                    drawing.Color.FromArgb(255, 255, 0, 255)
                });
            return surface != null;
        }

        private void OnResourceTilesetUnload(string fileName, ISurface surface)
        {
            surface?.Dispose();
        }

        private bool OnResourceTileLoad(int index, out TileEntry tileEntry)
        {
            var tileset = TileMap.Tilesets
                .LastOrDefault(x => index >= x.StartId);
            tileEntry = null;
            if (tileset != null)
            {
                var surface = _resTileset[tileset.ImagePath];
                if (surface != null)
                {
                    var realIndex = index - tileset.StartId;
                    var width = TileMap.TileSize.Width;
                    var height = TileMap.TileSize.Height;
                    var rectangle = new drawing.Rectangle()
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

#endregion
    }
}
