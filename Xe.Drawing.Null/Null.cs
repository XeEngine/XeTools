using System.Drawing;

namespace Xe.Drawing
{
    public partial class DrawingNull : Drawing
    {
        private Filter _filter = Filter.Nearest;

        public override ISurface Surface { get => null; set { } }
        public override Filter Filter
        {
            get => _filter;
            set => _filter = value;
        }

        public override void Clear(Color color)
        {
        }

        public override void Dispose()
        {
        }

        public override void DrawRectangle(RectangleF rect, Color color, float width = 1)
        {
        }

        public override void DrawSurface(ISurface surface, Rectangle src, Rectangle dst, Flip flip)
        {
        }
    }
}
