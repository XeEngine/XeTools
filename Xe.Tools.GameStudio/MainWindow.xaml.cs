﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Xe.Tools.Components;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Modules;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Dialogs;

namespace Xe.Tools.GameStudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IProject Project => GameStudioViewModel.Instance.Project;

        public MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

		public MainWindow()
		{
			InitializeComponent();

            Common.Initialize();
            DataContext = new MainWindowViewModel(GameStudioViewModel.Instance);
            ViewModel.TitleBase = "XeEngine Game Studio";
            FooterBar.DataContext = new StatusViewModel();
		}

        private void Window_Loaded(object sender, EventArgs e)
        {
            var tasks = new Task[] {
                Task.Run(() =>
                {
                    Common.SendMessage(MessageType.Initialization, "Loading modules...");
                    Globals.Modules = Module.GetModules().ToArray();
                    Common.SendMessage(MessageType.Initialization, "Loading components...");
                    Globals.Components = Component.GetComponents().ToArray();
                }),
                Task.Run(() =>
                {
                    var fileLastOpen = Properties.Settings.Default.FileLastOpen;
                    if (File.Exists(fileLastOpen))
                    {
                        Common.SendMessage(MessageType.Initialization, "Loading most recent project...");
                        GameStudioViewModel.Instance.LoadProject(fileLastOpen);
                    }
                })
            };

            Task.Run(() =>
            {
                Task.WaitAll(tasks);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Common.SendMessage(MessageType.Idle, "Ready");
                });
            });
        }

		private void MenuItem_ProjectPropertiesClick(object sender, RoutedEventArgs e)
		{
			var dialog = new ProjectProperties(Project);
			var result = dialog.ShowDialog();
        }

        private void MenuProjectConfiguration_Click(object sender, RoutedEventArgs e)
        {
            new ProjectSettings(Project).ShowDialog();
        }
    }
}
