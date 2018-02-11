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
	/// <summary>
	/// Interaction logic for SequenceEntryPanel.xaml
	/// </summary>
	public partial class SequenceEntryPanel : UserControl, INotifyPropertyChanged
	{
		public static readonly IEnumerable<Operation> OPERATIONS =
			Sequence.OPERATIONS.Values.Select(x => x.Operation);

		private IController _controller;
		private Panel _panel;

		public event PropertyChangedEventHandler PropertyChanged;

		public IEnumerable<Operation> Operations => OPERATIONS;

		public Sequence.Entry Entry { get; }
		
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
						break;
					case ParameterType.Integer:
						element = new NumericUpDown()
						{
							Value = (int)desc.DefaultValue,
							MinimumValue = (int)desc.MinimumValue,
							MaximumValue = (int)desc.MaximumValue
						};
						break;
					case ParameterType.Float:
						element = new NumericUpDownd()
						{
							Value = (float)desc.DefaultValue,
							MinimumValue = (float)desc.MinimumValue,
							MaximumValue = (float)desc.MaximumValue
						};
						break;
					case ParameterType.String:
						element = new TextBox()
						{
							Text = desc.DefaultValue?.ToString(),
						};
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

		private void ButtonUp_Click(object sender, RoutedEventArgs e)
		{
			_controller.MoveSequenceUp(Entry);
		}

		private void ButtonDown_Click(object sender, RoutedEventArgs e)
		{
			_controller.MoveSequenceDown(Entry);
		}
	}
}
