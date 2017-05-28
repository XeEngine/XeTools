using System;
using System.IO;

namespace Xe.Tools.Items
{
	public class ItemModule : IInfoLastEdit
	{
		public string FileName { get; private set; }

		public ItemModule(string filename)
		{
			FileName = filename;
		}

		public DateTime? GetInfoLastEdit()
		{
			if (!File.Exists(FileName))
				return null;
			return File.GetLastWriteTimeUtc(FileName);
		}

		public DateTime? GetInfoLastEditRecursive()
		{
			return GetInfoLastEdit();
		}
	}
}
