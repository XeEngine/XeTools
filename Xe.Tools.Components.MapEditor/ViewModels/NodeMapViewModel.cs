using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Services;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public enum NodeOrderMode
    {
        GroupByPriority,
        OriginalOrder
    }

    public class NodeMapViewModel : NodeBaseViewModel
    {
        private NodeOrderMode _nodeOrderMode = NodeOrderMode.GroupByPriority;
        public NodeOrderMode NodeOrderMode
        {
            get => NodeOrderMode;
            set
            {
                _nodeOrderMode = value;
                ReorderLayers();
                OnPropertyChanged();
            }
        }

        public override string Name { get; }

        public string FriendlyMapName { get; set; }

        public string BgmField
        {
            get => TileMap.BgmField;
            set => TileMap.BgmField = value;
        }

        public string BgmBattle
        {
            get => TileMap.BgmBattle;
            set => TileMap.BgmBattle = value;
        }

        public ObservableCollection<NodeBaseViewModel> Childs { get; private set; }

        public NodeMapViewModel(MainWindowViewModel vm, string mapName) :
            base(vm)
        {
            Name = mapName;
            Childs = new ObservableCollection<NodeBaseViewModel>();
            ReorderLayers();
        }

        private void ReorderLayers()
        {
            const int LayersCount = 11;

            Childs.Clear();
            switch (_nodeOrderMode)
            {
                case NodeOrderMode.GroupByPriority:
                    for (int i = 0; i < LayersCount; i++)
                        Childs.Add(new NodeLayerViewModel(MainWindow, TileMap.Layers.FlatterLayers(), LayersCount - i - 1));
                    break;
                case NodeOrderMode.OriginalOrder:
                    foreach (var layer in GetLayers(MainWindow, TileMap.Layers))
                        Childs.Add(layer);
                    break;
            }
            OnPropertyChanged(nameof(Childs));
        }

        public static IEnumerable<NodeBaseViewModel> GetLayers(MainWindowViewModel vm, IEnumerable<LayerBase> layers)
        {
            return layers
                .Select(x =>
                {
                    if (x is LayersGroup layerGroup)
                        return new NodeGroupViewModel(vm, layerGroup);
                    if (x is LayerTilemap layerTilemap)
                        return new NodeEntryTilemapViewModel(vm, layerTilemap);
                    if (x is LayerObjects objectsGroup)
                        return new NodeObjectsGroupViewModel(vm, objectsGroup);
                    return (NodeBaseViewModel)null;
                })
                .Where(x => x != null)
                .Reverse();
        }
    }
}
