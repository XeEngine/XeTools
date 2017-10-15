using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectCreateFileCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectCreateFileCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
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
                    var component = dialog.SelectedComponent;
                    var module = Globals.Modules
                        .Where(x => x.Name == component.ComponentInfo.ModuleName)
                        .FirstOrDefault();
                    if (module != null)
                    {
                        var file = (folder.Entry as IProjectDirectory).AddFile(fileName);
                        file.Format = module.Name;
                        folder.AddFile(file);
                    }
                    else
                        Log.Error($"Module {component.ComponentInfo.ModuleName} from component {component.Name} was not found.");
                }
            }
        }
    }
}
