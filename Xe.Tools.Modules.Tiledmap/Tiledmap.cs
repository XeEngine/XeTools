using System;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap : ModuleBase
    {
        private TilemapTiled MyTiledmap { get; set; }

        public Tiledmap(ModuleInit init) : base(init) { }
        
        public override bool OpenFileData(string fileName)
        {
            MyTiledmap = new TilemapTiled(fileName);
            return true;
        }
        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
            var extIndex = InputFileName.IndexOf(".json");
            if (extIndex >= 0)
            {
                return InputFileName.Substring(0, extIndex);
            }
            return InputFileName;
        }

        public override IEnumerable<string> GetSecondaryInputFileNames()
        {
            return new string[0];
        }

        public override IEnumerable<string> GetSecondaryOutputFileNames()
        {
            var basePath = Path.GetDirectoryName(InputFileName);
            var tilesetImage = $"{Path.GetFileNameWithoutExtension(OutputFileName)}.png";
            return new string[]
            {
                Path.Combine(basePath, tilesetImage)
            };
        }

        public override void Export()
        {
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(fStream);
            }
        }
    }
}
