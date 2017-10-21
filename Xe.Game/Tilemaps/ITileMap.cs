using System.Collections.Generic;
using System.Drawing;

namespace Xe.Game.Tilemaps
{
    public interface ITileMap
    {
        Size Size { get; }

        Size TileSize { get; }

        List<ITileset> Tilesets { get; }

        List<ILayerEntry> Layers { get; }
    }
}
