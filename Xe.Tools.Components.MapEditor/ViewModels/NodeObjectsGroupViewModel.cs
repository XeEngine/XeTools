using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class NodeObjectsGroupViewModel : NodeBaseViewModel
    {
        private ILayerObjects _objects;


        public new string Name
        {
            get => _objects.Name;
            set
            {
                _objects.Name = value;
                OnPropertyChanged();
            }
        }

        public int Priority
        {
            get => _objects.Priority;
            set
            {
                _objects.Priority = value;
                MainWindow.IsRedrawingNeeded = true;
                OnPropertyChanged();
            }
        }

        public NodeObjectsGroupViewModel(MainWindowViewModel vm, ILayerObjects objects) :
            base(vm)
        {
            _objects = objects;
        }
    }
}
