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
using Xe.Tools.GameStudio.Utility;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for ResourceTreeView.xaml
	/// </summary>
	public partial class ResourceView : UserControl
    {
        private class ItemNode
        {
            public string Name { get; set; }
            public Project.Item Item { get; set; }
            public Dictionary<string, ItemNode> Childs { get; set; } = new Dictionary<string, ItemNode>();
            public bool IsDirectory { get => Item == null; }
        }
        private class HeaderModel
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public object Icon { get; set; }
            public Brush TextColor { get; set; }
        }

        private Project _project;
        private Project.Container _container;
        private ItemNode _mainNode = new ItemNode();
        private bool _isUpdatingContainersList = false;

        public Project Project
        {
            get => _project;
            set
            {
                _project = value;
                UpdateContainersList();
            }
        }
        protected Project.Container Container
        {
            get => _container;
            set
            {
                _container = value;
                UpdateFilesList();
            }
        }

		public ResourceView()
		{
			InitializeComponent();
            //Container = new ContainerTest().Container;
        }

        private void ComboBoxContainers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingContainersList) return;

            var comboBox = sender as ComboBox;
            var index = comboBox.SelectedIndex;
            if (index >= comboBox.Items.Count - 1)
            {
                // Apre la finestra di gestione dei contenitori
                var dialog = new ContainersManager(Project);
                var result = dialog.ShowDialog();
                _isUpdatingContainersList = true;
                UpdateContainersList();
                _isUpdatingContainersList = false;
            }
            else if (index >= 0)
            {
                Container = Project.Containers[index];
            }
        }

        /// <summary>
        /// Aggiorna la combobox che contiene i contenitori
        /// </summary>
        private void UpdateContainersList()
        {
            var prevItem = _comboBoxContainers.SelectedValue;
            _comboBoxContainers.Items.Clear();
            foreach (var item in _project.Containers)
            {
                _comboBoxContainers.Items.Add(item);
            }
            _comboBoxContainers.Items.Add("<manage items>");
            if (_comboBoxContainers.Items.Count > 1)
            {
                if (prevItem != null)
                    _comboBoxContainers.SelectedValue = prevItem;
                else
                    _comboBoxContainers.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Aggiorna la lista dei files
        /// </summary>
        private void UpdateFilesList()
        {
            _mainNode.Childs.Clear();
            _mainNode.Name = Container.Name;
            if (Container.Items != null)
            {
                foreach (var item in Container.Items)
                {
                    var path = item.Input.Replace("$(InputDir)/", "").Split('/');
                    AddItemToNode(item, _mainNode, path, 0);
                }
            }

            treeFileView.Items.Clear();
            foreach (var item in _mainNode.Childs.Values)
            {
                AddNodeChildsToItemCollection(treeFileView.Items, item);
            }
        }
        
        private void AddItemToNode(Project.Item item, ItemNode node, string[] path, int index)
        {
            if (index < path.Length - 1)
            {
                var name = path[index];
                if (!node.Childs.TryGetValue(name, out var child))
                {
                    child = new ItemNode()
                    {
                        Name = name
                    };
                    node.Childs.Add(name, child);
                }
                AddItemToNode(item, child, path, index + 1);
            }
            else if (index < path.Length)
            {
                var name = path[index];
                node.Childs.Add(name, new ItemNode()
                {
                    Name = name,
                    Item = item
                });
            }
        }


        private void AddNodeChildsToItemCollection(ItemCollection itemCollection, ItemNode node)
        {
            var viewItem = new TreeViewItem
            {
                Header = new HeaderModel
                {
                    Name = node.Name,
                    Icon = node.IsDirectory ? Icons.Folder : Icons.Document,
                    TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0))
                },
                Tag = node.Item
            };
            itemCollection.Add(viewItem);

            foreach (var item in node.Childs)
            {
                AddNodeChildsToItemCollection(viewItem.Items, item.Value);
            }
        }
    }
}
