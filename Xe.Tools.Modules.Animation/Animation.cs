﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Animations;

namespace Xe.Tools.Modules
{
    public partial class Animation : IModule
    {
        private ModuleInit Init { get; }
        private AnimationData AnimationData { get; }

        public string FileName { get => Init.FileName; }
        public Tuple<string, string>[] Parameters { get => Init.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        public Animation(ModuleInit init)
        {
            Init = init;

            var fileName = Path.Combine(Init.InputPath, FileName);
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    AnimationData = JsonConvert.DeserializeObject<AnimationData>(reader.ReadToEnd());
                }
            }
            IsValid = AnimationData.Textures != null &&
                AnimationData.Frames != null &&
                AnimationData.Animations != null && 
                AnimationData.AnimationGroups != null;

            var inputBasePath = Path.Combine(Init.InputPath, Path.GetDirectoryName(fileName));
            var outputBasePath = Path.Combine(Init.OutputPath, Path.GetDirectoryName(fileName));

            var inputFiles = new List<string> { fileName };
            foreach (var texture in AnimationData.Textures)
                inputFiles.Add(Path.Combine(inputBasePath, texture.Name));
            InputFileNames = inputFiles.ToArray();

            var outputFiles = new List<string> { fileName };
            foreach (var texture in AnimationData.Textures)
                outputFiles.Add(Path.Combine(outputBasePath, texture.Name));
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
