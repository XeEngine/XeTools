using Xe.Game.Animations;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class AnimationViewModel
    {
        public string Name { get; set; }

        public AnimationViewModel(Animation animation)
        {
            Name = animation.Name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
