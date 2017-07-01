using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;

namespace Xe.Tools.Tilemap
{
    public interface ITileset
    {
        string ImagePath { get; }

        int TileWidth { get; }

        int TileHeight { get; }

        int TilesPerRow { get; }

        int TilesCount { get; }
    }
}
