using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;
using System.Windows;

namespace Xe.Tools.Wpf
{
    public static class ExtensionsDirect2D1
    {
        #region drawing

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            int x, int y, Flip flip = Flip.None)
        {
            float width = bitmap.Size.Width;
            float height = bitmap.Size.Height;
            var src = new RawRectangleF(0, 0, width, height);
            var dst = new RawRectangleF(x, y, width, height);
            DrawBitmap(context, bitmap, src, dst, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            int x, int y, int width, int height, Flip flip = Flip.None)
        {
            var src = new RawRectangleF(0, 0, bitmap.Size.Width, bitmap.Size.Height);
            var dst = new RawRectangleF(x, y, width, height);
            DrawBitmap(context, bitmap, src, dst, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            Rect dst, Flip flip = Flip.None)
        {
            var src = new RawRectangleF(0, 0, bitmap.Size.Width, bitmap.Size.Height);
            DrawBitmap(context, bitmap, src, dst, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            Rect src, int x, int y, Flip flip = Flip.None)
        {
            var dst = new RawRectangleF(x, y, (float)src.Width, (float)src.Height);
            DrawBitmap(context, bitmap, src, dst, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            RawRectangleF src, int x, int y, int width, int height, Flip flip = Flip.None)
        {
            var dst = new RawRectangleF(x, y, width, height);
            DrawBitmap(context, bitmap, src, dst, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            Rect src, Rect dst, Flip flip = Flip.None)
        {
            var srcf = new RawRectangleF((float)src.Left, (float)src.Top,
                (float)src.Right, (float)src.Bottom);
            var dstf = new RawRectangleF((float)dst.Left, (float)dst.Top,
                (float)dst.Right, (float)dst.Bottom);
            DrawBitmap(context, bitmap, srcf, dstf, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            RawRectangleF srcf, Rect dst, Flip flip = Flip.None)
        {
            var dstf = new RawRectangleF((float)dst.Left, (float)dst.Top,
                (float)dst.Right, (float)dst.Bottom);
            DrawBitmap(context, bitmap, srcf, dstf, flip);
        }

        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            Rect src, RawRectangleF dstf, Flip flip = Flip.None)
        {
            var srcf = new RawRectangleF((float)src.Left, (float)src.Top,
                (float)src.Right, (float)src.Bottom);
            DrawBitmap(context, bitmap, srcf, dstf, flip);
        }
        
        public static void DrawBitmap(this RenderTarget context, Bitmap bitmap,
            RawRectangleF srcf, RawRectangleF dstf, Flip flip = Flip.None)
        {
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
            context.DrawBitmap(bitmap, dstf, 1.0f, BitmapInterpolationMode.NearestNeighbor, srcf);
        }

        #endregion

        #region texture

        public static Bitmap1 LoadBitmap2D(this DeviceContext context, SharpDX.WIC.ImagingFactory2 factory, string fileName)
        {
            var bitmapSource = factory.LoadBitmapSource(fileName);
            return Bitmap1.FromWicBitmap(context, bitmapSource);
        }
        public static Bitmap1 LoadBitmap2D(this DeviceContext context, SharpDX.WIC.BitmapSource bitmapSource)
        {
            return Bitmap1.FromWicBitmap(context, bitmapSource, new BitmapProperties1()
            {
                BitmapOptions = BitmapOptions.Target | BitmapOptions.CpuRead | BitmapOptions.GdiCompatible
            });
        }

        #endregion
    }
}
