using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Animations
{
    public class AnimationGroup
    {
        /// <summary>
        /// Name of animation
        /// </summary>
        public string Name { get; set; }

        public List<AnimationRef> References { get; set; }
    }
}
