using Xe.Game.Kernel;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabBgm
{
	public class BgmViewModel : BaseNotifyPropertyChanged
	{
		public Bgm Bgm { get; }

		public string DisplayName => !string.IsNullOrEmpty(Bgm.Name) ? Bgm.Name : "<no name>";

		public string Name
		{
			get => Bgm.Name;
			set
			{
				Bgm.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string FileName
		{
			get => Bgm.FileName;
			set
			{
				Bgm.FileName = value;
				OnPropertyChanged();
			}
		}

		public string Loop
		{
			get => Bgm.Loop.ToString();
			set
			{
				if (int.TryParse(value, out var result))
					Bgm.Loop = result;
				else
					OnPropertyChanged();
			}
		}

		public BgmViewModel()
		{
			Bgm = new Bgm();
		}

		public BgmViewModel(Bgm bgm)
		{
			Bgm = bgm;
		}
	}
}
