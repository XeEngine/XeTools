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
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.MapEditor.Controls
{
    public class Tilemap : DrawingControl
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

        #endregion

        #region Resources
		
        private Map _tileMap => MapEditorViewModel.Instance.TileMap;
        private ResourceService<string, AnimationDataEntry> _resAnimationData;
        private ResourceService<AnimKeyEntry, FramesGroup> _resAnimations;

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
				DoRender();
            }
        }
        public int ScrollY
        {
            get => _scrollY;
            set
            {
                _scrollY = value;
                DoRender();
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
        }

		#region Public methods

		protected override void OnDrawingCreated()
		{
			_tileMapDrawer?.Dispose();
			_tileMapDrawer = new TilemapDrawer(Drawing)
			{
				ActionDrawObject = RenderObject
			};
			base.OnDrawingCreated();
		}

		protected override void OnDrawRequired()
		{
            if (TileMap != null)
            {
                RenderMap(TileMap);
            }
        }

        #endregion

        #region Rendering
		

        private void RenderMap(Map tileMap)
        {
            var size = tileMap.Size;
            var tileSize = tileMap.TileSize;
            
            var rect = new drawing.RectangleF(ScrollX, ScrollY, (float)ActualWidth, (float)ActualHeight);
            _tileMapDrawer.Map = TileMap;
            _tileMapDrawer.DrawBackground(rect);
            _tileMapDrawer.DrawMap(rect, true);
        }

        private void RenderObject(ObjectEntry entry, float x, float y, float alpha)
        {
			const string DEFAULT_ANIMDATA = "data/sprite/editor.anim.json";

			var realX = x + entry.Width / 2;
			var realY = y + entry.Height / 2;

			int errType;
			var strAnimData = entry.AnimationData;
			if (!string.IsNullOrEmpty(strAnimData))
			{
				var framesGroup = GetFramesGroup(strAnimData, entry.AnimationName, entry.Direction);
				if (framesGroup != null)
				{
					if (realX <= ActualWidth && realY <= ActualHeight)
					{
						Drawing.DrawAnimation(framesGroup, realX, realY, alpha);
						errType = 0;
					}
					else
						errType = 2;
				}
				else
					errType = 2;
			}
			else
				errType = 1;

			if (errType > 0)
			{
				string animName;
				switch (errType)
				{
					case 1:
						animName = "Unknown";
						break;
					case 2:
						animName = "Error";
						break;
					default:
						animName = "Unknown";
						break;
				}
				var framesGroup = GetFramesGroup(DEFAULT_ANIMDATA, animName, Direction.Undefined);
				Drawing.DrawAnimation(framesGroup, realX, realY, alpha);
			}

			if (entry == _objEntrySelected)
            {
				Drawing.DrawObjectEntryRect(entry);
            }
        }

        #endregion

        #region Event handler

        private bool _isMouseDown;
        private ObjectEntry _objEntrySelected;
        private Point _dragMousePosition;
        private double _dragObjEntryX, _dragObjEntryY;
		
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);
            var objEntrySelected = GetObjectEntry(ScrollX + position.X, ScrollY + position.Y);
            if (_objEntrySelected != objEntrySelected)
            {
                _objEntrySelected = objEntrySelected;
                DoRender();
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
            entry = AnimationDataEntry.Create(AnimationService, Drawing, filePath);
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
