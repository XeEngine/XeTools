using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xe.Tools.Modules
{
    using Fonts = Game.Fonts;

    public partial class Font : IModule
    {
        private ModuleInit Init { get; }
        public string FileName { get => Init.FileName; }
        public Tuple<string, string>[] Parameters { get => Init.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }
        
        private Fonts.Font MyFont { get; }

        public Font(ModuleInit init)
        {
            Init = init;

            var inputFileName = Path.Combine(Init.InputPath, FileName);
            using (var file = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    MyFont = JsonConvert.DeserializeObject<Fonts.Font>(reader.ReadToEnd());
                }
            }
            IsValid = !string.IsNullOrWhiteSpace(MyFont.FontName);
            CalculateFileNames();
        }

        private void CalculateFileNames()
        {
            var basePath = Path.GetDirectoryName(FileName);
            var inputBasePath = Path.Combine(Init.InputPath, basePath);
            var outputBasePath = Path.Combine(Init.OutputPath, basePath);

            var inputFiles = new List<string>
            {
                Path.Combine(Init.InputPath, FileName)
            };
            var outputFiles = new List<string>
            {
                Path.Combine(Init.OutputPath, Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName)))
            };

            foreach (var table in MyFont.Tables)
            {
                inputFiles.Add(Path.Combine(inputBasePath, table.Texture));
                outputFiles.Add(Path.Combine(outputBasePath, table.Texture));
            }

            InputFileNames = inputFiles.ToArray();
            OutputFileNames = outputFiles.ToArray();
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
