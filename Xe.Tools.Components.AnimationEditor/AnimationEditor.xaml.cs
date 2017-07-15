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
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.Components.AnimationEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void listPublicAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void buttonAddPublicAnimation_Click(object sender, RoutedEventArgs e)
        {
            /*var dialog = new SingleInputDialog
            {
                Title = "New animation to export"
            };*/
        }
        private void buttonRemovePublicAnimation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listPrivateAnimations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void buttonAddPrivateAnimation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonRemovePrivateAnimation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDirectionUndefined_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDirection_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkFlipHorizontally_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void checkFlipVertically_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
