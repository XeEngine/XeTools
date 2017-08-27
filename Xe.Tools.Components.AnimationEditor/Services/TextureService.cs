using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Services;

namespace Xe.Tools.Components.AnimationEditor.Services
{
    public class SpriteService
    {
        public static SpriteService Instance { get; private set; } = new SpriteService(".");

        public string BasePath { get; set; }

        private Dictionary<Guid, BitmapSource> _textures =
            new Dictionary<Guid, BitmapSource>(16);
        private Dictionary<Tuple<Guid, string>, BitmapSource> _frames =
            new Dictionary<Tuple<Guid, string>, BitmapSource>(1024);

        public BitmapSource this[Texture texture]
        {
            get
            {
                if (texture == null) return null;
                if (_textures.TryGetValue(texture.Id, out BitmapSource bitmap))
                    return bitmap;

                var fileName = Path.Combine(BasePath, texture.Name);
                return _textures[texture.Id] = ImageService.Open(fileName);
            }
        }

        public BitmapSource this[Texture texture, Frame frame]
        {
            get
            {
                if (texture == null) return null;
                if (frame == null) return null;
                var tuple = new Tuple<Guid, string>(texture.Id, frame.Name);
                if (_frames.TryGetValue(tuple, out BitmapSource bitmap))
                    return bitmap;
                
                if (!frame.IsEmpty)
                {
                    var textureBitmap = this[texture];
                    try
                    {
                        bitmap = new CroppedBitmap(textureBitmap,
                        new System.Windows.Int32Rect()
                        {
                            X = frame.Left,
                            Y = frame.Top,
                            Width = frame.Right - frame.Left,
                            Height = frame.Bottom - frame.Top
                        });
                    }
                    catch (ArgumentException)
                    {
                    }
                }
                else
                {
                    bitmap = BitmapSource.Create(1, 1, 96, 96, PixelFormats.Bgr24, null, new byte[3] { 0, 0, 0 }, 3);
                }
                return _frames[tuple] = bitmap;
            }
        }

        private SpriteService(string basePath)
        {
            BasePath = basePath;
        }

        public void Invalidate(Texture texture, Frame frame)
        {
            _frames.Remove(new Tuple<Guid, string>(texture.Id, frame.Name));
        }

        public void InvalidateAll()
        {
            _textures.Clear();
            _frames.Clear();
        }
    }
}
