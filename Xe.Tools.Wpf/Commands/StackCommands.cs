using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Wpf.Commands
{
	public class StackCommands
	{
		private Stack<(RelayCommand, object)> _commands = new Stack<(RelayCommand, object)>(100);

		public void Execute(RelayCommand command, object parameter)
		{
			if (command.CanExecute(parameter))
			{
				command.Execute(parameter);
				_commands.Push((command, parameter));
			}
		}

		public void Undo()
		{
			if (_commands.Count > 0)
			{
				var cmd = _commands.Pop();
				cmd.Item1.Undo(cmd.Item2);
			}
		}
	}
}
