using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private static string WritePriorityChunk(ITileMap tileMap, BinaryWriter w)
        {

            return "PRZ\x01";
        }
    }
}
