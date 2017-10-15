using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectOpenFileCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectOpenFileCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is IProjectFile file)
            {
                var moduleName = file.Format;
                var component = Globals.Components
                    .Where(x => x.ComponentInfo.ModuleName == moduleName)
                    .SingleOrDefault();
                
                try
                {
                    component.CreateInstance(new Components.ComponentProperties()
                    {
                        Project = _vm.Project,
                        File = file
                    }).ShowDialog();
                }
                catch (FileNotFoundException ex)
                {
                    Log.Error($"File {ex.FileName} not found.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message);
                }
            }
        }
    }
}
