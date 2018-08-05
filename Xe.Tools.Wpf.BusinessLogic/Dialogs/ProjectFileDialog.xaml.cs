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
using Xe.Tools.Projects;

namespace Xe.Tools.Wpf.Dialogs
{
    /// <summary>
    /// Interaction logic for ProjectFileDialog.xaml
    /// </summary>
    public partial class ProjectFileDialog : Window
    {
        private class ViewModel : BaseNotifyPropertyChanged
        {
            const string STR_ALL_FILES = "All files";

            public IEnumerable<IProjectFile> Files { get; }
            private string _selectedFileType;

            public IEnumerable<string> FileTypesList { get; }

            public string SelectedFileType
            {
                get => _selectedFileType;
                set
                {
                    _selectedFileType = value;
                    OnPropertyChanged(nameof(FileItems));
                }
            }

            public int SelectedFileTypeIndex { get; set; }

            public IEnumerable<IProjectFile> FileItems
            {
                get
                {
                    if (_selectedFileType == STR_ALL_FILES)
                        return Files;
                    return Files
                        .Where(x => x.Format == _selectedFileType);
                }
            }

            public IProjectFile SelectedFile { get; set; }

            public ViewModel(IProject project, IEnumerable<string> fileTypes)
            {
                Files = project.GetFiles();
                if (fileTypes != null)
                {
                    FileTypesList = fileTypes;
                }
                else
                {
                    FileTypesList = Files
                        .Select(x => x.Format)
                        .Distinct()
                        .Prepend(STR_ALL_FILES);
                }
            }
        }
        
        private ViewModel _vm;

        public IProjectFile SelectedFile { get; private set; }

        public ProjectFileDialog(IProject project, IEnumerable<string> fileTypes = null)
        {
            InitializeComponent();
            DataContext = _vm = new ViewModel(project, fileTypes);
            if (_vm.FileTypesList.Any())
                _vm.SelectedFileTypeIndex = 0;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedFile = _vm.SelectedFile;
            DialogResult = true;
            Close();
        }
    }
}
