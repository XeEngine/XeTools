using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Xe.Tools.Modules
{
    public class Image : IModule
    {
        private ModuleSettings Settings { get; }

        public string FileName { get => Settings.FileName; }
        public Tuple<string, string>[] Parameters { get => Settings.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        public Image(ModuleSettings settings)
        {
            Settings = settings;
            IsValid = true;
            InputFileNames = new string[] { Path.Combine(Settings.InputPath, FileName) };
            OutputFileNames = new string[] { Path.Combine(Settings.OutputPath, FileName) };
        }

        public void Export()
        {
            var inputFileName = Path.Combine(Settings.InputPath, FileName);
            var outputFileName = Path.Combine(Settings.OutputPath, FileName);
            var ouputFilePath = Path.GetDirectoryName(outputFileName);
            if (!Directory.Exists(ouputFilePath))
                Directory.CreateDirectory(ouputFilePath);

            using (var file = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                var decoder = new PngBitmapDecoder(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                var encoder = new PngBitmapEncoder()
                {
                    Interlace = PngInterlaceOption.Off,
                };
                encoder.Frames.Add(decoder.Frames[0]);
                if (decoder.Palette != null)
                    encoder.Palette = decoder.Palette;

                using (var outFile = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(outFile);
                }
            }
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
