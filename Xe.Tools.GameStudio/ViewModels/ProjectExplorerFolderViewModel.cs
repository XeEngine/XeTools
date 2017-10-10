using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class ProjectExplorerFolderViewModel :
        ProjectExplorerViewModel.ProjectExplorerItemViewModel
    {
        private IProjectDirectory _directory;

        public ObservableCollection<ProjectExplorerViewModel.ProjectExplorerItemViewModel> Childs { get; set; }

        public ProjectExplorerFolderViewModel(IProjectDirectory directory)
            : base(directory)
        {
            _directory = directory;
            Childs = new ObservableCollection<ProjectExplorerViewModel.ProjectExplorerItemViewModel>(
                directory.GetEntries()
                .Select(x =>
                {
                    if (x is IProjectDirectory folder)
                        return new ProjectExplorerFolderViewModel(folder)
                        as ProjectExplorerViewModel.ProjectExplorerItemViewModel;
                    if (x is IProjectFile file)
                        return new ProjectExplorerFileViewModel(file)
                        as ProjectExplorerViewModel.ProjectExplorerItemViewModel;
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
        }

        public void AddDirectory(IProjectDirectory directory)
        {
            Childs.Add(new ProjectExplorerFolderViewModel(directory));
        }

        public void AddFile(IProjectFile file)
        {
            Childs.Add(new ProjectExplorerFileViewModel(file));
        }
    }
}
