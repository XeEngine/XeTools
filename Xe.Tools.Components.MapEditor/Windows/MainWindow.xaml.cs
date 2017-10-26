using System;
using System.Diagnostics;
using System.Windows;
using Xe.Tools.Components.MapEditor.ViewModels;

namespace Xe.Tools.Components.MapEditor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double FPS = 15.0;

        private bool _isInvalidated;
        private object _isInvalidatedLock = new object();

        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        public MainWindow()
        {
            Initialize(MapEditorViewModel.Instance);
        }

        public MainWindow(MapEditorViewModel vm)
        {
            InitializeComponent();
            Initialize(vm);
        }

        public void Initialize(MapEditorViewModel vm)
        {
            DataContext = new MainWindowViewModel(vm);
            ViewModel.ObjectPropertiesViewModel.OnInvalidateEntry += (s, e) =>
            {
                Invalidate();
            };
            var timer = new System.Timers.Timer
            {
                Interval = 1000.0 / FPS
            };
            timer.Elapsed += (s, e) =>
            {
                bool isInvalidated;
                lock(_isInvalidatedLock)
                {
                    isInvalidated = _isInvalidated;
                    _isInvalidated = false;
                }
                if (isInvalidated)
                {
                    // Returns to the main thread
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var stopWatch = new Stopwatch();
                        stopWatch.Start();
                        tileMap.Render();
                        stopWatch.Stop();
                        ViewModel.LastRenderingTime = stopWatch.Elapsed.TotalMilliseconds; 
                    }));
                }
            };
            timer.Enabled = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            tileMap.OnSelectedEntity += (o, entity) =>
            {
                ViewModel.SelectedObjectEntry = entity;
            };
            tileMap.OnMoveEntry += (o, entity, x, y) =>
            {
                var vm = ViewModel.ObjectPropertiesViewModel;
                vm.X = (int)x;
                vm.Y = (int)y;
            };
        }

        private void Invalidate()
        {
            lock (_isInvalidatedLock)
            {
                _isInvalidated = true;
            }
        }
    }
}
