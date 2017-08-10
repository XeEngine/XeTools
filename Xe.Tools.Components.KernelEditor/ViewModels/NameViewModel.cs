using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class NameViewModel
    {
        public IEnumerable<Message> Messages { get; private set; }

        public string Id { get; set; }

        public Message Name { get; set; }

        public Message Description { get; set; }

        public MessageService MessageService { get; private set; }

        public NameViewModel(string id, Guid msgName, Guid msgDescription,
            MessageService messageService)
        {
            MessageService = messageService;
            Messages = messageService.Messages.Values.Select(x => x.Item3);

            Id = id;
            Name = MessageService.GetMessage(msgName);
            Description = MessageService.GetMessage(msgDescription);
        }
    }
}
