using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Kernel
{
	public enum Affect
	{
		Hp,
		Mp,
		Attack,
		Defense,
		SpecialAttack,
		SpecialDefense,
		Speed,
	}

    public class AlteredStatus
	{
		public Guid Id { get; set; }

		public int VirtualIndex { get; set; } 

		public string Name { get; set; }

		public Guid MsgName { get; set; }

		public Guid MsgDescription { get; set; }

		public Affect Affect { get; set; }
	}
}
