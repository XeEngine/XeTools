using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools.Items
{
    public static class Factory
    {
		public static Texture CreateImage(string filename)
		{
			return new Texture(filename);
		}

		public static ItemModule Create(string module, string filename)
		{
			switch (module)
			{
				case "image":
				case "texture":
					return CreateImage(filename);
				default:
					return null;
			}
		}
    }
}
