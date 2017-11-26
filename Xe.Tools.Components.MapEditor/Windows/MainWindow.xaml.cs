using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Xe.Tools.Components.MapEditor.ViewModels;

namespace Xe.Tools.Components.MapEditor.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double FPS = 60.0;
        
        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        private Point _mouseMove;
        private int _scrollX, _scrollY;

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
                ViewModel.IsRedrawingNeeded = true;
            };
            var timer = new System.Timers.Timer
            {
                Interval = 1000.0 / FPS
            };
            timer.Elapsed += (s, e) =>
            {
                // Returns to the main thread
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (ViewModel.IsRedrawingNeeded == true)
                    {
                        ViewModel.IsRedrawingNeeded = false;
                        var stopWatch = new Stopwatch();
                        stopWatch.Start();
                        tileMap.Render();
                        stopWatch.Stop();
                        ViewModel.LastRenderingTime = stopWatch.Elapsed.TotalMilliseconds;
                    }
                }));
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

        private void TileMap_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _mouseMove = e.GetPosition(this);
                _scrollX = tileMap.ScrollX;
                _scrollY = tileMap.ScrollY;
            }
        }

        private void TileMap_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var diff = _mouseMove - e.GetPosition(this);
                tileMap.ScrollX = (int)(_scrollX + diff.X);
                tileMap.ScrollY = (int)(_scrollY + diff.Y);
                ViewModel.IsRedrawingNeeded = true;
            }
        }

        private void TileMap_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var diff = _mouseMove - e.GetPosition(this);
                tileMap.ScrollX = (int)(_scrollX + diff.X);
                tileMap.ScrollY = (int)(_scrollY + diff.Y);
            }
        }
    }
}
