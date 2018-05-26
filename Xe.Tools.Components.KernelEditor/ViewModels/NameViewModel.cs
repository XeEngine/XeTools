using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class NameViewModel : BaseNotifyPropertyChanged
    {
		private string mTagName, mTagDescription;

		public MessageService MessageService { get; }

		public string Id { get; set; }

		public string TagName
		{
			get => mTagName;
			set
			{
				mTagName = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Name));
			}
		}

		public string TagDescription
		{
			get => mTagDescription;
			set
			{
				mTagDescription = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Description));
			}
		}

		public string Name => MessageService[mTagName];

		public string Description => MessageService[mTagDescription];

        public NameViewModel(string id, string tagName, string tagDescription,
            MessageService messageService)
        {
            MessageService = messageService;
			messageService.OnLanguageChanged += OnLanguageChanged;
			messageService.OnMessageChanged += OnMessageChanged;

			Id = id;
			mTagName = tagName;
			mTagDescription = tagDescription;
        }

		private void OnLanguageChanged(Language language)
		{
			OnPropertyChanged(Name);
			OnPropertyChanged(Description);
		}

		private void OnMessageChanged(string tag)
		{
			if (mTagName == tag)
				OnPropertyChanged(Name);
			if (mTagDescription == tag)
				OnPropertyChanged(Description);
		}
	}
}
