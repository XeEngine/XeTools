using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Xe.Tools.Services
{
    public class Color
    {
        public byte r, g, b, a;
    }
    public static class ImageService
    {
        public static void MakeTransparent(string fileOutput, string fileInput, Color[] colors)
        {
            var bitmapImage = MakeTransparent(fileInput, colors);

            var encoder = new PngBitmapEncoder()
            {
                Interlace = PngInterlaceOption.Off,
            };
            var frame = BitmapFrame.Create(bitmapImage);
            encoder.Frames.Add(frame);
            if (bitmapImage.Palette != null)
            {
                encoder.Palette = bitmapImage.Palette;
            }

            using (var outFile = new FileStream(fileOutput, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(outFile);
            }
        }

        public static BitmapSource MakeTransparent(string fileName, Color[] colors)
        {
            var bitmapImage = new BitmapImage(new System.Uri(fileName));
            return bitmapImage.MakeTransparent(colors);
        }

        public static BitmapSource MakeTransparent(this BitmapSource bitmap, Color[] colors)
        {
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var stride = bytesPerPixel * bitmap.PixelWidth;
            var length = stride * bitmap.PixelHeight;
            IntPtr data = Marshal.AllocHGlobal(length);
            var rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            bitmap.CopyPixels(rect, data, length, stride);
            MakeTransparent(data, stride, bitmap.PixelHeight, bitmap.Format, colors);
            var newBitmap = BitmapSource.Create(bitmap.PixelWidth, bitmap.PixelHeight,
                bitmap.DpiX, bitmap.DpiY, bitmap.Format, bitmap.Palette, data, length, stride);
            Marshal.FreeHGlobal(data);
            return newBitmap;
        }
        public static void MakeTransparent(IntPtr data, int stride, int height, PixelFormat pixelFormat, Color[] colors)
        {
            if (pixelFormat == PixelFormats.Bgra32)
            {
                foreach (var color in colors)
                {
                    int to = color.b | (color.g << 8) | (color.r << 16);
                    int from = to | (0xFF << 24);
                    MakeTransparent_Bpp32(data, stride, height, from, to);
                }
            }
            else
            {
                Log.Error($"Unsupported pixel format {pixelFormat}.");
            }
        }

        private static unsafe void MakeTransparent_Bpp32(IntPtr data, int stride, int height, int from, int to)
        {
            for (int i = 0; i < height; i++)
            {
                int* p = (int*)(data + i * stride);
                for (int j = 0; j < stride; j += 4, p++)
                {
                    if (*p == from)
                        *p = to;
                }
            }
        }
    }
}
