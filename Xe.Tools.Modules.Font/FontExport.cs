using System.IO;

namespace Xe.Tools.Modules
{
    public partial class Font
    {
        private const uint MagicCode = 0x544E4F46;
        
        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter w)
        {
            w.Write(MagicCode);
            w.Write((byte)1);
            w.Write((byte)MyFont.Tables.Count);
            w.Write((byte)MyFont.CharSets.MaximumWidth);
            w.Write((byte)MyFont.CharSets.MaximumHeight);
            w.Write((byte)MyFont.CharSets.Height);
            w.Write((byte)MyFont.CharSets.YOffset);

            var pOffName = w.BaseStream.Position;
            w.BaseStream.Seek(2, SeekOrigin.Current);
            var pOffPath = w.BaseStream.Position;
            w.BaseStream.Seek(2, SeekOrigin.Current);
            var pOffTable = w.BaseStream.Position;
            w.BaseStream.Seek(2, SeekOrigin.Current);

            var offName = w.BaseStream.Position;
            w.WriteStringData(MyFont.FontName);
            var offPath = w.BaseStream.Position;
            foreach (var table in MyFont.Tables)
                w.WriteStringData(table.Texture);
            var offTable = w.BaseStream.Position;
            var curOffset = offTable + MyFont.Tables.Count * 8;
            foreach (var table in MyFont.Tables)
            {
                table.SpaceOffset = (int)curOffset;
                curOffset += table.CharCount;
                w.Write((byte)table.CharStart);
                w.Write((byte)table.CharCount);
                w.Write((byte)table.CharPerRow);
                w.Write((byte)table.CharDefault);
                w.Write((byte)table.YOffset);
                w.Write((byte)table.CharSet);
                w.Write((ushort)table.SpaceOffset);
            }
            foreach (var table in MyFont.Tables)
            {
                w.BaseStream.Position = table.SpaceOffset;
                w.BaseStream.Write(table.Spaces, 0, table.CharCount);
            }

            w.BaseStream.Position = pOffName;
            w.Write((ushort)offName);
            w.BaseStream.Position = pOffPath;
            w.Write((ushort)offPath);
            w.BaseStream.Position = pOffTable;
            w.Write((ushort)offTable);
        }
    }
}
