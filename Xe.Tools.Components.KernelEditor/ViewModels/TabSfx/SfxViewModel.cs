using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabSfx
{
	public class SfxViewModel : BaseNotifyPropertyChanged
	{
		public Sfx Sfx { get; }

		public string DisplayName => !string.IsNullOrEmpty(Sfx.Name) ? Sfx.Name : "<no name>";

		public string Name
		{
			get => Sfx.Name;
			set
			{
				Sfx.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string FileName
		{
			get => Sfx.FileName;
			set
			{
				Sfx.FileName = value;
				OnPropertyChanged();
			}
		}

		public SfxViewModel()
		{
			Sfx = new Sfx();
		}

		public SfxViewModel(Sfx sfx)
		{
			Sfx = sfx;
		}
	}
}
