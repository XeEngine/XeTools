using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xe.Game.Animations
{
    public class AnimationsGroup
    {
        public List<string> SpriteSheets { get; set; }
        public Dictionary<string, Frame> Frames { get; set; }
        public Dictionary<string, Animation> Animations { get; set; }
        public List<AnimationRef> AnimationReferences { get; set; }
    }
}
