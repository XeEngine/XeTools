using Xe.Game.PalAnimations;

namespace Xe.Tools.Components.AnimatedPaletteEditor.ViewModels
{
	public class MainViewModel
	{
		private PalAnimation animation;

		public MainViewModel()
		{
			Actions = new ActionsViewModel();
		}

		public PalAnimation Animation
		{
			get => animation;
			set
			{
				animation = value;
				Actions = new ActionsViewModel(animation?.Actions);
			}
		}

		public ActionsViewModel Actions { get; set; }
	}
}
