using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class ElementsModel : GenericListModel<ElementModel>
	{
		private readonly MessageService messageService;

		public ElementsModel(IEnumerable<Element> list,
			MessageService messageService) :
			base(list.Take(0x20).Select((x, i) => new ElementModel(i, x, messageService)))
		{
			for (int i = this.list.Count; i < 0x20; i++)
			{
				this.list.Add(new ElementModel(i, new Element(), messageService));
			}

			Messages = messageService.Tags;
			this.messageService = messageService;
		}

		public IEnumerable<string> Messages { get; set; }

		protected override ElementModel OnNewItem()
		{
			return new ElementModel(list.Count, new Element(), messageService);
		}

		protected override void OnSelectedItem(ElementModel item)
		{
		}
	}

	public class ElementModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;

		public ElementModel(int index, Element element,
			MessageService messageService)
		{
			this.Index = index;
			Item = element;
			this.messageService = messageService;
		}

		public Element Item { get; }

		public int Index { get; }

		public string DisplayName => $"{Index.ToString("X02")} {Code}";

		public string TextName => messageService[Name];

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

		public string Name
		{
			get => Item.Name;
			set
			{
				Item.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextName));
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
