using System.Collections.Generic;
using System.Linq;
using Xe.Game.PalAnimations;
using Xe.Tools.Components.AnimatedPaletteEditor.Models;
using Xe.Tools.Wpf.Models;

namespace Xe.Tools.Components.AnimatedPaletteEditor.ViewModels
{
	public class ActionsViewModel : GenericListModel<CommandsViewModel>
	{
		public ActionsViewModel()
			: this((IEnumerable<CommandsViewModel>)null)
		{

		}

		public ActionsViewModel(IEnumerable<PalAction> list)
			: this(list.Select(x => new CommandsViewModel(x)))
		{
		}

		public ActionsViewModel(IEnumerable<CommandsViewModel> list)
			: base(list)
		{
		}

		protected override CommandsViewModel OnNewItem()
		{
			return new CommandsViewModel();
		}
	}
}
