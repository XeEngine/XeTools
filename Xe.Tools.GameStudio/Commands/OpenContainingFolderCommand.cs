using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace Xe.Tools.GameStudio.Commands
{
    public class OpenContainingFolderCommand : ICommand
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
			var attr = File.GetAttributes(path);

            string param;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                param = path;
            }
            else
            {
                param = $"/select,\"{path}\"";
            }

            Process.Start("explorer.exe", param);
        }
    }
}
