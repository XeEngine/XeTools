using System.Collections.Generic;

namespace Xe.Game.Animations
{
    public class AnimationData
    {
        public List<Texture> Textures { get; set; }

        public List<Frame> Frames { get; set; }

        public List<Animation> Animations { get; set; }

        public List<AnimationRef> AnimationGroups { get; set; }
    }
}
