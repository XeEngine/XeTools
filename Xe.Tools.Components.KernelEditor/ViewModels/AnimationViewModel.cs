using Xe.Game.Animations;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class AnimationDefinitionViewModel
    {
        public string Name { get; set; }

        public AnimationDefinitionViewModel(AnimationDefinition animationDefinition)
        {
            Name = animationDefinition.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
