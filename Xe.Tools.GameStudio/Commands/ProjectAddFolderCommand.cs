using System;
using System.Windows.Input;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectAddFolderCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectAddFolderCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_vm.SelectedProjectEntry is ProjectFolderViewModel directory) {
                var dialog = new SingleInputDialog()
                {
                    Title = "Create a new folder",
                    Description = "Please specify the name of the folder that you want to create",
                    Text = "new folder"
                };
                if (dialog.ShowDialog() ?? false)
                {
                    var entry = (directory.Entry as IProjectDirectory).AddDirectory(dialog.Text);
                    directory.AddDirectory(entry);
                }
            }
        }
    }
}
