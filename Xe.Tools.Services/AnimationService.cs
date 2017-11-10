using System.Collections.Generic;
using System.Linq;
using Xe.Game.Animations;
using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
    /// <summary>
    /// Manages animation files from a project
    /// </summary>
    public class AnimationService
    {
        public ProjectService ProjectService { get; }

        public IEnumerable<IProjectFile> ProjectFiles => ProjectService.Items.Where(x => x.Format == "animation");

        public IEnumerable<string> AnimationFilesData => ProjectFiles.Select(x =>
        {
            var strName = x.Name;
            var extIndex = strName.IndexOf(".json");
            return strName.Substring(0, extIndex);
        });

        public AnimationService(ProjectService projectService)
        {
            ProjectService = projectService;
        }

        public AnimationData GetAnimationData(string fileName)
        {
            return GetAnimationData(ProjectFiles.SingleOrDefault(x => x.Name == fileName));
        }

        public AnimationData GetAnimationData(IProjectFile item)
        {
            return ProjectService?.DeserializeItem<AnimationData>(item);
        }

        public IEnumerable<string> GetAnimationDefinitions(string animationData)
        {
            var item = ProjectFiles.FirstOrDefault(x => x.Path == animationData);
            if (item != null)
            {
                return GetAnimationData(item).AnimationDefinitions.Select(x => x.Name);
            }
            return null;
        }
    }
}
