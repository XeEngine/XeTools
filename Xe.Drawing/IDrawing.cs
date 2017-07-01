using System;
using System.Drawing;
using System.Drawing.Imaging;

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
        ISurface Surface { get; }
        Filter Filter { get; set; }

        ISurface CreateSurface(int width, int height, PixelFormat pixelFormat);
        ISurface CreateSurface(string filename);

        void Clear(Color color);
        void DrawSurface(ISurface surface, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, int x, int y, int width, int height, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle dst, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, int x, int y, int width, int height, Flip flip = Flip.None);
        void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip = Flip.None);
    }
}