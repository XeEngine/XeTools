using System.Collections.Generic;
using Xe.Game;
using Xe.Game.PalAnimations;
using Xe.Tools.Models;

namespace Xe.Tools.Components.AnimatedPaletteEditor.Models
{
	public class CommandModel : BaseNotifyPropertyChanged
	{
		public CommandModel() :
			this(new PalCommand()
			{
				Parameters = new List<object>()
			})
		{ }

		public CommandModel(PalCommand command)
		{
			Command = command;

			CommandTypes = new EnumModel<CommandType>();
			EaseTypes = new EnumModel<Ease>();
			SelectedCommandChanged(CommandType);
		}

		public PalCommand Command { get; set; }

		public string DisplayName => CommandType.ToString() ?? "<null>";

		public EnumModel<CommandType> CommandTypes { get; }

		public EnumModel<Ease> EaseTypes { get; }

		public string CommandDescription { get; private set; }

		public CommandType CommandType
		{
			get => Command?.Command ?? CommandType.None;
			set => SelectedCommandChanged(Command.Command = value);
		}

		public Ease Ease
		{
			get => Command?.Ease ?? Ease.Linear;
			set => Command.Ease = value;
		}

		public double Start
		{
			get => Command?.Start ?? 0.0;
			set
			{
				Command.Start = (float)value;
				OnPropertyChanged();
			}
		}

		public double End
		{
			get => Command?.End ?? 0.0;
			set
			{
				Command.End = (float)value;
				OnPropertyChanged();
			}
		}

		public double Loop
		{
			get => Command?.Loop ?? 0.0;
			set
			{
				Command.Loop = (float)value;
				OnPropertyChanged();
			}
		}

		public bool LoopEnabled
		{
			get => Loop == float.MaxValue;
			set
			{
				Loop = value ? float.MaxValue : 0.0;
				OnPropertyChanged();
			}
		}

		private void SelectedCommandChanged(CommandType command)
		{
			if (CommandDescriptor.Commands
				.TryGetValue(command, out var descriptor))
			{
				CommandDescription = descriptor.Description;
			}
			else
			{
				CommandDescription = "<unknown>";
			}

			OnPropertyChanged(nameof(DisplayName));
			OnPropertyChanged(nameof(CommandDescription));
		}
	}
}
