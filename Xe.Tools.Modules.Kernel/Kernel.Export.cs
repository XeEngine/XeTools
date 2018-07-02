using System.Collections.Generic;
using System.IO;

namespace Xe.Tools.Modules.Kernel
{
    internal static class Extensions
    {
        public static void Write(this BinaryWriter writer, Dictionary<uint, int> dictionary, string data)
        {
            uint hash = data.GetXeHash();
            if (!dictionary.TryGetValue(hash, out int index))
                index = -1;
            writer.Write((ushort)index);
        }
    }

    public partial class Kernel
    {
        /// <summary>
        /// Description of file itselfs
        /// </summary>
        struct Header
        {
            /// <summary>
            /// Identifier of file's header
            /// </summary>
            public byte MagicCode;

            /// <summary>
            /// Current version of the file
            /// </summary>
            public byte Version;

            /// <summary>
            /// How big is the file itself
            /// </summary>
            public byte HeaderSize;

            /// <summary>
            /// Bytes alignment
            /// </summary>
            public byte Align;
        }

        /// <summary>
        /// Description for each content entry
        /// </summary>
        struct Entry
        {
            public int Offset { get; set; }

			public int Length { get; set; }
		}

		public Dictionary<string, List<string>> Table { get; set; } =
			new Dictionary<string, List<string>>();

		private void Export(BinaryWriter w)
        {
            // Populate the header
            var header = new Header()
            {
                MagicCode = 0x4B,
                Version = 2,
                HeaderSize = 8,
                Align = 8
            };

			WriteHeader(w, header);

			WriteChunk(w, KernelData, WriteZones);
			WriteChunk(w, KernelData, WriteBgm);
			WriteChunk(w, KernelData, WriteElements);
			WriteChunk(w, KernelData, WriteStatus);
			WriteChunk(w, KernelData, WriteInventory);
			WriteChunk(w, KernelData, WriteActor);

			WriteTable(w, "BgmFiles");
			WriteTable(w, "ActorsAnimations");

			WriteChunkEnd(w);
		}

		private void WriteTable(BinaryWriter writable, string entry)
		{
			if (Table.TryGetValue(entry, out var list))
				WriteChunk(writable, list, (x, w) => WriteStrings(x, entry, w));
		}

        private static void WriteHeader(BinaryWriter w, Header header)
        {
            w.Write(header.MagicCode);
            w.Write(header.Version);
            w.Write(header.HeaderSize);
            w.Write(header.Align);
            w.Write((uint)0);
        }
    }
}
