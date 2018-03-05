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
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Services;

namespace Xe.Tools.Components.MapEditor
{
    namespace Utility
    {
        public static class Extensions
		{
			public static void DrawObjectEntryRect(this IDrawing drawing, ObjectEntry objectEntry)
			{
				DrawObjectEntryRect(drawing, objectEntry,
					System.Drawing.Color.Cyan, 0.0f);
				DrawObjectEntryRect(drawing, objectEntry,
					System.Drawing.Color.Fuchsia, -objectEntry.Z);
			}
			public static void DrawObjectEntryRect(this IDrawing drawing, ObjectEntry objectEntry,
				System.Drawing.Color color, double yMod)
			{
				drawing.DrawRectangle(new System.Drawing.RectangleF()
				{
					X = (float)objectEntry.X,
					Y = (float)(objectEntry.Y + yMod),
					Width = (float)objectEntry.Width,
					Height = (float)objectEntry.Height
				}, color, 2.0f);
			}

			public static void DrawAnimation(this IDrawing drawing, FramesGroup framesGroup, double x, double y,
                float alpha = 1.0f, Drawing.Flip flip = Drawing.Flip.None)
            {
                var frame = framesGroup.Frames.FirstOrDefault();
                if (frame != null)
                {
                    var src = frame.Source;

                    flip = flip.Add(Drawing.Flip.FlipHorizontal, framesGroup.Reference.FlipX)
                        .Add(Drawing.Flip.FlipVertical, framesGroup.Reference.FlipY);

                    var nx = x - frame.Pivot.X;
                    var ny = y - frame.Pivot.Y;
                    if (flip.HasFlag(Drawing.Flip.FlipHorizontal))
                        nx += frame.Pivot.X * 2 - src.Width;
                    if (flip.HasFlag(Drawing.Flip.FlipVertical))
                        ny -= frame.Pivot.Y * 2 - src.Height;

                    drawing.DrawSurface(framesGroup.Texture,
                        new System.Drawing.Rectangle((int)src.Left, (int)src.Top, (int)src.Width, (int)src.Height),
                        new System.Drawing.Rectangle((int)nx, (int)ny, (int)src.Width, (int)src.Height), alpha, flip);
                }
            }

            public static Drawing.Flip Add(this Drawing.Flip x, Drawing.Flip y)
            {
                return (Drawing.Flip)((int)x ^ (int)y);
            }

            public static Drawing.Flip Add(this Drawing.Flip x, Drawing.Flip y, bool reverse)
            {
                return reverse ? (Drawing.Flip)((int)x ^ (int)y) : x;
            }
        }
    }
}
