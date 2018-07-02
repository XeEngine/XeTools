using System.IO;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		private static string WriteElements(KernelData k, BinaryWriter w)
		{
			foreach (var item in k.Elements)
			{
				w.Write(item.Code.GetXeHash());
				w.Write(item.Name.GetXeHash());
				w.Write(item.Description.GetXeHash());
				w.Write(0);
			}

			return "Elements1";
		}
	}
}
