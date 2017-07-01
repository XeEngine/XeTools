using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Xe.Drawing.GdiPlus
{
    internal static class BitmapExtensions
    {
        internal static BitmapData LockBits(this Bitmap bitmap, ImageLockMode lockMode)
        {
            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            return bitmap.LockBits(rect, lockMode, bitmap.PixelFormat);
        }
    }
}
