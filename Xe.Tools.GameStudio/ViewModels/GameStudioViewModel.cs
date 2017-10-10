using System.IO;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class GameStudioViewModel : BaseNotifyPropertyChanged
    {
        private IProject _project;
        private IProjectEntry _selectedProjectEntry;

        public static GameStudioViewModel Instance = new GameStudioViewModel();

        public delegate void ProjectChanged(object sender, IProject project);
        public delegate void SelectProjectEntry(object sender, IProjectEntry project);
        public event ProjectChanged OnProjectChanged;
        public event SelectProjectEntry OnSelectProjectEntry;

        public IProject Project
        {
            get => _project;
            private set
            {
                _project = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsProjectLoaded));
                OnProjectChanged?.Invoke(this, value);
            }
        }
        public string ProjectFileName { get; private set; }

        public bool IsProjectLoaded => Project != null;

        public IProjectEntry SelectedProjectEntry
        {
            get => _selectedProjectEntry;
            set
            {
                _selectedProjectEntry = value;
                OnSelectProjectEntry?.Invoke(this, value);
            }
        }

        public GameStudioViewModel()
        {
        }

        public void LoadProject(string fileName)
        {
            ProjectFileName = fileName;
            Project = new XeGsProj().Open(fileName);
        }
        public void SaveProject(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = ProjectFileName;
            Project = new XeGsProj().Open(fileName);
        }
    }
}
