using Xe.Game.Tilemaps;

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
