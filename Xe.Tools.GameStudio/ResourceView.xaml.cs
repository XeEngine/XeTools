using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.Wpf.Dialogs;
using static Xe.Tools.GameStudio.Utility.ResourceManager;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for ResourceTreeView.xaml
	/// </summary>
	public partial class ResourceView : UserControl
    {
        private Project _project;
        private ResourceManager _resourceManager;
        private bool _isUpdatingContainersList = false;

        public Project Project
        {
            get => _project;
            set
            {
                if (value == null) return;
                _project = value;
                _resourceManager = new ResourceManager(_project, treeFileView);
                _resourceManager.OnFileOverwriteConfirm += ResourceManager_OnFileOverwriteConfirm;
                UpdateContainersList();
            }
        }

        private bool ResourceManager_OnFileOverwriteConfirm(string originalFile, string newFile)
        {
            return MessageBox.Show("Do you want to overwrite the existing file?",
                "Overwrite confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) ==
                MessageBoxResult.Yes;
        }

        protected Project.Container Container
        {
            get => _resourceManager.Container;
            set => _resourceManager.Container = value;
        }

        private ItemNode _mainNode { get => _resourceManager.MainNode; }
        
        private ItemNode SelectedNode
        {
            get => _resourceManager.SelectedNode;
        }

		public ResourceView()
		{
			InitializeComponent();
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
#if DEBUG
                //Test("sprite/ch/dummy.anim.json");
#endif
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

        private void treeFileView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            bool isItemSelected = SelectedNode != null;
            ctrlButtonNewItem.IsEnabled = isItemSelected;
            ctrlButtonAddItem.IsEnabled = isItemSelected;
            ctrlButtonRemoveItem.IsEnabled = isItemSelected;
            ctrlButtonAddFolder.IsEnabled = isItemSelected;
        }

        private void ctrlButtonNewItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialogs.NewFileDialog();
            if (dialog.ShowDialog() == true)
            {
                var fileName = dialog.FileName;
                var component = dialog.SelectedComponent;
                var module = Globals.Modules
                    .Where(x => x.Name == component.ComponentInfo.ModuleName)
                    .FirstOrDefault();
                if (module != null)
                    _resourceManager.CreateFile(fileName, module);
                else
                    Log.Error($"Module {component.ComponentInfo.ModuleName} from component {component.Name} was not found.");
            }
        }
        private void ctrlButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(FileDialog.Behavior.Open, FileDialog.Type.Any, true);
            if (fd.ShowDialog() ?? false)
            {
                foreach  (var filename in fd.FileNames)
                {
                    _resourceManager.AddFile(filename);
                }
            }
        }
        private void ctrlButtonRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var node = _resourceManager.SelectedNode;
            if (node == null) return;

            const string strDeleteDir = "You are deleting a folder with some files inside.\nDo you want to delete the content? If you click No, they will be deleted only from this application.";
            const string strFileDir = "Do you want to delete the file? If you click No, it will be deleted only from this application.";

            string strMessage;
            var path = _resourceManager.SelectedFullPath;
            if (node.IsDirectory && Directory.Exists(path))
            {
                if (Directory.EnumerateFileSystemEntries(path).Count() > 0)
                    strMessage = strDeleteDir;
                else
                    strMessage = null;
            }
            else
                strMessage = strFileDir;

            bool physicalDelete;
            if (strMessage != null)
            {
                var r = MessageBox.Show(strMessage, "Delete confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                physicalDelete = r == MessageBoxResult.Yes;
            }
            else
                physicalDelete = true;
            _resourceManager.Delete(physicalDelete);
        }

        private void ctrlButtonAddFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SingleInputDialog()
            {
                Title = "Create a new folder",
                Description = "Please specify the name of the folder that you want to create",
                Text = "new folder"
            };
            if (dialog.ShowDialog() ?? false)
            {
                _resourceManager.CreateDirectory(dialog.Text);
            }
        }

        private void treeFileView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var node = SelectedNode;
            if (node != null && !node.IsDirectory)
            {
                var moduleName = node.Item.Type;
                var component = Globals.Components
                    .Where(x => x.ComponentInfo.ModuleName == moduleName)
                    .FirstOrDefault();

                bool? result;
                if (component != null)
                {
                    var instance = component.CreateInstance(new Components.ComponentProperties()
                    {
                        Project = Project,
                        Container = Container,
                        Item = node.Item
                    });
                    instance.ShowSettings();
                }
                else
                {
                    var dialog = new Dialogs.EmptyComponentDialog(moduleName);
                    result = dialog.ShowDialog();
                }
            }
        }
        
        private void Test(string inputFileName)
        {
            var item = Container.Items.FirstOrDefault(x => x.Input == inputFileName);
            if (item != null)
            {
                var component = Globals.Components
                    .FirstOrDefault(x => x.ComponentInfo.ModuleName == item.Type);
                if (component != null)
                {
                    component.CreateInstance(new Components.ComponentProperties()
                    {
                        Project = Project,
                        Container = Container,
                        Item = item
                    }).ShowSettings();
                }
            }
        }
    }
}
