using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;
using Xe.Tools.Components.MapEditor.Services;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public abstract class NodeBaseViewModel : BaseNotifyPropertyChanged
    {
        public MainWindowViewModel MainWindow { get; }

        public ITileMap TileMap => MainWindow.MapEditor.TileMap;

        public virtual string Name { get; }

        public NodeBaseViewModel(MainWindowViewModel vm)
        {
            MainWindow = vm;
        }
    }

    public class NodeGroupViewModel : NodeBaseViewModel
    {
        public override string Name { get; }

        public IEnumerable<NodeBaseViewModel> Childs { get; private set; }

        public NodeGroupViewModel(MainWindowViewModel vm, ILayersGroup layersGroup) : base(vm)
        {
            Name = layersGroup.Name;
            Childs = NodeMapViewModel.GetLayers(vm, layersGroup.Layers);
        }
    }

    public class NodeLayerViewModel : NodeBaseViewModel
    {
        public override string Name => TilemapService.GetLayerName(Priority);

        public int Priority { get; }

        public ObservableCollection<NodeBaseViewModel> Childs { get; set; }

        public NodeLayerViewModel(MainWindowViewModel vm,
            IEnumerable<ILayerEntry> layers, int priority) :
            base(vm)
        {
            Priority = priority;
            Childs = new ObservableCollection<NodeBaseViewModel>(
                layers.Where(x =>
                {
                    if (x is ILayerTilemap layerTilemap)
                        return layerTilemap.Priority == priority;
                    if (x is ILayerObjects objectsGroup)
                        return objectsGroup.Priority == priority;
                    return false;
                })
                .Select(x =>
                {
                    if (x is ILayerTilemap layerTilemap)
                        return new NodeEntryTilemapViewModel(vm, layerTilemap);
                    if (x is ILayerObjects objectsGroup)
                        return new NodeObjectsGroupViewModel(vm, objectsGroup);
                    return (NodeBaseViewModel)null;
                })
                .Where(x => x != null)
            );
        }
    }
}
