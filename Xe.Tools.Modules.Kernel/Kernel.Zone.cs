using System.IO;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
    public partial class Kernel
	{
		private static string WriteZones(KernelData k, BinaryWriter w)
		{
			foreach (var item in k.Zones)
			{
				w.Write(item.Code.GetXeHash());
				w.Write(item.Title.GetXeHash());
				w.Write(item.Description.GetXeHash());
				w.Write(0);
			}

			return "Zone1";
		}
	}
}
