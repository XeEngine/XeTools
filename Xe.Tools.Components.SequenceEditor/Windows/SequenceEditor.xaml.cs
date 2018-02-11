using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xe.Game.Sequences;
using Xe.Tools.Components.SequenceEditor.Controls;

namespace Xe.Tools.Components.SequenceEditor.Windows
{
	/// <summary>
	/// Interaction logic for SequenceEditor.xaml
	/// </summary>
	public partial class SequenceEditor : Window, IController
	{
		public Sequence _sequence;

		public Sequence Sequence => _sequence;

		public SequenceEditor()
		{
			InitializeComponent();
			_sequence = new Sequence();
		}

		public void AddSequenceOperator()
		{
			var entry = new Sequence.Entry(Operation.None);
			_sequence.Entries.Add(entry);
			OperationsPanel.Children.Add(new SequenceEntryPanel(this, entry));
		}

		public void RemoveSequenceOperator(Sequence.Entry entry)
		{
			UIElement elementToRemove = GetElementFromEntry(entry);
			if (elementToRemove != null)
			{
				_sequence.Entries.Remove(entry);
				OperationsPanel.Children.Remove(elementToRemove);
			}
		}

		public void MoveSequenceUp(Sequence.Entry entry)
		{
			UIElement element = GetElementFromEntry(entry);
			if (element != null)
			{
				var index = OperationsPanel.Children.IndexOf(element);
				if (index > 0)
				{
					_sequence.Entries.RemoveAt(index);
					_sequence.Entries.Insert(index - 1, entry);
					OperationsPanel.Children.RemoveAt(index);
					OperationsPanel.Children.Insert(index - 1, element);
				}
			}
		}

		public void MoveSequenceDown(Sequence.Entry entry)
		{
			UIElement element = GetElementFromEntry(entry);
			if (element != null)
			{
				var index = OperationsPanel.Children.IndexOf(element);
				if (index < OperationsPanel.Children.Count - 1)
				{
					_sequence.Entries.RemoveAt(index);
					_sequence.Entries.Insert(index + 1, entry);
					OperationsPanel.Children.RemoveAt(index);
					OperationsPanel.Children.Insert(index + 1, element);
				}
			}
		}

		private UIElement GetElementFromEntry(Sequence.Entry entry)
		{
			foreach (var item in OperationsPanel.Children)
			{
				if (item is SequenceEntryPanel _panel)
				{
					if (_panel.Entry == entry)
					{
						return _panel;
					}
				}
			}
			return null;
		}

		private void ButtonAddOperation_Click(object sender, RoutedEventArgs e)
		{
			AddSequenceOperator();
		}
	}
}
