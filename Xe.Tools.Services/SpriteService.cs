using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game.Animations;

namespace Xe.Tools.Services
{
    public class SpriteService
    {
        private class SpriteItem
        {
            public string Name { get; set; }

            public BitmapImage Sprite { get; set; }

            public int AreaSize { get; set; }

            public Recti Rectangle { get; set; }
        }

        public BitmapSource Texture { get; private set; }

        public List<Frame> Frames { get; }

        public SpriteService(BitmapSource texture, List<Frame> frames)
        {
            Texture = texture;
            Frames = frames;
        }

        public void ImportFrames(string[] fileNames, int padding)
        {
            var sprites = fileNames.Select(x =>
            {
                var item = new SpriteItem
                {
                    Name = Path.GetFileNameWithoutExtension(x),
                    Sprite = new BitmapImage(new Uri(x))
                };
                item.AreaSize = item.Sprite.PixelWidth * item.Sprite.PixelHeight;
                return item;
            })
            .OrderByDescending(x => x.AreaSize);

            int width = 64, height = 64;
            bool increaser = false;
            
            bool failed = true;
            do
            {
                failed = false;
                var packer = new ArevaloRectanglePacker(width, height);
                foreach (var item in sprites)
                {
                    Sizei size = new Sizei
                    {
                        Width = item.Sprite.PixelWidth + padding,
                        Height = item.Sprite.PixelHeight + padding
                    };

                    if (!packer.TryPack(size.Width, size.Height, out Pointi origin))
                    {
                        failed = true;
                        break;
                    }

                    item.Rectangle = Recti.FromSize(origin.X, origin.Y, size.Width, size.Height);
                }

                if (failed)
                {
                    if (increaser) height *= 2;
                    else width *= 2;
                    increaser = !increaser;
                }
            } while (failed);
            
            var drawingVisual = new DrawingVisual();
            var drawingContext = drawingVisual.RenderOpen();
            foreach (var sprite in sprites)
            {
                drawingContext.DrawImage(sprite.Sprite, new Rect()
                    {
                        X = sprite.Rectangle.X,
                        Y = sprite.Rectangle.Y,
                        Width = sprite.Rectangle.Width,
                        Height = sprite.Rectangle.Height
                    });
            }
            drawingContext.Close();

            var renderTargetBitmap = new RenderTargetBitmap(width, height, 96.0f, 96.0f, PixelFormats.Bgra32);
            renderTargetBitmap.Render(drawingVisual);
            Texture = renderTargetBitmap;

            var dicFrames = Frames.ToDictionary(x => x.Name, x => x);
            var newFramesList = new List<Frame>(Frames.Capacity);
            foreach (var sprite in sprites)
            {
                if (dicFrames.TryGetValue(sprite.Name, out var frame))
                {
                    frame.Left = sprite.Rectangle.Left;
                    frame.Top = sprite.Rectangle.Top;
                    frame.Right = sprite.Rectangle.Right;
                    frame.Bottom = sprite.Rectangle.Bottom;
                }
                else
                {
                    Frames.Add(new Frame()
                    {
                        Left = sprite.Rectangle.Left,
                        Top = sprite.Rectangle.Top,
                        Right = sprite.Rectangle.Right,
                        Bottom = sprite.Rectangle.Bottom,
                        CenterX = sprite.Rectangle.Width / 2,
                        CenterY = sprite.Rectangle.Height / 2
                    });
                }
                newFramesList.Add(frame);
            }

            Frames.Clear();
            Frames.AddRange(newFramesList);
        }

        public void ExportFrames(string outputDir, Func<string, string, bool> overwriteCallback)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                foreach (var frame in Frames)
                {
                    var outputFileName = Path.Combine(outputDir, $"{frame.Name}.png");

                    // overwrite confirmation
                    if (File.Exists(outputFileName) &&
                        overwriteCallback != null &&
                        overwriteCallback.Invoke(frame.Name, outputFileName) == false)
                    {
                        continue;
                    }

                    var sprite = new CroppedBitmap(Texture, new Int32Rect()
                    {
                        X = frame.Left,
                        Y = frame.Top,
                        Width = frame.Right - frame.Left,
                        Height = frame.Bottom - frame.Top
                    });

                    sprite.Save(outputFileName);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }
    }
}
