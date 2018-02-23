using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Particles
{
	public class ParticlesGroup
    {
		public string AnimationName { get; set; }

		public int ParticlesCount { get; set; }

		public double GlobalDelay { get; set; }

		public double GlobalDuration { get; set; }

		public double Delay { get; set; }

		public List<Effect> Effects { get; set; }
	}
}
