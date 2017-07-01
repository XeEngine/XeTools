using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Animations;

namespace Xe.Tools.Modules
{
    public partial class Animation : IModule
    {
        private ModuleSettings Settings { get; }
        private AnimationsGroup AnimationsGroup { get; }

        public string FileName { get => Settings.FileName; }
        public Tuple<string, string>[] Parameters { get => Settings.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        public Animation(ModuleSettings settings)
        {
            Settings = settings;

            var fileName = Path.Combine(Settings.InputPath, FileName);
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    AnimationsGroup = JsonConvert.DeserializeObject<AnimationsGroup>(reader.ReadToEnd());
                }
            }
            IsValid = AnimationsGroup.SpriteSheets != null &&
                AnimationsGroup.Frames != null &&
                AnimationsGroup.Animations != null && 
                AnimationsGroup.AnimationReferences != null;

            var inputBasePath = Path.Combine(Settings.InputPath, Path.GetDirectoryName(fileName));
            var outputBasePath = Path.Combine(Settings.OutputPath, Path.GetDirectoryName(fileName));

            var inputFiles = new List<string> { fileName };
            foreach (var spriteFileName in AnimationsGroup.SpriteSheets)
                inputFiles.Add(Path.Combine(inputBasePath, spriteFileName));
            InputFileNames = inputFiles.ToArray();

            var outputFiles = new List<string> { fileName };
            foreach (var spriteFileName in AnimationsGroup.SpriteSheets)
                outputFiles.Add(Path.Combine(outputBasePath, spriteFileName));
            OutputFileNames = outputFiles.ToArray();
        }

        public void Export()
        {


            /*var inputFileName = Path.Combine(Settings.InputPath, FileName);
            var outputFileName = Path.Combine(Settings.OutputPath, FileName);
            var ouputFilePath = Path.GetDirectoryName(outputFileName);
            if (!Directory.Exists(ouputFilePath))
                Directory.CreateDirectory(ouputFilePath);
            File.Copy(inputFileName, outputFileName);*/
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
