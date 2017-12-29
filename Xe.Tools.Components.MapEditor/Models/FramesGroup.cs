using System.Collections.Generic;
using Xe.Drawing;

namespace Xe.Tools.Components.MapEditor.Models
{
    public class FramesGroup
    {
        public Xe.Game.Animations.AnimationDefinition Definition { get; set; }

        public Xe.Game.Animations.AnimationReference Reference { get; set; }

        public Xe.Game.Animations.Animation Animation { get; set; }

        public ISurface Texture { get; set; }

        public IEnumerable<Frame> Frames { get; set; }
    }
}
