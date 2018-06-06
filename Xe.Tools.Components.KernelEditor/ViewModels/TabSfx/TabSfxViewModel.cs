using System.Collections.Generic;
using Xe.Game.Kernel;
using Xe.Tools.Components.KernelEditor.Models;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabSfx
{
	public class TabSfxViewModel : GenericListModel<Sfx, SfxViewModel>
	{
		public TabSfxViewModel(KernelData kData) :
			base(kData.Sfxs = kData.Sfxs ?? new List<Sfx>())
		{ }

		protected override Sfx NewItem()
		{
			return new Sfx();
		}

		protected override SfxViewModel NewViewModel(Sfx item)
		{
			return new SfxViewModel(item);
		}

		protected override void OnSelectedItem(SfxViewModel item)
		{

		}
	}
}
