using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Messages
{
    public class MessageContainer
    {
        public static Language CurrentLanguage { get; set; }

        public List<Segment> Segments { get; set; }

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
            foreach (var item in Segments)
            {
                var index = item.GetMessageIndex(id);
                if (index >= 0)
                    return (ushort)(item.Id + index);
            }
            return 0;
        }
        public bool GetMessage(ushort id, out string str)
        {
            if (GetMessage(id, out Message msg))
            {
                str = msg.ToString();
                return true;
            }
            str = null;
            return false;
        }
        public string GetMessage(ushort id)
        {
            return GetMessage(id, out string str) ? str : null;
        }
        public string GetMessage(Guid id)
        {
            return GetMessage(id, out Message msg) ? msg.Text : null;
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
