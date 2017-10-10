using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Xe.Tools.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for XeWindow.xaml
    /// </summary>
    public partial class XeWindow : UserControl
    {
        private Window _window;

        private Window Window
        {
            get
            {
                if (_window == null)
                {
                    var parent = Parent;
                    while (!(parent is Window))
                        parent = (parent as FrameworkElement).Parent;
                    _window = parent as Window;
                }
                return _window;
            }
        }

        public XeWindow()
        {
            InitializeComponent();
            Loaded += (x, y) =>
            {
                Window.WindowStyle = WindowStyle.None;
                ProcessState(Window?.WindowState);
            };
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    InvertWindowState();
                }
                else
                {
                    Window.DragMove();
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Maximized);
        }
        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindowState(WindowState.Normal);
        }

        private void InvertWindowState()
        {
            SetWindowState(Window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);
        }
        private void SetWindowState(WindowState state)
        {
            Window.WindowState = state;
            ProcessState(state);
        }
        private void ProcessState(WindowState? state)
        {
            switch (state)
            {
                case WindowState.Maximized:
                    ButtonRestore.Visibility = Visibility.Visible;
                    ButtonMaximize.Visibility = Visibility.Collapsed;
                    break;
                case WindowState.Normal:
                    ButtonRestore.Visibility = Visibility.Collapsed;
                    ButtonMaximize.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
