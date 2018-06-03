using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.MessagesEditor.ViewModels
{
	public class MessagesViewModel : BaseNotifyPropertyChanged
	{
		private readonly IProjectFile file;
		private MessageViewModel selectedMessage;

		public MessagesViewModel(IProjectFile file, Context context)
		{
			this.file = file;

			AddItem = new RelayCommand(param =>
			{
				var tag = "NEWTAG_" + Guid.NewGuid().ToString("N");
				var vm = new MessageViewModel(context.MessagesService, file.Name, tag);
				Messages.Add(vm);
				SelectedMessage = vm;

				OnNotifyModify.Invoke();
			});

			RemoveItem = new RelayCommand(param =>
			{
				context.MessagesService.RemoveMessage(file.Name,
					SelectedMessage.Tag, Language.English);
				context.MessagesService.RemoveMessage(file.Name,
					SelectedMessage.Tag, Language.Italian);
				Messages.Remove(selectedMessage);
				SelectedMessage = null;

				OnNotifyModify.Invoke();
			}, x => selectedMessage != null);

			var msgs = context.MessagesService
				.MessageContainers
				.SelectMany(entry => entry.Item2
					.Messages.GroupBy(y => y.Tag)
					.Select(tag => new MessageViewModel(
						context.MessagesService,
						tag.AsEnumerable(), file.Name, tag.Key)
					)
				);

			Messages = new ObservableCollection<MessageViewModel>(msgs);
			foreach (var msg in Messages)
			{
				msg.OnNotifyModify += () => OnNotifyModify?.Invoke();
			}

			context.MessagesService.OnMessageChanged += MessagesService_OnMessageChanged;
		}

		public event NotifyModify OnNotifyModify;

		public RelayCommand AddItem { get; set; }

		public RelayCommand RemoveItem { get; set; }

		public string SearchTerms { get; set; }

		public ObservableCollection<MessageViewModel> Messages { get; set; }

		public MessageViewModel SelectedMessage
		{
			get => selectedMessage;
			set
			{
				selectedMessage = value;

				RemoveItem.CanExecute(selectedMessage);
				OnPropertyChanged(nameof(RemoveItem));
				OnPropertyChanged();
			}
		}

		private void MessagesService_OnMessageChanged(string tag)
		{
			if (SelectedMessage?.Tag == tag)
			{
				SelectedMessage.OnAllPropertiesChanged();
			}
		}
	}
}
