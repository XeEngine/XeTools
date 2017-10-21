using System;

namespace Xe.Game.Kernel
{
    [Flags]
    public enum TargetType
    {
        None = 0,
        Self = 1 << 0,
        Opponent = 1 << 1,
        Npc = 1 << 2,
        Entity = 1 << 3,
    }

    public enum DamageFormula
    {
        None = 0
    }
}
