using System;
using System.Windows;
using System.Windows.Media;
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

        private VisualCollection children;

        #region Services

        public ProjectService ProjectService => AnimationService?.ProjectService;
        public AnimationService AnimationService => MapEditorViewModel.Instance.AnimationService;

        #endregion

        #region Resources

        private ITileMap TileMap => MapEditorViewModel.Instance.TileMap;
        private ResourceService<string, AnimationDataEntry> _resAnimationData;
        private ResourceService<AnimKeyEntry, FramesGroup> _resAnimations;

        #endregion

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

            children = new VisualCollection(this);

            var visual = new DrawingVisual();
            children.Add(visual);

            using (var dc = visual.RenderOpen())
            {
                Render(dc);
            }
        }

        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return children[index];
        }

        #region Rendering

        private void Render(DrawingContext dc)
        {
            if (TileMap != null)
            {
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
                dc.DrawAnimation(framesGroup, entry.X, entry.Y);
            }
            else
            {

            }
        }

        #endregion

        #region Resource events and utilities

        public FramesGroup GetFramesGroup(string name, string animation, Direction direction = Direction.Undefined)
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

        #endregion
    }
}
