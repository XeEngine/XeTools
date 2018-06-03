using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Xe.Game.Messages;
using System.Collections.Generic;

namespace Xe.Tools.Modules
{
    public partial class Message : ModuleBase
    {
        private MessageContainer Messages { get; set; }
		private Dictionary<Language, (string, IEnumerable<Xe.Game.Messages.Message>)> fileNames =
			new Dictionary<Language, (string, IEnumerable<Game.Messages.Message>)>();

        public Message(ModuleInit init) : base(init)
		{
			var fileName = InternalGetOutputFileName();

			foreach (var entry in Messages.Messages.GroupBy(x => x.Language))
			{
				string id;

				switch (entry.Key)
				{
					case Language.English:
						id = "en";
						break;
					case Language.Italian:
						id = "it";
						break;
					case Language.French:
						id = "fr";
						break;
					case Language.Deutsch:
						id = "de";
						break;
					case Language.Spanish:
						id = "es";
						break;
					case Language.Japanese:
						id = "jp";
						break;
					default:
						id = "xx";
						break;
				}

				AddFileName(fileName, entry.Key, entry.AsEnumerable(), id);
			}
		}

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
			var strOriginal = GetOutputFileName();
			var strBase = Path.GetFileNameWithoutExtension(strOriginal);
			var strExt = Path.GetExtension(strOriginal);

			return new string[0];
        }

        public override void Export()
        {
			foreach (var entry in fileNames)
			{
				var outputFileName = Path.Combine(OutputWorkingPath, entry.Value.Item1);
				using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
				{
					Export(fStream, entry.Value.Item2);
				}
			}
        }

		private string InternalGetOutputFileName()
		{
			var extIndex = InputFileName.IndexOf(".json");
			if (extIndex >= 0)
			{
				return InputFileName.Substring(0, extIndex);
			}
			return InputFileName;
		}

		private void AddFileName(string fileName, Language language, IEnumerable<Xe.Game.Messages.Message> messages, string id)
		{
			string strFinal;
			var strOriginal = GetOutputFileName();

			if (!string.IsNullOrEmpty(id))
			{
				var strPath = Path.GetDirectoryName(strOriginal);
				var strBase = Path.GetFileNameWithoutExtension(strOriginal);
				var strExt = Path.GetExtension(strOriginal);
				strFinal = Path.Combine(strPath, $"{strBase}_{id}{strExt}");
			}
			else
			{
				strFinal = strOriginal;
			}

			fileNames.Add(language, (strFinal, messages));
		}
    }
}
