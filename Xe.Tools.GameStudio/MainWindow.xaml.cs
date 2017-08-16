using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xe.Tools.Components;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Modules;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Project _project;

		private Project Project
		{
			get { return _project; }
			set
			{
				_project = value;
                foreach (var container in _project.Containers)
                {
                    container.Parent = _project;
                    foreach (var item in container.Items)
                    {
                        item.Parent = container;
                    }
                }
                UpdateWindowName();
                ctrlResourceView.Project = _project;
            }
		}

		public MainWindow()
		{
			InitializeComponent();

            Common.Initialize();
            FooterBar.DataContext = new StatusViewModel();
		}

        private void Window_Loaded(object sender, EventArgs e)
        {
            Project project = null;
            var tasks = new Task[] {
                Task.Run(() =>
                {
                    Common.SendMessage(MessageType.Initialization, "Loading modules...");
                    Globals.Modules = Module.GetModules().ToArray();
                    Common.SendMessage(MessageType.Initialization, "Loading components...");
                    Globals.Components = Component.GetComponents().ToArray();
                }),
                Task.Run(() =>
                {
                    var fileLastOpen = Properties.Settings.Default.FileLastOpen;
                    if (File.Exists(fileLastOpen))
                    {
                        Common.SendMessage(MessageType.Initialization, "Loading most recent project...");
                        project = Project.Open(fileLastOpen);
                    }
                    else
                    {
                        project = new Project();
                    }
                })
            };

            Task.Run(() =>
            {
                Task.WaitAll(tasks);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Project = project;
                    Common.SendMessage(MessageType.Idle, "Ready");
                });
            });
        }

        private void MenuItem_FileNewClick(object sender, RoutedEventArgs e)
		{
			Project = new Project();
		}

		private void MenuItem_FileOpenClick(object sender, RoutedEventArgs e)
		{
			var fd = FileDialog.Factory(FileDialog.Behavior.Open,
				FileDialog.Type.XeGameProject);
			if (fd.ShowDialog() ?? false == true)
            {
                Common.SendMessage(MessageType.Initialization, "Loading most recent project...");
                Project = Project.Open(fd.FileName);
                Properties.Settings.Default.FileLastOpen = fd.FileName;
                Properties.Settings.Default.Save();
                Common.SendMessage(MessageType.Idle, "Ready");
            }
		}

		private void MenuItem_FileSaveClick(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(Project.ProjectPath) &&
				Directory.Exists(Project.ProjectPath))
				Project.Save();
			else
				SaveProjectDialog();
		}

		private void MenuItem_FileSaveAsClick(object sender, RoutedEventArgs e)
		{
			SaveProjectDialog();
		}

		private void MenuItem_ExitClick(object sender, RoutedEventArgs e)
		{
            Close();
		}

		private void MenuItem_ProjectPropertiesClick(object sender, RoutedEventArgs e)
		{
			var dialog = new ProjectProperties(_project);
			var result = dialog.ShowDialog();
			UpdateWindowName();
        }
        private void MenuProjectConfiguration_Click(object sender, RoutedEventArgs e)
        {
            new ProjectSettings(_project).ShowDialog();
        }
        private void MenuItem_ProjectRun_Click(object sender, RoutedEventArgs e)
        {
            var config = Settings.GetProjectConfiguration(Project);
            if (string.IsNullOrEmpty(config.Executable) ||
                string.IsNullOrEmpty(config.WorkingDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else if (!File.Exists(config.Executable) ||
                Directory.Exists(config.WorkingDirectory))
            {
                Helpers.ShowMessageBoxWarning($"{config.Executable} not found.");
            }
            else
            {
                Helpers.RunApplication(config.Executable, config.WorkingDirectory);
            }
        }
        private void MenuItem_ProjectBuild_Click(object sender, RoutedEventArgs e)
        {
            var config = Settings.GetProjectConfiguration(Project);
            if (string.IsNullOrEmpty(config.OutputDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else
            {
                Task.Run(() =>
                {
                    Common.ProjectBuild(Project, config.OutputDirectory);
                });
            }
        }
        private void MenuItem_ProjectClean_Click(object sender, RoutedEventArgs e)
        {
            var config = Settings.GetProjectConfiguration(Project);
            if (string.IsNullOrEmpty(config.OutputDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else
            {
                Task.Run(() =>
                {
                    Common.ProjectClean(Project, config.OutputDirectory);
                });
            }
        }

        private void UpdateWindowName()
		{
			var projectName = _project?.Name ?? "<unknown>";
			if (string.IsNullOrWhiteSpace(projectName))
				projectName = "<untitled>";
			Title = $"{projectName} - XeEngine Game Studio";
		}
		private void SaveProjectDialog()
		{
			var fd = FileDialog.Factory(FileDialog.Behavior.Save,
				FileDialog.Type.XeGameProject);
			if (fd.ShowDialog() ?? false == true)
			{
				Project.Save(fd.FileName);
			}
		}
    }
}
