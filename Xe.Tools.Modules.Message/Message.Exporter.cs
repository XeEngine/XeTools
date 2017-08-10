using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Xe.Tools.Modules
{
    public partial class Message
    {
        private const uint MagicCode = 0x0147534D;

        private class BinaryEntry
        {
            public ushort Id;
            public uint Position;
        }

        public Language CurrentLanguage
        {
            get
            {
                var str = Parameters.Where(x => x.Item1 == "language")
                    .Select(x => x.Item2)
                    .FirstOrDefault();
                switch (str)
                {
                    case "english": return Language.English;
                    case "italian": return Language.Italian;
                    case "french": return Language.French;
                    case "deutsch": return Language.Deutsch;
                    case "spanish": return Language.Spanish;
                    case "japanese": return Language.Japanese;
                    default: return Language.English;
                }
            }
        }

        private void Export(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer);
        }

        private void Export(BinaryWriter writer)
        {
            var entries = new List<BinaryEntry>(0x4000);
            using (var memStream = new MemoryStream(0x20000))
            {
                var w = new BinaryWriter(memStream);
                foreach (var segment in Messages.Segments)
                {
                    ushort id = segment.Id;
                    foreach (var message in segment.Messages)
                    {
                        string str;
                        switch (CurrentLanguage)
                        {
                            case Language.English: str = message.En; break;
                            case Language.Italian: str = message.It; break;
                            case Language.French: str = message.Fr; break;
                            case Language.Deutsch: str = message.De; break;
                            case Language.Spanish: str = message.Sp; break;
                            default: str = ""; break;
                        }
                        if (str == null) str = "";
                        var bytes = GetBytesFromString(str);
                        var entry = new BinaryEntry()
                        {
                            Id = id++,
                            Position = (ushort)w.BaseStream.Position
                        };
                        entries.Add(entry);
                        w.Write(bytes);
                        w.Write((byte)0);
                    }
                }
                memStream.Flush();

                writer.Write(MagicCode);
                writer.Write((ushort)entries.Count);
                writer.Write((ushort)((memStream.Position + 7) / 8));

                entries.Sort((x, y) => x.Id - y.Id);
                foreach (var e in entries)
                    writer.Write(e.Id);
                foreach (var e in entries)
                    writer.Write(e.Position);
                writer.Write(memStream.GetBuffer(), 0, (int)memStream.Length);
            }
            writer.Flush();
        }
        private byte[] GetBytesFromString(string str)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(str);
            List<byte> data = new List<byte>(tmp.Length);
            for (int i = 0; i < tmp.Length; i++)
            {
                byte c = tmp[i];
                switch ((char)c)
                {
                    case '{':
                        int cmdBegin = i + 1;
                        int cmdEnd = -1;
                        for (; i < tmp.Length; i++)
                        {
                            if ((char)tmp[i] == '}')
                            {
                                cmdEnd = i;
                                var cmd = Encoding.UTF8.GetString(tmp, cmdBegin, cmdEnd - cmdBegin).
                                    Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                                if (cmd.Length > 0)
                                {
                                    switch (cmd[0])
                                    {
                                        case "COLOR":
                                            if (cmd.Length == 2)
                                            {
                                                if (int.TryParse(cmd[1], NumberStyles.HexNumber, null, out int hexcolor))
                                                {
                                                    hexcolor |= 0xF000;
                                                    data.Add(0x10);
                                                    data.Add((byte)(hexcolor >> 8));
                                                    data.Add((byte)(hexcolor & 0xFF));
                                                    break;
                                                }
                                            }
                                            Log.Error(string.Format("Invalid arguments for {0} on {1}.", cmd[0], str));
                                            break;

                                    }
                                }
                                else
                                    Log.Warning(string.Format("Empty brackets on {0}.", str));
                                break;
                            }
                        }
                        break;
                    default:
                        data.Add(c);
                        break;
                }
            }
            return data.ToArray();
        }
    }
}