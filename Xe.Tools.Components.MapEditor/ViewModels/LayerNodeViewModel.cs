using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class LayerNodeViewModel : BaseNotifyPropertyChanged
    {
        public ITileMap TileMap { get; set; }

        public string Name => GetNameFromPriority(Priority);

        public int Priority { get; }

        public ObservableCollection<LayerEntryViewModel> Childs { get; set; }

        public LayerNodeViewModel(ITileMap tileMap, int priority)
        {
            TileMap = tileMap;
            Priority = priority;
            Childs = new ObservableCollection<LayerEntryViewModel>(
                tileMap.Layers
                .Where(x =>
                {
                    if (x is ILayerTilemap layerTilemap)
                        return layerTilemap.Priority == priority;
                    return false;
                })
                .Select(x => new LayerEntryViewModel(x as ILayerTilemap))
            );
        }

        private static string GetNameFromPriority(int priority)
        {
            switch (priority)
            {
                case 0: return "Programmable";
                case 1: return "Background 1";
                case 2: return "Background 2";
                case 3: return "Low tilemap";
                case 4: return "Low shadows";
                case 5: return "High tilemap";
                case 6: return "High shadows";
                case 7: return "Highest tilemap";
                case 8: return "Highest shadows";
                case 9: return "Foreground 1";
                case 10: return "Foreground 2";
                default: return $"Unknown 0x{priority.ToString("X02")}";
            }
        }
    }
}
