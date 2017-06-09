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

namespace Xe.Tools.GameStudio.Dialogs
{
    /// <summary>
    /// Interaction logic for SingleInputDialog.xaml
    /// </summary>
    public partial class SingleInputDialog : Window
    {
        public string Description
        {
            get => ctrlLabelDescription.Text;
            set => ctrlLabelDescription.Text = value;
        }
        public string Text
        {
            get => ctrlTextboxInput.Text;
            set => ctrlTextboxInput.Text = value;
        }

        public SingleInputDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlTextboxInput.Focus();
            ctrlTextboxInput.SelectAll();
        }

        private void ctrlTextboxInput_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
