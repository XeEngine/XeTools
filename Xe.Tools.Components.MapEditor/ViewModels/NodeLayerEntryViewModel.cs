using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Models;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class NodeLayerEntryViewModel : NodeBaseViewModel
    {
        public MapEditorViewModel ViewModel { get; }

        private ILayerEntry _layerEntry;
        public ILayerEntry LayerEntry
        {
            get => _layerEntry;
            set
            {
                _layerEntry = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(Priority));
            }
        }

        public new string Name
        {
            get => LayerEntry.Name;
            set
            {
                LayerEntry.Name = value;
                OnPropertyChanged();
            }
        }

        public int Priority
        {
            get => LayerEntry.Priority;
            set
            {
                LayerEntry.Priority = value;
                MainWindow.IsRedrawingNeeded = true;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => LayerEntry.Visible;
            set
            {
                LayerEntry.Visible = value;
                OnPropertyChanged();
            }
        }

        public LayerType LayerType { get; }

        public Brush IconColor
        {
            get
            {
                switch (LayerType)
                {
                    case LayerType.Unknown:
                        return new SolidColorBrush(Colors.DarkGray);
                    case LayerType.Tilemap:
                        return new SolidColorBrush(Colors.Fuchsia);
                    case LayerType.ObjectGroup:
                        return new SolidColorBrush(Colors.Blue);
                    default:
                        return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public NodeLayerEntryViewModel(MainWindowViewModel vm, ILayerEntry layerEntry) :
            base(vm)
        {
            LayerEntry = layerEntry;
            if (layerEntry is ILayerTilemap)
                LayerType = LayerType.Tilemap;
            else if (layerEntry is ILayerObjects)
                LayerType = LayerType.ObjectGroup;
            else
                LayerType = LayerType.Unknown;
        }
    }
}
