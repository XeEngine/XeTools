using System.IO;

namespace Xe.Tools.Modules
{
	public partial class ParticleAnim
	{
		public override void Export()
		{
			var outputFileName = Path.Combine(OutputWorkingPath, OutputFileName);
			using (var fStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
			{
				Export(fStream);
			}
		}

		private void Export(Stream stream)
		{
			using (var writer = new BinaryWriter(stream))
				Export(writer);
		}

		private void Export(BinaryWriter w)
		{
			const uint MagicCode = 0x01545250;

			w.Write(MagicCode);
			w.Write(0);
			w.WriteChunk(particlesData.AnimationDataName, WriteAnimationDataNameChunk);
			w.WriteChunk(particlesData.Groups, WriteParticlesGroupChunk);
			w.WriteChunkEnd();
		}


		private string WriteAnimationDataNameChunk(string particle, BinaryWriter w)
		{
			w.Write(particle);
			w.Write((byte)0);
			w.Align(8);
			return "ANM\x01";
		}
	}
}
