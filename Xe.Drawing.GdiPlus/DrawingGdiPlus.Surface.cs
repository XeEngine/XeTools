using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus
    {
        internal class CSurface : ISurface
        {
            internal Bitmap Bitmap { get; set; }

            public int Width => Bitmap.Width;

            public int Height => Bitmap.Height;

            public Size Size => Bitmap.Size;

            public PixelFormat PixelFormat => Bitmap.PixelFormat;

            public void Save(string filename)
            {
                Bitmap.Save(filename);
            }

            public void Dispose()
            {
                Bitmap.Dispose();
            }

            internal CSurface(string filename)
            {
                Bitmap = new Bitmap(filename);
                Init();
            }
            internal CSurface(Image image)
            {
                Bitmap = new Bitmap(image);
                Init();
            }
            internal CSurface(Bitmap bitmap)
            {
                Bitmap = bitmap.Clone() as Bitmap;
                Init();
            }

            private void Init()
            {
                Bitmap.MakeTransparent(Color.Magenta);
            }
        }

        public override ISurface CreateSurface(string filename)
        {
            return new CSurface(filename);
        }
        public ISurface CreateSurface(Image image)
        {
            return new CSurface(image);
        }
        public ISurface CreateSurface(Bitmap bitmap)
        {
            return new CSurface(bitmap);
        }
    }
}
