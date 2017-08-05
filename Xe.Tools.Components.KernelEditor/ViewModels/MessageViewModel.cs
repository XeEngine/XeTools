using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Messages;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class MessageViewModel
    {
        public Message Message { get; private set; }

        public string Category { get; private set; }

        public Guid Id => Message.UID;

        public string English
        {
            get => Message.En;
            set => Message.En = value;
        }

        public string Italian
        {
            get => Message.It;
            set => Message.It = value;
        }

        public string French
        {
            get => Message.Fr;
            set => Message.Fr = value;
        }

        public string German
        {
            get => Message.De;
            set => Message.De = value;
        }

        public string Spanish
        {
            get => Message.Sp;
            set => Message.Sp = value;
        }

        public string Japanese
        {
            get => "Not implemented yet";
        }

        public MessageViewModel(string category, Message message)
        {
            Category = category;
            Message = message;
        }

        public override string ToString()
        {
            return English ?? Italian ?? French ?? German ?? Spanish ?? Japanese ?? "<null>";
        }
    }
}
