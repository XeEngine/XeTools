using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Particles
{
	public class Effect : IEffect
	{
		public Ease Ease { get; set; }

		public ParameterType Parameter { get; set; }

		public double Speed { get; set; }

		public double FixStep { get; set; }

		public double Sum { get; set; }

		public double Multiplier { get; set; }

		public double Delay { get; set; }

		public double Duration { get; set; }

		public double Get(double x)
		{
			return EaseUtility.Calculate(Ease, x * Speed + FixStep) * Multiplier + Sum;
		}
	}
}
