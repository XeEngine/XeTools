using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game.PalAnimations;
using Xe.Tools.Components.AnimatedPaletteEditor.Services;
using Xe.Tools.Wpf.Commands;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimatedPaletteEditor.ViewModels
{
	public class MainViewModel : BaseNotifyPropertyChanged
	{
		private PalAnimation animation;
		private BitmapSource bitmapSource;
		private BitmapSource bitmapSpritesheet;

		public MainViewModel()
		{
			Actions = new ActionsViewModel();

			PlayActionCommand = new RelayCommand(x =>
			{
				PaletteAnimator.ResetPalette();
				PaletteAnimator.PlayAction(Actions?.SelectedItem.Action);
			}, x => true);

			PlayCommandCommand = new RelayCommand(x =>
			{
				PaletteAnimator.ResetPalette();
				PaletteAnimator.PlayCommand(Actions?.SelectedItem?.SelectedItem?.Command);
			}, x => true);

			StopCommand = new RelayCommand(x =>
			{
				PaletteAnimator.ResetPalette();
				PaletteAnimator.PlayAction((PalAction)null);
			}, x => true);

			LoadImageCommand = new RelayCommand(x =>
			{
				var fd = FileDialog.Factory(null, FileDialog.Behavior.Open, FileDialog.Type.ImagePng);
				if (fd.ShowDialog() == true)
				{
					var bitmap = new BitmapImage(new System.Uri(fd.FileName));
					if (bitmap.Format == PixelFormats.Indexed8)
					{
						PaletteAnimator.LoadPalette(bitmap);
						Spritesheet = bitmap;
					}
					else
					{
						MessageBox.Show("The selected image does not contain any palette.",
							"Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
				}
			}, x => true);
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

		public PaletteAnimator PaletteAnimator => Actions?.PaletteAnimator;

		public BitmapSource Palette
		{
			get => bitmapSource;
			set
			{
				bitmapSource = value;
				OnPropertyChanged();
			}
		}

		public BitmapSource Spritesheet
		{
			get => bitmapSpritesheet;
			set
			{
				bitmapSpritesheet = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand PlayActionCommand { get; set; }

		public RelayCommand PlayCommandCommand { get; set; }

		public RelayCommand StopCommand { get; set; }

		public RelayCommand LoadImageCommand { get; set; }
	}
}
