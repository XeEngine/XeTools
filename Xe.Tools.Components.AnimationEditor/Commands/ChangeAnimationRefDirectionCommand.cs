using System;

namespace Xe.Tools.Components.AnimationEditor.Commands
{
    internal class ChangeAnimationRefDirectionCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void UnExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
