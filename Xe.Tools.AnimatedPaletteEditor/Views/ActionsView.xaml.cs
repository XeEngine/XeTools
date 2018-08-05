using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xe.Tools.Components.AnimatedPaletteEditor.ViewModels;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimatedPaletteEditor.Views
{
	/// <summary>
	/// Interaction logic for ActionsView.xaml
	/// </summary>
	public partial class ActionsView : UserControl
	{
		public ActionsView()
		{
			InitializeComponent();
		}

		private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (DataContext is ActionsViewModel vm && vm.IsItemSelected)
			{
				var item = vm.SelectedItem;

				var dialog = new SingleInputDialog()
				{
					Title = "Action name",
					Description = "Change the action name",
					Text = item.Name,
				};

				if (dialog.ShowDialog() == true)
				{
					item.Name = dialog.Text;
				}
			}
		}
	}
}
