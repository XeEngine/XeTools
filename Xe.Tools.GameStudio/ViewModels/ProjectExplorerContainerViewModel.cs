using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class ProjectExplorerContainerViewModel : ProjectExplorerFolderViewModel
    {
        public ProjectExplorerContainerViewModel(IProjectDirectory directory)
            : base(directory) { }
    }
}
