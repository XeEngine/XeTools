using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Xe.Game.Sequences;
using Xe.Tools.Components.SequenceEditor.Controls;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.SequenceEditor.Windows
{
	/// <summary>
	/// Interaction logic for SequenceEditor.xaml
	/// </summary>
	public partial class SequenceEditor : WindowEx, IController, INotifyPropertyChanged
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
		private IProjectFile _file;

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
		}

		public void Open(IProject project, IProjectFile file)
		{
			ProjectService = new ProjectService(project);
			_file = file;
			using (var reader = File.OpenText(file.FullPath))
			{
				var sequence = JsonConvert.DeserializeObject<Sequence>(reader.ReadToEnd());
				// HACK Newtonsoft converts integers to Int64. We need to convert them back to Int32.
				foreach (var operation in sequence.Entries)
				{
					for (int i = 0; i < operation.Parameters.Length; i++)
					{
						if (operation.Parameters[i] is System.Int64 value)
						{
							operation.Parameters[i] = (int)(System.Int64)operation.Parameters[i];
						}
					}
				}
				Sequence = sequence;
			}
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

		protected override bool DoSaveChanges()
		{
			using (var writer = File.CreateText(_file.FullPath))
			{
				writer.Write(JsonConvert.SerializeObject(Sequence, Formatting.Indented));
			}
			return true;
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
