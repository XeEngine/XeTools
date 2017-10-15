using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class ProjectExplorerViewModel : BaseNotifyPropertyChanged
    {
        private GameStudioViewModel _vm;
        private List<ProjectEntryViewModel> _items;
        private string _searchTerms;

        public ObservableCollection<ProjectEntryViewModel> Items
        {
            get
            {
                if (_items == null)
                    return null;
                if (string.IsNullOrEmpty(_searchTerms))
                    return new ObservableCollection<ProjectEntryViewModel>(_items);
                return new ObservableCollection<ProjectEntryViewModel>(SearchEntries());
            }
            set { }
        }
        public string SearchTerms
        {
            get => _searchTerms;
            set
            {
                _searchTerms = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ProjectExplorerViewModel(GameStudioViewModel vm)
        {
            _vm = vm;
            Register();
            if (_vm.Project != null)
                ProcessProject();
            else
                CleanTree();
        }

        public void Register()
        {
            _vm.OnProjectChanged += OnProjectChanged;
        }
        public void Unregister()
        {
            _vm.OnProjectChanged -= OnProjectChanged;
        }

        private void ProcessProject()
        {
            _items = new List<ProjectEntryViewModel>(
                _vm.Project.GetEntries()
                .Select(x =>
                {
                    if (x is IProjectContainer container)
                        return new ProjectExplorerContainerViewModel(container)
                        as ProjectEntryViewModel;
                    if (x is IProjectDirectory folder)
                        return new ProjectFolderViewModel(folder)
                        as ProjectEntryViewModel;
                    if (x is IProjectFile file)
                        return new ProjectExplorerFileViewModel(file)
                        as ProjectEntryViewModel;
                    return null;
                })
                .OrderBy(x =>
                {
                    if (x is ProjectExplorerContainerViewModel)
                        return 0;
                    if (x is ProjectFolderViewModel)
                        return 1;
                    if (x is ProjectExplorerFileViewModel)
                        return 2;
                    return int.MaxValue;
                })
                .ThenBy(x => x.Name)
                .Where(x => x != null));
            OnPropertyChanged(nameof(Items));
        }
        private void CleanTree()
        {
            _items?.Clear();
            OnPropertyChanged(nameof(Items));
        }
        private IEnumerable<ProjectEntryViewModel> SearchEntries()
        {
            return _items.Where(x => x is ProjectFolderViewModel)
                .SelectMany(x => SearchEntries(x as ProjectFolderViewModel))
                .Union(_items.Where(x => x.Name.Contains(SearchTerms)))
                .OrderBy(x => x.Name.IndexOf(SearchTerms));
        }
        private IEnumerable<ProjectEntryViewModel> SearchEntries(ProjectFolderViewModel folder)
        {
            return folder.Childs.Where(x => x is ProjectFolderViewModel)
                .SelectMany(x => SearchEntries(x as ProjectFolderViewModel))
                .Union(folder.Childs.Where(x => x.Name.Contains(SearchTerms)));
        }

        private void OnProjectChanged(object sender, IProject project)
        {
            if (project != null)
                ProcessProject();
            else
                CleanTree();
        }
    }
    
    public class ProjectEntryViewModel : BaseNotifyPropertyChanged
    {
        public IProjectEntry Entry { get; }

        public string Name
        {
            get => Entry.Name;
            set
            {
                Entry.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ProjectEntryViewModel(IProjectEntry entry)
        {
            Entry = entry;
        }
    }

    public class ProjectExplorerContainerViewModel : ProjectFolderViewModel
    {
        public ProjectExplorerContainerViewModel(IProjectDirectory directory)
            : base(directory) { }
    }

    public class ProjectFolderViewModel : ProjectEntryViewModel
    {
        private IProjectDirectory _directory;

        public ObservableCollection<ProjectEntryViewModel> Childs { get; set; }

        public ProjectFolderViewModel(IProjectDirectory directory)
            : base(directory)
        {
            _directory = directory;
            Childs = new ObservableCollection<ProjectEntryViewModel>(
                directory.GetEntries()
                .Select(x =>
                {
                    if (x is IProjectDirectory folder)
                        return new ProjectFolderViewModel(folder) as ProjectEntryViewModel;
                    if (x is IProjectFile file)
                        return new ProjectExplorerFileViewModel(file) as ProjectEntryViewModel;
                    return null;
                })
                .OrderBy(x =>
                {
                    if (x is ProjectExplorerContainerViewModel)
                        return 0;
                    if (x is ProjectFolderViewModel)
                        return 1;
                    if (x is ProjectExplorerFileViewModel)
                        return 2;
                    return int.MaxValue;
                })
                .ThenBy(x => x.Name)
                .Where(x => x != null));
        }

        public void AddDirectory(IProjectDirectory directory)
        {
            Childs.Add(new ProjectFolderViewModel(directory));
        }

        public void AddFile(IProjectFile file)
        {
            Childs.Add(new ProjectExplorerFileViewModel(file));
        }
    }

    public class ProjectExplorerFileViewModel : ProjectEntryViewModel
    {
        private IProjectFile _file;

        public ProjectExplorerFileViewModel(IProjectFile file)
            : base(file)
        {
            _file = file;
        }
    }
}
