namespace Xe.Game.Particles
{
	public enum Ease
	{
		Linear,
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

	public enum ParameterType
	{
		None,
		X,
		Y,
		Z,
		RotationX,
		RotationY,
		RotationZ,
		ScaleX,
		ScaleY,
		ScaleZ,
		CenterX,
		CenterY,
		CenterZ,
		ColorRed,
		ColorGreen,
		ColorBlue,
		ColorAlpha,

		// Helpers
		ScaleXYZ,
		ColorRGB
	}

	public interface IEffect
	{
		double Get(double x);
	}
}
