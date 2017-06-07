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

namespace Xe.Tools.GameStudio
{
    /// <summary>
    /// Interaction logic for ContainersManager.xaml
    /// </summary>
    public partial class ContainersManager : Window
    {
        private Project _project;

        public Project Project
        {
            get => _project;
            set
            {
                _project = value;
                listContainers.IsEnabled = true;
                UpdateListBox();
            }
        }

        public ContainersManager(Project project)
        {
            InitializeComponent();
            Project = project;
        }

        private void UpdateListBox()
        {
            listContainers.Items.Clear();
            foreach (var item in _project.Containers)
            {
                listContainers.Items.Add(item);
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = listContainers.SelectedIndex;
            var enabled = index >= 0;
            buttonAdd.IsEnabled = enabled;
            buttonRemove.IsEnabled = enabled;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var container = new Project.Container()
            {
                Name = "new item"
            };
            _project.Containers.Add(container);
            listContainers.Items.Add(container);
        }
        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            var index = listContainers.SelectedIndex;
            if (index >= 0)
            {
                _project.Containers.RemoveAt(index);
                listContainers.Items.RemoveAt(index);
            }
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
