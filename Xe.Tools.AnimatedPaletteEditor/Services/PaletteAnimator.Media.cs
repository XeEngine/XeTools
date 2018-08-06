using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Xe.Graphics;

namespace Xe.Tools.Components.AnimatedPaletteEditor.Services
{
    public partial class PaletteAnimator
	{
		private const int ColorsCount = 256;

		private byte[] originalPalette;
		private byte[] palette;
		private byte[] backbuffer;

		public PaletteAnimator()
		{
			originalPalette = new byte[ColorsCount * 4];
			palette = new byte[ColorsCount * 4];
			backbuffer = new byte[ColorsCount * 4];

			for (int i = 0; i < ColorsCount; i++)
			{
				originalPalette[i * 4 + 0] = 0xFF;
				originalPalette[i * 4 + 1] = 0x00;
				originalPalette[i * 4 + 2] = 0xFF;
				originalPalette[i * 4 + 3] = 0xFF;
			}
			LoadPalette(originalPalette);
		}


		public void LoadPalette(byte[] data)
		{
			if (data.Length < palette.Length)
				throw new ArgumentException("The specified data is smaller than the palette itself.");

			Array.Copy(data, originalPalette, originalPalette.Length);
			Array.Copy(data, palette, palette.Length);
			FillBackbuffer();
		}

		public void LoadPalette(Color[] data)
		{
			int colorsCount = System.Math.Min(ColorsCount, data.Length);
			for (int i = 0; i < ColorsCount; i++)
			{
				palette[i * 4 + 0] = data[i].r;
				palette[i * 4 + 1] = data[i].g;
				palette[i * 4 + 2] = data[i].b;
				palette[i * 4 + 3] = data[i].a;
			}

			LoadPalette(palette);
		}

		public void LoadPalette(IList<System.Windows.Media.Color> color)
		{
			int colorsCount = System.Math.Min(ColorsCount, color.Count);
			for (int i = 0; i < colorsCount; i++)
			{
				palette[i * 4 + 0] = color[i].R;
				palette[i * 4 + 1] = color[i].G;
				palette[i * 4 + 2] = color[i].B;
				palette[i * 4 + 3] = color[i].A;
			}

			LoadPalette(palette);
		}

		public void LoadPalette(BitmapSource bitmap)
		{
			if (bitmap.Palette != null)
			{
				LoadPalette(bitmap.Palette.Colors);
			}
		}

		public void ResetPalette()
		{
			Array.Copy(originalPalette, palette, palette.Length);
		}

		public BitmapSource ApplyPalette(BitmapSource bitmap)
		{
			byte[] bmpData = new byte[bitmap.PixelWidth * bitmap.PixelHeight];
			bitmap.CopyPixels(bmpData, bitmap.PixelWidth, 0);

			int colorsCount = System.Math.Min(ColorsCount, bitmap.Palette.Colors.Count);
			var colors = new List<System.Windows.Media.Color>();
			for (int i = 0; i < colorsCount; i++)
			{
				colors.Add(new System.Windows.Media.Color()
				{
					R = palette[i * 4 + 0],
					G = palette[i * 4 + 1],
					B = palette[i * 4 + 2],
					A = palette[i * 4 + 3],
				});
			}

			return BitmapSource.Create(
				bitmap.PixelWidth, bitmap.PixelHeight,
				bitmap.DpiX, bitmap.DpiY,
				bitmap.Format, new BitmapPalette(colors),
				bmpData, bitmap.PixelWidth);
		}

		private void FillBackbuffer()
		{
			Array.Copy(palette, backbuffer, palette.Length);
		}
	}
}
