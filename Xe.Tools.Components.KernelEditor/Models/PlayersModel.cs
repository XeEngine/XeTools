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
			Item = player;
			this.messageService = messageService;
		}

		public Actor Item { get; }

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

		public string Animation
		{
			get => Item?.Animation;
			set
			{
				Item.Animation = value;
				OnPropertyChanged();
			}
		}

		public bool Enabled
		{
			get => Item?.Enabled ?? default(bool);
			set
			{
				Item.Enabled = value;
				OnPropertyChanged();
			}
		}

		public bool Locked
		{
			get => Item?.Locked ?? default(bool);
			set
			{
				Item.Locked = value;
				OnPropertyChanged();
			}
		}

		public int Level
		{
			get => Item?.Level ?? default(int);
			set
			{
				Item.Level = value;
				OnPropertyChanged();
			}
		}

		public int Experience
		{
			get => Item?.Experience ?? default(int);
			set
			{
				Item.Experience = value;
				OnPropertyChanged();
			}
		}

		public int Health
		{
			get => Item?.Health ?? default(int);
			set
			{
				Item.Health = value;
				OnPropertyChanged();
			}
		}

		public int Mana
		{
			get => Item?.Mana ?? default(int);
			set
			{
				Item.Mana = value;
				OnPropertyChanged();
			}
		}

		public int HealthCurrent
		{
			get => Item?.HealthCurrent ?? default(int);
			set
			{
				Item.HealthCurrent = value;
				OnPropertyChanged();
			}
		}

		public int ManaCurrent
		{
			get => Item?.ManaCurrent ?? default(int);
			set
			{
				Item.ManaCurrent = value;
				OnPropertyChanged();
			}
		}

		public int Attack
		{
			get => Item?.Attack ?? default(int);
			set
			{
				Item.Attack = value;
				OnPropertyChanged();
			}
		}

		public int Defense
		{
			get => Item?.Defense ?? default(int);
			set
			{
				Item.Defense = value;
				OnPropertyChanged();
			}
		}

		public int AttackSpecial
		{
			get => Item?.AttackSpecial ?? default(int);
			set
			{
				Item.AttackSpecial = value;
				OnPropertyChanged();
			}
		}

		public int DefenseSpecial
		{
			get => Item?.DefenseSpecial ?? default(int);
			set
			{
				Item.DefenseSpecial = value;
				OnPropertyChanged();
			}
		}

		public int DropHp
		{
			get => Item?.Drop?.Hp ?? default(int);
			set
			{
				Item.Drop.Hp = value;
				OnPropertyChanged();
			}
		}

		public int DropMp
		{
			get => Item?.Drop?.Mp ?? default(int);
			set
			{
				Item.Drop.Mp = value;
				OnPropertyChanged();
			}
		}

		public int DropExp
		{
			get => Item?.Drop?.Exp ?? default(int);
			set
			{
				Item.Drop.Exp = value;
				OnPropertyChanged();
			}
		}

		public int DropMoney
		{
			get => Item?.Drop?.Money ?? default(int);
			set
			{
				Item.Drop.Money = value;
				OnPropertyChanged();
			}
		}
	}
}
