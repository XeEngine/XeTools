using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class TextureViewModel
    {
        public Texture Texture { get; private set; }

        public BitmapSource Image { get; private set; }

        public string StrSize { get; private set; }

        public string StrFormat { get; private set; }

        public TextureViewModel(Texture texture, string basePath)
        {
            Texture = texture;

            var uri = new Uri(Path.Combine(basePath, texture.Name), UriKind.RelativeOrAbsolute);
            Image = new BitmapImage(uri);
            StrSize = $"{Image.PixelWidth}x{Image.PixelHeight}";
            StrFormat = Image.Format.ToString();
            if (Image.Palette.Colors.Count > 0)
            {
                StrFormat = $"{Image.Format.BitsPerPixel}bpp, palette {Image.Palette.Colors} colors";
            }
            else
            {
                StrFormat = $"{Image.Format.BitsPerPixel}bpp";
            }
        }
    }
}
