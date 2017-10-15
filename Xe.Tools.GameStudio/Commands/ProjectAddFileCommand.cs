using System;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectAddFileCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectAddFileCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return false;
        }

        public void Execute(object parameter)
        {
            var window = parameter as Window;
            if (_vm.SelectedProjectEntry is ProjectFolderViewModel folder)
            {
                var dialog = new Dialogs.NewFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    var fileName = dialog.FileName;
                }
            }
        }
    }
}
