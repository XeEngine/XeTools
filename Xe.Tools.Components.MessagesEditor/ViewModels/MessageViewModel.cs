using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MessagesEditor.ViewModels
{
	public delegate void NotifyModify();

	public class MessageViewModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;
		private Message msgEn, msgIt;
		private string fileName;
		private string tag;

		public MessageViewModel(
			MessageService messageService,
			string fileName,
			string tag) :
			this(messageService,
				messageService.MessageContainers
				.Where(x => x.Item1.Name == fileName)
				.SelectMany(x => x.Item2.GetMessagesByTag(tag)),
				fileName, tag)
		{ }

		public MessageViewModel(
			MessageService messageService,
			IEnumerable<Message> messages,
			string fileName,
			string tag)
		{
			this.messageService = messageService;
			this.fileName = fileName;
			this.tag = tag;
			msgEn = messages.FirstOrDefault(x => x.Language == Language.English);
			msgIt = messages.FirstOrDefault(x => x.Language == Language.Italian);
		}

		public event NotifyModify OnNotifyModify;

		public string File => fileName;

		public string Languages
		{
			get
			{
				var langs = new List<string>();
				if (!string.IsNullOrEmpty(msgEn?.Text)) langs.Add("En");
				if (!string.IsNullOrEmpty(msgIt?.Text)) langs.Add("It");

				return string.Join(" ", langs.ToArray());
			}
		}

		public string Tag
		{
			get => tag;
			set
			{
				tag = value;
				if (msgEn != null) msgEn.Tag = tag;
				if (msgIt != null) msgIt.Tag = tag;

				OnPropertyChanged();
				OnNotifyModify?.Invoke();
			}
		}

		public string English
		{
			get => msgEn?.Text;
			set
			{
				SetText(ref msgEn, Language.English, value);
				OnPropertyChanged(nameof(English));
				OnPropertyChanged(nameof(Languages));
			}
		}

		public string Italian
		{
			get => msgIt?.Text;
			set
			{
				SetText(ref msgIt, Language.Italian, value);
				OnPropertyChanged(nameof(Italian));
				OnPropertyChanged(nameof(Languages));
			}
		}

		private void SetText(ref Message message, Language language, string text)
		{
			if (message == null)
			{
				message = messageService.SetMessage(fileName, Tag, language, text);
			}
			else
			{
				Debug.Assert(message.Language == language);
				message.Text = text;
			}

			OnNotifyModify?.Invoke();
		}
	}
}
