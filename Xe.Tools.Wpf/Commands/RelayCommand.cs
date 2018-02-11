using System;
using System.Windows.Input;

namespace Xe.Tools.Wpf.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute, _undo;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute,
			Func<object, bool> canExecute = null,
			Action<object> undo = null)
        {
            _execute = execute;
			_undo = undo;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

		public void Undo(object parameter)
		{
			_undo(parameter);
		}
    }
}
