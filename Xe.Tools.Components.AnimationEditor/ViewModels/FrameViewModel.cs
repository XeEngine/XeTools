using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Xe.Game.Animations;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.AnimationEditor.ViewModels
{
    public class FrameViewModel : BaseNotifyPropertyChanged
    {
        private AnimationData _animationData;
        private TextureViewModel _selectedTexture;
        private Frame _selectedFrame;
        private double _viewWidth, _viewHeight, _zoom = 1.0;

        public double ViewWidth
        {
            get => _viewWidth;
            set
            {
                _viewWidth = value;
                OnPropertyChanged(nameof(RectCenterLeft));
                OnPropertyChanged(nameof(RectCenterTop));
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
                OnPropertyChanged(nameof(SpriteCenter));
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
                OnPropertyChanged(nameof(SpriteCenter));
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Max(Math.Min(value, 16), 0.25);
                OnPropertyChanged(nameof(SpriteScaleX));
                OnPropertyChanged(nameof(SpriteScaleY));
            }
        }

        public BitmapSource Sprite => SelectedTexture != null ? new CroppedBitmap(SelectedTexture.Image,
            new Int32Rect()
            {
                X = Left,
                Y = Top,
                Width = Right - Left,
                Height = Bottom - Top
            }) : null;

        public double ViewCenterX => ViewWidth / 2.0;
        public double ViewCenterY => ViewHeight / 2.0;

        public double RectCenterSize => 16;
        public double RectCenterLeft => ViewCenterX - RectCenterSize / 2.0;
        public double RectCenterTop => ViewCenterY - RectCenterSize / 2.0;

        public double SpriteLeft => ViewCenterX - CenterX;
        public double SpriteTop => ViewCenterY - CenterY;
        public double SpriteRight => SpriteLeft + (Right - Left);
        public double SpriteBottom => SpriteTop + (Bottom - Top);
        public Point SpriteCenter => new Point((double)CenterX / (Right - Left), (double)CenterY / (Bottom - Top));
        public double SpriteScaleX => Zoom;
        public double SpriteScaleY => Zoom;

        public List<TextureViewModel> Textures { get; }

        public ObservableCollection<Frame> Frames { get; set; }

        public TextureViewModel SelectedTexture
        {
            get => _selectedTexture;
            set
            {
                _selectedTexture = value;
                OnPropertyChanged(nameof(Sprite));
            }
        }

        public bool IsItemSelected => SelectedFrame != null;

        public int SelectedIndex { get; set; }
        
        public Frame SelectedFrame
        {
            get => _selectedFrame;
            set
            {
                _selectedFrame = value;
                OnPropertyChanged(nameof(IsItemSelected));
                OnPropertyChanged(nameof(Sprite));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Left));
                OnPropertyChanged(nameof(Top));
                OnPropertyChanged(nameof(Right));
                OnPropertyChanged(nameof(Bottom));
                OnPropertyChanged(nameof(CenterX));
                OnPropertyChanged(nameof(CenterY));
                OnPropertyChanged(nameof(SpriteLeft));
                OnPropertyChanged(nameof(SpriteTop));
                OnPropertyChanged(nameof(SpriteRight));
                OnPropertyChanged(nameof(SpriteBottom));
                OnPropertyChanged(nameof(SpriteCenter));
            }
        }

        public string Name
        {
            get => SelectedFrame?.Name;
            set
            {
                SelectedFrame.Name = value;
                OnFrameNameChanged();
            }
        }

        public int Left
        {
            get => SelectedFrame?.Left ?? 0;
            set
            {
                SelectedFrame.Left = value;
                OnFrameChanged();
            }
        }

        public int Top
        {
            get => SelectedFrame?.Top ?? 0;
            set
            {
                SelectedFrame.Top = value;
                OnFrameChanged();
            }
        }

        public int Right
        {
            get => SelectedFrame?.Right ?? 0;
            set
            {
                SelectedFrame.Right = value;
                OnFrameChanged();
            }
        }

        public int Bottom
        {
            get => SelectedFrame?.Bottom ?? 0;
            set
            {
                SelectedFrame.Bottom = value;
                OnFrameChanged();
            }
        }

        public int CenterX
        {
            get => SelectedFrame?.CenterX ?? 0;
            set
            {
                SelectedFrame.CenterX = value;
                OnFrameChanged();
            }
        }

        public int CenterY
        {
            get => SelectedFrame?.CenterY ?? 0;
            set
            {
                SelectedFrame.CenterY = value;
                OnFrameChanged();
            }
        }

        private int _maximumWidth;
        public int MaximumWidth
        {
            get => _maximumWidth;
            set => _maximumWidth = value;
        }

        private int _maximumHeight;
        public int MaximumHeight
        {
            get => _maximumHeight;
            set => _maximumHeight = value;
        }

        public FrameViewModel(AnimationData animationData, string basePath)
        {
            _animationData = animationData;
            MaximumWidth = 4096;
            MaximumHeight = 4096;
            Textures = animationData.Textures
                .Select(x => new TextureViewModel(x, basePath))
                .ToList();
            Frames = new ObservableCollection<Frame>(animationData.Frames);
        }

        public void OnFrameChanged()
        {
            OnPropertyChanged(nameof(Sprite));
            OnPropertyChanged(nameof(SpriteLeft));
            OnPropertyChanged(nameof(SpriteTop));
            OnPropertyChanged(nameof(SpriteRight));
            OnPropertyChanged(nameof(SpriteBottom));
            OnPropertyChanged(nameof(SpriteCenter));
        }

        public void OnFrameNameChanged()
        {
            var index = SelectedIndex;
            var item = Frames[index];
            Frames.RemoveAt(index);
            Frames.Insert(index, item);
            OnPropertyChanged(nameof(SelectedIndex));
        }

        public void AddFrame()
        {
            Frames.Add(new Frame()
            {
                Name = "<new frame>"
            });
            SelectedIndex = Frames.Count - 1;
        }

        public void RemoveFrame()
        {
            Frames.RemoveAt(SelectedIndex);
        }

        public void SaveChanges()
        {
            _animationData.Frames.Clear();
            _animationData.Frames.AddRange(Frames);
        }
    }
}
