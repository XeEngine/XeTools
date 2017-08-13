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
            using (var file = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                var decoder = new PngBitmapDecoder(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var encoder = new PngBitmapEncoder()
                {
                    Interlace = PngInterlaceOption.Off,
                };
                var frame = decoder.Frames[0];
                encoder.Frames.Add(decoder.Frames[0]);
                if (decoder.Palette != null)
                    encoder.Palette = decoder.Palette;

                using (var outFile = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(outFile);
                }
            }
        }
    }
}
