using System.Collections.Generic;
using System.Linq;
using Xe.Game.PalAnimations;
using Xe.Tools.Components.AnimatedPaletteEditor.Models;
using Xe.Tools.Wpf.Models;

namespace Xe.Tools.Components.AnimatedPaletteEditor.ViewModels
{
	public class CommandsViewModel : GenericListModel<CommandModel>
	{
		public CommandsViewModel()
			: this(new PalAction()
			{
				Commands = new List<PalCommand>()
			})
		{ }

		public CommandsViewModel(PalAction action)
			: base(action.Commands.Select(x => new CommandModel(x)))
		{
			Action = action;
		}

		protected override CommandModel OnNewItem()
		{
			return new CommandModel();
		}

		public PalAction Action { get; private set; }

		public string Name
		{
			get => Action?.Name;
			set
			{
				Action.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string DisplayName => Name ?? "<unnamed>";
	}
}
