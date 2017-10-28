using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Drawing;
using Xe.Game.Animations;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor
{
    namespace Utility
    {
        public static class Extensions
        {
            public static void DrawAnimation(this IDrawing drawing, FramesGroup framesGroup, double x, double y)
            {
                var frame = framesGroup.Frames.FirstOrDefault();
                if (frame != null)
                {
                    var src = frame.Source;
                    drawing.DrawSurface(framesGroup.Texture,
                        new System.Drawing.Rectangle((int)src.Left, (int)src.Top, (int)src.Width, (int)src.Height),
                        new System.Drawing.Rectangle((int)x - (int)frame.Pivot.X, (int)y - (int)frame.Pivot.Y, (int)src.Width, (int)src.Height),
                        Drawing.Flip.None);
                }
            }

        }
    }
}
