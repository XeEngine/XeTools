using Newtonsoft.Json;
using System;
using System.IO;
using Xe.Game.Messages;
using System.Collections.Generic;

namespace Xe.Tools.Modules
{
    public partial class Message : ModuleBase
    {
        private MessageContainer Messages { get; set; }

        public Message(ModuleInit init) : base(init) { }

        public override bool OpenFileData(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Messages = JsonConvert.DeserializeObject<MessageContainer>(reader.ReadToEnd());
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
