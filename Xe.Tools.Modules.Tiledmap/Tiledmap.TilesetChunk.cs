using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Security;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private string WriteTilesetChunk(Map tileMap, BinaryWriter w)
        {
            w.WriteStringData(GetTilesetFileName());
            w.Align(8);
            return "TSX\x01";
        }
    }
}
