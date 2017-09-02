using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Xe.Game;
using Xe.Game.Animations;
using Xe.Tools.Components.AnimationEditor.ViewModels;

namespace Xe.Tools.Components.AnimationEditor.Windows
{
    /// <summary>
    /// Interaction logic for WindowFrames.xaml
    /// </summary>
    public partial class WindowFrames : Window
    {
        private FrameViewModel ViewModel => DataContext as FrameViewModel;

        public WindowFrames(AnimationData animationData, string basePath)
        {
            InitializeComponent();
            DataContext = new FrameViewModel(animationData, basePath);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModel.SaveChanges();
            base.OnClosing(e);
        }

        private void ButtonFrameAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddFrame();
        }

        private void ButtonFrameRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveFrame();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewModel.ViewWidth = e.NewSize.Width;
            ViewModel.ViewHeight = e.NewSize.Height;
        }

        private void Canvas_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ViewModel.Zoom += (e.Delta / 120) * 0.25;
        }
    }
}
