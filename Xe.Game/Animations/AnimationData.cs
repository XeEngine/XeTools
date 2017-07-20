using System.Collections.Generic;

namespace Xe.Game.Animations
{
    public class AnimationData
    {
        public List<Texture> Textures { get; set; }

        public Dictionary<string, Frame> Frames { get; set; }

        public Dictionary<string, Animation> Animations { get; set; }

        public List<AnimationRef> AnimationGroups { get; set; }
    }
}
