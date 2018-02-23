using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Particles
{
    public class ParticlesData
    {
		public string AnimationDataName { get; set; }

		public List<ParticlesGroup> Groups { get; set; } =
			new List<ParticlesGroup>();
	}
}
