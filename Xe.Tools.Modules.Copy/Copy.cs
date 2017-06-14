using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class Copy : IModule
    {
        private ModuleSettings Settings { get; }

        public string FileName { get => Settings.FileName; }
        public Tuple<string, string>[] Parameters { get => Settings.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        public Copy(ModuleSettings settings)
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
            File.Copy(inputFileName, outputFileName);
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
