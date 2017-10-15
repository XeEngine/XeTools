using System.Collections.Generic;
using System.Linq;
using Xe.Game.Animations;
using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
    public class AnimationService
    {
        public ProjectService ProjectService { get; private set; }

        public IEnumerable<IProjectFile> ProjectFiles { get; private set; }

        public IEnumerable<string> AnimationData => ProjectFiles.Select(x =>
        {
            var strName = x.Name;
            var extIndex = strName.IndexOf(".json");
            return strName.Substring(0, extIndex);
        });

        internal AnimationService(ProjectService projectService)
        {
            ProjectService = projectService;
            ProjectFiles = ProjectService.Items.Where(x => x.Format == "animation");
        }

        public AnimationData GetAnimationData(IProjectFile item)
        {
            return ProjectService.DeserializeItem<AnimationData>(item);
        }

        public IEnumerable<string> GetAnimationDefinitions(string animationData)
        {
            var item = ProjectFiles.FirstOrDefault(x => x.Name == animationData);
            if (item != null)
            {
                return GetAnimationData(item).AnimationDefinitions.Select(x => x.Name);
            }
            return null;
        }
    }
}
