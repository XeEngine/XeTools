using System.Collections.Generic;
using System.Linq;
using Xe.Tools.GameStudio.Commands;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;

namespace Xe.Tools.GameStudio.ViewModels
{
	public class MainWindowViewModel : BaseNotifyPropertyChanged
	{
		private GameStudioViewModel _vm;
		private string _title;

		public IProject Project => _vm.Project;
		public string ProjectFileName => _vm.ProjectFileName;

		public string Title
		{
			get
			{
				var projectName = _vm.Project?.Name ?? "<unknown>";
				if (string.IsNullOrWhiteSpace(projectName))
					projectName = "<untitled>";
				return $"{projectName} - XeEngine Game Studio";
			}
		}

		public WindowCloseCommand WindowClose { get; }
		public ProjectOpenCommand ProjectOpen { get; }
		public ProjectSaveCommand ProjectSave { get; }
		public ProjectSaveCommand ProjectSaveAs { get; }
		public ProjectCreateFileCommand ProjectCreateFile { get; }
		public ProjectAddFileCommand ProjectAddFile { get; }
		public ProjectAddFolderCommand ProjectAddFolder { get; }
		public ProjectRemoveEntryCommand ProjectRemoveEntry { get; }
		public ProjectRunCommand ProjectRun { get; }
		public ProjectBuildCommand ProjectBuild { get; }
		public ProjectCleanCommand ProjectClean { get; }

		public string TitleBase
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		public IEnumerable<string> Configurations =>
			Settings.GetProjectConfiguration(Project)?
			.Configurations?
			.Select(x => x.Name) ??
			new string[0];

		public string CurrentConfiguration
		{
			get => Settings.GetProjectConfiguration(Project)?.CurrentConfiguration;
			set
			{
				var settings = Settings.GetProjectConfiguration(Project);
				settings.CurrentConfiguration =
					ProjectRun.ConfigurationName =
					ProjectBuild.ConfigurationName =
					ProjectClean.ConfigurationName = value;
				Settings.SaveProjectConfiguration(Project, settings);
			}
		}

        public MainWindowViewModel(GameStudioViewModel vm)
        {
            _vm = vm;
            WindowClose = new WindowCloseCommand();
            ProjectOpen = new ProjectOpenCommand(this);
            ProjectSave = new ProjectSaveCommand(this, false);
            ProjectSaveAs = new ProjectSaveCommand(this, true);
            ProjectCreateFile = new ProjectCreateFileCommand(_vm);
            ProjectAddFile = new ProjectAddFileCommand(_vm);
            ProjectAddFolder = new ProjectAddFolderCommand(_vm);
            ProjectRemoveEntry = new ProjectRemoveEntryCommand(_vm);
            ProjectRun = new ProjectRunCommand(_vm);
            ProjectBuild = new ProjectBuildCommand(_vm);
            ProjectClean = new ProjectCleanCommand(_vm);
            Register();
        }

        public void Register()
        {
            _vm.OnProjectChanged += OnProjectChanged;
		}
        public void Unregister()
        {
            _vm.OnProjectChanged -= OnProjectChanged;
        }

        public void LoadProject(string fileName)
        {
            _vm.LoadProject(fileName);
        }
        public void SaveProject(string fileName = null)
        {
            _vm.SaveProject(fileName);
        }

        private void OnProjectChanged(object sender, IProject project)
        {
            OnPropertyChanged(nameof(Title));
			OnPropertyChanged(nameof(Configurations));
			OnPropertyChanged(nameof(CurrentConfiguration));

			ProjectRun.ConfigurationName =
				ProjectBuild.ConfigurationName =
				ProjectClean.ConfigurationName = CurrentConfiguration;
		}
    }
}
