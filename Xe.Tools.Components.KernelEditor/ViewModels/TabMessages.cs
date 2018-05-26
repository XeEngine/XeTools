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
        public MessageService MessageService { get; private set; }

        public TabMessagesViewModel(MessageService messageService)
        {
            MessageService = messageService;
        }
    }
}
