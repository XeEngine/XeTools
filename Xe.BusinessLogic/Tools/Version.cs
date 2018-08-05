using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools
{
    public class Version
	{
		public int Major { get; set; }
		public int Minor { get; set; }
		public int Revision { get; set; }
		public string Info { get; set; }

		public override string ToString()
		{
			return $"{Major}.{Minor}.{Revision} {Info}";
		}
	}
}
