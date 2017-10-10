using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio
{
    /// <summary>
    /// Interaction logic for ProjectSettings.xaml
    /// </summary>
    public partial class ProjectSettings : Window
    {
        private Project _project;
        private ProjectConfiguration _configuration;

        public ProjectSettings(Project project)
        {
            InitializeComponent();
            _project = project;
            OpenConfiguration();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_configuration != null)
            {
                _configuration.Executable = textGameExecutable.Text;
                _configuration.WorkingDirectory = textWorkingDirectory.Text;
                _configuration.OutputDirectory = textBuildOutputDirectory.Text;
            }
            else
            {
                MessageBox.Show("Unable to save the configuration", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            SaveConfiguration();
            base.OnClosed(e);
        }

        private void OpenConfiguration()
        {
            try
            {
                _configuration = Settings.GetProjectConfiguration(_project);
                textGameExecutable.Text = _configuration.Executable;
                textWorkingDirectory.Text = _configuration.WorkingDirectory;
                textBuildOutputDirectory.Text = _configuration.OutputDirectory;
            }
            catch (Exception ex)
            {
                var msg = $"There was an error during the opening of project's configuration:\n{ex.Message}\nDo you want to create a new configuration?";
                if (Helpers.ShowMessageBoxError(msg, askConfirm: true) ?? false == false)
                    Close();
            }
        }
        private void SaveConfiguration()
        {
            if (_configuration == null)
                return;
            try
            {
                Settings.SaveProjectConfiguration(_project, _configuration);
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
                textGameExecutable.Text = fd.FileName;
            }
        }

        private void ButtonChooseWorkingDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(this, FileDialog.Behavior.Folder);
            if (fd.ShowDialog() ?? false == true)
            {
                textWorkingDirectory.Text = fd.FileName;
            }
        }

        private void ButtonChooseOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(this, FileDialog.Behavior.Folder);
            if (fd.ShowDialog() ?? false == true)
            {
                textBuildOutputDirectory.Text = fd.FileName;
            }
        }
    }
}
