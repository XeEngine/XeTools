using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xe.Tools.GameStudio.Utility;
using Xe.Tools.GameStudio.ViewModels;
using Xe.Tools.Projects;
using Xe.Tools.Services;

namespace Xe.Tools.GameStudio.Controls
{
    /// <summary>
    /// Interaction logic for ProjectTreeView.xaml
    /// </summary>
    public partial class ProjectTreeView : UserControl
    {
        private ProjectExplorerViewModel ViewModel => DataContext as ProjectExplorerViewModel;

        public ProjectTreeView()
        {
            InitializeComponent();
            DataContext = new ProjectExplorerViewModel(GameStudioViewModel.Instance);
        }

        private void TreeProject_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView)
            {
                if (treeView.SelectedItem is ProjectEntryViewModel item)
                {
                    GameStudioViewModel.Instance.SelectedProjectEntry = item;
                }
            }
        }

        private void TreeProject_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var gs = GameStudioViewModel.Instance;
            var entry = GameStudioViewModel.Instance.SelectedProjectEntry;
            if (entry != null && entry is ProjectExplorerFileViewModel fileVm)
            {
                var file = fileVm.Entry as IProjectFile;
                var moduleName = file.Format;
                var component = Globals.Components
                    .Where(x => x.ComponentInfo.ModuleName == moduleName)
                    .SingleOrDefault();

				if (component != null)
				{
					try
					{
						component.CreateInstance(new Components.ComponentProperties()
						{
							Project = gs.Project,
							File = file,
							Context = gs.Context
						}).ShowDialog();
					}
					catch (FileNotFoundException ex)
					{
						Log.Error($"File {ex.FileName} not found.");
					}
					catch (Exception ex)
					{
						Log.Error(ex.Message);
					}
				}
				else
				{
					Log.Error($"No tools for the module {moduleName} has been found; the file {file.Name} cannot be opened.");
				}
            }
        } 
    }
}
