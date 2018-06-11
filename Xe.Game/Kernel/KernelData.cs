using System.Collections.Generic;

namespace Xe.Game.Kernel
{
    public class KernelData
	{
		public List<Zone> Zones { get; set; }

		public List<Bgm> Bgms { get; set; }

		public List<Sfx> Sfxs { get; set; }

		public List<Element> Elements { get; set; }

		public List<Status> Status { get; set; }

		public List<Skill> Skills { get; set; }

		public List<InventoryItem> InventoryItems { get; set; }

		public List<Ability> Abilities { get; set; }

        public List<Actor> Actors { get; set; }
	}
}
