using System.Drawing.Imaging;
using static Xe.Drawing.Helpers;

namespace Xe.Drawing
{
    using SharpDX;
    using SharpDX.IO;
    using d2 = SharpDX.Direct2D1;
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;
    using wic = SharpDX.WIC;
    using dw = SharpDX.DirectWrite;
    using System.IO;
    using System.Drawing;
    using SharpDX.Win32;
    using System;

    public partial class DrawingDirectX
    {
        public class CSurface : ISurface
        {
            private DrawingDirectX _drawing;
            private SharpDX.Direct2D1.Bitmap1 _bitmap;
            private PixelFormat _pixelFormat;

            public int Width => _bitmap.PixelSize.Width;

            public int Height => _bitmap.PixelSize.Height;

            public Size Size => new Size(Width, Height);

            public PixelFormat PixelFormat => _pixelFormat;

            internal SharpDX.Direct2D1.Bitmap1 Bitmap => _bitmap;

            public void Save(string filename)
            {
                _drawing.Save(_bitmap, filename);
            }

            public void Dispose()
            {
                _bitmap.Dispose();
            }

            internal CSurface(DrawingDirectX drawing, SharpDX.Direct2D1.Bitmap1 bitmap)
            {
                _drawing = drawing;
                _bitmap = bitmap;
                _pixelFormat = GetPixelFormat(_bitmap.PixelFormat.Format);
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat)
        {
            return CreateSurface(width, height, pixelFormat, d2.BitmapOptions.None);
        }
        internal ISurface CreateSurfaceAsRenderTarget(int width, int height, PixelFormat pixelFormat)
        {
            return CreateSurface(width, height, pixelFormat,
                d2.BitmapOptions.Target | d2.BitmapOptions.CannotDraw);
        }

        public override ISurface CreateSurface(string filename)
        {
            var imagingFactory = device.ImagingFactory;
            using (var inputStream = new wic.WICStream(imagingFactory, filename, NativeFileAccess.Read))
            {
                using (var pngDecoder = new wic.PngBitmapDecoder(imagingFactory))
                {
                    pngDecoder.Initialize(inputStream, wic.DecodeOptions.CacheOnLoad);

                    // decode the loaded image to a format that can be consumed by D2D
                    using (var formatConverter = new wic.FormatConverter(imagingFactory))
                    {
                        var frame = pngDecoder.GetFrame(0);
                        formatConverter.Initialize(frame, wicPixelFormat);

                        // load the base image into a D2D Bitmap
                        var bitmapProperties = new d2.BitmapProperties1(d2PixelFormat);
                        var inputBitmap = d2.Bitmap1.FromWicBitmap(d2dContext, formatConverter, bitmapProperties);
                        //MakeTransparent(inputBitmap, new Color[] { Color.Magenta });

                        return new CSurface(this, inputBitmap);
                    }
                }
            }
        }

        private void MakeTransparent(d2.Bitmap1 bitmap, Color[] colors)
        {
            var data = bitmap.Map(d2.MapOptions.Read);
            foreach (var color in colors)
                MakeTransparent(bitmap, data, color);
            bitmap.Unmap();
        }
        private unsafe void MakeTransparent(d2.Bitmap1 bitmap, DataRectangle data, Color color)
        {
            for (int j = 0; j < bitmap.Size.Height; j++)
            {
                int* p = (int*)(data.DataPointer + data.Pitch * j);
                for (int i = 0; i < bitmap.Size.Width; i++)
                {
                    if ((*p & 0x00FF00FF) == 0x00FF00FF)
                        *p = 0;
                    p++;
                }
            }
        }

        private ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, d2.BitmapOptions options)
        {
            // create the d2d bitmap description and 96 DPI
            var d2dBitmapProps = new d2.BitmapProperties1(d2PixelFormat, 96, 96, options);
            var bitmap = new d2.Bitmap1(d2dContext, new Size2(width, height), d2dBitmapProps);
            return new CSurface(this, bitmap);
        }

        internal void Save(d2.Bitmap1 bitmap, string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            var imagingFactory = device.ImagingFactory;
            using (var stream = new wic.WICStream(imagingFactory, filename, NativeFileAccess.Write))
            {
                using (var encoder = new wic.PngBitmapEncoder(imagingFactory))
                {
                    encoder.Initialize(stream);
                    using (var bitmapFrameEncode = new wic.BitmapFrameEncode(encoder))
                    {
                        var pixelSize = bitmap.PixelSize;
                        var dpi = bitmap.DotsPerInch;

                        bitmapFrameEncode.Initialize();
                        bitmapFrameEncode.SetSize(pixelSize.Width, pixelSize.Height);
                        bitmapFrameEncode.SetPixelFormat(ref wicPixelFormat);
                        using (var imageEncoder = new wic.ImageEncoder(imagingFactory, d2dDevice))
                        {
                            var imageParameters = new wic.ImageParameters(bitmap.PixelFormat,
                                dpi.Width, dpi.Height, 0, 0, pixelSize.Width, pixelSize.Height);
                            imageEncoder.WriteFrame(bitmap, bitmapFrameEncode, imageParameters);

                            bitmapFrameEncode.Commit();
                            encoder.Commit();
                        }
                    }
                }
            }
        }

    }
}
