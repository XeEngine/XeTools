using SharpDX.WIC;

namespace Xe.Tools.Wpf
{
    public static class ExtensionsWIC
    {
        public static BitmapSource LoadBitmapSource(this ImagingFactory2 factory, string fileName)
        {
            var bitmapDecoder = new BitmapDecoder(factory,
                fileName, DecodeOptions.CacheOnDemand);

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0), PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);

            return formatConverter;
        }
    }
}
