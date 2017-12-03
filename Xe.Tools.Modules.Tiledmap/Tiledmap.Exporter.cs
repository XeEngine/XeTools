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
            // Metadata support implemented yet...
            w.Write((byte)0xFF);
            Align(w, 8, 0xFF);
            #endregion
            WriteChunk(_tiledmap, w, WriteTilemapChunk);
            WriteChunk(_tiledmap, w, WriteCollisionChunk);
            WriteChunk(_tiledmap, w, WritePriorityChunk);
            WriteChunk(_tiledmap, w, WriteObjectsChunk);
        }

        private static void WriteChunk(Map tileMap, BinaryWriter writer, Func<Map, BinaryWriter, string> action)
        {
            using (var memoryStream = new MemoryStream(0x8000))
            {
                using (var memoryWriter = new BinaryWriter(memoryStream))
                {
                    var strChunk = action(tileMap, memoryWriter);
                    if (memoryStream.Length > 0)
                    {
                        var head = System.Text.Encoding.ASCII.GetBytes(strChunk);
                        writer.Write(head, 0, 4);
                        writer.Write((uint)memoryStream.Length);
                        writer.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    }
                }
            }
        }

        private static void Align(BinaryWriter w, int align, byte fill = 0)
        {
            int remainingData = -(int)(w.BaseStream.Position - ((w.BaseStream.Position + align - 1) / align) * align);
            while (remainingData-- > 0)
                w.Write(fill);
        }
    }
}
