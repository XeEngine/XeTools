using System;
using System.Collections.Generic;

namespace Xe.Game.Kernel
{
    public class Actor
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

		public string Animation { get; set; }

		public bool Enabled { get; set; }

        public bool Locked { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

		public int Health { get; set; }

		public int Mana { get; set; }

		public int Attack { get; set; }

		public int Defense { get; set; }

		public int AttackSpecial { get; set; }

		public int DefenseSpecial { get; set; }

		public int HealthCurrent { get; set; }

		public int ManaCurrent { get; set; }

		public List<SkillUsage> Skills { get; set; }
    }
}
