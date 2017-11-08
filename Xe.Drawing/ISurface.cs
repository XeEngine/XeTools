using System;
using System.Drawing;

namespace Xe.Drawing
{
    public interface ISurface : IDisposable
    {
        int Width { get; }
        int Height { get; }
        Size Size { get; }
        PixelFormat PixelFormat { get; }

        IMappedResource Map();
        void Save(string filename);
    }
}
