using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class NameViewModel
    {
        public class MsgVM
        {
            public Guid Id { get; set; }

            public MessagesViewModel Messages { get; private set; }

            internal MsgVM(Guid id, MessagesViewModel messages)
            {
                Id = id;
                Messages = messages;
            }

            public override string ToString()
            {
                return Messages?[Id]?.English ?? "NOMSG";
            }
        }

        public string Id { get; set; }

        public MessageViewModel Name { get; set; }

        public MessageViewModel Description { get; set; }

        public MessageService MessageService { get; private set; }

        public NameViewModel(string id, Guid msgName, Guid msgDescription, MessageService messageService)
        {
            Id = id;
            Name = new MessageViewModel("skill", messageService.GetMessage(msgName));
            Description = new MessageViewModel("skill", messageService.GetMessage(msgDescription));
            MessageService = messageService;
        }
    }
}
