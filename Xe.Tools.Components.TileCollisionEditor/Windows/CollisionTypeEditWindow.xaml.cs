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
    /// Interaction logic for CollisionTypeEditWindow.xaml
    /// </summary>
    public partial class CollisionTypeEditWindow : Window
    {
        public CollisionTypeEditViewModel ViewModel => DataContext as CollisionTypeEditViewModel;

        public CollisionTypeEditWindow(CollisionType collisionType)
        {
            InitializeComponent();
            DataContext = new CollisionTypeEditViewModel(collisionType);
        }
    }
}
