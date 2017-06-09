using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xe.Tools.GameStudio.Utility
{
    internal class ResourceManager
    {
        public class ItemNode
        {
            public ItemNode Parent { get; set; }
            public string Name { get; set; }
            public string Path
            {
                get => Parent == null ? string.Empty : System.IO.Path.Combine(Parent.Path, Name);
            }

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

        public delegate bool FileOverwriteConfirm(string originalFile, string newFile);
        public event FileOverwriteConfirm OnFileOverwriteConfirm;

        public ItemNode MainNode { get; private set; }

        public Project Project { get; private set; }
        public Project.Container Container
        {
            get => _container;
            set
            {
                if (!Project.Containers.Contains(value))
                    throw new ArgumentOutOfRangeException("Invalid container specified");
                _container = value;
                MainNode = new ItemNode()
                {
                    Parent = null,
                    Name = _container.Name,
                };
                CreateNodeFromContainer();
                PopulateTreeView();
            }
        }

        public TreeView TreeView { get; private set; }
        public TreeViewItem SelectedTreeViewItem
        {
            get => TreeView.SelectedItem as TreeViewItem;
        }
        public ItemNode SelectedNode
        {
            get => SelectedTreeViewItem?.Tag as ItemNode;
        }
        public string SelectedPath
        {
            get => SelectedNode?.Path;
        }
        public string SelectedFullPath
        {
            get
            {
                var basePath = ContainerPath;
                var selectedPath = SelectedPath;
                if (selectedPath == null)
                    return basePath;
                return Path.Combine(basePath, selectedPath);
            }
        }

        public ResourceManager(Project project, TreeView treeView)
        {
            Project = project;
            TreeView = treeView;
            MainNode = new ItemNode();
        }


        /// <summary>
        /// Create a directory and add it to the TreeView.
        /// </summary>
        /// <param name="name">Name of the directory to add</param>
        public void CreateDirectory(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                if (name.Contains(c))
                    throw new ArgumentException($"Character {c} is not allowed for a directory.", name);
            }
            var fullPath = Path.Combine(SelectedFullPath, name);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // Add to the current node and TreeViewItem
            if (!SelectedNode.Childs.Keys.Contains(name))
            {
                var node = AddNodeToNode(SelectedNode, name);
                AddNodeToItemCollection(SelectedTreeViewItem.Items, node);
            }
        }
        
        public void AddFile(string file)
        {
            // Obtains the node that is a directory
            var treeViewItem = SelectedTreeViewItem;
            if (!SelectedNode.IsDirectory)
                treeViewItem = GetTreeViewItemParent(treeViewItem);
            var node = treeViewItem.Tag as ItemNode;

            var path = node.Path;
            var filename = Path.GetFileName(file);
            var dstFile = Path.Combine(path, filename);
            var fullPath = Path.GetFullPath(
                Path.Combine(Project.ProjectPath,
                Path.Combine(Container.Name, dstFile)));
            if (File.Exists(fullPath))
            {
                if (fullPath != Path.GetFullPath(file))
                {
                    if (!OnFileOverwriteConfirm?.Invoke(fullPath, file) ?? false)
                        return;
                }
                // source file and destination file matches, no copy or overwrite message needed.
            }
            else
            {
                File.Copy(file, fullPath);
            }

            // Add the node
            var newNode = AddNodeToNode(node, filename, new Project.Item()
            {
                Type = "copy",
                Input = path
            });

            // Add the item to the tree
            treeViewItem.Items.Add(new TreeViewItem()
            {
                Header = new HeaderModel
                {
                    Name = newNode.Name,
                    Icon = Icons.Document,
                    TextColor = TreeView.Foreground
                },
                Tag = newNode
            });
        }

        public void Delete(bool physicalDelete)
        {
            var node = SelectedNode;
            if (physicalDelete)
            {
                var path = SelectedFullPath;
                if (!node.IsDirectory)
                    File.Delete(path);
                else
                    Directory.Delete(path, true);
            }

            node.Parent.Childs.Remove(node.Name);

            var treeViewItem = SelectedTreeViewItem;
            var parent = GetTreeViewItemParent(treeViewItem);
            if (parent != null)
            {
                parent.Items.Remove(treeViewItem);
            }
            else
            {
                // HACK Se per puro caso non dovesse trovare il parent, ricostruisci l'intero albero.
                PopulateTreeView();
            }
        }


        private Project.Container _container;

        private string ContainerPath
        {
            get => Path.Combine(Project.ProjectPath, Container.Name);
        }
        
        #region nodes management
        /// <summary>
        /// Aggiorna la lista dei files.
        /// Crea un albero, dove ogni nodo è un file o un contenitore di nodi
        /// </summary>
        private void CreateNodeFromContainer()
        {
            MainNode.Childs.Clear();
            MainNode.Name = Container.Name;
            if (Container.Items != null)
            {
                foreach (var item in Container.Items)
                {
                    var path = item.Input.Replace("$(InputDir)/", "").Split('/');
                    AddItemToNode(item, MainNode, path, string.Empty, 0);
                }
            }
        }
        private void AddItemToNode(Project.Item item, ItemNode node, string[] path, string fullpath, int index)
        {
            if (index < path.Length - 1)
            {
                var name = path[index];
                fullpath = Path.Combine(fullpath, name);
                if (!node.Childs.TryGetValue(name, out var child))
                {
                    child = AddNodeToNode(node, name);
                }
                AddItemToNode(item, child, path, fullpath, index + 1);
            }
            else if (index < path.Length)
            {
                AddNodeToNode(node, path[index], item);
            }
        }
        private static ItemNode AddNodeToNode(ItemNode parent, string name, Project.Item item = null)
        {
            ItemNode node;
            parent.Childs.Add(name, node = new ItemNode()
            {
                Parent = parent,
                Name = name,
                Item = item,
            });
            return node;
        }
        #endregion

        #region tree management
        private void PopulateTreeView()
        {
            TreeView.Items.Clear();
            foreach (var item in MainNode.Childs.Values)
            {
                AddNodeToItemCollection(TreeView.Items, item);
            }
        }
        private void AddNodeToItemCollection(ItemCollection itemCollection, ItemNode node)
        {
            var viewItem = new TreeViewItem
            {
                Header = new HeaderModel
                {
                    Name = node.Name,
                    Icon = node.IsDirectory ? Icons.Folder : Icons.Document,
                    TextColor = TreeView.Foreground
                },
                Tag = node
            };
            itemCollection.Add(viewItem);

            foreach (var item in node.Childs)
            {
                AddNodeToItemCollection(viewItem.Items, item.Value);
            }
        }
        private static TreeViewItem GetTreeViewItemParent(TreeViewItem treeViewItem)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(treeViewItem);
            while (!(parent is TreeViewItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TreeViewItem;
        }
        #endregion
    }
}
