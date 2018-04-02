using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xe.Game.Particles;

namespace Xe.Tools.Modules
{
    public partial class ParticleAnim
	{
		private string WriteParticlesGroupChunk(List<ParticlesGroup> groups, BinaryWriter w)
		{
			w.Write((ushort)groups.Count);
			w.Write((ushort)0);
			w.Write((int)0);
			w.Align(8);

			foreach (var item in groups)
			{
				WriteParticlesGroup(item, w);
			}

			return "PAG\x01";
		}

		private void WriteParticlesGroup(ParticlesGroup group, BinaryWriter w)
		{
			int effectsCount = group.Effects.Count;
			if (effectsCount > byte.MaxValue)
			{
				Log.Warning($"A group exceed the maximum effects count: {byte.MaxValue} effects will be exported instead of {effectsCount}.");
				effectsCount = byte.MaxValue;
			}

			w.Write(group.AnimationName.GetXeHash());
			w.Write((byte)group.ParticlesCount);
			w.Write((byte)effectsCount);
			w.Write(ProcessValue(group.GlobalDelay));
			w.Write(ProcessValue(group.GlobalDuration));
			w.Write(ProcessValue(group.Delay));
			w.Align(8);

			for (int i = 0; i < effectsCount; i++)
			{
				var item = group.Effects[i];
				WriteParticleEffect(item, w);
			}
		}

		private void WriteParticleEffect(Effect effect, BinaryWriter w)
		{
			w.Write((byte)effect.Ease);
			w.Write((byte)effect.Parameter);
			w.Write((byte)0);
			w.Write((byte)0);
			w.Write(ProcessValue(effect.Speed));
			w.Write(ProcessValue(effect.FixStep));
			w.Write(ProcessValue(effect.Sum));
			w.Write(ProcessValue(effect.Multiplier));
			w.Write(ProcessValue(effect.Delay));
			w.Write(ProcessValue(effect.Duration));
			w.Align(8);
		}

		private short ProcessValue(double value)
		{
			return (short)Math.Min(short.MaxValue, Math.Max(short.MinValue, Math.Round(value * 256.0)));
		}
	}
}
