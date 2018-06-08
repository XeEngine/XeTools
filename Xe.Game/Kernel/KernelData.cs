﻿using System.Collections.Generic;

namespace Xe.Game.Kernel
{
    public class KernelData
    {
		public List<Element> Elements { get; set; }

		public List<Skill> Skills { get; set; }

        public List<Ability> Abilities { get; set; }

        public List<Actor> Players { get; set; }

		public List<Zone> Zones { get; set; }

		public List<Bgm> Bgms { get; set; }

		public List<Sfx> Sfxs { get; set; }
	}
}
