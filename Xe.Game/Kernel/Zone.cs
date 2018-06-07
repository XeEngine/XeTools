using System;

namespace Xe.Game.Kernel
{
    public class Zone
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Code { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }
	}
}
