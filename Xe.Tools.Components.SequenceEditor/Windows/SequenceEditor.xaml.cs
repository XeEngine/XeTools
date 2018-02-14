using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Xe.Game.Sequences;
using Xe.Tools.Components.SequenceEditor.Controls;
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.Components.SequenceEditor.Windows
{
	/// <summary>
	/// Interaction logic for SequenceEditor.xaml
	/// </summary>
	public partial class SequenceEditor : Window, IController, INotifyPropertyChanged
	{
		private enum State
		{
			NotRunning,
			Running,
			Pasued
		}

		private ProjectService _projectService;
		private double _renderTime, _executionTime, _mulTime;
		private State _state;

		public event PropertyChangedEventHandler PropertyChanged;

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

		public double RenderTime
		{
			get => _renderTime;
			set
			{
				_renderTime = value;
				OnPropertyChanged();
			}
		}

		public double ExecutionTime
		{
			get => _executionTime;
			set
			{
				_executionTime = value;
				OnPropertyChanged();
			}
		}

		public double TimeMultiplier
		{
			get => _mulTime;
			set
			{
				_mulTime = value;
				OnPropertyChanged();
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
				RunningState = State.Running;
			}
		}

		public bool IsSequenceFinished
		{
			set
			{
				RunningState = State.NotRunning;
			}
		}

		private State RunningState
		{
			get => _state;
			set
			{
				_state = value;
				OnPropertyChanged(nameof(VisibilityIsRunning));
				OnPropertyChanged(nameof(VisibilityIsPaused));
				OnPropertyChanged(nameof(VisibilityIsNotRunning));
			}
		}

		public Visibility VisibilityIsRunning => RunningState == State.Running ? Visibility.Visible : Visibility.Collapsed;

		public Visibility VisibilityIsPaused => RunningState == State.Pasued ? Visibility.Visible : Visibility.Collapsed;

		public Visibility VisibilityIsNotRunning => RunningState == State.NotRunning ? Visibility.Visible : Visibility.Collapsed;

		public System.Drawing.Point CurrentCamera => new System.Drawing.Point(0, 0);

		public System.Drawing.Point CurrentViewport => new System.Drawing.Point(
			ctrlSequenceSimulator.CameraWidth, ctrlSequenceSimulator.CameraHeight);

		public SequenceEditor()
		{
			InitializeComponent();
			DataContext = this;
			ctrlSequenceSimulator.Controller = this;
			RunningState = State.NotRunning;

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
						.SetValue(2, 160.0),
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
						.SetValue(0, 500)
						.SetValue(1, 400),
					new Sequence.Entry(Operation.FadeInBlack)
						.SetAsynchronous(true),
					new Sequence.Entry(Operation.EntityAnimation)
						.SetValue(0, "PlayerTestCutscene")
						.SetValue(1, "Walk")
						.SetValue(2, 3),
					new Sequence.Entry(Operation.EntityMove)
						.SetValue(0, "PlayerTestCutscene")
						.SetValue(1, 416)
						.SetValue(2, 468)
						.SetValue(3, 40.0),
					new Sequence.Entry(Operation.EntityAnimation)
						.SetValue(0, "PlayerTestCutscene")
						.SetValue(1, "Stand")
						.SetValue(2, 1),
					new Sequence.Entry(Operation.Sleep).SetValue(0, 0.25),
					new Sequence.Entry(Operation.EntityAnimation)
						.SetValue(0, "PlayerTestCutscene")
						.SetValue(1, "FightStand")
						.SetValue(2, 1),
					new Sequence.Entry(Operation.Abort)
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

		private void Button_CollapseAll(object sender, RoutedEventArgs e)
		{
			foreach (var item in OperationsPanel.Children)
			{
				if (item is SequenceEntryPanel sequenceEntryPanel)
					sequenceEntryPanel.IsContentCollapsed = true;
			}
		}

		private void Button_SequenceStop(object sender, RoutedEventArgs e)
		{
			ctrlSequenceSimulator.Reset();
		}

		private void Button_SequencePause(object sender, RoutedEventArgs e)
		{
			ctrlSequenceSimulator.Paused = true;
		}

		private void Button_SequencePlay(object sender, RoutedEventArgs e)
		{
			ctrlSequenceSimulator.Paused = false;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
