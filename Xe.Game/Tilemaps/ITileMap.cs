using System.Collections.Generic;
using System.Drawing;

namespace Xe.Tools.Tilemap
{
    public interface ITileMap
    {
        Size Size { get; }

        Size TileSize { get; }

        List<ITileset> Tilesets { get; }

        List<ILayer> Layers { get; }
    }
}
