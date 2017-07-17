using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap : IModule
    {
        private ModuleInit Init { get; }
        public string FileName { get => Init.FileName; }
        public Tuple<string, string>[] Parameters { get => Init.Parameters; }
        public bool IsValid { get; private set; }
        public string[] InputFileNames { get; private set; }
        public string[] OutputFileNames { get; private set; }

        private TilemapTiled MyTiledmap { get; }

        public Tiledmap(ModuleInit init)
        {
            Init = init;

            var inputFileName = Path.Combine(Init.InputPath, FileName);
            MyTiledmap = new TilemapTiled(inputFileName);
            IsValid = true;
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
            foreach (var tileset in MyTiledmap.Tilesets)
            {
                var filePath = Path.Combine(basePath, tileset.ImagePath);
                var fullPath = Path.Combine(inputBasePath, filePath);
                inputFiles.Add(fullPath);
            }
            InputFileNames = inputFiles.ToArray();

            var outFileNameBase = Path.Combine(Init.OutputPath, Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName)));
            OutputFileNames = new string[]
            {
                $"{outFileNameBase}.map",
                $"{outFileNameBase}.png"
            };
        }

        public static bool Validate(string filename)
        {
            return true;
        }
    }
}
