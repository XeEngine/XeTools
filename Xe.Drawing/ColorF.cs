namespace Xe.Drawing
{
    public struct ColorF
    {
		public float R;

		public float G;

		public float B;

		public float A;

		public ColorF(float r, float g, float b, float a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public ColorF(System.Drawing.Color color)
		{
			R = color.R / 255.0f;
			G = color.G / 255.0f;
			B = color.B / 255.0f;
			A = color.A / 255.0f;
		}
	}
}
