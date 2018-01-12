using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public static class Extensions
    {
        public static Stream OpenModuleSettings(this Project project, string moduleName, FileAccess access)
        {
            var path = Path.Combine(project.ProjectPath, ".settings/modules/");
            if (!Directory.Exists(path))
            {
                switch (access)
                {
                    case FileAccess.Read:
                        throw new DirectoryNotFoundException(path);
                    case FileAccess.Write:
                    case FileAccess.ReadWrite:
                        Directory.CreateDirectory(path);
                        break;
                }
            }

            var fileName = Path.Combine(path, $"{moduleName}.json");

            FileMode mode;
            FileShare share;
            switch (access)
            {
                case FileAccess.Read:
                    mode = FileMode.Open;
                    share = FileShare.ReadWrite | FileShare.Delete;
                    break;
                case FileAccess.Write:
                    mode = FileMode.Create;
                    share = FileShare.Read;
                    break;
                case FileAccess.ReadWrite:
                    mode = FileMode.OpenOrCreate;
                    share = FileShare.Read;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(access), "Invalid parameter");
            }

            return new FileStream(path, mode, access, share);
		}

		public static void Align(this BinaryWriter w, int align, byte fill = 0)
		{
			int remainingData = -(int)(w.BaseStream.Position - ((w.BaseStream.Position + align - 1) / align) * align);
			while (remainingData-- > 0)
				w.Write(fill);
		}


		public static void WriteChunk<T>(this BinaryWriter writer, T data, Func<T, BinaryWriter, string> action) where T : class
		{
			using (var memoryStream = new MemoryStream(0x8000))
			{
				using (var memoryWriter = new BinaryWriter(memoryStream))
				{
					var strChunk = action(data, memoryWriter);
					if (strChunk != null && memoryStream.Length > 0)
					{
						var head = System.Text.Encoding.ASCII.GetBytes(strChunk);
						writer.Write(head, 0, 4);
						writer.Write((uint)memoryStream.Length);
						writer.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
					}
				}
			}
		}
		public static void WriteChunkEnd(this BinaryWriter writer)
		{
			writer.Write(ulong.MaxValue);
		}
	}
}
