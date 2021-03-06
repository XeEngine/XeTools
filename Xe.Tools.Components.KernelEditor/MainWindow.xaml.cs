﻿using System.Windows;
using Xe.Tools.Components.KernelEditor.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Wpf.Controls;

namespace Xe.Tools.Components.KernelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowEx
    {
        public KernelViewModel Kernel { get; set; }

        public MainWindow(IProject project, IProjectFile file)
        {
            InitializeComponent();

            Kernel = new KernelViewModel(project, file);
            DataContext = Kernel;
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            Kernel.SaveChanges();
        }

		protected override bool DoSaveChanges()
		{
			Kernel.SaveChanges();
			return true;
		}
	}
}
