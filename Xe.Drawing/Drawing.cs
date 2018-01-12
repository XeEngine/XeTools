using System.Drawing;

namespace Xe.Drawing
{
    public enum SurfaceType
    {
        // Surface used as input for drawing
        Input,
        // Surface used where the content will be drawn.
        Output,
        // Used as input and output
        InputOutput
    }

    public abstract class Drawing : IDrawing
    {
        public abstract ISurface Surface { get; set; }
        public abstract Filter Filter { get; set; }

        public abstract void Clear(Color color);
        public abstract ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type);
        public abstract ISurface CreateSurface(string filename, Color[] filterColors = null);
        public abstract void Dispose();
        
        public void DrawSurface(ISurface surface, int x, int y, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            var dst = new Rectangle(x, y, src.Width, src.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, int x, int y, int width, int height, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            var dst = new Rectangle(x, y, width, height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle dst, Flip flip)
        {
            var src = new Rectangle(0, 0, surface.Width, surface.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle src, int x, int y, Flip flip)
        {
            var dst = new Rectangle(x, y, src.Width, src.Height);
            DrawSurface(surface, src, dst, flip);
        }

        public void DrawSurface(ISurface surface, Rectangle src, int x, int y, int width, int height, Flip flip)
        {
            var dst = new Rectangle(x, y, width, height);
            DrawSurface(surface, src, dst, flip);
		}

		public void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
		{
			DrawSurface(surface, src, new RectangleF(dst.X, dst.Y, dst.Width, dst.Height), flip);
		}

		public abstract void DrawRectangle(RectangleF rect, Color color, float width = 1.0f);
        public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip);
		public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip = Flip.None);
		public abstract void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None);
	}
}
