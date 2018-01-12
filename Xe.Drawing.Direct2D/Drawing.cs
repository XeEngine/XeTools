using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Drawing;
using System.Drawing.Imaging;

namespace Xe.Drawing
{
    public partial class DrawingDirect2D : Drawing
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
            set
            {
                var oldSurface = Surface;
                if (value is CSurface surface)
                {
                    _surface = surface;
                    d2dContext.Target = surface.Bitmap;
                }
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

        public override void DrawRectangle(RectangleF rect, Color color, float width)
        {
            using (var brush = new SolidColorBrush(d2dContext, ToRaw(color)))
            {
                d2dContext.DrawRectangle(ToRaw(rect), brush, width);
            }
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, Flip flip)
		{
			DrawSurface(surface, src, dst, 1.0f, flip);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, float alpha, Flip flip)
		{
			var s = surface as CSurface;
			var srcf = new RawRectangleF(src.Left, src.Top, src.Right, src.Bottom);
			var dstf = new RawRectangleF(dst.Left, dst.Top, dst.Right, dst.Bottom);
			RawMatrix? matrix;
			switch (flip)
			{
				case Flip.FlipHorizontal:
					matrix = new RawMatrix()
					{
						M11 = -1,
						M22 = +1,
						M33 = +1,
						M44 = +1,

						M41 = dstf.Left * 2 + src.Width,
					};
					break;
				case Flip.FlipVertical:
					matrix = new RawMatrix()
					{
						M11 = +1,
						M22 = -1,
						M33 = +1,
						M44 = +1,

						M42 = dstf.Top * 2 + src.Height,
					};
					break;
				case Flip.FlipBoth:
					matrix = new RawMatrix()
					{
						M11 = -1,
						M22 = -1,
						M33 = +1,
						M44 = +1,

						M41 = dstf.Left * 2 + src.Width,
						M42 = dstf.Top * 2 + src.Height,
					};
					break;
				default:
					matrix = null;
					break;
			}
			Invalidate();
			d2dContext.DrawBitmap(s.Bitmap, dstf, alpha, _interpolationMode, srcf, matrix);
		}

		public override void DrawSurface(ISurface surface, Rectangle src, RectangleF dst, ColorF color, Flip flip = Flip.None)
		{
			DrawSurface(surface, src, dst, color.A, flip);
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

        public DrawingDirect2D()
        {
            CommonInit();
        }

        private void CommonInit()
        {
            Filter = Filter.Nearest;
            Initialize();
        }

        private static RawRectangle ToRaw(Rectangle rect)
        {
            return new RawRectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private static RawRectangleF ToRaw(RectangleF rect)
        {
            return new RawRectangleF(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private static RawColor4 ToRaw(Color color)
        {
            return new RawColor4(color.R / 255.0f, color.G / 255.0f,
                color.B / 255.0f, color.A / 255.0f);
        }
    }
}
