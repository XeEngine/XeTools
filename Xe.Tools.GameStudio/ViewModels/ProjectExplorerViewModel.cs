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
        public class ProjectExplorerItemViewModel : BaseNotifyPropertyChanged
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

            public ProjectExplorerItemViewModel(IProjectEntry entry)
            {
                Entry = entry;
            }
        }

        private GameStudioViewModel _vm;
        private List<ProjectExplorerItemViewModel> _items;
        private string _searchTerms;

        public ObservableCollection<ProjectExplorerItemViewModel> Items
        {
            get
            {
                if (_items == null)
                    return null;
                if (string.IsNullOrEmpty(_searchTerms))
                    return new ObservableCollection<ProjectExplorerItemViewModel>(_items);
                return new ObservableCollection<ProjectExplorerItemViewModel>(SearchEntries());
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
            _items = new List<ProjectExplorerItemViewModel>(
                _vm.Project.GetEntries()
                .Select(x =>
                {
                    if (x is IProjectContainer container)
                        return new ProjectExplorerContainerViewModel(container)
                        as ProjectExplorerItemViewModel;
                    if (x is IProjectDirectory folder)
                        return new ProjectExplorerFolderViewModel(folder)
                        as ProjectExplorerItemViewModel;
                    if (x is IProjectFile file)
                        return new ProjectExplorerFileViewModel(file)
                        as ProjectExplorerItemViewModel;
                    return null;
                })
                .OrderBy(x =>
                {
                    if (x is ProjectExplorerContainerViewModel)
                        return 0;
                    if (x is ProjectExplorerFolderViewModel)
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
        private IEnumerable<ProjectExplorerItemViewModel> SearchEntries()
        {
            return _items.Where(x => x is ProjectExplorerFolderViewModel)
                .SelectMany(x => SearchEntries(x as ProjectExplorerFolderViewModel))
                .Union(_items.Where(x => x.Name.Contains(SearchTerms)))
                .OrderBy(x => x.Name.IndexOf(SearchTerms));
        }
        private IEnumerable<ProjectExplorerItemViewModel> SearchEntries(ProjectExplorerFolderViewModel folder)
        {
            return folder.Childs.Where(x => x is ProjectExplorerFolderViewModel)
                .SelectMany(x => SearchEntries(x as ProjectExplorerFolderViewModel))
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
}
