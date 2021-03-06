﻿using System;
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

        private void Export(Stream stream, IEnumerable<Xe.Game.Messages.Message> messages)
        {
            using (var writer = new BinaryWriter(stream))
                Export(writer, messages);
        }

        private void Export(BinaryWriter writer, IEnumerable<Xe.Game.Messages.Message> messages)
        {
            var entries = new List<BinaryEntry>(0x4000);
            using (var memStream = new MemoryStream(0x20000))
            {
                var w = new BinaryWriter(memStream);

				var msgOrdered = new SortedDictionary<uint, Xe.Game.Messages.Message>(
					messages.ToDictionary(x => x.Tag.GetXeHash(), x => x));

				w.Write(0x0247534D); // MSGv2
				w.Write(msgOrdered.Count);
				var headerPos = (int)w.BaseStream.Position;
				var hashPos = headerPos + 8;
				var offPos = hashPos + msgOrdered.Count * sizeof(uint);
				var msgPos = offPos + msgOrdered.Count * sizeof(uint);
				var msgOff = msgPos;
				foreach (var msg in msgOrdered)
				{
					w.BaseStream.Position = hashPos;
					w.Write(msg.Key);
					hashPos += sizeof(uint);

					w.BaseStream.Position = offPos;
					w.Write(msgPos - msgOff);
					offPos += sizeof(uint);

					var strData = GetBytesFromString(msg.Value.Text);
					w.BaseStream.Position = msgPos;
					w.Write(strData);
					w.BaseStream.WriteByte(0);
					msgPos += strData.Length + 1;
				}

				w.BaseStream.Position = headerPos;
				w.Write(msgOff);
				w.Write((int)(w.BaseStream.Length - msgOff));

				writer.Write(memStream.GetBuffer(), 0, (int)w.BaseStream.Length);
            }
            writer.Flush();
        }

        private byte[] GetBytesFromString(string str)
        {
            var tmp = Encoding.UTF8.GetBytes(str);
            var data = new List<byte>(tmp.Length);
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