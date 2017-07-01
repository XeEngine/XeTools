using static Xe.Math;

namespace Xe.Game.Animations
{
    public class HitboxEx : Hitbox
    {
        private ushort type;

        public int Type
        {
            get => type;
            set => type = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }
    }
}
