using System.IO;

namespace Xe.Tools.Modules
{
    public partial class TileCollision
    {
        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter writer)
        {
            writer.Flush();
        }
    }
}
