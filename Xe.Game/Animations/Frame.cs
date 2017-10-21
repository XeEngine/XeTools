using Newtonsoft.Json;
using static Xe.Math;

namespace Xe.Game.Animations
{
    public class Frame : IDeepCloneable
    {
        private ushort left, top, right, bottom;
        private short centerx, centery;
        
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
        public bool IsEmpty
        {
            get => (left == right) || (top == bottom);
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


        public override int GetHashCode() { return base.GetHashCode(); }

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
