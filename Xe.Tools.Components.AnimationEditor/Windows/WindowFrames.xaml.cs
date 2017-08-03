using CommonDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowFrames.xaml
    /// </summary>
    public partial class WindowFrames : Window, IDisposable, INotifyPropertyChanged
    {
        private string _basePath;
        private ObservableCollection<Xe.Game.Animations.Frame> _frames;
        private SharpDX.Direct2D1.Bitmap1 _d2dBitmap;

        private FrameViewModel _frameViewModel;
        private TexturesViewModel _texturesViewModel;

        //private DeviceManager _deviceManager;
        //private WindowTarget _controlTarget;

        public event PropertyChangedEventHandler PropertyChanged;

        private FrameViewModel CurrentFrame
        {
            get => _frameViewModel;
            set => FramePanel.DataContext = _frameViewModel = value;
        }

        //private SharpDX.Direct2D1.DeviceContext Context2D => _deviceManager.ContextDirect2D;
        //private SharpDX.Direct3D11.DeviceContext1 Context3D => _deviceManager.ContextDirect3D;

        public IEnumerable<Xe.Game.Animations.Frame> Frames => _frames;

        public WindowFrames(List<Xe.Game.Animations.Frame> frames, List<Texture> textures, string basePath)
        {
            //_deviceManager = new DeviceManager();
            _basePath = basePath;
            _frames = new ObservableCollection<Game.Animations.Frame>(frames);
            _texturesViewModel = new TexturesViewModel(textures, _basePath);

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //_deviceManager.Initialize(96.0f);
            //_controlTarget = new WindowTarget(this);
            //_controlTarget.Initialize(_deviceManager);
            ListFramesView.DataContext = _frames;
            ListTextures.DataContext = _texturesViewModel;
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);
        //    _controlTarget.UpdateForSizeChange();
        //}

        //protected override void OnContentRendered(EventArgs e)
        //{
        //    var clearColor = new SharpDX.Mathematics.Interop.RawColor4(0.0f, 0.0f, 0.0f, 0.0f);

        //    Context2D.BeginDraw();
        //    Context2D.Clear(clearColor);
        //    if (_d2dBitmap != null)
        //    {
        //        Context2D.DrawBitmap(_d2dBitmap, 0, 0, Flip.None);
        //    }
        //    Context2D.EndDraw();

        //    base.OnContentRendered(e);
        //}

        private void ListTextures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var selectedIndex = comboBox?.SelectedIndex ?? -1;
            if (selectedIndex >= 0)
            {
                if (comboBox.SelectedValue is Xe.Game.Texture texture)
                {
                    var filePath = System.IO.Path.Combine(_basePath, texture.Name);
                    using (var imagingFactory = new SharpDX.WIC.ImagingFactory2())
                    {
                        //_d2dBitmap?.Dispose();
                        //_d2dBitmap = Context2D.LoadBitmap2D(imagingFactory, filePath);
                        //if (_d2dBitmap != null)
                        //{
                        //    if (_frameViewModel != null)
                        //    {
                        //        _frameViewModel.MaximumWidth = (int)_d2dBitmap.Size.Width;
                        //        _frameViewModel.MaximumHeight = (int)_d2dBitmap.Size.Height;
                        //    }
                        //}
                        //OnContentRendered(new EventArgs());
                    }
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
                CurrentFrame = new FrameViewModel(frame)
                {
                    MaximumWidth = short.MaxValue,
                    MaximumHeight = short.MaxValue
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

        public void Dispose()
        {
            _d2dBitmap?.Dispose();
        }
    }
}
