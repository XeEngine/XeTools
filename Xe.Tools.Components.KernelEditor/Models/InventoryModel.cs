using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Models;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class InventoryEntriesModel : GenericListModel<InventoryEntryModel>
	{
		private readonly MessageService messageService;
		private readonly KernelData Kernel;

		public InventoryEntriesModel(
			IEnumerable<InventoryItem> list,
			KernelData kernel,
			MessageService messageService) :
			base(list?.Select(x => new InventoryEntryModel(x, messageService)))
		{
			Kernel = kernel;
			this.messageService = messageService;
		}

		public EnumModel<InventoryItemType> Types { get; } =
			new EnumModel<InventoryItemType>();

		public EnumModel<InventoryItemEffect> Effects { get; } =
			new EnumModel<InventoryItemEffect>();

		public IEnumerable<string> Messages => messageService.Tags;

		protected override InventoryEntryModel OnNewItem()
		{
			return new InventoryEntryModel(new InventoryItem()
			{
				Id = Guid.NewGuid()
			}, messageService);
		}

		protected override void OnSelectedItem(InventoryEntryModel item)
		{
		}
	}

	public class InventoryEntryModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;

		public InventoryEntryModel(InventoryItem item, MessageService messageService)
		{
			Item = item;
			this.messageService = messageService;
		}

		public InventoryItem Item { get; }

		public string DisplayName => !string.IsNullOrEmpty(Code) ? Code : "<no name>";

		public string TextName => messageService[Name];

		public string TextDescription => messageService[Description];

		public string Code
		{
			get => Item?.Code;
			set
			{
				Item.Code = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string Name
		{
			get => Item?.Name;
			set
			{
				Item.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextName));
			}
		}

		public string Description
		{
			get => Item?.Description;
			set
			{
				Item.Description = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextDescription));
			}
		}

		public InventoryItemType Type
		{
			get => Item?.Type ?? default(InventoryItemType);
			set
			{
				Item.Type = value;
				OnPropertyChanged();
			}
		}

		public InventoryItemEffect Effect
		{
			get => Item?.Effect ?? default(InventoryItemEffect);
			set
			{
				Item.Effect = value;
				OnPropertyChanged();
			}
		}

		public Guid SkillId
		{
			get => Item?.SkillId ?? Guid.Empty;
			set
			{
				Item.SkillId = value;
				OnPropertyChanged();
			}
		}
	}
}
