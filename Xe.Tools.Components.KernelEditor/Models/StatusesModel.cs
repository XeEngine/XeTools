using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class StatusesModel : GenericListModel<StatusModel>
	{
		private readonly MessageService messageService;

		public StatusesModel(IEnumerable<Status> list,
			MessageService messageService) :
			base(list?.Take(0x20).Select((x, i) => new StatusModel(i, x, messageService)))
		{
			for (int i = this.list.Count; i < 0x20; i++)
			{
				this.list.Add(new StatusModel(i, new Status(), messageService));
			}

			Messages = messageService.Tags;
			this.messageService = messageService;
		}

		public IEnumerable<string> Messages { get; set; }

		protected override StatusModel OnNewItem()
		{
			return new StatusModel(list.Count, new Status(), messageService);
		}

		protected override void OnSelectedItem(StatusModel item)
		{
		}
	}

	public class StatusModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;

		public StatusModel(int index, Status element,
			MessageService messageService)
		{
			Index = index;
			Status = element;
			this.messageService = messageService;
		}

		public Status Status { get; }

		public int Index { get; }

		public string DisplayName => $"{Index.ToString("X02")} {Code}";

		public string TextName => messageService[Name];

		public string TextDescription => messageService[Description];

		public string Code
		{
			get => Status.Code;
			set
			{
				Status.Code = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string Name
		{
			get => Status.Name;
			set
			{
				Status.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextName));
			}
		}

		public string Description
		{
			get => Status.Description;
			set
			{
				Status.Description = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextDescription));
			}
		}
	}
}
