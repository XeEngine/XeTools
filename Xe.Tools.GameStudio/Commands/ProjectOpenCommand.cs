using System;
using System.IO;
using System.Windows.Input;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectOpenCommand : ICommand
    {
        private MainWindowViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectOpenCommand(MainWindowViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var fd = FileDialog.Factory(parameter as System.Windows.Window, FileDialog.Behavior.Open,
                FileDialog.Type.XeGameProject);
            if (fd.ShowDialog() ?? false == true)
            {
                Common.SendMessage(MessageType.Initialization, "Loading project...");
                _vm.LoadProject(fd.FileName);
                Properties.Settings.Default.FileLastOpen = fd.FileName;
                Properties.Settings.Default.Save();
                Common.SendMessage(MessageType.Idle, "Ready");
            }
        }
    }
}
