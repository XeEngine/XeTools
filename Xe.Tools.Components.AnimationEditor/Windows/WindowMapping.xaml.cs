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
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowMapping.xaml
    /// </summary>
    public partial class WindowMapping : Window
    {
        private AnimationsMappingViewModel ViewModel => DataContext as AnimationsMappingViewModel;

        public WindowMapping()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsAnimationDefSelected)
            {
                ViewModel.AnimationDefs.Remove(ViewModel.SelectedAnimationDef);
            }
        }
    }
}
