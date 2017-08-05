using System.Windows;
using Xe.Tools;
using Xe.Tools.Components.KernelEditor.ViewModels;

namespace Xe.Tools.Components.KernelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KernelViewModel Kernel { get; set; }

        public MainWindow(Project project, Project.Container container, Project.Item item)
        {
            InitializeComponent();

            Kernel = new KernelViewModel(project, container, item);
            DataContext = Kernel;
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {
            Kernel.SaveChanges();
        }
    }
}
