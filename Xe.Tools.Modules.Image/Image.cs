using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Xe.Tools.Services;

namespace Xe.Tools.Modules
{
    public class Image : ModuleBase
    {
        public Image(ModuleInit init) : base(init) { }

        public override bool OpenFileData(string fileName) { return true; }

        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
            return InputFileName;
        }

        public override IEnumerable<string> GetSecondaryInputFileNames()
        {
            return new string[0];
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            return new string[0];
        }

        public override void Export()
        {
            var inputFileName = Path.Combine(InputWorkingPath, InputFileName);
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);
            ImageService.MakeTransparent(outputFileName, inputFileName, new Xe.Graphics.Color[]
            {
                new Xe.Graphics.Color() { r = 255, g = 0, b = 255 },
                new Xe.Graphics.Color() { r = 255, g = 128, b = 0 }
            });
        }
    }
}
