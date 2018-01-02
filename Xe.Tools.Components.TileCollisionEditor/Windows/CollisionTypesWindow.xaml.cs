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
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Components.TileCollisionEditor.ViewModels;

namespace Xe.Tools.Components.TileCollisionEditor.Windows
{
    /// <summary>
    /// Interaction logic for CollisionTypesWindow.xaml
    /// </summary>
    public partial class CollisionTypesWindow : Window
    {
        public CollisionTypesViewModel ViewModel => DataContext as CollisionTypesViewModel;

        public IEnumerable<CollisionType> CollisionTypes => ViewModel.CollisionTypes;

        public CollisionTypesWindow(IEnumerable<CollisionType> collisionTypes)
        {
            InitializeComponent();
            DataContext = new CollisionTypesViewModel(this, collisionTypes);
        }
    }
}
