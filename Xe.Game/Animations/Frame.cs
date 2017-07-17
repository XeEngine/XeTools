using Newtonsoft.Json;
using System.Drawing;
using static Xe.Math;

namespace Xe.Game.Animations
{
    public class Frame : IDeepCloneable
    {
        private ushort left, top, right, bottom;
        private short centerx, centery;

        [JsonIgnore]
        public string Name { get; set; }

        public int Left
        {
            get => left;
            set => left = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }
        public int Top
        {
            get => top;
            set => top = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }
        public int Right
        {
            get => right;
            set => right = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }
        public int Bottom
        {
            get => bottom;
            set => bottom = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }
        public int CenterX
        {
            get => centerx;
            set => centerx = (short)Range(value, short.MinValue, short.MaxValue);
        }
        public int CenterY
        {
            get => centery;
            set => centery = (short)Range(value, short.MinValue, short.MaxValue);
        }

        [JsonIgnore]
        public Rectangle Rectangle
        {
            get => new Rectangle(left, top, right - left, bottom - top);
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
            get => new Point(centerx, centery);
        }

        [JsonIgnore]
        public Size Size
        {
            get => Rectangle.Size;
        }

        [JsonIgnore]
        public bool IsEmpty
        {
            get => (left == top) || (right == bottom);
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
        
        virtual public object DeepClone()
        {
            return new Frame()
            {
                Name = Name,
                left = left,
                top = top,
                right = right,
                bottom = bottom,
                centerx = centerx,
                centery = centery
            };
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
