using System.Windows;
using Xe.Tools.Components.KernelEditor.ViewModels;

namespace Xe.Tools.Components.KernelEditor.Dialogs
{
    /// <summary>
    /// Interaction logic for NameEditor.xaml
    /// </summary>
    public partial class NameEditor : Window
    {
        public NameViewModel ViewModel
        {
            get => DataContext as NameViewModel;
            set => DataContext = value;
        }

        public NameEditor()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
