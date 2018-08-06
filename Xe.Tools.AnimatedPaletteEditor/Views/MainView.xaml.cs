using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game.PalAnimations;
using Xe.Tools.Components.AnimatedPaletteEditor.Services;
using Xe.Tools.Components.AnimatedPaletteEditor.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.AnimatedPaletteEditor.Views
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : WindowEx
	{
		const float START = 1.0f;

		private class ByteViewModel
		{
			private readonly PalCommand command;
			private readonly int paramIndex;

			public ByteViewModel(PalCommand command, int paramIndex)
			{
				this.command = command;
				this.paramIndex = paramIndex;
			}

			public int Value
			{
				get => (byte)command.Parameters[paramIndex];
				set => command.Parameters[paramIndex] = (byte)value;
			}
		}

		private class IntViewModel
		{
			private readonly PalCommand command;
			private readonly int paramIndex;

			public IntViewModel(PalCommand command, int paramIndex)
			{
				this.command = command;
				this.paramIndex = paramIndex;
			}

			public int Value
			{
				get => (int)command.Parameters[paramIndex];
				set => command.Parameters[paramIndex] = value;
			}
		}

		private class FloatViewModel
		{
			private readonly PalCommand command;
			private readonly int paramIndex;

			public FloatViewModel(PalCommand command, int paramIndex)
			{
				this.command = command;
				this.paramIndex = paramIndex;
			}

			public double Value
			{
				get => (float)command.Parameters[paramIndex];
				set => command.Parameters[paramIndex] = (float)value;
			}
		}

		private readonly Timer timer;
		private readonly Stopwatch stopwatch;
		private PalAnimation paletteAnimation;

		public MainView(IProject project, IProjectFile file) :
			this(null)
		{
			Project = project;
			ProjectFile = file;

			WorkingFileName = ProjectFile.FullPath;
			using (var reader = File.OpenText(WorkingFileName))
			{
				PaletteAnimation = JsonConvert.DeserializeObject<PalAnimation>(reader.ReadToEnd());
			}
		}

		public MainView() :
			this(new PalAnimation()
			{
				Actions = new List<PalAction>
				{
					new PalAction()
					{
						Name = "Rot Test",
						Commands = new List<PalCommand>()
						{
							new PalCommand()
							{
								Command = CommandType.RotateRight,
								Ease = Game.Ease.Linear,
								Start = 0.0f,
								End = 0.75f,
								Loop = 0.0f,
								Parameters = new List<object>()
								{
									(byte)4,(byte)4
								}
							}
						}
					},
					new PalAction()
					{
						Name = "debug",
						Commands = new List<PalCommand>()
						{
							new PalCommand()
							{
								Command = CommandType.SetColor,
								Ease = Game.Ease.Linear,
								Start = START + 0.0f,
								End = START + 2.0f,
								Parameters = new List<object>()
								{
									(byte)0, (byte)0, (byte)255, (byte)0
								}
							},
							new PalCommand()
							{
								Command = CommandType.SetColor,
								Ease = Game.Ease.Quadratic,
								Start = START + 0.0f,
								End = START + 2.0f,
								Parameters = new List<object>()
								{
									(byte)1, (byte)0, (byte)255, (byte)0
								}
							},
							new PalCommand()
							{
								Command = CommandType.AddHue,
								Ease = Game.Ease.Linear,
								Start = START + 0.0f,
								End = START + 2.5f,
								Parameters = new List<object>()
								{
									(byte)2, 360.0f
								}
							},
							new PalCommand()
							{
								Command = CommandType.AddHue,
								Ease = Game.Ease.SineEaseInOut,
								Start = START + 0.0f,
								End = START + 2.5f,
								Parameters = new List<object>()
								{
									(byte)3, 360.0f
								}
							},
							new PalCommand()
							{
								Command = CommandType.Invert,
								Ease = Game.Ease.Linear,
								Start = START + 0.0f,
								End = START + 2.5f,
								Parameters = new List<object>()
								{
									(byte)4, 1.0f
								}
							},
						}
					}
				}
			})
		{ }

		public MainView(PalAnimation paletteAnimation)
		{
			InitializeComponent();

			PaletteAnimation = paletteAnimation;

			stopwatch = new Stopwatch();
			timer = new Timer()
			{
				Interval = 1.0 / 60.0,
				Enabled = true
			};
			timer.Elapsed += Timer_Elapsed;

			Closing += (s, e) => timer.Enabled = false;
		}

		public IProject Project { get; private set; }

		public IProjectFile ProjectFile { get; private set; }

		private string WorkingFileName { get; set; }

		private string BasePath => Path.GetDirectoryName(WorkingFileName);

		private PaletteAnimator PaletteAnimator => (DataContext as MainViewModel)?.PaletteAnimator;

		private PalAnimation PaletteAnimation
		{
			get => paletteAnimation;
			set
			{
				paletteAnimation = value;
				DataContext = new MainViewModel()
				{
					Palette = GenerateBitmapPalette(),
					Actions = new ActionsViewModel(paletteAnimation?.Actions)
				};
			}
		}

		private void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				PaletteAnimator.Update(1.0 / 60.0);
				if (DataContext is MainViewModel vm)
				{
					vm.Palette = PaletteAnimator.ApplyPalette(vm.Palette);
					if (vm.Spritesheet != null)
					{
						vm.Spritesheet = PaletteAnimator.ApplyPalette(vm.Spritesheet);
					}
				}
			}));
		}

		private void Command_Selected(object sender, RoutedEventArgs e)
		{
			try
			{
				if (DataContext is MainViewModel vm)
				{
					var cmd = vm.Actions?.SelectedItem?.SelectedItem?.Command;
					if (cmd != null && CommandDescriptor.Commands.TryGetValue(cmd.Command, out var desc))
					{
						if (CheckParameterTypes(cmd, desc) == false)
						{
							var paramsLength = desc.Parameters?.Length ?? 0;
							cmd.Parameters = new List<object>(paramsLength);
							for (int i = 0; i < paramsLength; i++)
							{
								cmd.Parameters.Add(Activator.CreateInstance(desc.Parameters[i].Type));
							}
						}

						commandParametersPanel.Children.Clear();
						AddControls(desc, cmd);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private BitmapSource GenerateBitmapPalette()
		{
			var data = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				data[i] = (byte)i;
			}

			var palette = new List<Color>();
			for (int i = 0; i < 256; i++)
			{
				palette.Add(Colors.Fuchsia);
			}

			return BitmapSource.Create(16, 16, 96.0, 96.0, PixelFormats.Indexed8, new BitmapPalette(palette), data, 16);
		}

		private bool CheckParameterTypes(PalCommand command, CommandDescriptor desc)
		{
			var paramsCount = command.Parameters.Count;
			if (desc.Parameters != null && desc.Parameters.Length == paramsCount)
			{
				for (int i = 0; i < paramsCount; i++)
				{
					if (command.Parameters[i].GetType() != desc.Parameters[i].Type)
					{
						command.Parameters[i] = Convert.ChangeType(command.Parameters[i], desc.Parameters[i].Type);
					}
				}

				return true;
			}

			return false;
		}

		private void AddControls(CommandDescriptor desc, PalCommand cmd)
		{
			for (int i = 0; i < (desc.Parameters?.Length ?? 0); i++)
			{
				var parameter = desc.Parameters[i];
				var c = AddControl(parameter, cmd, i);
				if (c == null) continue;
				c.Margin = new Thickness(0.0, 0.0, 0.0, 5.0);

				commandParametersPanel.Children.Add(new TextBlock()
				{
					Text = parameter.Name
				});
				commandParametersPanel.Children.Add(c);
			}
		}

		private Control AddControl(CommandParameter parameter, PalCommand cmd, int paramIndex)
		{
			Control c;

			if (parameter.Type == typeof(byte))
			{
				c = new NumericUpDown()
				{
					MinimumValue = (int)parameter.Minimum,
					MaximumValue = (int)parameter.Maximum,
				};
				c.DataContext = new ByteViewModel(cmd, paramIndex);
				c.SetBinding(NumericUpDown.ValueProperty, new Binding(nameof(ByteViewModel.Value)) { Mode = BindingMode.TwoWay });
			}
			else if (parameter.Type == typeof(int))
			{
				c = new NumericUpDown()
				{
					MinimumValue = (int)parameter.Minimum,
					MaximumValue = (int)parameter.Maximum,
				};
				c.DataContext = new IntViewModel(cmd, paramIndex);
				c.SetBinding(NumericUpDown.ValueProperty, new Binding(nameof(IntViewModel.Value)) { Mode = BindingMode.TwoWay });
			}
			else if (parameter.Type == typeof(float))
			{
				c = new NumericUpDownd()
				{
					MinimumValue = (float)parameter.Minimum,
					MaximumValue = (float)parameter.Maximum,
				};
				c.DataContext = new FloatViewModel(cmd, paramIndex);
				c.SetBinding(NumericUpDownd.ValueProperty, new Binding(nameof(FloatViewModel.Value)) { Mode = BindingMode.TwoWay });
			}
			else
			{
				c = null;
			}

			return c;
		}

		protected override bool DoSaveChanges()
		{
			var vm = DataContext as MainViewModel;
			foreach (var actionVm in vm.Actions.Items)
			{
				actionVm.Action.Commands = actionVm.Items
					.Select(x => x.Command)
					.ToList();
			}

			vm.Animation = new PalAnimation()
			{
				Actions = vm.Actions.Items
					.Select(x => x.Action)
					.ToList()
			};

			using (var writer = File.CreateText(WorkingFileName))
			{
				var json = JsonConvert.SerializeObject(vm.Animation, Formatting.Indented);
				writer.Write(json);
			}
			return true;
		}
	}
}
