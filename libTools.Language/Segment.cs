using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe;

namespace libTools.Language
{
    public class Segment : IDeepCloneable
    {
        public ushort Id;
        public string Name;
        public List<Message> Messages;

        public Segment()
        {
            Messages = new List<Message>();
        }
        public override string ToString()
        {
            if (Messages.Count > 1)
                return string.Format("[{0}-{1}] {2}", Id, Id + Messages.Count - 1, Name);
            else if (Messages.Count == 0)
                return string.Format("[empty] {0}", Name);
            else
                return string.Format("[{0}] {1}", Id, Name);
        }

        public object DeepClone()
        {
            var item = new Segment();
            item.Id = Id;
            item.Name = Name != null ? Name.Clone() as string : null;
            item.Messages = new List<Message>();
            foreach (var o in Messages) item.Messages.Add(o.DeepClone() as Message);
            return item;
        }

        public int GetMessageIndex(Guid id)
        {
            for (int i = 0; i < Messages.Count; i++)
            {
                if (Messages[i].UID == id)
                    return i;
            }
            return -1;
        }

        public bool GetMessage(int index, out Message message)
        {
            if (index < 0 || index >= Messages.Count)
            {
                message = null;
                return false;
            }
            message = Messages[index];
            return true;
        }
        public bool GetMessage(Guid id, out Message message)
        {
            foreach (var item in Messages)
                if (item.UID == id)
                {
                    message = item;
                    return true;
                }
            message = null;
            return false;
        }
    }
}
