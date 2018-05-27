using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio
{
    /// <summary>
    /// Interaction logic for ProjectSettings.xaml
    /// </summary>
    public partial class ProjectSettings : Window
    {
        private IProject project;

		public ProjectSettingsViewModel ViewModel => DataContext as ProjectSettingsViewModel;


		public ProjectSettings(IProject project)
        {
            InitializeComponent();
			this.project = project;

            OpenConfiguration();
        }

        protected override void OnClosing(CancelEventArgs e)
		{
			Settings.SaveProjectConfiguration(project, ViewModel.ProjectSettings);
			base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
		{
			if (ViewModel?.ProjectSettings != null)
			{
				SaveConfiguration();
			}

            base.OnClosed(e);
        }

        private void OpenConfiguration()
        {
            try
			{
				DataContext = new ProjectSettingsViewModel(project);
			}
            catch (Exception ex)
            {
                var msg = $"There was an error during the opening of project's configuration:\n{ex.Message}\nDo you want to create a new configuration?";
                if (Helpers.ShowMessageBoxError(msg, askConfirm: true) ?? false == false)
				{
					Close();
				}
				else
				{
					NewConfiguration();
				}
            }
        }

		private void NewConfiguration()
		{
			Settings.SaveProjectConfiguration(project, new Models.ProjectSettings()
			{
				CurrentConfiguration = "Develop",
				Configurations = new List<ProjectConfiguration>()
				{
					new ProjectConfiguration()
					{
						Name = "Develop"
					},
					new ProjectConfiguration()
					{
						Name = "Release"
					},
				}
			});
			OpenConfiguration();
		}

        private void SaveConfiguration()
        {
            try
            {
                Settings.SaveProjectConfiguration(project, ViewModel.ProjectSettings);
            }
            catch (Exception ex)
            {
                Helpers.ShowMessageBoxError($"Unable to save the current configuration.\n{ex.Message}");
            }
        }
        
        private void ButtonChooseBinary_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(this, FileDialog.Behavior.Open, FileDialog.Type.Executable);
            if (fd.ShowDialog() ?? false == true)
            {
                ViewModel.SelectedConfiguration.Value.Executable = fd.FileName;
				ViewModel?.ConfigurationChanged();
			}
        }

        private void ButtonChooseWorkingDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(this, FileDialog.Behavior.Folder);
            if (fd.ShowDialog() ?? false == true)
			{
				ViewModel.SelectedConfiguration.Value.WorkingDirectory = fd.FileName;
				ViewModel?.ConfigurationChanged();
			}
        }

        private void ButtonChooseOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(this, FileDialog.Behavior.Folder);
            if (fd.ShowDialog() ?? false == true)
			{
				ViewModel.SelectedConfiguration.Value.OutputDirectory = fd.FileName;
				ViewModel?.ConfigurationChanged();
			}
        }
    }
}
