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
        private class ViewModel : BaseNotifyPropertyChanged
        {
            private string _description;
            private IEnumerable<object> _items;
            private object _selectedItem;

            public string Description
            {
                get => _description;
                set
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }

            public IEnumerable<object> Items
            {
                get => _items;
                set
                {
                    _items = value;
                    OnPropertyChanged();
                }
            }

            public object SelectedValue
            {
                get => _selectedItem;
                set
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private ViewModel _vm;

        public string Description
        {
            get => _vm.Description;
            set => _vm.Description = value;
        }

        public IEnumerable<object> Items
        {
            get => _vm.Items;
            set => _vm.Items = value;
        }

        public object SelectedItem
        {
            get => _vm.SelectedValue;
            set => _vm.SelectedValue = value;
        }

        public SingleSelectionDialog()
        {
            InitializeComponent();
            DataContext = _vm = new ViewModel();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
