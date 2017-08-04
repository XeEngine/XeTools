using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Wpf
{
    [Flags]
    public enum Flip
    {
        None = 0,
        FlipHorizontal = 1,
        FlipVertical = 2,
        FlipBoth = FlipHorizontal | FlipVertical
    }

    public enum Filter
    {
        Nearest,
        Linear,
        Cubic
    }
}
