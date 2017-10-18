using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game.Animations;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor
{
    public enum Direction { Unspecified, Up, Right, Down, Left }

    namespace Utility
    {
        public static class Extensions
        {
            public static void DrawAnimation(this DrawingContext dc, FramesGroup framesGroup, double x, double y)
            {
                var frame = framesGroup.Frames.SingleOrDefault();
                if (frame != null)
                {
                    var src = frame.Source;
                    var dstRect = new Rect(x/* - frame.Pivot.X*/, y/* - frame.Pivot.Y*/, src.Width, src.Height);
                    var sprite = new CroppedBitmap(framesGroup.Texture,
                        new Int32Rect()
                        {
                            X = (int)src.Left,
                            Y = (int)src.Top,
                            Width = (int)src.Width,
                            Height = (int)src.Height
                        });
                    dc.DrawImage(sprite, dstRect);
                }
            }
        }
    }
}
