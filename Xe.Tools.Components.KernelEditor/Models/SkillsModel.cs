using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Models;
using Xe.Tools.Services;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class SkillsModel : GenericListModel<SkillModel>
	{
		private readonly MessageService messageService;
		private readonly AnimationService animationService;

		public SkillsModel(
			IEnumerable<Skill> list,
			MessageService messageService,
			AnimationService animationService) :
			base(list?.Select(x => new SkillModel(x, messageService, animationService)))
		{
			this.messageService = messageService;
			this.animationService = animationService;
		}

		public IEnumerable<string> AnimationFileNames =>
			animationService.AnimationFilesData;

		public EnumModel<DamageFormula> Formula { get; } =
			new EnumModel<DamageFormula>();

		public EnumModel<TargetType> Types { get; } =
			new EnumModel<TargetType>();

		public IEnumerable<string> Messages => messageService.Tags;

		protected override SkillModel OnNewItem()
		{
			return new SkillModel(new Skill(), messageService, animationService);
		}

		protected override void OnSelectedItem(SkillModel item)
		{
		}
	}

	public class SkillModel : BaseNotifyPropertyChanged
	{
		private readonly MessageService messageService;
		private readonly AnimationService animationService;

		public SkillModel(
			Skill item,
			MessageService messageService,
			AnimationService animationService)
		{
			Item = item;
			this.messageService = messageService;
			this.animationService = animationService;
		}

		public Skill Item { get; }

		public Guid Id => Item?.Id ?? Guid.Empty;

		public string DisplayName => !string.IsNullOrEmpty(Code) ? Code : "<no name>";

		public string TextName => messageService[Name];

		public string TextDescription => messageService[Description];

		public IEnumerable<string> Animations =>
			animationService.GetAnimationDefinitions(GfxName);

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
			get => Item?.Code;
			set
			{
				Item.Code = value;
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

		public string GfxName
		{
			get => Item?.GfxName;
			set
			{
				Item.GfxName = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(Animations));
			}
		}

		public string GfxAnimation
		{
			get => Item?.GfxAnimation;
			set
			{
				Item.GfxAnimation = value;
				OnPropertyChanged();
			}
		}

		public string Sfx
		{
			get => Item?.Sfx;
			set
			{
				Item.Sfx = value;
				OnPropertyChanged();
			}
		}

		public DamageFormula DamageFormula
		{
			get => Item?.DamageFormula ?? default(DamageFormula);
			set
			{
				Item.DamageFormula = value;
				OnPropertyChanged();
			}
		}

		public TargetType Target
		{
			get => Item?.Target ?? default(TargetType);
			set
			{
				Item.Target = value;
				OnPropertyChanged();
			}
		}

		public int Damage
		{
			get => Item?.Damage ?? 0;
			set
			{
				Item.Damage = value;
				OnPropertyChanged();
			}
		}
	}
}
