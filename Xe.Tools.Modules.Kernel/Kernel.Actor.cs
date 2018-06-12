using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;
using static Xe.Game.Kernel.Actor;

namespace Xe.Tools.Modules.Kernel
{
	public partial class Kernel
	{
		private string WriteActor(KernelData k, BinaryWriter w)
		{
			List<string> anims = new List<string>();

			foreach (var item in k.Actors)
			{
				int animIndex = anims.IndexOf(item.Animation);
				if (animIndex < 0)
				{
					animIndex = anims.Count;
					anims.Add(item.Animation);
				}

				w.Write(item.Code.GetXeHash());
				w.Write(item.Name.GetXeHash());
				w.Write(item.Description.GetXeHash());
				w.Write(animIndex);

				w.Write((byte)item.Level);
				w.Write((byte)0);
				w.Write((byte)0);
				w.Write((byte)0);
				w.Write(item.Experience);
				w.Write((short)item.Health);
				w.Write((short)item.HealthCurrent);
				w.Write((short)item.Mana);
				w.Write((short)item.ManaCurrent);
				w.Write((byte)item.Attack);
				w.Write((byte)item.Defense);
				w.Write((byte)item.AttackSpecial);
				w.Write((byte)item.DefenseSpecial);
				w.Write(0);

				w.Write((byte)item.Drop.Hp);
				w.Write((byte)item.Drop.Mp);
				w.Write((byte)0);
				w.Write((byte)0);
				w.Write((short)item.Drop.Exp);
				w.Write((short)item.Drop.Money);

				WriteDropItem(w, k, item?.Drop?.Items?.FirstOrDefault());
				WriteDropItem(w, k, item?.Drop?.Items?.Skip(1)?.FirstOrDefault());
			}

			Table["ActorsAnimations"] = anims;

			return "Actor1";
		}

		private static void WriteDropItem(BinaryWriter w, KernelData k, DropItem dropItem)
		{
			if (dropItem != null)
			{
				var item = k.InventoryItems.FirstOrDefault(x => x.Id == dropItem.ItemId)?.Code;
				var id = item?.GetXeHash() ?? 0;

				w.Write(id);
				w.Write((byte)dropItem.Count);
				w.Write((byte)dropItem.Rarity);
				w.Write((short)0);
			}
			else
			{
				w.Write(0);
				w.Write(0);
			}
		}
	}
}
