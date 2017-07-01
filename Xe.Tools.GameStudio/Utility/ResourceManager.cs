using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xe.Tools.Components;
using Xe.Tools.Modules;
using static Xe.Tools.GameStudio.Utility.ResourceManager;

namespace Xe.Tools.GameStudio.Utility
{
    internal static class Extensions
    {
        internal static string GetFileNameWithoutExtensions(this string path)
        {
            return Path.GetFileName(path)?.Split('.').FirstOrDefault();
        }

        internal static string GetFullPath(this Project.Container container, Project project)
        {
            return Path.Combine(project.ProjectPath, container.Name);
        }

        internal static string GetFullPath(this ItemNode node, Project.Container container, Project project)
        {
            if (node == null) return null;
            return Path.Combine(container.GetFullPath(project), node.Path);
        }

        internal static ItemNode GetNode(this TreeViewItem treeViewItem)
        {
            return treeViewItem?.Tag as ItemNode;
        }

        internal static TreeViewItem GetParent(this TreeViewItem treeViewItem)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(treeViewItem);
            while (!(parent is TreeViewItem))
            {
                if (parent == null) return null;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TreeViewItem;
        }
        internal static TreeViewItem GetSelectedItem(this TreeView treeView)
        {
            return treeView.SelectedItem as TreeViewItem;
        }
        internal static ItemNode GetSelectedNode(this TreeView treeView)
        {
            return treeView.GetSelectedItem()?.GetNode();
        }
        internal static string GetSelectedPath(this TreeView treeView)
        {
            return treeView.GetSelectedNode()?.Path;
        }
        internal static ItemNode GetDirectoryNode(this TreeView treeView)
        {
            var node = treeView.GetSelectedNode();
            if (node != null && !node.IsDirectory)
            {
                node = treeView.GetSelectedItem()?.GetParent()?.GetNode();
            }
            return node;
        }
    }

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
        
        /// <summary>
        /// Node selected on the TreeView
        /// </summary>
        public ItemNode SelectedNode
        {
            get => TreeView.GetSelectedNode();
        }

        /// <summary>
        /// Obtains the full path of the currently selected item
        /// </summary>
        public string SelectedFullPath => TreeView.GetSelectedNode().GetFullPath(Container, Project);

        /// <summary>
        /// Obtains the path of the currently selected item
        /// </summary>
        public string SelectedDirectoryPath => TreeView.GetDirectoryNode().Path;

        /// <summary>
        /// Obtains the full path of the currently selected item
        /// </summary>
        public string SelectedDirectoryFullPath => TreeView.GetDirectoryNode().GetFullPath(Container, Project);

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
            // Search for invalid characters
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                if (name.Contains(c))
                    throw new ArgumentException($"Character {c} is not allowed for a directory.", name);
            }

            // Obtain the current directory and create a full path
            var fullPath = Path.Combine(SelectedDirectoryFullPath, name);
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // Add to the current node and TreeViewItem
            if (!SelectedNode.Childs.Keys.Contains(name))
            {
                var node = AddNodeToNode(SelectedNode, name);
                AddNodeToItemCollection(TreeView.GetSelectedItem().Items, node);
            }
        }

        /// <summary>
        /// Obtain the node with the specified file name, searching it from
        /// the current directory node
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ItemNode GetNodeFromCurrentDirectory(string fileName)
        {
            var fileName2 = fileName.GetFileNameWithoutExtensions();
            return TreeView.GetDirectoryNode().Childs.Values
                .Where(x => x.Name.GetFileNameWithoutExtensions() == fileName2)
                .FirstOrDefault();
        }
        
        public void CreateFile(string fileName, Module module, bool deleteExisting = false)
        {
            Delete(GetNodeFromCurrentDirectory(fileName), deleteExisting);

            var treeViewItem = TreeView.GetSelectedItem();
            var node = SelectedNode;
            if (!node.IsDirectory)
            {
                treeViewItem = treeViewItem.GetParent();
                node = treeViewItem.GetNode();
            }

            var fullPath = Path.Combine(node.GetFullPath(Container, Project), fileName);
            var directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // TODO create a new file with the structure of specified module
            using (var writer = new StreamWriter(fullPath))
                writer.WriteLine("{}");

            // Create the project item
            var item = new Project.Item()
            {
                Type = module.Name,
                Parent = Container
            };
            var newNode = AddNodeToNode(node, fileName, item);
            item.Input = newNode.Path.Replace('\\', '/');
            Container.Items.Add(item);

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

        public void AddFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            // file already exists, no need to add it back.
            if (GetNodeFromCurrentDirectory(filePath) != null)
            {
                Log.Message($"File {filePath} does already exist in {SelectedDirectoryPath}");
            }

            var treeViewItem = TreeView.GetSelectedItem();
            var node = SelectedNode;
            if (!node.IsDirectory)
            {
                treeViewItem = treeViewItem.GetParent();
                node = treeViewItem.GetNode();
            }
            
            var fullPath = Path.Combine(node.GetFullPath(Container, Project), fileName);
            if (File.Exists(fullPath))
            {
                if (Path.GetFullPath(filePath) != fullPath)
                {
                    if (!OnFileOverwriteConfirm?.Invoke(fullPath, filePath) ?? false)
                        return;
                }
                // source file and destination file matches, no copy or overwrite message needed.
            }
            else
            {
                var dstPath = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(dstPath))
                    Directory.CreateDirectory(dstPath);
                File.Copy(filePath, fullPath);
            }

            // Create the project item
            var item = new Project.Item()
            {
                Type = "copy"
            };

            // Add the node
            var newNode = AddNodeToNode(node, fileName, item);
            item.Input = newNode.Path.Replace('\\', '/');
            Container.Items.Add(item);

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
            Delete(SelectedNode, physicalDelete);
        }
        public void Delete(ItemNode node, bool physicalDelete)
        {
            if (node == null) return;
            bool isDirectory = node.IsDirectory;
            if (physicalDelete)
            {
                var path = SelectedFullPath;
                if (!isDirectory)
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
                else
                {
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                }
            }

            RemoveNode(node);
            if (node.Parent != null)
                node.Parent.Childs.Remove(node.Name);

            var treeViewItem = TreeView.GetSelectedItem();
            var parent = treeViewItem?.GetParent();
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
                    var path = item.Input.Replace("$(InputDir)/", "").Split(new char[] { '/', '\\' });
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
        private void RemoveNode(ItemNode node)
        {
            foreach (var itemNode in node.Childs.Values)
                RemoveNode(itemNode);
            node.Childs.Clear();
            if (node.Item != null)
                Container.Items.Remove(node.Item);
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

            var viewItem = new TreeViewItem
            {
                Header = new HeaderModel
                {
                    Name = MainNode.Name,
                    Icon = Icons.SpecialFolder,
                    TextColor = TreeView.Foreground
                },
                Tag = MainNode
            };
            foreach (var item in MainNode.Childs.Values)
            {
                AddNodeToItemCollection(viewItem.Items, item);
            }
            TreeView.Items.Add(viewItem);
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
        #endregion
    }
}
