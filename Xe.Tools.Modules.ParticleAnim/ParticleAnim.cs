using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xe.Game.Particles;

namespace Xe.Tools.Modules
{
	public partial class ParticleAnim : ModuleBase
	{
		private ParticlesData particlesData;

		public ParticleAnim(ModuleInit init) : base(init) { }

		public override bool OpenFileData(FileStream stream)
		{
			using (var reader = new StreamReader(stream))
			{
				particlesData = JsonConvert.DeserializeObject<ParticlesData>(reader.ReadToEnd());
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
	}
}
