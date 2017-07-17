using TiledSharp;
using Xe.Tools.Tilemap;

namespace Xe.Game.Tilemaps
{
    public partial class TilemapTiled : ITileMap
    {
        public TilemapTiled(string filename) :
            this(new TmxMap(filename))
        { }
    }
}
