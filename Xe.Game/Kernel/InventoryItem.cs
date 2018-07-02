using System;

namespace Xe.Game.Kernel
{
	public enum InventoryItemType
	{
		Skill,
		Consumable,
		Material,
		Key
	}

	public enum InventoryItemEffect
	{

	}

    public class InventoryItem
    {
		public Guid Id { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public InventoryItemType Type { get; set; }

		public InventoryItemEffect Effect { get; set; }

		public Guid SkillId { get; set; }
	}
}
