using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xe.Tools.GameStudio.ViewModels;

namespace Xe.Tools.GameStudio.Commands
{
    public class ProjectOpenCommand : ICommand
    {
        private GameStudioViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public ProjectOpenCommand(GameStudioViewModel vm)
        {
            _vm = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
