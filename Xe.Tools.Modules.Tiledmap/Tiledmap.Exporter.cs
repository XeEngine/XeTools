using System;
using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter w)
        {
            const uint MagicCode = 0x0250414DU;
            const int TileSizeMin = 4;
            const int TileSizeMax = 256;

            int tileWidth = _tiledmap.TileSize.Width;
            int tileHeight = _tiledmap.TileSize.Height;
            TileSizeException.Assert(tileWidth, tileHeight, TileSizeMin, TileSizeMax);

            #region Header
            w.Write(MagicCode);
            w.Write((byte)tileWidth);
            w.Write((byte)tileHeight);
            w.Write((byte)0); // RESERVED (orthogonal, isometric, etc.)
            w.Write((byte)0); // RESERVED
			#endregion
			#region Metadata
			#endregion
			//w.WriteChunk(_tiledmap, WriteMetadataChunk);
			w.WriteChunk(_tiledmap, WriteTilesetChunk);
            w.WriteChunk(_tiledmap, WriteTilemapChunk);
            w.WriteChunk(_tiledmap, WriteCollisionChunk);
            w.WriteChunk(_tiledmap, WritePriorityChunk);
            w.WriteChunk(_tiledmap, WriteObjectsChunk);
			w.WriteChunkEnd();
        }
    }
}
