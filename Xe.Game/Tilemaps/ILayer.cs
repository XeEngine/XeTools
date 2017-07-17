using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Tilemap
{
    public interface ILayer
    {
        string Name { get; }

        bool Visible { get; }

        int Width { get; }

        int Height { get; }

        int Index { get; }

        ITile GetTile(int x, int y);
    }
}
