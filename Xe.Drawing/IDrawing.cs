using System;
using System.Drawing;

namespace Xe.Drawing
{
    [Flags]
    public enum Flip
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2,
        FlipBoth = FlipHorizontal | FlipVertical
    }

    public enum Filter
    {
        Nearest,
        Linear,
        Cubic
    }

    public interface IDrawing : IDisposable
    {
        ISurface Surface { get; set; }
        Filter Filter { get; set; }

        ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type = SurfaceType.Input);
        ISurface CreateSurface(string filename, Color[] filterColors = null);

        void Clear(Color color);
        void DrawRectangle(RectangleF rect, Color color, float width = 1.0f);
        void DrawSurface(ISurface surface, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, int x, int y, int width, int height, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle dst, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, int width, int height, Flip flip = Flip.None);
		void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip = Flip.None);

		void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip = Flip.None);

		void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None);

		void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None);
	}
}