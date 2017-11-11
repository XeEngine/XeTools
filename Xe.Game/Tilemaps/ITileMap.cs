using System.Collections.Generic;
using System.Drawing;

namespace Xe.Game.Tilemaps
{
    public interface ITileMap
    {
        Size Size { get; }

        Size TileSize { get; }

        Color BackgroundColor { get; }

        string BgmField { get; set; }

        string BgmBattle { get; set; }

        List<ITileset> Tilesets { get; }

        List<ILayerEntry> Layers { get; }

        void Save(string fileName);
    }
}
