using SharpDX.Mathematics.Interop;
using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingDirectX : Drawing
    {
        private CSurface _surface;
        private bool _invalidated;
        private Filter _filter;
        private SharpDX.Direct2D1.InterpolationMode _interpolationMode;

        public override ISurface Surface
        {
            get
            {
                if (_invalidated)
                {
                    _invalidated = false;
                    d2dContext.Flush();
                    d2dContext.EndDraw();
                }
                return _surface;
            }
        }

        public override Filter Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                switch (value)
                {
                    case Filter.Nearest:
                        _interpolationMode = SharpDX.Direct2D1.InterpolationMode.NearestNeighbor;
                        break;
                    case Filter.Linear:
                        _interpolationMode = SharpDX.Direct2D1.InterpolationMode.Linear;
                        break;
                    case Filter.Cubic:
                        _interpolationMode = SharpDX.Direct2D1.InterpolationMode.Cubic;
                        break;
                }
            }
        }

        public override void Clear(Color color)
        {
            var r = color.R / 255.0f;
            var g = color.G / 255.0f;
            var b = color.B / 255.0f;
            var a = color.A / 255.0f;
            Invalidate();
            d2dContext.Clear(new RawColor4(r, g, b, a));
        }

        public override void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
        {
            var s = surface as CSurface;
            var srcf = new RawRectangleF(src.Left, src.Top, src.Right, src.Bottom);
            var dstf = new RawRectangleF(dst.Left, dst.Top, dst.Right, dst.Bottom);
            float tmp;
            switch (flip)
            {
                case Flip.FlipHorizontal:
                    tmp = srcf.Left;
                    srcf.Left = srcf.Right;
                    srcf.Right = tmp;
                    break;
                case Flip.FlipVertical:
                    tmp = srcf.Top;
                    srcf.Top = srcf.Bottom;
                    srcf.Bottom = tmp;
                    break;
                case Flip.FlipBoth:
                    tmp = srcf.Left;
                    srcf.Left = srcf.Right;
                    srcf.Right = tmp;
                    tmp = srcf.Top;
                    srcf.Top = srcf.Bottom;
                    srcf.Bottom = tmp;
                    break;
            }
            Invalidate();
            d2dContext.DrawBitmap(s.Bitmap, dstf, 1.0f, _interpolationMode, srcf, null);
        }

        public override void Dispose()
        {
            _surface?.Dispose();
            d2dContext?.Dispose();
        }

        private void Invalidate()
        {
            if (!_invalidated)
            {
                _invalidated = true;
                d2dContext.BeginDraw();
            }
        }

        private DrawingDirectX(int width, int height)
        {
            CommonInit();
            ResizeRenderTarget(width, height);
        }
        private DrawingDirectX(ISurface surface)
        {
            CommonInit();
        }

        private void CommonInit()
        {
            Filter = Filter.Nearest;
            Initialize();
        }

        public static DrawingDirectX Factory(int width, int height, PixelFormat pixelFormat)
        {
            return new DrawingDirectX(width, height);
        }
        public static DrawingDirectX Factory(ISurface surface)
        {
            return new DrawingDirectX(surface);
        }
    }
}
