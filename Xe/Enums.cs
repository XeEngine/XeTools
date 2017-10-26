using System;

namespace Xe
{
    [Flags]
    public enum Flip
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2,
        FlipBoth = FlipHorizontal | FlipVertical
    }
}
