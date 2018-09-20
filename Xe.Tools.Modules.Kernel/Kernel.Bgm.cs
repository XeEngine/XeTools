using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		struct BgmHeader
		{
			int HeaderSize;
			int BgmCount;
		}

		private string WriteBgm(KernelData k, BinaryWriter w)
		{
			w.Write(0x08); // Header size
			w.Write(k.Bgms.Count); // Bgms count

			for (int i = 0; i < k.Bgms.Count; i++)
			{
				var bgm = k.Bgms[i];
				w.Write(k.Bgms[i]?.Name?.GetXeHash() ?? 0);
				w.Write(48000);
				w.Write(k.Bgms[i]?.Loops?.Count ?? 0);
				w.Write(k.Bgms[i]?.Starts?.Count ?? 0);
			}

			for (int i = 0; i < k.Bgms.Count; i++)
			{
				var bgm = k.Bgms[i];

				for (int j = 0; j < (bgm?.Loops?.Count ?? 0); j++)
				{
					w.Write((int)bgm.Loops[j].OffsetStart);
					w.Write((int)bgm.Loops[j].OffsetEnd);
				}

				for (int j = 0; j < (bgm?.Starts?.Count ?? 0); j++)
				{
					w.Write((int)bgm.Starts[j].OffsetStart);
				}
			}

			Table["BgmFiles"] = k.Bgms.Select(x => x.FileName).ToList();

			return "Bgm1";
		}
	}
}
