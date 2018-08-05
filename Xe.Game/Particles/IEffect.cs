namespace Xe.Game.Particles
{
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
		ScaleXYZ,
		ColorRGB
	}

	public interface IEffect
	{
		double Get(double x);
	}
}
