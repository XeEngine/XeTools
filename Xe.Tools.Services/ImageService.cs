﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Xe.Tools.Services
{
    /// <summary>
    /// Manage opening, saving and operations on images
    /// </summary>
    public static class ImageService
    {
        public static BitmapFrame Open(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var decoder = new PngBitmapDecoder(fStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    return decoder.Frames.FirstOrDefault();
                }
            }
            else
            {
                Log.Warning($"Texture file {fileName} not found.");
            }
            return null;
        }

        public static void Save(this BitmapSource bitmap, string fileName)
        {
            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                var encoder = new PngBitmapEncoder
                {
                    Interlace = PngInterlaceOption.Off
                };
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fStream);
            }
        }

        public static void MakeTransparent(string fileOutput, string fileInput, Xe.Graphics.Color[] colors)
        {
            var bitmapImage = MakeTransparent(fileInput, colors);

            var encoder = new PngBitmapEncoder()
            {
                Interlace = PngInterlaceOption.Off,
            };
            var frame = BitmapFrame.Create(bitmapImage);
            encoder.Frames.Add(frame);

            using (var outFile = new FileStream(fileOutput, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(outFile);
            }
        }

        public static BitmapSource MakeTransparent(string fileName, Xe.Graphics.Color[] colors)
        {
            var bitmapImage = Open(fileName);
            return bitmapImage.MakeTransparent(colors);
        }

        public static BitmapSource MakeTransparent(this BitmapSource bitmap, Xe.Graphics.Color[] colors)
        {
            BitmapSource newBitmap = null;

            // patch for 24-bit bitmaps
            if (bitmap.Format == PixelFormats.Bgr32)
            {
                var formatter = new FormatConvertedBitmap();
                formatter.BeginInit();
                formatter.Source = bitmap;
                formatter.DestinationFormat = PixelFormats.Bgra32;
                formatter.EndInit();
                bitmap = formatter;
            }

            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var stride = bytesPerPixel * bitmap.PixelWidth;
            var length = stride * bitmap.PixelHeight;

            var processPixels = true;
            var palette = bitmap.Palette;
            if (bitmap.Format == PixelFormats.Indexed1 ||
                bitmap.Format == PixelFormats.Indexed2 ||
                bitmap.Format == PixelFormats.Indexed4 ||
                bitmap.Format == PixelFormats.Indexed8)
            {
                var srcColors = bitmap.Palette.Colors.ToList();
                for (int i = 0; i < srcColors.Count; i++)
                {
                    var color = srcColors[i];
                    for (int j = 0; j < colors.Length; j++)
                    {
                        var c = colors[j];
                        if (color.R == c.r ||
                            color.G == c.g ||
                            color.B == c.b)
                            color.A = 0;
                    }
                    srcColors[i] = color;
                }
                palette = new BitmapPalette(srcColors);
                processPixels = false;
            }

            IntPtr data = Marshal.AllocHGlobal(length);
            try
            {
                var rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
                bitmap.CopyPixels(rect, data, length, stride);
                var pixelFormat = bitmap.Format;
                if (processPixels)
                {
                    MakeTransparent(data, stride, bitmap.PixelHeight, bitmap.Format, colors);
                }
                newBitmap = BitmapSource.Create(bitmap.PixelWidth, bitmap.PixelHeight,
                    bitmap.DpiX, bitmap.DpiY, pixelFormat, palette, data, length, stride);
            }
            finally
            {
                Marshal.FreeHGlobal(data);
            }
            return newBitmap;
        }
        public static PixelFormat MakeTransparent(IntPtr data, int stride, int height, PixelFormat pixelFormat, Xe.Graphics.Color[] colors)
        {
            if (pixelFormat == PixelFormats.Bgr32 ||
                pixelFormat == PixelFormats.Bgra32)
            {
                foreach (var color in colors)
                {
                    int to = color.b | (color.g << 8) | (color.r << 16);
                    int from = to | (0xFF << 24);
                    Xe.Tools.Utilities.BitmapUtility.MakeTransparent_Bpp32(data, stride, height, from, to);
                }
                return PixelFormats.Bgra32;
            }
            else
            {
                Log.Error($"Unsupported pixel format {pixelFormat}.");
                return pixelFormat;
            }
        }
    }
}
