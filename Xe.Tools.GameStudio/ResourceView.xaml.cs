using System;
using System.Collections.Generic;
using System.IO;
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

        private TreeViewItem SelectedTreeViewItem
        {
            get => _resourceManager.SelectedTreeViewItem;
        }
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
            ctrlButtonAddItem.IsEnabled = isItemSelected;
            ctrlButtonRemoveItem.IsEnabled = isItemSelected;
            ctrlButtonAddFolder.IsEnabled = isItemSelected;
        }

        private void ctrlButtonNewItem_Click(object sender, RoutedEventArgs e)
        {
        }
        private void ctrlButtonAddItem_Click(object sender, RoutedEventArgs e)
        {
            var fd = FileDialog.Factory(FileDialog.Behavior.Open, FileDialog.Type.Any);
            if (fd.ShowDialog() ?? false)
            {
                _resourceManager.AddFile(fd.FileName);
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
            if (node.IsDirectory)
            {
                if (Directory.EnumerateFileSystemEntries(path)
                    .Count() > 0)
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
            var dialog = new Dialogs.SingleInputDialog()
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
    }
}
