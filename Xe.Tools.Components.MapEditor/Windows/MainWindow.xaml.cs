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
using Xe.Tools.Components.MapEditor.ViewModels;

namespace Xe.Tools.Components.MapEditor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            Initialize(MapEditorViewModel.Instance);
        }

        public MainWindow(MapEditorViewModel vm)
        {
            InitializeComponent();
            Initialize(vm);
        }

        public void Initialize(MapEditorViewModel vm)
        {
            DataContext = new MainWindowViewModel(vm);
            //tileMap?.DataContext = new TilemapViewModel(ViewModel.MapEditor);
        }
    }
}
