using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private static string WritePriorityChunk(Map tileMap, BinaryWriter w)
        {

            return "PRZ\x01";
        }
    }
}
