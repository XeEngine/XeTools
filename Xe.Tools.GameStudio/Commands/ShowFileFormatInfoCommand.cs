using System;
using System.Windows.Input;

namespace Xe.Tools.GameStudio.Commands
{
	public class ShowFileFormatInfoCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var v = parameter as string;
		}
	}
}
