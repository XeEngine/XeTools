using System;
using System.Windows;
using System.Windows.Media;
using Xe.Game.Animations;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Components.MapEditor.Utility;
using Xe.Tools.Components.MapEditor.ViewModels;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor.Controls
{
    public class Tilemap : FrameworkElement
    {
        private VisualCollection children;

        public ProjectService ProjectService => AnimationService?.ProjectService;
        public AnimationService AnimationService => MapEditorViewModel.Instance.AnimationService;

        public Tilemap()
        {
            var animDataOgre = GetAnimation("ogre");
            var animOgre = animDataOgre.GetAnimation("Stand", Direction.Left);

            children = new VisualCollection(this);

            var visual = new DrawingVisual();
            children.Add(visual);

            using (var dc = visual.RenderOpen())
            {
                dc.DrawAnimation(animOgre, 0, 0);
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

        private AnimationDataEntry GetAnimation(string name)
        {
            var fileName = $"{name}.anim.json";
            return AnimationDataEntry.Create(AnimationService, fileName);
        }
    }
}
