using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private static string WriteCollisionChunk(Map tileMap, BinaryWriter w)
        {
            return "COL\x01";
        }
    }
}
