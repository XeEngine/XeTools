using Newtonsoft.Json;
using System.Drawing;
using Xe;
using static Xe.Math;

namespace libTools.Anim
{
    public class Frame : IDeepCloneable
    {
        private ushort left, top, right, bottom;
        private short centerx, centery;

        [JsonIgnore]
        public string Name;
        public int Left
        {
            get { return left; }
            set { left = (ushort)Range(value, ushort.MinValue, ushort.MaxValue); }
        }
        public int Top
        {
            get { return top; }
            set { top = (ushort)Range(value, ushort.MinValue, ushort.MaxValue); }
        }
        public int Right
        {
            get { return right; }
            set { right = (ushort)Range(value, ushort.MinValue, ushort.MaxValue); }
        }
        public int Bottom
        {
            get { return bottom; }
            set { bottom = (ushort)Range(value, ushort.MinValue, ushort.MaxValue); }
        }
        public int CenterX
        {
            get { return centerx; }
            set { centerx = (short)Range(value, short.MinValue, short.MaxValue); }
        }
        public int CenterY
        {
            get { return centery; }
            set { centery = (short)Range(value, short.MinValue, short.MaxValue); }
        }
        
        [JsonIgnore]
        public Rectangle Rectangle
        {
            get { return new Rectangle(left, top, right - left, bottom - top); }
            set
            {
                Left = value.Left;
                Top = value.Top;
                Right = value.Right;
                Bottom = value.Bottom;
            }
        }
        [JsonIgnore]
        public Point Center
        {
            get { return new Point(centerx, centery); }
        }
        [JsonIgnore]
        public Size Size
        {
            get { return Rectangle.Size; }
        }

        public override int GetHashCode()
        {
            return unchecked((int)((uint)centerx ^
                (((uint)centery << 9) | ((uint)centery >> 4)) ^
                (((uint)left << 13) | ((uint)left >> 6)) ^
                (((uint)top << 17) | ((uint)top >> 8)) ^
                (((uint)right << 21) | ((uint)right >> 10)) ^
                (((uint)bottom << 25) | ((uint)bottom >> 9))));
        }

        public object DeepClone()
        {
            var item = new Frame()
            {
                Name = Name,
                left = left,
                top = top,
                right = right,
                bottom = bottom,
                centerx = centerx,
                centery = centery
            };
            return item;
        }

        public override string ToString() { return Name; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return this == obj as Frame;
        }
        public static bool operator ==(Frame x, Frame y)
        {
            if (x as object == y as object) return true;
            if (x as object == null || y as object == null) return false;
            return x.left == y.left &&
                x.top == y.top &&
                x.right == y.right &&
                x.bottom == y.bottom &&
                x.centerx == y.centerx &&
                x.centery == y.centery;
        }
        public static bool operator !=(Frame x, Frame y)
        {
            return !(x == y);
        }
    }
}
