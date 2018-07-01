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
using System.Windows.Shapes;
using Xe.Tools.Components.KernelEditor.Models;

namespace Xe.Tools.Components.KernelEditor.Views
{
	/// <summary>
	/// Interaction logic for DialogElementsSelection.xaml
	/// </summary>
	public partial class DialogElementsSelection : Window
	{
		private readonly BitmaskSelectionModel bitmaskSelectionModel;

		public DialogElementsSelection(IEnumerable<string> names)
		{
			InitializeComponent();

			bitmaskSelectionModel = new BitmaskSelectionModel(names, 0);
			DataContext = bitmaskSelectionModel;
		}

		public uint Value
		{
			get => bitmaskSelectionModel.Value;
			set => bitmaskSelectionModel.Value = value;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
