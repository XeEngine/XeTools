using System;
using System.Collections.Generic;
using Xe.Game.Kernel;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class SfxsModel : GenericListModel<SfxModel>
	{
		public SfxsModel(IEnumerable<SfxModel> list) :
			base(list)
		{ }

		protected override SfxModel OnNewItem()
		{
			return new SfxModel(new Sfx()
			{
				Id = Guid.NewGuid()
			});
		}

		protected override void OnSelectedItem(SfxModel item)
		{
		}
	}

	public class SfxModel : BaseNotifyPropertyChanged
	{
		public SfxModel(Sfx sfx)
		{
			Sfx = sfx;
		}

		public Sfx Sfx { get; }

		public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : "<no name>";

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
	}
}
