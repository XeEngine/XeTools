using System.Collections.Generic;
using Xe.Game.Kernel;
using Xe.Tools.Components.KernelEditor.ViewModels.Generics;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabBgm
{
	public class TabBgmViewModel : TabGenericListViewModel<Bgm, BgmViewModel>
	{
		public TabBgmViewModel(KernelData kData) :
			base(kData.Bgms = kData.Bgms ?? new List<Bgm>())
		{ }

		protected override Bgm NewItem()
		{
			return new Bgm();
		}

		protected override BgmViewModel NewViewModel(Bgm item)
		{
			return new BgmViewModel(item);
		}

		protected override void OnSelectedItem(BgmViewModel item)
		{

		}
	}
}
