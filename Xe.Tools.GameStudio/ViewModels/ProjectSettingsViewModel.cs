using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.Windows;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio.ViewModels
{
	public class ProjectSettingsViewModel : BaseNotifyPropertyChanged
	{
		internal enum ItemType
		{
			None,
			Item,
			Command,
		}

		public class Entry
		{
			internal ItemType Type { get; set; }

			public string Name { get; set; }

			public ProjectConfiguration Value { get; set; }
		}

		private readonly IProject mProject;
		private Entry selectedConfiguration;
		private int selectedConfigIndex;

		public ProjectSettingsViewModel(IProject project)
		{
			mProject = project;
			ProjectSettings = Settings.GetProjectConfiguration(mProject);

			AddCommand = new RelayCommand(param =>
			{
				var dialog = new SingleInputDialog()
				{
					Title = "New configuration",
					Description = "Choose a name for the new configuration:"
				};

				if (dialog.ShowDialog() == true)
				{
					var name = dialog.Text;

					if (ProjectSettings.Configurations == null)
					{
						ProjectSettings.Configurations = new List<ProjectConfiguration>();
					}

					if (!ProjectSettings.Configurations.Any(x => x.Name == name))
					{
						ProjectSettings.Configurations.Add(new ProjectConfiguration()
						{
							Name = dialog.Text
						});

						OnPropertyChanged(nameof(Configurations));
						OnPropertyChanged(nameof(RealConfigurations));
					}
				}
			}, x => true);

			EditCommand = new RelayCommand(param =>
			{
				var dialog = new SingleInputDialog()
				{
					Title = "Rename configuration",
					Description = $"New name for {param} configuration:",
					Text = param?.ToString()
				};

				if (dialog.ShowDialog() == true)
				{
					var name = dialog.Text;

					if (!ProjectSettings.Configurations.Any(x => x.Name == name))
					{
						SelectedConfiguration.Value.Name = dialog.Text;

						OnPropertyChanged(nameof(Configurations));
						OnPropertyChanged(nameof(RealConfigurations));
					}
				}
			}, x => x != null);

			RemoveCommand = new RelayCommand(param =>
			{
				if (MessageBox.Show(
					$"Do you want to remove {param} configuration?",
					"Remove",
					MessageBoxButton.YesNo,
					MessageBoxImage.Warning) == MessageBoxResult.Yes)
				{
					ProjectSettings.Configurations.RemoveAt(SelectedConfigurationIndex);
					OnPropertyChanged(nameof(Configurations));
					OnPropertyChanged(nameof(RealConfigurations));
				}
			}, x => ProjectSettings.Configurations != null && SelectedConfigurationIndex >= 0);

			MoveUpCommand = new RelayCommand(param =>
			{
				if (SelectedConfigurationIndex > 0)
				{
					var configs = ProjectSettings.Configurations;
					var selectedIndex = SelectedConfigurationIndex;
					var item = configs[selectedIndex];
					configs.RemoveAt(selectedIndex);
					configs.Insert(--selectedIndex, item);
					SelectedConfigurationIndex = selectedIndex;

					OnPropertyChanged(nameof(Configurations));
					OnPropertyChanged(nameof(RealConfigurations));
				}
			}, x => ProjectSettings.Configurations != null && SelectedConfigurationIndex > 0);

			MoveDownCommand = new RelayCommand(x =>
			{
				var configs = ProjectSettings.Configurations;
				if (SelectedConfigurationIndex >= 0 && SelectedConfigurationIndex + 1 < (configs?.Count ?? 0))
				{
					var selectedIndex = SelectedConfigurationIndex;
					var item = configs[selectedIndex];
					configs.RemoveAt(selectedIndex);
					configs.Insert(++selectedIndex, item);
					SelectedConfigurationIndex = selectedIndex;

					OnPropertyChanged(nameof(Configurations));
					OnPropertyChanged(nameof(RealConfigurations));
				}
			}, x => ProjectSettings.Configurations != null && SelectedConfigurationIndex >= 0 && SelectedConfigurationIndex + 1 < (ProjectSettings.Configurations?.Count ?? 0));
		}

		public Models.ProjectSettings ProjectSettings { get; }

		public IEnumerable<Entry> Configurations
		{
			get
			{
				var configs = ProjectSettings
					.Configurations?
					.Select(x => new Entry()
					{
						Type = ItemType.Item,
						Name = x.Name,
						Value = x
					}) ?? new Entry[]
					{
						new Entry()
						{
							Name = "empty"
						}
					};

				return configs
					.Append(new Entry()
					{
						Type = ItemType.Command,
						Name = "Configuration Manager..."
					});
			}
		}

		public IEnumerable<Entry> RealConfigurations
		{
			get => ProjectSettings
				.Configurations?
				.Select(x => new Entry()
				{
					Type = ItemType.Item,
					Name = x.Name,
					Value = x
				}) ?? new Entry[0];
		}
		
		public Entry SelectedConfiguration
		{
			get => selectedConfiguration;
			set
			{
				switch (value?.Type)
				{
					case ItemType.Item:
						selectedConfiguration = value;
						OnPropertyChanged(nameof(SelectedConfiguration));
						break;
					case ItemType.Command:
						SelectedConfigurationIndex = -1;
						new WindowConfigurations()
						{
							DataContext = this
						}.ShowDialog();
						break;
				}
				EditCommand.CanExecute(SelectedConfiguration);
				RemoveCommand.CanExecute(SelectedConfiguration);
			}
		}

		public int SelectedConfigurationIndex
		{
			get => selectedConfigIndex;
			set
			{
				selectedConfigIndex = value;
				MoveUpCommand.CanExecute(value);
				MoveDownCommand.CanExecute(value);
				OnPropertyChanged();
			}
		}

		public RelayCommand AddCommand { get; set; }

		public RelayCommand EditCommand { get; set; }

		public RelayCommand RemoveCommand { get; set; }

		public RelayCommand MoveUpCommand { get; set; }

		public RelayCommand MoveDownCommand { get; set; }

		public RelayCommand CloseCommand { get; set; }
		
		public void ConfigurationChanged()
		{
			OnPropertyChanged(nameof(SelectedConfiguration));
		}
	}
}
