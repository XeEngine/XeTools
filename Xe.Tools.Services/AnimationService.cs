using System.Collections.Generic;
using System.Linq;
using Xe.Game.Animations;
using static Xe.Tools.Project;

namespace Xe.Tools.Services
{
    public class AnimationService
    {
        public ProjectService ProjectService { get; private set; }

        public IEnumerable<Item> Items { get; private set; }

        public IEnumerable<string> AnimationData => Items.Select(x => x.Input);

        internal AnimationService(ProjectService projectService)
        {
            ProjectService = projectService;
            Items = ProjectService.Items.Where(x => x.Type == "animation");
        }

        public AnimationData GetAnimationData(Item item)
        {
            return ProjectService.DeserializeItem<AnimationData>(item);
        }

        public IEnumerable<string> GetAnimationDefinitions(string animationData)
        {
            var item = Items.FirstOrDefault(x => x.Input == animationData);
            if (item != null)
            {
                return GetAnimationData(item).AnimationDefinitions.Select(x => x.Name);
            }
            return null;
        }
    }
}
