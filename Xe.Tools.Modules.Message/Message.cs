using Newtonsoft.Json;
using System;
using System.IO;
using Xe.Game.Messages;

namespace Xe.Tools.Modules
{
    public partial class Message : IModule
    {
        private ModuleInit Init { get; }
        public string FileName { get => Init.FileName; }
        public Tuple<string, string>[] Parameters { get => Init.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        private MessageContainer Messages { get; }

        public Message(ModuleInit init)
        {
            Init = init;

            var inputFileName = Path.Combine(Init.InputPath, FileName);
            using (var file = new FileStream(inputFileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    Messages = JsonConvert.DeserializeObject<MessageContainer>(reader.ReadToEnd());
                }
            }
            IsValid = true;
            CalculateFileNames();
        }

        private void CalculateFileNames()
        {
            var basePath = Path.GetDirectoryName(FileName);
            var inputBasePath = Path.Combine(Init.InputPath, basePath);

            InputFileNames = new string[]
            {
                Path.Combine(inputBasePath, Path.GetFileName(FileName))
            };

            string outputFileName;
            if (Init.OutputFileName != null)
            {
                outputFileName = Path.Combine(Init.OutputPath, Init.OutputFileName);
            }
            else
            {
                var outputBasePath = Path.Combine(Init.OutputPath, basePath);
                outputFileName = Path.Combine(outputBasePath, Init.OutputFileName ?? Path.GetFileNameWithoutExtension(FileName));
            }
            OutputFileNames = new string[]
            {
                outputFileName
            };
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
