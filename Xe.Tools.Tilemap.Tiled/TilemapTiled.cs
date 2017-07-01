using TiledSharp;

namespace Xe.Tools.Tilemap
{
    public partial class TilemapTiled : ITileMap
    {
        public TilemapTiled(string filename) :
            this(new TmxMap(filename))
        { }
    }
}
