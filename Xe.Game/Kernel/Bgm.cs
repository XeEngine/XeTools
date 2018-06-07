using System;

namespace Xe.Game.Kernel
{
    public class Bgm
    {
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }

		public string FileName { get; set; }

		public int Loop { get; set; }
	}
}
