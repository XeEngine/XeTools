using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class AnimationViewModel : BaseNotifyPropertyChanged
    {
        #region definitions

        public class FrameRefViewModel
        {
            private SpriteService SpriteService => SpriteService.Instance;

            public Texture Texture { get; private set; }

            public FrameRef FrameRef { get; private set; }

            public Frame Frame { get; private set; }

            public string Name => FrameRef.Frame;

            public BitmapSource Sprite => SpriteService[Texture, Frame];

            public FrameRefViewModel(Texture texture, FrameRef frameRef, Frame frame)
            {
                Texture = texture;
                FrameRef = frameRef;
                Frame = frame;
            }
        }

        #endregion

        #region private

        private static readonly int TIMESTEP = 21600; // 5^2 + 3^3 + 2^5

        private AnimationService _animService;

        private SpriteService SpriteService => SpriteService.Instance;

        public Animation _selectedAnimation;

        #endregion

        public AnimationData AnimationData { get; private set; }

        public string BasePath { get; private set; }

        public ObservableCollection<Animation> Animations { get; set; }

        #region current animation view

        private double _viewWidth, _viewHeight, _zoom = 1.0;

        public double ViewWidth
        {
            get => _viewWidth;
            set
            {
                _viewWidth = value;
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
            }
        }

        public double ViewHeight
        {
            get => _viewHeight;
            set
            {
                _viewHeight = value;
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Max(Math.Min(value, 16), 0.25);
                OnPropertyChanged();
            }
        }

        public BitmapSource Sprite => SpriteService[CurrentTexture, _animService.CurrentFrame];

        public double SpriteLeft => ViewWidth / 2.0 - (_animService.CurrentFrame?.CenterX ?? 0);
        public double SpriteTop => ViewHeight / 2.0 - (_animService.CurrentFrame?.CenterY ?? 0);
        public double SpriteRight => SpriteLeft + (_animService.CurrentFrame?.Right - _animService.CurrentFrame?.Left) ?? 0;
        public double SpriteBottom => SpriteTop + (_animService.CurrentFrame?.Bottom - _animService.CurrentFrame?.Top) ?? 0;
        public Point SpriteCenter
        {
            get
            {
                var frame = _animService.CurrentFrame;
                if (frame != null)
                {
                    var width = frame.Right - frame.Left;
                    var height = frame.Bottom - frame.Top;
                    return new Point((double)frame.CenterX / width, (double)frame.CenterY / height);
                }
                return new Point(0.5, 0.5);
            }
        }
        public double SpriteScaleX => SelectedFrameReferenceFlipX ? -Zoom : Zoom;
        public double SpriteScaleY => SelectedFrameReferenceFlipY ? -Zoom : Zoom;


        public bool IsRunning
        {
            get => _animService.IsRunning;
            set
            {
                _animService.IsRunning = value;
                OnPropertyChanged(nameof(IsNotRunning));
            }
        }
        public bool IsNotRunning => !IsRunning;

        public bool ShowCenter { get; set; }

        public bool ShowFieldHitbox { get; set; }

        public bool ShowEntityHitbox { get; set; }

        #endregion

        #region current animation properties

        /// <summary>
        /// List of all textures
        /// </summary>
        public List<Texture> Textures => AnimationData.Textures;

        /// <summary>
        /// List of all frames of animation data
        /// </summary>
        public IEnumerable<string> Frames => AnimationData.Frames.Select(x => x.Name);

        /// <summary>
        /// List of all frames used from the selected animation
        /// </summary>
        public ObservableCollection<FrameRefViewModel> AnimationFrames { get; set; }

        /// <summary>
        /// elected animation
        /// </summary>
        public Animation SelectedAnimation
        {
            get => _selectedAnimation;
            set
            {
                _selectedAnimation = value;
                _animService.Animation = value?.Name;

                if (_selectedAnimation != null)
                {
                    var texture = AnimationData.Textures.FirstOrDefault(x => x.Id == _selectedAnimation.Texture);
                    AnimationFrames = new ObservableCollection<FrameRefViewModel>(
                        _selectedAnimation.Frames
                        .Select(x => new FrameRefViewModel(texture, x,
                            AnimationData.Frames.FirstOrDefault(f => f.Name == x.Frame)
                            )
                        )
                    );
                }
                else
                {
                    AnimationFrames = null;
                }
                OnPropertyChanged(nameof(AnimationFrames));

                OnPropertyChanged(nameof(IsAnimationSelected));
                OnPropertyChanged(nameof(FramesCount));
                OnPropertyChanged(nameof(CurrentTexture));
                OnPropertyChanged(nameof(FramePerSec));
                OnPropertyChanged(nameof(Loop));
            }
        }

        public int SelectedFrameIndex
        {
            get => _animService.FrameIndex;
            set
            {
                if (value >= 0)
                {
                    _animService.FrameIndex = value;
                }
            }
        }

        public bool IsFrameSelected => SelectedFrameReference != null;

        public FrameRef SelectedFrameReference => _animService.CurrentFrameReference;

        public string SelectedFrameReferenceName
        {
            get => SelectedFrameReference?.Frame;
            set
            {
                SelectedFrameReference.Frame = value;
                OnPropertyChanged(nameof(Sprite));
            }
        }

        public bool SelectedFrameReferenceFlipX
        {
            get => SelectedFrameReference?.FlipX ?? false;
            set
            {
                SelectedFrameReference.FlipX = value;
                OnPropertyChanged(nameof(Sprite));
                OnPropertyChanged(nameof(SpriteScaleX));
            }
        }

        public bool SelectedFrameReferenceFlipY
        {
            get => SelectedFrameReference?.FlipY ?? false;
            set
            {
                SelectedFrameReference.FlipY = value;
                OnPropertyChanged(nameof(Sprite));
                OnPropertyChanged(nameof(SpriteScaleY));
            }
        }

        public bool SelectedFrameReferenceTrigger
        {
            get => SelectedFrameReference?.Trigger ?? false;
            set
            {
                SelectedFrameReference.Trigger = value;
            }
        }

        public int SelectedFrameReferenceHitboxLeft
        {
            get => SelectedFrameReference?.Hitbox.Left ?? 0;
            set
            {
                SelectedFrameReference.Hitbox.Left = value;
            }
        }

        public int SelectedFrameReferenceHitboxTop
        {
            get => SelectedFrameReference?.Hitbox.Top ?? 0;
            set
            {
                SelectedFrameReference.Hitbox.Top = value;
            }
        }

        public int SelectedFrameReferenceHitboxRight
        {
            get => SelectedFrameReference?.Hitbox.Right ?? 0;
            set
            {
                SelectedFrameReference.Hitbox.Right = value;
            }
        }

        public int SelectedFrameReferenceHitboxBottom
        {
            get => SelectedFrameReference?.Hitbox.Bottom ?? 0;
            set
            {
                SelectedFrameReference.Hitbox.Bottom = value;
            }
        }

        public bool IsAnimationSelected => SelectedAnimation != null;

        public int FramesCount => SelectedAnimation?.Frames.Count ?? 0;

        /// <summary>
        /// Get or set the texture for the selected animation
        /// </summary>
        public Texture CurrentTexture
        {
            get
            {
                if (SelectedAnimation == null) return null;

                var id = SelectedAnimation.Texture;
                if (id == Guid.Empty)
                {
                    return new Texture()
                    {
                        Id = id,
                        Name = "<empty>"
                    };
                }

                var texture = Textures.FirstOrDefault(x => x.Id == id);
                if (texture == null)
                {
                    return new Texture()
                    {
                        Id = id,
                        Name = $"<{id}>"
                    };
                }
                return texture;
            }
            set => SelectedAnimation.Texture = value?.Id ?? Guid.NewGuid();
        }

        public int FramePerSec
        {
            get => SelectedAnimation?.Speed > 0 ? TIMESTEP / SelectedAnimation.Speed : 0;
            set => SelectedAnimation.Speed = value > 0 ? Math.Round(TIMESTEP / value) : 0;
        }

        public int Loop
        {
            get => SelectedAnimation?.Loop ?? 0;
            set => SelectedAnimation.Loop = value;
        }

        #endregion

        #region methods

        public AnimationViewModel(AnimationData animationData, string basePath)
        {
            AnimationData = animationData;
            BasePath = basePath;
            _animService = new AnimationService(AnimationData);

            _animService.OnFrameChanged += (service) =>
            {
                OnPropertyChanged(nameof(IsFrameSelected));
                OnPropertyChanged(nameof(SelectedFrameIndex));
                OnPropertyChanged(nameof(Sprite));
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
                OnPropertyChanged(nameof(SpriteCenter));
                OnPropertyChanged(nameof(SpriteScaleX));
                OnPropertyChanged(nameof(SpriteScaleY));
                OnPropertyChanged(nameof(SelectedFrameReferenceName));
                OnPropertyChanged(nameof(SelectedFrameReferenceHitboxLeft));
                OnPropertyChanged(nameof(SelectedFrameReferenceHitboxTop));
                OnPropertyChanged(nameof(SelectedFrameReferenceHitboxRight));
                OnPropertyChanged(nameof(SelectedFrameReferenceHitboxBottom));
                OnPropertyChanged(nameof(SelectedFrameReferenceFlipX));
                OnPropertyChanged(nameof(SelectedFrameReferenceFlipY));
                OnPropertyChanged(nameof(SelectedFrameReferenceTrigger));
            };

            Animations = new ObservableCollection<Animation>(AnimationData.Animations);
        }

        public void SaveChanges()
        {
            AnimationData.Animations =
                Animations.ToList();
        }

        #endregion
    }
}
