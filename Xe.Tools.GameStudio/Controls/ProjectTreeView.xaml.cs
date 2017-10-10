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
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Controls
{
    /// <summary>
    /// Interaction logic for ProjectTreeView.xaml
    /// </summary>
    public partial class ProjectTreeView : UserControl
    {
        private ProjectExplorerViewModel ViewModel => DataContext as ProjectExplorerViewModel;

        public ProjectTreeView()
        {
            InitializeComponent();
            DataContext = new ProjectExplorerViewModel(GameStudioViewModel.Instance);
        }

        private void TreeProject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView)
            {
                if (treeView.SelectedItem is ProjectExplorerViewModel.ProjectExplorerItemViewModel item)
                {
                    GameStudioViewModel.Instance.SelectedProjectEntry = item.Entry;
                }
            }
        }
    }
}
