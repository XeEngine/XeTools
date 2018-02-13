using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xe.Game.Sequences;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.SequenceEditor.Controls
{
	public enum Status
	{
		NotStarted,
		Running,
		Finished
	}

	/// <summary>
	/// Interaction logic for SequenceEntryPanel.xaml
	/// </summary>
	public partial class SequenceEntryPanel : UserControl, INotifyPropertyChanged
	{
		public static readonly IEnumerable<Operation> OPERATIONS =
			Sequence.OPERATIONS.Values.Select(x => x.Operation);

		private IController _controller;
		private Panel _panel;
		private Status _status;

		public event PropertyChangedEventHandler PropertyChanged;

		public IEnumerable<Operation> Operations => OPERATIONS;

		public Sequence.Entry Entry { get; }

		public string Description => Entry.Description;
		public string UserDescription
		{
			get
			{
				if (IsUserEditDescription)
					return Entry.UserDescription;
				if (!string.IsNullOrWhiteSpace(Entry.UserDescription))
					return Entry.UserDescription;
				return "<user_description>";
			}
			set
			{
				Entry.UserDescription = value;
				OnPropertyChanged();
			}
		}
		public bool IsAsynchronous { get => Entry.IsAsynchronous; set => Entry.IsAsynchronous = value; }
		public object Parameter0 { get => Entry.Parameters[0]; set => Entry.Parameters[0] = value; }
		public object Parameter1 { get => Entry.Parameters[1]; set => Entry.Parameters[1] = value; }
		public object Parameter2 { get => Entry.Parameters[2]; set => Entry.Parameters[2] = value; }
		public object Parameter3 { get => Entry.Parameters[3]; set => Entry.Parameters[3] = value; }
		public object Parameter4 { get => Entry.Parameters[4]; set => Entry.Parameters[4] = value; }
		public object Parameter5 { get => Entry.Parameters[5]; set => Entry.Parameters[5] = value; }
		public object Parameter6 { get => Entry.Parameters[6]; set => Entry.Parameters[6] = value; }
		public object Parameter7 { get => Entry.Parameters[7]; set => Entry.Parameters[7] = value; }

		public Visibility UserDescriptionVisibility { get; private set; } = Visibility.Visible;
		public Visibility UserDescriptionEditVisibility { get; private set; } = Visibility.Collapsed;
		public bool IsUserEditDescription
		{
			get => UserDescriptionVisibility != Visibility.Visible;
			set
			{
				if (value)
				{
					UserDescriptionVisibility = Visibility.Collapsed;
					UserDescriptionEditVisibility = Visibility.Visible;
				}
				else
				{
					UserDescriptionVisibility = Visibility.Visible;
					UserDescriptionEditVisibility = Visibility.Collapsed;
				}
				OnPropertyChanged(nameof(UserDescription));
				OnPropertyChanged(nameof(UserDescriptionVisibility));
				OnPropertyChanged(nameof(UserDescriptionEditVisibility));
			}
		}

		public Status Status
		{
			get => _status;
			set
			{
				_status = value;
				IsStatusNotStarted = Visibility.Collapsed;
				IsStatusRunning = Visibility.Collapsed;
				IsStatusFinished = Visibility.Collapsed;
				
				switch (value)
				{
					case Status.NotStarted:
						IsStatusNotStarted = Visibility.Visible;
						break;
					case Status.Running:
						IsStatusRunning = Visibility.Visible;
						break;
					case Status.Finished:
						IsStatusFinished = Visibility.Visible;
						break;
				}

				OnPropertyChanged(nameof(IsStatusNotStarted));
				OnPropertyChanged(nameof(IsStatusRunning));
				OnPropertyChanged(nameof(IsStatusFinished));
			}
		}

		public Visibility IsStatusNotStarted { get; private set; }
		public Visibility IsStatusRunning { get; private set; }
		public Visibility IsStatusFinished { get; private set; }

		public SequenceEntryPanel(IController controller, Sequence.Entry entry)
		{
			InitializeComponent();
			DataContext = this;
			_controller = controller;
			Entry = entry;
			RecreateControls();
		}

		public Operation SelectedOperation
		{
			get => Entry.Operation;
			set
			{
				Entry.Operation = value;
				OnPropertyChanged();
				RecreateControls();
			}
		}

		private void RecreateControls()
		{
			_panel.Children.Clear();
			for (int i = 0; i < Entry.Parameters.Length; i++)
			{
				var value = Entry.Parameters[i];
				var desc = Entry.ParametersDescription[i];

				_panel.Children.Add(new TextBlock()
				{
					Text = desc.Description
				});

				FrameworkElement element;
				switch (desc.Type)
				{
					case ParameterType.Boolean:
						element = new CheckBox()
						{
							IsChecked = (bool)desc.DefaultValue
						};
						element.SetBinding(CheckBox.IsCheckedProperty, new Binding($"Parameter{i}")
						{
							Source = this,
							Mode = BindingMode.TwoWay
						});
						break;
					case ParameterType.Integer:
						element = new NumericUpDown()
						{
							Value = (int)desc.DefaultValue,
							MinimumValue = (int)desc.MinimumValue,
							MaximumValue = (int)desc.MaximumValue
						};
						element.SetBinding(NumericUpDown.ValueProperty, new Binding($"Parameter{i}")
						{
							Source = this,
							Mode = BindingMode.TwoWay
						});
						break;
					case ParameterType.Double:
						element = new NumericUpDownd()
						{
							Value = (double)desc.DefaultValue,
							MinimumValue = (double)desc.MinimumValue,
							MaximumValue = (double)desc.MaximumValue,
						};
						element.SetBinding(NumericUpDownd.ValueProperty, new Binding($"Parameter{i}")
						{
							Source = this,
							Mode = BindingMode.TwoWay
						});
						break;
					case ParameterType.String:
						element = new TextBox()
						{
							Text = desc.DefaultValue?.ToString(),
						};
						element.SetBinding(TextBox.TextProperty, new Binding($"Parameter{i}")
						{
							Source = this,
							Mode = BindingMode.TwoWay
						});
						break;
					default:
						element = null;
						break;
				}
				if (element != null)
				{
					element.Margin = new Thickness(0.0, 0.0, 0.0, 4.0);
					_panel.Children.Add(element);
				}
			}
		}

		private void StackPanel_Initialized(object sender, EventArgs e)
		{
			_panel = sender as Panel;
		}

		private void ButtonRemove_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("Do you want to remove the current sequence?",
				"Removal confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
			if (result == MessageBoxResult.Yes)
			{
				_controller.RemoveSequenceOperator(Entry);
			}
		}
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
		{
			_controller.MoveSequenceUp(Entry);
		}

		private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
		{
			_controller.MoveSequenceDown(Entry);
		}

		private void TextOpDescription_MouseDown(object sender, MouseButtonEventArgs e)
		{
			IsUserEditDescription = true;
		}

		private void TextOpDescription_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				e.Handled = true;
				IsUserEditDescription = false;
			}
		}

		private void TextOpDescription_LostFocus(object sender, RoutedEventArgs e)
		{
			IsUserEditDescription = false;
		}
	}
}
