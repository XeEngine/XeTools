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
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.Components.SequenceEditor.Windows
{
	/// <summary>
	/// Interaction logic for SequenceEditor.xaml
	/// </summary>
	public partial class SequenceEditor : Window, IController
	{
		private ProjectService _projectService;

		public ProjectService ProjectService
		{
			get => _projectService;
			set
			{
				_projectService = value;
			}
		}

		public Sequence Sequence
		{
			get => ctrlSequenceSimulator.Sequence;
			set
			{
				ctrlSequenceSimulator.Sequence = value;
				foreach (var item in value.Entries)
				{
					AddSequenceOperator(item);
				}
			}
		}

		public int CurrentOperationIndex
		{
			set
			{
				int index = 0;
				foreach (var item in OperationsPanel.Children)
				{
					if (item is SequenceEntryPanel _item)
					{
						if (index < value - 1)
							_item.Status = Status.Finished;
						else if (index >= value)
							_item.Status = Status.NotStarted;
						else
							_item.Status = Status.Running;
						index++;
					}
				}
			}
		}

		public SequenceEditor()
		{
			InitializeComponent();
			ctrlSequenceSimulator.Controller = this;
			
			Sequence = new Sequence()
			{
				Entries = new List<Sequence.Entry>()
				{
					new Sequence.Entry(Operation.ChangeMap)
						.SetValue(0, 1)
						.SetValue(1, 3),
					new Sequence.Entry(Operation.CameraSet)
						.SetValue(0, 600)
						.SetValue(1, 300),
					new Sequence.Entry(Operation.CameraShake),
					new Sequence.Entry(Operation.CameraMove)
						.SetValue(0, 200)
						.SetValue(1, 200)
						.SetValue(2, 80.0),
					new Sequence.Entry(Operation.FadeOutWhite),
					new Sequence.Entry(Operation.ChangeMap)
						.SetValue(0, 0)
						.SetValue(1, 4),
					new Sequence.Entry(Operation.Sleep).SetValue(0, 0.5),
					new Sequence.Entry(Operation.FadeInWhite),
					new Sequence.Entry(Operation.Sleep).SetValue(0, 0.5),
					new Sequence.Entry(Operation.FadeOutBlack),
					new Sequence.Entry(Operation.ChangeMap)
						.SetValue(0, 1)
						.SetValue(1, 0),
					new Sequence.Entry(Operation.CameraSet)
						.SetValue(0, 400)
						.SetValue(1, 400),
					new Sequence.Entry(Operation.FadeInBlack),
					new Sequence.Entry(Operation.Sleep)
						.SetValue(0, 10.0),
				}
			};
		}

		public void Open(IProject project, IProjectFile file)
		{
			ProjectService = new ProjectService(project);
		}

		public void AddSequenceOperator()
		{
			var entry = new Sequence.Entry(Operation.None);
			Sequence.Entries.Add(entry);
			AddSequenceOperator(entry);
		}

		public void AddSequenceOperator(Sequence.Entry entry)
		{
			OperationsPanel.Children.Add(new SequenceEntryPanel(this, entry)
			{
				
			});
		}

		public void RemoveSequenceOperator(Sequence.Entry entry)
		{
			UIElement elementToRemove = GetElementFromEntry(entry);
			if (elementToRemove != null)
			{
				Sequence.Entries.Remove(entry);
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
					Sequence.Entries.RemoveAt(index);
					Sequence.Entries.Insert(index - 1, entry);
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
					Sequence.Entries.RemoveAt(index);
					Sequence.Entries.Insert(index + 1, entry);
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ctrlSequenceSimulator.Reset();
		}
	}
}
