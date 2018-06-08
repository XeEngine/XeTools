using System.Collections.Generic;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class PlayersModel : GenericListModel<PlayerModel>
	{
		private readonly MessageService messageService;
		private readonly AnimationService animationService;

		public PlayersModel(
			IEnumerable<Actor> players,
			MessageService messageService,
			AnimationService animationService) :
			base(players.Select(x => new PlayerModel(x, messageService)))
		{
			this.messageService = messageService;
			this.animationService = animationService;
		}

		public IEnumerable<string> Tags => messageService.Tags;

		public IEnumerable<string> Animations => animationService.AnimationFilesData;

		protected override PlayerModel OnNewItem()
		{
			return new PlayerModel(new Actor(), messageService);
		}

		protected override void OnSelectedItem(PlayerModel item)
		{
		}
	}

	public class PlayerModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;

		public PlayerModel(Actor player, MessageService messageService)
		{
			Player = player;
			this.messageService = messageService;
		}

		public Actor Player { get; }

		public string DisplayName => !string.IsNullOrEmpty(Code) ? Code : "<no name>";

		public string TextName => messageService[Name];

		public string TextDescription => messageService[Description];

		public string Code
		{
			get => Player?.Code;
			set
			{
				Player.Code = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public string Name
		{
			get => Player?.Name;
			set
			{
				Player.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextName));
			}
		}

		public string Description
		{
			get => Player?.Description;
			set
			{
				Player.Description = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(TextDescription));
			}
		}

		public string Animation
		{
			get => Player?.Animation;
			set
			{
				Player.Animation = value;
				OnPropertyChanged();
			}
		}

		public bool Enabled
		{
			get => Player?.Enabled ?? default(bool);
			set
			{
				Player.Enabled = value;
				OnPropertyChanged();
			}
		}

		public bool Locked
		{
			get => Player?.Locked ?? default(bool);
			set
			{
				Player.Locked = value;
				OnPropertyChanged();
			}
		}

		public int Level
		{
			get => Player?.Level ?? default(int);
			set
			{
				Player.Level = value;
				OnPropertyChanged();
			}
		}

		public int Experience
		{
			get => Player?.Experience ?? default(int);
			set
			{
				Player.Experience = value;
				OnPropertyChanged();
			}
		}

		public int Health
		{
			get => Player?.Health ?? default(int);
			set
			{
				Player.Health = value;
				OnPropertyChanged();
			}
		}

		public int Mana
		{
			get => Player?.Mana ?? default(int);
			set
			{
				Player.Mana = value;
				OnPropertyChanged();
			}
		}

		public int HealthCurrent
		{
			get => Player?.HealthCurrent ?? default(int);
			set
			{
				Player.HealthCurrent = value;
				OnPropertyChanged();
			}
		}

		public int ManaCurrent
		{
			get => Player?.ManaCurrent ?? default(int);
			set
			{
				Player.ManaCurrent = value;
				OnPropertyChanged();
			}
		}

		public int Attack
		{
			get => Player?.Attack ?? default(int);
			set
			{
				Player.Attack = value;
				OnPropertyChanged();
			}
		}

		public int Defense
		{
			get => Player?.Defense ?? default(int);
			set
			{
				Player.Defense = value;
				OnPropertyChanged();
			}
		}

		public int AttackSpecial
		{
			get => Player?.AttackSpecial ?? default(int);
			set
			{
				Player.AttackSpecial = value;
				OnPropertyChanged();
			}
		}

		public int DefenseSpecial
		{
			get => Player?.DefenseSpecial ?? default(int);
			set
			{
				Player.DefenseSpecial = value;
				OnPropertyChanged();
			}
		}
	}
}
