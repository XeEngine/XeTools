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
using Xe.Tools.Components.MapEditor.ViewModels;

namespace Xe.Tools.Components.MapEditor.Windows
{
    /// <summary>
    /// Interaction logic for EventsPropertiesWindow.xaml
    /// </summary>
    public partial class EventsPropertiesWindow : Window
    {
        private EventsEditorViewModel ViewModel => DataContext as EventsEditorViewModel;

        public EventsPropertiesWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            DataContext = new EventsEditorViewModel(vm);
        }
    }
}
