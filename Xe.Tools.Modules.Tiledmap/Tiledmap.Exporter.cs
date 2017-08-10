using System;
using System.IO;

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
            throw new NotImplementedException("Not implemented yet");
        }
    }
}
