using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Xe.Game.PalAnimations;
using Xe.Tools.Components.AnimatedPaletteEditor.Models;
using Xe.Tools.Components.AnimatedPaletteEditor.Services;
using Xe.Tools.Wpf.Commands;
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
			: this(list?.Select(x => new CommandsViewModel(x)))
		{
		}

		public ActionsViewModel(IEnumerable<CommandsViewModel> list)
			: base(list)
		{
			PaletteAnimator = new PaletteAnimator();

			DuplicateCommand = new RelayCommand(x =>
			{
				var serializer = new XmlSerializer(SelectedItem.Action.GetType());
				using (var memStream = new MemoryStream())
				{
					serializer.Serialize(memStream, SelectedItem.Action);
					memStream.Position = 0;
					var newItem = serializer.Deserialize(memStream);
					Items.Add(new CommandsViewModel(newItem as PalAction));
				}
			}, x => IsItemSelected);
		}

		public PaletteAnimator PaletteAnimator { get; }

		public RelayCommand DuplicateCommand { get; set; }

		protected override CommandsViewModel OnNewItem()
		{
			return new CommandsViewModel();
		}

		protected override void OnSelectedItem(CommandsViewModel item)
		{
			base.OnSelectedItem(item);

			PaletteAnimator.ResetPalette();
			PaletteAnimator.PlayAction(item?.Action?.Commands);
			OnPropertyChanged(nameof(DuplicateCommand));
		}
	}
}
