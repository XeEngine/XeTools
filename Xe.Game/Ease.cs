namespace Xe.Game
{
	public enum Ease
	{
		Linear,
		Floor,
		Round,
		Ceiling,
		QuadraticEaseIn,
		QuadraticEaseOut,
		Quadratic,
		CubicEaseIn,
		CubicEaseOut,
		CubicEaseInOut,
		QuarticEaseIn,
		QuarticEaseOut,
		QuarticEaseInOut,
		QuinticEaseIn,
		QuinticEaseOut,
		QuinticEaseInOut,
		SineEaseIn,
		SineEaseOut,
		SineEaseInOut,
		CircularEaseIn,
		CircularEaseOut,
		CircularEaseInOut,
		ExponentialEaseIn,
		ExponentialEaseOut,
		ExponentialEaseInOut,
		ElasticEaseIn,
		ElasticEaseOut,
		ElasticEaseInOut,
		BackEaseIn,
		BackEaseOut,
		BackEaseInOut,
		BounceEaseIn,
		BounceEaseOut,
		BounceEaseInOut
	}

	public static class EaseUtility
	{
		public static double Calculate(Ease ease, double x)
		{
			double f;
			switch (ease)
			{
				case Ease.Linear: return x;
				case Ease.Floor: return System.Math.Floor(x);
				case Ease.Round: return System.Math.Round(x);
				case Ease.Ceiling: return System.Math.Ceiling(x);
				case Ease.QuadraticEaseIn: return x * x;
				case Ease.QuadraticEaseOut: return -(x * (x - 2));
				case Ease.Quadratic: return x > 0.5 ? 2.0 * x * x : ((-2.0 * x * x) + (4.0 * x) - 1.0);
				case Ease.CubicEaseIn: return x * x * x;
				case Ease.CubicEaseOut:
					f = (x - 1.0);
					return f * f * f + 1.0;
				case Ease.CubicEaseInOut:
					if (x < 0.5)
						return 4 * x * x * x;
					f = ((2.0 * x) - 2.0);
					return 0.5 * f * f * f + 1.0;
				case Ease.QuarticEaseIn: return x * x * x * x;
				case Ease.QuarticEaseOut:
					f = (x - 1.0);
					return f * f * f * (1 - x) + 1;
				case Ease.QuarticEaseInOut:
					if (x < 0.5)
						return 8 * x * x * x * x;
					f = (x - 1);
					return -8.0 * f * f * f * f + 1;
				case Ease.QuinticEaseIn: return x * x * x * x * x;
				case Ease.QuinticEaseOut:
					f = (x - 1);
					return f * f * f * f * f + 1;
				case Ease.QuinticEaseInOut:
					if (x < 0.5)
						return 16.0 * x * x * x * x * x;
					f = ((2 * x) - 2.0);
					return 0.5 * f * f * f * f * f + 1;
				case Ease.SineEaseIn: return System.Math.Sin((x - 1) * System.Math.PI / 2.0) + 1.0;
				case Ease.SineEaseOut: return System.Math.Sin(x * System.Math.PI / 2.0);
				case Ease.SineEaseInOut: return 0.5 * (1.0 - System.Math.Cos(x * System.Math.PI));
				case Ease.CircularEaseIn: return 1 - System.Math.Sqrt(1 - (x * x));
				case Ease.CircularEaseOut: return System.Math.Sqrt((2 - x) * x);
				case Ease.CircularEaseInOut:
					if (x < 0.5)
						return 0.5 * (1 - System.Math.Sqrt(1 - 4 * (x * x)));
					return 0.5 * (System.Math.Sqrt(-((2 * x) - 3) * ((2 * x) - 1)) + 1);
				case Ease.ExponentialEaseIn: return (x == 0.0) ? x : System.Math.Pow(2, 10 * (x - 1));
				case Ease.ExponentialEaseOut: return (x == 1.0) ? x : 1 - System.Math.Pow(2, -10 * x);
				case Ease.ExponentialEaseInOut:
					if (x == 0.0 || x == 1.0) return x;
					if (x < 0.5)
					{
						return 0.5 * System.Math.Pow(2, (20 * x) - 10);
					}
					else
					{
						return -0.5 * System.Math.Pow(2, (-20 * x) + 10) + 1;
					}
				case Ease.ElasticEaseIn: return System.Math.Sin(13 * System.Math.PI / 2.0 * x) * System.Math.Pow(2, 10 * (x - 1));
				case Ease.ElasticEaseOut: return System.Math.Sin(-13 * System.Math.PI / 2.0 * (x + 1)) * System.Math.Pow(2, -10 * x) + 1;
				case Ease.ElasticEaseInOut:
					if (x < 0.5)
						return 0.5 * System.Math.Sin(13 * System.Math.PI / 2.0 * (2 * x)) * System.Math.Pow(2, 10 * ((2 * x) - 1));
					return 0.5 * (System.Math.Sin(-13 * System.Math.PI / 2.0 * ((2 * x - 1) + 1)) * System.Math.Pow(2, -10 * (2 * x - 1)) + 2);
				case Ease.BackEaseIn: return x * x * x - x * System.Math.Sin(x * System.Math.PI);
				case Ease.BackEaseOut:
					f = (1 - x);
					return 1 - (f * f * f - f * System.Math.Sin(f * System.Math.PI));
				case Ease.BackEaseInOut:
					if (x < 0.5)
					{
						f = 2 * x;
						return 0.5 * (f * f * f - f * System.Math.Sin(f * System.Math.PI));
					}
					else
					{
						f = (1 - (2 * x - 1));
						return 0.5 * (1 - (f * f * f - f * System.Math.Sin(f * System.Math.PI))) + 0.5;
					}
				case Ease.BounceEaseIn: return 1 - Calculate(Ease.BounceEaseOut, 1 - x);
				case Ease.BounceEaseOut:
					if (x < 4 / 11.0)
						return (121 * x * x) / 16.0;
					if (x < 8 / 11.0)
						return (363 / 40.0 * x * x) - (99 / 10.0 * x) + 17 / 5.0;
					if (x < 9 / 10.0)
						return (4356 / 361.0 * x * x) - (35442 / 1805.0 * x) + 16061 / 1805.0;
					return (54 / 5.0 * x * x) - (513 / 25.0 * x) + 268 / 25.0;
				case Ease.BounceEaseInOut:
					if (x < 0.5)
						return 0.5 * Calculate(Ease.BounceEaseIn, x * 2);
					else
						return 0.5 * Calculate(Ease.BounceEaseOut, x * 2 - 1) + 0.5;
				default: return 0.0;
			}
		}
	}
}
