using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xe.Game.Kernel;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class BgmsModel : GenericListModel<BgmModel>
	{
		public BgmsModel(IEnumerable<BgmModel> list) :
			base(list)
		{ }

		protected override BgmModel OnNewItem()
		{
			return new BgmModel(new Bgm()
			{
				Id = Guid.NewGuid()
			});
		}

		protected override void OnSelectedItem(BgmModel item)
		{
		}
	}

	public class BgmModel : BaseNotifyPropertyChanged
	{
		public BgmModel(Bgm bgm)
		{
			Item = bgm;
			Loops = new ObservableCollection<BgmLoop>(Item.Loops ?? new List<BgmLoop>());
			Starts = new ObservableCollection<BgmStart>(Item.Starts ?? new List<BgmStart>() { new BgmStart() });
		}

		public Bgm Item { get; }

		public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : "<no name>";

		public string Name
		{
			get => Item.Name;
			set
			{
				Item.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string FileName
		{
			get => Item.FileName;
			set
			{
				Item.FileName = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<BgmLoop> Loops { get; set; }

		public ObservableCollection<BgmStart> Starts { get; set; }
	}
}
