using System;

namespace Xe.Game.Kernel
{
	public class Status
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
