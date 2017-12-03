using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class Copy : ModuleBase
    {
        public Copy(ModuleInit init) : base(init) { }

        public override bool OpenFileData(string fileName) { return true; }

        public override bool OpenFileData(FileStream stream) { return true; }

        public override string GetOutputFileName()
        {
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
            var inputFileName = Path.Combine(InputWorkingPath, InputFileName);
            var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
            if (File.Exists(outputFileName))
                File.Delete(outputFileName);
            File.Copy(inputFileName, outputFileName);
        }
    }
}
