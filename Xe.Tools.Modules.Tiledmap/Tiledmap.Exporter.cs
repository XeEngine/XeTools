using System;
using System.IO;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {

        public void Export()
        {
            var outputFileName = OutputFileNames[0];
            var ouputFilePath = Path.GetDirectoryName(outputFileName);
            if (!Directory.Exists(ouputFilePath))
                Directory.CreateDirectory(ouputFilePath);
            using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(fStream);
            }
        }

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
