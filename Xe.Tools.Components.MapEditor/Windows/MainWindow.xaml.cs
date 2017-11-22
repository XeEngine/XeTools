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
        const double FPS = 60.0;
        
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
        
    }
}
