using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class AnimationGroupsViewModel
    {
        public ObservableCollection<AnimationsViewModel> AnimationGroups { get; set; }

        public AnimationGroupsViewModel(Project project, Project.Container container)
        {
            AnimationGroups = new ObservableCollection<AnimationsViewModel>(container.Items
                .Where(x => x.Type == "animation")
                .Select(x => new AnimationsViewModel(project, container, x)));
        }
    }
}
