using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class MainWindowViewModel : BaseNotifyPropertyChanged
    {
        public MapEditorViewModel MapEditor { get; }


        public MainWindowViewModel(MapEditorViewModel vm)
        {
            MapEditor = vm;
        }
    }
}
