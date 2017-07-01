using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libTools.Language;
using System.ComponentModel;
using System.Collections.Generic;
using Xe;

namespace libTools
{
    public enum Languages
    {
        English, Italian, French, German, Spanish
    }

    public partial class Lang : IO<Lang>
    {
        private static readonly uint MagicCode = 0x0147534D;

        private class BinaryEntry
        {
            public ushort Id;
            public uint Position;
        }

        private List<Segment> _segments = new List<Segment>();

        public static Languages CurrentLanguage { get; set; }
        public static Lang Instance = new Lang();

        public List<Segment> Segments
        {
            get { return _segments; }
            set { _segments = value; }
        }

        public Lang()
        {
            Instance = this;
        }
        public Lang(string filename) : base(filename)
        {
            Instance = this;
        }
        public Lang(FileStream stream) : base(stream)
        {
            Instance = this;
        }
        protected override void Export(BinaryWriter writer)
        {
            var entries = new List<BinaryEntry>(0x4000);
            using (var memStream = new MemoryStream(0x20000))
            {
                var w = new BinaryWriter(memStream);
                foreach (var segment in Segments)
                {
                    ushort id = segment.Id;
                    foreach (var message in segment.Messages)
                    {
                        string str;
                        switch (CurrentLanguage)
                        {
                            case Languages.English: str = message.En; break;
                            case Languages.Italian: str = message.It; break;
                            case Languages.French: str = message.Fr; break;
                            case Languages.German: str = message.De; break;
                            case Languages.Spanish: str = message.Sp; break;
                            default: str = ""; break;
                        }
                        if (str == null) str = "";
                        var bytes = GetBytesFromString(str);
                        var entry = new BinaryEntry();
                        entry.Id = id++;
                        entry.Position = (ushort)w.BaseStream.Position;
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
                                                int hexcolor;
                                                if (int.TryParse(cmd[1], NumberStyles.HexNumber, null, out hexcolor))
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

        protected override void Import(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void MyLoad(Lang item)
        {
            Segments = item.Segments;
        }

        public Segment GetSegment(string name)
        {
            foreach (var segment in Segments)
            {
                if (segment.Name.Length == name.Length)
                {
                    if (segment.Name.ToLower().CompareTo(name.ToLower()) == 0)
                        return segment;
                }
            }
            return null;
        }
        public ushort GetMessageId(Guid id)
        {
            foreach (var item in _segments)
            {
                var index = item.GetMessageIndex(id);
                if (index >= 0)
                    return (ushort)(item.Id + index);
            }
            return 0;
        }
        public bool GetMessage(ushort id, out string str)
        {
            Message msg;
            if (GetMessage(id, out msg))
            {
                str = msg.ToString();
                return true;
            }
            str = null;
            return false;
        }
        public string GetMessage(ushort id)
        {
            string str;
            return GetMessage(id, out str) ? str : null;
        }
        public string GetMessage(Guid id)
        {
            Message msg;
            return GetMessage(id, out msg) ? msg.Text : null;
        }
        public bool GetMessage(ushort id, out Message str)
        {
            foreach (var segment in Segments)
            {
                if (segment.Id >= id ||
                    segment.Id + segment.Messages.Count < id)
                {
                    str = segment.Messages[id - segment.Id];
                    return true;
                }
            }
            str = null;
            return false;
        }
        public bool GetMessage(Guid id, out Message str)
        {
            foreach (var segment in Segments)
            {
                if (segment.GetMessage(id, out str))
                    return true;
            }
            str = null;
            return false;
        }
    }
}
