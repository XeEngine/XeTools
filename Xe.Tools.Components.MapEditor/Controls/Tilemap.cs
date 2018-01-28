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
using Xe.Tools.Tilemap;

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

        #region Delegates and events

        public delegate void SelectedEntity(object sender, ObjectEntry objectEntry);
        public delegate void MoveEntry(object sender, ObjectEntry objectEntry, double newX, double newY);

        public SelectedEntity OnSelectedEntity;
        public MoveEntry OnMoveEntry;

        #endregion

        #region Services

        private TilemapDrawer _tileMapDrawer;
        public ProjectService ProjectService => AnimationService?.ProjectService;
        public AnimationService AnimationService => MapEditorViewModel.Instance.AnimationService;
        private IDrawing _drawingService;

        #endregion

        #region Resources

        private VisualCollection _children;
        private DrawingVisual _visual;
        private Map _tileMap => MapEditorViewModel.Instance.TileMap;
        private ResourceService<string, AnimationDataEntry> _resAnimationData;
        private ResourceService<AnimKeyEntry, FramesGroup> _resAnimations;
        private WriteableBitmap _writeableBitmap;

        #endregion

        public Map TileMap
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
                RenderMap(TileMap);
                var surface = _drawingService.Surface;
                using (var dc = _visual.RenderOpen())
                {
                    Flush(dc, surface);
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
                _drawingService = Xe.Factory.Resolve<IDrawing>();
                if (_drawingService != null)
                {
                    _tileMapDrawer?.Dispose();
                    _tileMapDrawer = new TilemapDrawer(_drawingService)
                    {
                        ActionDrawObject = RenderObject
                    };
                }
            }
            if (_drawingService != null)
            {
                _drawingService.Surface?.Dispose();
                _drawingService.Surface = _drawingService.CreateSurface(
                    width, height, Drawing.PixelFormat.Format32bppArgb, SurfaceType.InputOutput);
                Render();
            }
        }

        private void RenderMap(Map tileMap)
        {
            var size = tileMap.Size;
            var tileSize = tileMap.TileSize;
            
            var rect = new drawing.RectangleF(ScrollX, ScrollY, (float)ActualWidth, (float)ActualHeight);
            _tileMapDrawer.Map = TileMap;
            _tileMapDrawer.DrawBackground(rect);
            _tileMapDrawer.DrawMap(rect, true);
        }

        private void Flush(DrawingContext dc, ISurface surface)
        {
            if (surface != null && surface.Width > 0 && surface.Height > 0)
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
            }

            if (_writeableBitmap != null)
            {
                dc.DrawImage(_writeableBitmap, new Rect()
                {
                    X = 0,
                    Y = 0,
                    Width = _writeableBitmap.Width,
                    Height = _writeableBitmap.Height
                });
            }
        }

        private void RenderObject(ObjectEntry entry, float x, float y, float alpha)
        {
            var strAnimData = entry.AnimationData ?? "data/sprite/editor.anim.json";
            var framesGroup = GetFramesGroup(strAnimData, entry.AnimationName, entry.Direction);
            if (framesGroup != null)
            {
                var realX = x + entry.Width / 2;
                var realY = y + entry.Height / 2;
                if (realX > ActualWidth ||
                    realY > ActualHeight)
                    return;
                _drawingService.DrawAnimation(framesGroup, realX, realY, alpha);
            }
            if (entry == _objEntrySelected)
            {
                _drawingService.DrawObjectEntryRect(entry);
            }
        }

        #endregion

        #region Event handler

        private bool _isMouseDown;
        private ObjectEntry _objEntrySelected;
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
            var objEntrySelected = GetObjectEntry(ScrollX + position.X, ScrollY + position.Y);
            if (_objEntrySelected != objEntrySelected)
            {
                _objEntrySelected = objEntrySelected;
                Render();
            }
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

        private ObjectEntry GetObjectEntry(double x, double y)
        {
            foreach (var layer in TileMap.Layers
                .FlatterLayers<LayerObjects>()
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

#endregion
    }
}
