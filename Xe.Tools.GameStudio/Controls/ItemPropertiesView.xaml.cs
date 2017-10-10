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
using Xe.Tools.GameStudio.ViewModels;

namespace Xe.Tools.GameStudio.Controls
{
    /// <summary>
    /// Interaction logic for ItemPropertiesView.xaml
    /// </summary>
    public partial class ItemPropertiesView : UserControl
    {
        public ItemPropertiesView()
        {
            InitializeComponent();
            DataContext = new ItemPropertiesViewModel(GameStudioViewModel.Instance);
        }
    }
}
