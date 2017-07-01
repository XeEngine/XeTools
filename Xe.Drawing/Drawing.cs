

using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public abstract class Drawing : IDrawing
    {
        public abstract ISurface Surface { get; }
        public abstract Filter Filter { get; set; }

        public abstract void Clear(Color color);
        public abstract ISurface CreateSurface(int width, int height, PixelFormat pixelFormat);
        public abstract ISurface CreateSurface(string filename);
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

        public abstract void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip);
    }
}
