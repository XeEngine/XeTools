using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Animations;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class AnimationsMappingViewModel
    {
        private List<AnimationRef> _animationsRef { get; set; }

        public ObservableCollection<AnimationMappingViewModel> AnimationRefs { get; private set; }

        /// <summary>
        /// List of AnimationData.Animations' names
        /// </summary>
        public List<string> Animations { get; private set; }

        /// <summary>
        /// List of public animations defined from editor's settings
        /// </summary>
        public List<string> AllowedAnimations { get; private set; }

        public EnumViewModel<Direction> Directions { get; private set; } = new EnumViewModel<Direction>();

        public AnimationsMappingViewModel(List<AnimationRef> animationsRef,
            List<Animation> animations,
            List<string> allowedAnimations)
        {
            _animationsRef = animationsRef;
            AnimationRefs = new ObservableCollection<AnimationMappingViewModel>(_animationsRef
                .Select(x => new AnimationMappingViewModel(x)));
            Animations = animations.Select(x => x.Name).ToList();
            AllowedAnimations = allowedAnimations;
        }

        public void SaveChanges()
        {
            _animationsRef.Clear();
            _animationsRef.AddRange(AnimationRefs.
                Select(x => x.AnimationReference));
        }
    }
}
