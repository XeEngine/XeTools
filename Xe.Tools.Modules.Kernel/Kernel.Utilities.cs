using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xe.Tools.Modules.Kernel
{
    public partial class Kernel
	{
		private static int CountBits(int data)
		{
			int bitsCount = 0;
			for (int i = 0; i < 32; i++)
			{
				bitsCount += (data & (1 << i)) != 0 ? 1 : 0;
			}
			return bitsCount;
		}
		private static int AlignOffset(int offset, int alignment)
		{
			int diff = offset % alignment;
			return diff > 0 ? offset + alignment - diff : offset;
		}
		private static Dictionary<uint, int> GetHashList<T>(List<T> items, Func<T, uint> func)
		{
			var index = 0;
			var uniqueList = items.Distinct();
			var dictionary = new Dictionary<uint, int>(uniqueList.Count());
			foreach (var item in uniqueList)
			{
				var hash = func.Invoke(item);
				dictionary.Add(hash, index++);
			}
			return dictionary;
		}
		private static Dictionary<uint, int> GetHashList(List<string> items)
		{
			return GetHashList(items, x => x.GetXeHash());
		}
		private static Dictionary<uint, int> WriteHashList(BinaryWriter w, List<string> items)
		{
			return GetHashList(items, x =>
			{
				var hash = x.GetXeHash();
				w.Write(hash);
				return hash;
			});
		}

		public static void WriteChunk<T>(BinaryWriter writer, T data, Func<T, BinaryWriter, string> action) where T : class
		{
			using (var memoryStream = new MemoryStream(0x8000))
			{
				using (var memoryWriter = new BinaryWriter(memoryStream))
				{
					var strChunk = action(data, memoryWriter);
					if (strChunk != null && memoryStream.Length > 0)
					{
						var head = strChunk.GetXeHash();
						writer.Write(head);
						writer.Write((uint)memoryStream.Length);
						writer.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}
		}


		private static string WriteStrings(List<string> list, string name, BinaryWriter w)
		{
			var index = 0;
			var offset = list.Count * sizeof(long);

			foreach (var item in list)
			{
				var data = System.Text.Encoding.UTF8.GetBytes(item);

				w.BaseStream.Position = index++ * sizeof(long);
				w.Write((long)offset);
				w.BaseStream.Position = offset;
				w.Write(data);
				w.Write((byte)0);
				offset += data.Length + 1;
			}

			w.Align(8);

			return name;
		}

		public static void WriteChunkEnd(BinaryWriter writer)
		{
			writer.Write(ulong.MaxValue);
		}
	}
}
