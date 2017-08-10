using Newtonsoft.Json;
using System;
using System.IO;
using Xe.Game.Kernel;
using System.Collections.Generic;

namespace Xe.Tools.Modules.Kernel
{
    public partial class Kernel : ModuleBase
    {
        private KernelData KernelData { get; set; }

        public Kernel(ModuleInit init) : base(init) { }

        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        public override bool OpenFileData(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                KernelData = JsonConvert.DeserializeObject<KernelData>(reader.ReadToEnd());
            }
            return true;
        }

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
