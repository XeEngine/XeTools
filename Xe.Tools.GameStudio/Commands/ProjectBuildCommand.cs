using System;
using System.Windows.Input;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.GameStudio.Utility;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectBuildCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectBuildCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var project = _vm.Project;
            if (project != null)
            {
                project.Build();
            }
        }
    }
}
