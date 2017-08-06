using System.Drawing;
using Newtonsoft.Json;
using static Xe.Math;

namespace Xe.Game.Animations
{
    public class Hitbox : IDeepCloneable
    {
        private short left, top, right, bottom;

        public int Left
        {
            get => left;
            set => left = (short)Range(value, short.MinValue, short.MaxValue);
        }
        public int Top
        {
            get => top;
            set => top = (short)Range(value, short.MinValue, short.MaxValue);
        }
        public int Right
        {
            get => right;
            set => right = (short)Range(value, short.MinValue, short.MaxValue);
        }
        public int Bottom
        {
            get => bottom;
            set => bottom = (short)Range(value, ushort.MinValue, short.MaxValue);
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
            return unchecked((int)(
                (((uint)left << 21) | ((uint)left >> 11)) ^
                (((uint)top << 23) | ((uint)top >> 5)) ^
                (((uint)right << 15) | ((uint)right >> 9)) ^
                (((uint)bottom << 27) | ((uint)bottom >> 0))));
        }

        virtual public object DeepClone()
        {
            return new Hitbox()
            {
                left = left,
                top = top,
                right = right,
                bottom = bottom
            };
        }

        public override string ToString()
        {
            return $"({left}, {top}, {right}, {bottom})";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return this == obj as Hitbox;
        }

        public static bool operator ==(Hitbox x, Hitbox y)
        {
            if (x as object == y as object) return true;
            if (x as object == null || y as object == null) return false;
            return x.left == y.left &&
                x.top == y.top &&
                x.right == y.right &&
                x.bottom == y.bottom;
        }

        public static bool operator !=(Hitbox x, Hitbox y)
        {
            return !(x == y);
        }
    }
}
