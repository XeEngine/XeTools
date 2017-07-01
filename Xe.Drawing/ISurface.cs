using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public interface ISurface : IDisposable
    {
        int Width { get; }
        int Height { get; }
        Size Size { get; }
        PixelFormat PixelFormat { get; }

        void Save(string filename);
    }
}
