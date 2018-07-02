using System.IO;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		private static string WriteStatus(KernelData k, BinaryWriter w)
		{
			if (k.Status == null)
				return null;

			foreach (var item in k.Status)
			{
				w.Write(item.Code.GetXeHash());
				w.Write(item.Name.GetXeHash());
				w.Write(item.Description.GetXeHash());
				w.Write(0);
			}

			return "Status1";
		}
	}
}
