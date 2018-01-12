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
        private LayerObjects _objects;


        public new string Name
        {
            get => _objects.Name;
            set
            {
                _objects.Name = value;
                OnPropertyChanged();
            }
        }

        public Guid DefinitionId
        {
            get => _objects.DefinitionId;
            set
            {
                _objects.DefinitionId = value;
                MainWindow.IsRedrawingNeeded = true;
                OnPropertyChanged();
            }
        }

		public bool IsVisible
		{
			get => _objects.Visible;
			set
			{
				_objects.Visible = value;
				OnPropertyChanged();
				MainWindow.IsRedrawingNeeded = true;
			}
		}

        public NodeObjectsGroupViewModel(MainWindowViewModel vm, LayerObjects objects) :
            base(vm)
        {
            _objects = objects;
        }
    }
}
