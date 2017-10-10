using System;
using System.IO;
using System.Windows.Input;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectSaveCommand : ICommand
    {
        private MainWindowViewModel _vm;
        private bool _saveInstantly;

        public event EventHandler CanExecuteChanged;

        public ProjectSaveCommand(MainWindowViewModel vm, bool saveInstantly)
        {
            _vm = vm;
            _saveInstantly = saveInstantly;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_saveInstantly && !string.IsNullOrEmpty(_vm.ProjectFileName))
            {
                _vm.SaveProject();
            }
            else
            {
                var fd = FileDialog.Factory(parameter as System.Windows.Window, FileDialog.Behavior.Save,
                    FileDialog.Type.XeGameProject);
                if (fd.ShowDialog() ?? false == true)
                {
                    Common.SendMessage(MessageType.Initialization, "Saving project...");
                    _vm.SaveProject(fd.FileName);
                    Properties.Settings.Default.FileLastOpen = fd.FileName;
                    Properties.Settings.Default.Save();
                    Common.SendMessage(MessageType.Idle, "Ready");
                }
            }
        }
    }
}
