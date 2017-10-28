using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus
    {
        private class MappedResource : IMappedResource
        {
            public MappedResource(Bitmap bitmap)
            {
                Bitmap = bitmap;
                BitmapData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    bitmap.PixelFormat);
            }

            public Bitmap Bitmap { get; }

            public BitmapData BitmapData { get; }

            public IntPtr Data => BitmapData.Scan0;

            public int Stride => BitmapData.Stride;

            public int Length => BitmapData.Stride * BitmapData.Height;

            public void Dispose()
            {
                Bitmap.UnlockBits(BitmapData);
            }
        }

        internal class CSurface : ISurface
        {
            internal Bitmap Bitmap { get; set; }

            public int Width => Bitmap.Width;

            public int Height => Bitmap.Height;

            public Size Size => Bitmap.Size;

            public PixelFormat PixelFormat => Bitmap.PixelFormat;


            public IMappedResource Map()
            {
                return new MappedResource(Bitmap);
            }

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

        public override ISurface CreateSurface(string filename, Color[] filterColors)
        {
            var surface = new CSurface(filename);
            if (filterColors != null && filterColors.Length > 0)
            {
                foreach (var color in filterColors)
                {
                    surface.Bitmap.MakeTransparent(color);
                }
            }
            return surface;
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
