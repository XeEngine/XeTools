using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.GameStudio.Commands;
using Xe.Tools.Projects;
using Xe.Tools.Wpf;

namespace Xe.Tools.GameStudio.ViewModels
{
    public class GameStudioViewModel : BaseNotifyPropertyChanged
    {
        private IProject _project;
        private ProjectOpenCommand ProjectOpen { get; }

        public IProject Project
        {
            get => _project;
            set
            {
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsProjectLoaded));
            }
        }

        public bool IsProjectLoaded => Project != null;

        public GameStudioViewModel()
        {
            ProjectOpen = new ProjectOpenCommand(this);
        }
    }
}
