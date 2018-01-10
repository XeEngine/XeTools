using Newtonsoft.Json;
using System;
using System.IO;
using Xe.Game.Collisions;
using System.Collections.Generic;

namespace Xe.Tools.Modules
{
    public partial class TileCollision : ModuleBase
    {
        private CollisionSystem CollisionSystem { get; set; }

        public TileCollision(ModuleInit init) : base(init)
        { }

        public override bool OpenFileData(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                CollisionSystem = JsonConvert.DeserializeObject<CollisionSystem>(reader.ReadToEnd());
            }
            return true;
        }

        public override string GetOutputFileName()
        {
            string baseFileName;
            var extIndex = InputFileName.IndexOf(".json");
            if (extIndex >= 0)
            {
                baseFileName = InputFileName.Substring(0, extIndex);
            }
            else
                baseFileName = InputFileName;
            return $"{baseFileName}_tmp.bin";
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
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
            {
                Export(fStream);
            }
        }
    }
}
