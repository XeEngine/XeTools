using System;
using System.Collections;
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

namespace Xe.Tools.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for SingleSelectionDialog.xaml
    /// </summary>
    public partial class SingleSelectionDialog : Window
    {
        public string Description
        {
            get => ctrlLabelDescription.Text;
            set => ctrlLabelDescription.Text = value;
        }
        public IEnumerable ItemsSource
        {
            get => ctrlListAnimations.ItemsSource;
            set => ctrlListAnimations.ItemsSource = value;
        }
        public object Value
        {
            get => ctrlListAnimations.SelectedValue;
            set => ctrlListAnimations.SelectedValue = value;
        }

        public SingleSelectionDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctrlListAnimations.Focus();
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
