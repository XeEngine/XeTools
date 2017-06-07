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
	public static class Helper
	{
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
