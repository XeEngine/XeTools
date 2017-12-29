using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingGdiPlus : Drawing
    {
        private Graphics _graphics;
        private bool _invalidated;
        private CSurface _surface;
        private Filter _filter;

        public override ISurface Surface
        {
            get
            {
                if (_invalidated)
                {
                    _invalidated = false;
                    _graphics.Flush();
                }
                return _surface;
            }
            set
            {
                if (_surface != value)
                {
                    _surface?.Dispose();
                    _graphics?.Flush();
                    if (value is CSurface surface)
                    {
                        _surface = surface;
                        _graphics = Graphics.FromImage(_surface.Bitmap);
                    }
                    else
                    {
                        _surface = null;
                        _graphics = null;
                    }
                }
            }
        }

        public override Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                InterpolationMode interpolationMode;
                switch (value)
                {
                    case Filter.Nearest:
                        interpolationMode = InterpolationMode.NearestNeighbor;
                        break;
                    case Filter.Linear:
                        interpolationMode = InterpolationMode.Bilinear;
                        break;
                    case Filter.Cubic:
                        interpolationMode = InterpolationMode.Bicubic;
                        break;
                    default:
                        interpolationMode = InterpolationMode.Invalid;
                        break;
                }
                _graphics.InterpolationMode = interpolationMode;
            }
        }

        public override ISurface CreateSurface(int width, int height, PixelFormat pixelFormat, SurfaceType type)
        {
            using (var bitmap = new Bitmap(width, height, GetPixelFormat(pixelFormat)))
            {
                return new CSurface(bitmap)
                {
                    PixelFormat = pixelFormat
                };
            }
        }

        public override void Clear(Color color)
        {
            _graphics.Clear(color);
        }

        public override void DrawRectangle(RectangleF rect, Color color, float width = 1.0f)
        {
            using (var brush = new SolidBrush(color))
            {
                using (var pen = new Pen(brush, width))
                {
                    _graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public override void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
        {
            var mySurface = surface as CSurface;
            if (mySurface == null)
                throw new ArgumentException("Invalid surface specified", nameof(surface));
            var bitmap = mySurface.Bitmap;
            switch (flip)
            {
                case Flip.None:
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    break;
                case Flip.FlipHorizontal:
                    _graphics.TranslateTransform(dst.Width, 0);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, 1);
                    _graphics.TranslateTransform(-dst.Width, 0);
                    break;
                case Flip.FlipVertical:
                    _graphics.TranslateTransform(0, dst.Height);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(1, -1);
                    _graphics.TranslateTransform(0, -dst.Height);
                    break;
                case Flip.FlipBoth:
                    _graphics.TranslateTransform(dst.Width, dst.Height);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.DrawImage(bitmap, dst, src, GraphicsUnit.Pixel);
                    _graphics.ScaleTransform(-1, -1);
                    _graphics.TranslateTransform(-dst.Width, -dst.Height);
                    break;
            }
            Invalidate();
        }

        public override void Dispose()
        {
            _surface.Dispose();
            _graphics.Dispose();
        }

        /*private DrawingGdiPlus(int width, int height, PixelFormat pixelFormat)
        {
            using (var bitmap = new Bitmap(width, height, GetPixelFormat(pixelFormat)))
            {
                _surface = CreateSurface(bitmap) as CSurface;
                _surface.PixelFormat = pixelFormat;
                _graphics = Graphics.FromImage(_surface.Bitmap);
            }
        }
        private DrawingGdiPlus(CSurface surface)
        {
            _surface = surface;
            _graphics = Graphics.FromImage(_surface.Bitmap);
        }*/

        private void Invalidate()
        {
            _invalidated = true;
        }

        private static System.Drawing.Imaging.PixelFormat GetPixelFormat(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format16bppRgb555: return System.Drawing.Imaging.PixelFormat.Format16bppRgb555;
                case PixelFormat.Format16bppRgb565: return System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
                case PixelFormat.Format24bppRgb: return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                case PixelFormat.Format32bppRgb: return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
                case PixelFormat.Format1bppIndexed: return System.Drawing.Imaging.PixelFormat.Format1bppIndexed;
                case PixelFormat.Format4bppIndexed: return System.Drawing.Imaging.PixelFormat.Format4bppIndexed;
                case PixelFormat.Format8bppIndexed: return System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                case PixelFormat.Format16bppArgb1555: return System.Drawing.Imaging.PixelFormat.Format16bppArgb1555;
                case PixelFormat.Format32bppPArgb: return System.Drawing.Imaging.PixelFormat.Format32bppPArgb;
                case PixelFormat.Format16bppGrayScale: return System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
                case PixelFormat.Format48bppRgb: return System.Drawing.Imaging.PixelFormat.Format48bppRgb;
                case PixelFormat.Format64bppPArgb: return System.Drawing.Imaging.PixelFormat.Format64bppPArgb;
                case PixelFormat.Format32bppArgb: return System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                case PixelFormat.Format64bppArgb: return System.Drawing.Imaging.PixelFormat.Format64bppArgb;
                default: return System.Drawing.Imaging.PixelFormat.Undefined;
            }
        }
    }
}
