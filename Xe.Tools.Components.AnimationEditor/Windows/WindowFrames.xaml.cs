using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowFrames.xaml
    /// </summary>
    public partial class WindowFrames : Window, INotifyPropertyChanged
    {
        private string _basePath;
        private ObservableCollection<Xe.Game.Animations.Frame> _frames;

        private int _textureMaximumWidth = ushort.MaxValue;
        private int _textureMaximumHeight = ushort.MaxValue;
        private FrameViewModel _frameViewModel = new FrameViewModel(new Game.Animations.Frame());
        private TexturesViewModel _texturesViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private FrameViewModel CurrentFrame
        {
            get => _frameViewModel;
            set => FramePanel.DataContext = _frameViewModel = value;
        }
        
        public IEnumerable<Xe.Game.Animations.Frame> Frames => _frames;

        public WindowFrames(List<Xe.Game.Animations.Frame> frames, List<Texture> textures, string basePath)
        {
            _basePath = basePath;
            _frames = new ObservableCollection<Game.Animations.Frame>(frames);
            _texturesViewModel = new TexturesViewModel(textures, _basePath);

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            
            ListFramesView.DataContext = _frames;
            ListTextures.DataContext = _texturesViewModel;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            var clearColor = new SharpDX.Mathematics.Interop.RawColor4(0.0f, 0.0f, 0.0f, 1.0f);
            base.OnContentRendered(e);
        }

        private void ListTextures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedIndex = comboBox?.SelectedIndex ?? -1;
            if (selectedIndex >= 0)
            {
                if (comboBox.SelectedValue is TextureViewModel texture)
                {
                    CanvasFrame.FileName = System.IO.Path.Combine(_basePath, texture.FileName);
                }
            }
        }

        private void ListFramesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            var selectedIndex = listView?.SelectedIndex ?? -1;
            if (selectedIndex >= 0)
            {
                var frame = _frames[selectedIndex];
                CanvasFrame.Frame = frame;
                CurrentFrame = new FrameViewModel(frame)
                {
                    MaximumWidth = _textureMaximumWidth,
                    MaximumHeight = _textureMaximumHeight
                };
            }
        }

        private void TextFrameName_LostFocus(object sender, RoutedEventArgs e)
        {
            var selectedIndex = ListFramesView.SelectedIndex;
            var frame = _frames[selectedIndex];
            frame.Name = TextFrameName.Text;

            ListFramesView.SelectedIndex = -1;
            _frames.RemoveAt(selectedIndex);
            _frames.Insert(selectedIndex, frame);
            ListFramesView.SelectedIndex = selectedIndex;
        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            _frames.Add(new Xe.Game.Animations.Frame()
            {
                Name = "<new frame>"
            });
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            var index = ListFramesView.SelectedIndex;
            if (index >= 0)
            {
                _frames.RemoveAt(index);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CanvasFrame_OnTextureLoaded(SharpDX.Direct2D1.Bitmap1 texture, Size size)
        {
            _frameViewModel.MaximumWidth = _textureMaximumWidth = (int)size.Width;
            _frameViewModel.MaximumHeight = _textureMaximumHeight = (int)size.Height;
        }
    }
}
