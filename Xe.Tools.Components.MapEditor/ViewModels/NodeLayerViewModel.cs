using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;
using Xe.Tools.Components.MapEditor.Services;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class NodeBaseViewModel : BaseNotifyPropertyChanged
    {
        public ITileMap TileMap { get; }

        public NodeBaseViewModel(ITileMap tileMap)
        {
            TileMap = tileMap;
        }
    }

    public class NodeLayerViewModel : NodeBaseViewModel
    {
        public string Name => TilemapService.GetLayerName(Priority);

        public int Priority { get; }

        public ObservableCollection<NodeLayerEntryViewModel> Childs { get; set; }

        public NodeLayerViewModel(ITileMap tileMap, int priority) :
            base(tileMap)
        {
            Priority = priority;
            Childs = new ObservableCollection<NodeLayerEntryViewModel>(
                tileMap.Layers
                .Where(x =>
                {
                    if (x is ILayerTilemap layerTilemap)
                        return layerTilemap.Priority == priority;
                    return false;
                })
                .Select(x => new NodeEntryTilemapViewModel(tileMap, x as ILayerTilemap))
            );
        }
    }
}
