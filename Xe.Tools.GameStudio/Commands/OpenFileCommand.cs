using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Xe.Tools.GameStudio.Commands
{
	public class OpenFileCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var path = parameter as string;
			path = path.Replace('/', '\\');

			try
			{
				Process.Start(path);
			}
			catch (Win32Exception)
			{
				Process.Start(new ProcessStartInfo()
				{
					FileName = "notepad",
					Arguments = path
				});
			}
		}
	}
}
