using System;
using System.Collections.Generic;

namespace Xe.Game.Kernel
{
    public class Bgm
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }

		public string FileName { get; set; }

		public List<BgmLoop> Loops { get; set; }

		public List<BgmStart> Starts { get; set; }
	}
}
