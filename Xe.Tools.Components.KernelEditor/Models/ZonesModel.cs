using System;
using System.Collections.Generic;
using Xe.Game.Kernel;
using Xe.Game.Messages;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class ZonesModel : GenericListModel<ZoneModel>
	{
		private readonly MessageService messageService;

		public ZonesModel(
			IEnumerable<ZoneModel> list,
			MessageService messageService) :
			base(list)
		{
			Messages = messageService.Tags;
			this.messageService = messageService;
		}

		public IEnumerable<string> Messages { get; set; }

		protected override ZoneModel OnNewItem()
		{
			return new ZoneModel(new Zone()
			{
				Id = Guid.NewGuid()
			}, messageService);
		}

		protected override void OnSelectedItem(ZoneModel item)
		{
		}
	}

	public class ZoneModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;

		public ZoneModel(Zone zone,
			MessageService messageService)
		{
			Item = zone;
			this.messageService = messageService;
		}

		public Zone Item { get; }

		public string DisplayName => !string.IsNullOrEmpty(Code) ? Code : "<no name>";

		public string TextTitle => messageService[Title];

		public string TextDescription => messageService[Description];

		public string Code
		{
			get => Item.Code;
			set
			{
				Item.Code = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string Title
		{
			get => Item.Title;
			set
			{
				Item.Title = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextTitle));
			}
		}

		public string Description
		{
			get => Item.Description;
			set
			{
				Item.Description = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextDescription));
			}
		}
	}
}
