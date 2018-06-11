using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		private string WriteBgm(KernelData k, BinaryWriter w)
		{
			foreach (var item in k.Bgms)
			{
				w.Write(item.Name.GetXeHash());
				w.Write(item.Loop);
			}

			Table["BgmFiles"] = k.Bgms.Select(x => x.FileName).ToList();

			return "Bgm1";
		}
	}
}
