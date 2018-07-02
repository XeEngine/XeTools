using System.IO;
using System.Linq;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		private static string WriteInventory(KernelData k, BinaryWriter w)
		{
			foreach (var item in k.InventoryItems)
			{
				w.Write(item.Code.GetXeHash());
				w.Write(item.Name.GetXeHash());
				w.Write(item.Description.GetXeHash());
				w.Write((byte)item.Type);
				w.Write((byte)0);
				w.Write((byte)0);
				w.Write((byte)0);
				
				switch (item.Type)
				{
					case InventoryItemType.Skill:
						w.Write(k.Skills?.FirstOrDefault(x => x.Id == item.SkillId)?.Name?.GetXeHash() ?? 0);
						w.Write(0);
						break;
					case InventoryItemType.Consumable:
						w.Write((byte)item.Effect);
						w.Write((byte)0);
						w.Write((byte)0);
						w.Write((byte)0);
						w.Write(0);
						break;
					default:
						w.Write(new byte[8]);
						break;
				}
			}

			return "Inventory1";
		}
	}
}
