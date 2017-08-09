using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Animations;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class AnimationsMappingViewModel : BaseNotifyPropertyChanged
    {
        private List<AnimationDefinition> _animationsDef { get; set; }
        private AnimationDefinition _selectedAnimationDef;
        private bool[] _checks = new bool[5] { true, false, false, false, false };

        public ObservableCollection<AnimationDefinition> AnimationDefs { get; private set; }

        /// <summary>
        /// List of AnimationData.Animations' names
        /// </summary>
        public List<string> Animations { get; private set; }

        /// <summary>
        /// List of public animations defined from editor's settings
        /// </summary>
        public List<string> AllowedAnimations { get; private set; }


        public AnimationDefinition SelectedAnimationDef
        {
            get => _selectedAnimationDef;
            set
            {
                _selectedAnimationDef = value;
                OnPropertyChanged(nameof(IsAnimationDefSelected));
                OnPropertyChanged(nameof(AnimRef_Default));
                OnPropertyChanged(nameof(AnimRef_Up));
                OnPropertyChanged(nameof(AnimRef_Right));
                OnPropertyChanged(nameof(AnimRef_Down));
                OnPropertyChanged(nameof(AnimRef_Left));
                OnPropertyChanged(nameof(CurrentAnimationReference));
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public bool IsAnimationDefSelected => SelectedAnimationDef != null;

        public AnimationReference AnimRef_Default
        {
            get
            {
                if (IsAnimationDefSelected)
                {
                    if (SelectedAnimationDef.Default == null)
                        SelectedAnimationDef.Default = new AnimationReference();
                    return SelectedAnimationDef.Default;
                }
                else return null;
            }
        }
        public AnimationReference AnimRef_Up
        {
            get
            {
                if (IsAnimationDefSelected)
                {
                    if (SelectedAnimationDef.DirectionUp == null)
                        SelectedAnimationDef.DirectionUp = new AnimationReference();
                    return SelectedAnimationDef.DirectionUp;
                }
                else return null;
            }
        }
        public AnimationReference AnimRef_Right
        {
            get
            {
                if (IsAnimationDefSelected)
                {
                    if (SelectedAnimationDef.DirectionRight == null)
                        SelectedAnimationDef.DirectionRight = new AnimationReference();
                    return SelectedAnimationDef.DirectionRight;
                }
                else return null;
            }
        }
        public AnimationReference AnimRef_Down
        {
            get
            {
                if (IsAnimationDefSelected)
                {
                    if (SelectedAnimationDef.DirectionDown == null)
                        SelectedAnimationDef.DirectionDown = new AnimationReference();
                    return SelectedAnimationDef.DirectionDown;
                }
                else return null;
            }
        }
        public AnimationReference AnimRef_Left
        {
            get
            {
                if (IsAnimationDefSelected)
                {
                    if (SelectedAnimationDef.DirectionLeft == null)
                        SelectedAnimationDef.DirectionLeft = new AnimationReference();
                    return SelectedAnimationDef.DirectionLeft;
                }
                else return null;
            }
        }

        public bool EditDefault
        {
            get => _checks[0];
            set
            {
                _checks[0] = value;
                OnPropertyChanged(nameof(CurrentAnimationReference));
                OnPropertyChanged(nameof(SelectedAnimationDirection));
            }
        }

        public bool IsCheckedDirection_Default
        {
            get => _checks[0];
            set
            {
                _checks[0] = value;
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public bool IsCheckedDirection_Up
        {
            get => _checks[1];
            set
            {
                _checks[1] = value;
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public bool IsCheckedDirection_Right
        {
            get => _checks[2];
            set
            {
                _checks[2] = value;
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public bool IsCheckedDirection_Down
        {
            get => _checks[3];
            set
            {
                _checks[3] = value;
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public bool IsCheckedDirection_Left
        {
            get => _checks[4];
            set
            {
                _checks[4] = value;
                OnPropertyChanged(nameof(SelectedAnimationDirection));
                OnPropertyChanged(nameof(SelectedAnimationFlipX));
                OnPropertyChanged(nameof(SelectedAnimationFlipY));
            }
        }

        public AnimationReference CurrentAnimationReference
        {
            get
            {
                if (_checks[0]) return AnimRef_Default;
                if (_checks[1]) return AnimRef_Up;
                if (_checks[2]) return AnimRef_Right;
                if (_checks[3]) return AnimRef_Down;
                if (_checks[4]) return AnimRef_Left;
                return null;
            }
        }

        public string SelectedAnimationDirection
        {
            get => CurrentAnimationReference?.Name;
            set
            {
                if (CurrentAnimationReference != null)
                {
                    CurrentAnimationReference.Name = value;
                    OnPropertyChanged(nameof(AnimRef_Default));
                    OnPropertyChanged(nameof(AnimRef_Up));
                    OnPropertyChanged(nameof(AnimRef_Right));
                    OnPropertyChanged(nameof(AnimRef_Down));
                    OnPropertyChanged(nameof(AnimRef_Left));
                }
            }
        }

        public bool SelectedAnimationFlipX
        {
            get => CurrentAnimationReference?.FlipX ?? false;
            set
            {
                if (CurrentAnimationReference != null)
                    CurrentAnimationReference.FlipX = value;
            }
        }

        public bool SelectedAnimationFlipY
        {
            get => CurrentAnimationReference?.FlipY ?? false;
            set
            {
                if (CurrentAnimationReference != null)
                    CurrentAnimationReference.FlipY = value;
            }
        }

        public AnimationsMappingViewModel(List<AnimationDefinition> animationsDef,
            List<Animation> animations,
            List<string> allowedAnimations)
        {
            _animationsDef = animationsDef;
            AnimationDefs = new ObservableCollection<AnimationDefinition>(_animationsDef);
            Animations = animations.Select(x => x.Name).ToList();
            AllowedAnimations = allowedAnimations;
        }

        public void SaveChanges()
        {
            _animationsDef.Clear();
            _animationsDef.AddRange(AnimationDefs);
        }
    }
}
