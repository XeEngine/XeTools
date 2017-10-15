using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.GameStudio.ViewModels;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectRemoveEntryCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectRemoveEntryCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            const string strDeleteDir = "You are removing a folder with some files inside.\nDo you want to delete the content? If you click No, they will be removed only by the project.";
            const string strDeleteFile = "Do you want to delete the file? If you click No, it will be removed only by the project.";

            bool isDirectory = _vm.SelectedProjectEntry is ProjectFolderViewModel ? true : false;
            var strMessage = isDirectory ? strDeleteDir : strDeleteFile;

            bool? physicalDelete;
            if (strMessage != null)
            {
                switch (MessageBox.Show(parameter as Window, strMessage, "Delete confirmation",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning))
                {
                    case MessageBoxResult.Yes:
                    case MessageBoxResult.OK:
                        physicalDelete = true;
                        break;
                    case MessageBoxResult.No:
                        physicalDelete = false;
                        break;
                    default:
                        physicalDelete = null;
                        break;
                }
            }
            else
                physicalDelete = true;

            if (physicalDelete.HasValue)
            {
                _vm.SelectedProjectEntry.Entry.Remove(physicalDelete.Value);
            }
        }
    }
}
