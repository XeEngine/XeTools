using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;

namespace Xe.Tools.GameStudio.Utility
{
	public static class Helpers
    {
        public static bool? ShowMessageBoxError(string message, string title = null, bool askConfirm = false)
        {
            return ShowMessageBox(message, MessageBoxImage.Error, title, askConfirm);
        }
        public static bool? ShowMessageBoxWarning(string message, string title = null, bool askConfirm = false)
        {
            return ShowMessageBox(message, MessageBoxImage.Warning, title, askConfirm);
        }
        public static bool? ShowMessageBox(string message, MessageBoxImage image, string title = null, bool askConfirm = false)
        {
            var buttons = askConfirm ? MessageBoxButton.YesNo : MessageBoxButton.OK;
            switch (MessageBox.Show(message, title ?? "Error", buttons, image))
            {
                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                case MessageBoxResult.None:
                    return false;
                case MessageBoxResult.Cancel:
                default:
                    return null;
            }
        }
        public static void RunApplication(string executable, string workingDirectory)
        {

        }
        public static void Wpf_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
        /*public static void Wpf_OnDpiChanged(object sender, DpiChangedEventArgs e)
        {
            
        }*/
	}
}
