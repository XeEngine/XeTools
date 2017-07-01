using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xe.Tools.Components;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.Modules;

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
                UpdateWindowName();
                ctrlResourceView.Project = _project;
            }
		}

		public MainWindow()
		{
			InitializeComponent();

            Globals.Modules = Module.GetModules().ToArray();
            Globals.Components = Component.GetComponents().ToArray();

            var fileLastOpen = Properties.Settings.Default.FileLastOpen;
            if (File.Exists(fileLastOpen))
            {
                Project = Project.Open(fileLastOpen);
            }
            else
            {
                Project = new Project();
            }
		}

        private void Window_Initialized(object sender, EventArgs e)
        {

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
                Project = Project.Open(fd.FileName);
                Properties.Settings.Default.FileLastOpen = fd.FileName;
                Properties.Settings.Default.Save();
            }
		}

		private void MenuItem_FileSaveClick(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(Project.ProjectPath) &&
				System.IO.Directory.Exists(Project.ProjectPath))
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
            Builder.Program.Build(Project, config.OutputDirectory);
        }
        private void MenuItem_ProjectClean_Click(object sender, RoutedEventArgs e)
        {
            var config = Settings.GetProjectConfiguration(Project);
            Builder.Program.Clean(Project, config.OutputDirectory);
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
