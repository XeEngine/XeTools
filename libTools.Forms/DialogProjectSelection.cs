using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace libTools.Forms
{
    public partial class DialogProjectSelection : Form
    {
        private Project _Project;
        private string _TypeFilter;
        private IList<Project.Container> currentContainerList;
        private Project.Container _CurrentContainer;
        private Project.Item _CurrentItem;
        private IEnumerable<Project.Item> currentItemList;
        private Project.Item[] currentItemListArray;

        public Project Project { get { return _Project; } }
        public string TypeFilter { get { return _TypeFilter; } }
        public Project.Container CurrentContainer { get { return _CurrentContainer; } }
        public Project.Item CurrentItem { get { return _CurrentItem; } }
        public string FileName
        {
            get
            {
                if (CurrentContainer == null || CurrentItem == null)
                    return null;
                return CurrentItem.FileNameInput;
            }
        }

        public DialogProjectSelection(Project project, string typeFilter)
        {
            _Project = project;
            _TypeFilter = typeFilter;
            InitializeComponent();
        }

        private void ShowErrorNoFiles()
        {
            var msg = string.Format("Unable to find any files of {0} type.", TypeFilter);
            MessageBox.Show(msg, "No files found", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DialogProjectSelection_Load(object sender, EventArgs e)
        {
            currentContainerList = Project.Containers.Where(
                x => x.GetItemsFromType(TypeFilter).Count() > 0).
                ToList();

            switch (currentContainerList.Count)
            {
                case 0: // Non ha trovato nessun file corrispondente al tipo cercato.
                    ShowErrorNoFiles();
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                case 1:
                    _CurrentContainer = currentContainerList.First();
                    currentItemList = CurrentContainer.GetItemsFromType(TypeFilter);
                    currentItemList = currentItemList.Where(x => !x.FileNameInput.Contains("$(TempDir)"));
                    if (currentItemList.Count() == 1)
                    {
                        // Ha trovato un solo file di quel solo tipo, lo ritorna subito.
                        _CurrentItem = currentItemList.First();
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }
                    break;
            }
            labelInfo.Text = string.Format("{0}, Developed by {1} - {2}",
                Project.Name, Project.Company, Project.Year);
            comboBoxContainer.DataSource = currentContainerList;
            comboBoxContainer.Enabled = currentContainerList.Count > 1;
        }

        private void comboBoxContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ((ComboBox)sender).SelectedIndex;
            if (index >= 0)
            {
                _CurrentContainer = currentContainerList.ElementAt(index);
                currentItemList = _CurrentContainer.GetItemsFromType(TypeFilter);
                currentItemListArray = currentItemList.ToArray();
                listBoxItems.Enabled = true;
                listBoxItems.DataSource = currentItemListArray;
            }
            else
            {
                _CurrentContainer = null;
                listBoxItems.Enabled = false;
                listBoxItems.DataSource = null;
            }
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ((ListBox)sender).SelectedIndex;
            if (index >= 0)
            {
                _CurrentItem = currentItemListArray[index];
                buttonOk.Enabled = true;
            }
            else
            {
                _CurrentItem = null;
                buttonOk.Enabled = false;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
