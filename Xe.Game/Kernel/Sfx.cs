﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Kernel
{
    public class Sfx
	{
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Name { get; set; }

		public string FileName { get; set; }
	}
}
