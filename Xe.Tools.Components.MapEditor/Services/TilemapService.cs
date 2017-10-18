using Xe.Game.Tilemaps;
using Xe.Tools.Tilemap;

namespace Xe.Tools.Components.MapEditor.Services
{
    public static class TilemapService
    {
        public static ITileMap Open(string fileName)
        {
            return new TilemapTiled(fileName);
        }
    }
}
