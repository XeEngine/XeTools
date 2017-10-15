using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf;
using static Xe.Tools.Project;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class TabMessagesViewModel : BaseNotifyPropertyChanged
    {
        public class MessageViewModel
        {
            public IProjectFile ProjectFile { get; private set; }

            public string Category { get; private set; }

            public Message Message { get; private set; }

            public string File { get; private set; }

            public string Text
            {
                get => Message.En;
                set => Message.En = value;
            }

            public MessageViewModel(IProjectFile file, string category, Message message)
            {
                ProjectFile = file;
                Category = category;
                Message = message;
                File = Path.GetFileName(file.Name)
                    .Split('.')[0];
            }
        }

        public MessageService MessageService { get; private set; }

        public IEnumerable<MessageViewModel> Messages { get; private set; }

        public TabMessagesViewModel(MessageService messageService)
        {
            MessageService = messageService;
            Messages = MessageService.Messages.Select(x =>
                new MessageViewModel(x.Value.Item1, x.Value.Item2, x.Value.Item3))
                .OrderBy(x => x.ProjectFile).ThenBy(x => x.Category);
        }
    }
}
