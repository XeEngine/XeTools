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
using Xe.Tools.Components.KernelEditor.ViewModels;

namespace Xe.Tools.Components.KernelEditor.Views
{
    /// <summary>
    /// Interaction logic for TabPlayers.xaml
    /// </summary>
    public partial class TabActors : UserControl
    {
        public TabPlayersViewModel ViewModel => DataContext as TabPlayersViewModel;

        public TabActors()
        {
            InitializeComponent();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Players.Add(new Game.Kernel.Actor());
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Players.Remove(ViewModel.SelectedPlayer);
        }
    }
}
