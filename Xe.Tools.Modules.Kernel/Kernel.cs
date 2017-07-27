using Newtonsoft.Json;
using System;
using System.IO;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
    public partial class Kernel : IModule
    {
        private ModuleInit Init { get; }
        public string FileName { get => Init.FileName; }
        public Tuple<string, string>[] Parameters { get => Init.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        private KernelData KernelData { get; set; }

        public Kernel(ModuleInit init)
        {
            Init = init;

            var inputFileName = Path.Combine(Init.InputPath, FileName);
            using (var file = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    KernelData = JsonConvert.DeserializeObject<KernelData>(reader.ReadToEnd());
                }
            }
            IsValid = KernelData != null;
            CalculateFileNames();
        }
        
        private void CalculateFileNames()
        {
            var basePath = Path.GetDirectoryName(FileName);
            var inputBasePath = Path.Combine(Init.InputPath, basePath);
            var outputBasePath = Path.Combine(Init.OutputPath, basePath);

            InputFileNames = new string[]
            {
                Path.Combine(Init.InputPath, FileName)
            };
            OutputFileNames = new string[]
            {
                Path.Combine(Init.OutputPath, Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName)))
            };
        }

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

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
