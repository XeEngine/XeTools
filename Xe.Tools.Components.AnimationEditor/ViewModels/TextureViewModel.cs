﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game;
using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class TextureViewModel : BaseNotifyPropertyChanged
    {
        private bool _isLoaded = false;
        private BitmapSource _image;
        private string _strSize;
        private string _strFormat;

        public Texture Texture { get; private set; }

        public string FileName { get; private set; }

        public BitmapSource Image
        {
            get
            {
                RequireImage();
                return _image;
            }
            set
            {
                _image = value;
                if (_image != null)
                {
                    _strSize = $"{_image.PixelWidth}x{_image.PixelHeight}";
                    if (_image.Palette?.Colors.Count > 0)
                    {
                        _strFormat = $"{_image.Format.BitsPerPixel}bpp, palette {_image.Palette.Colors} colors";
                    }
                    else
                    {
                        _strFormat = $"{_image.Format.BitsPerPixel}bpp";
                    }
                }
                OnPropertyChanged(nameof(Image));
                OnPropertyChanged(nameof(StrSize));
                OnPropertyChanged(nameof(StrFormat));
            }
        }

        public string StrSize => _strSize;

        public string StrFormat => _strFormat;

        public TextureViewModel(Texture texture, string basePath)
        {
            Texture = texture;
            FileName = Path.Combine(basePath, texture.Name);
        }

        private void RequireImage()
        {
            if (!_isLoaded)
            {
                _isLoaded = true;
                Image = ImageService.Open(FileName);
            }
        }

        public override string ToString()
        {
            return Texture?.Name ?? base.ToString();
        }
    }
}
