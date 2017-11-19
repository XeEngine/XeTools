using System.Collections.Generic;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class NodeMapViewModel : NodeBaseViewModel
    {
        public ITileMap TileMap { get; }

        public string Name { get; }

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

        public IEnumerable<NodeLayerViewModel> Childs { get; }

        public NodeMapViewModel(ITileMap tileMap, string mapName) :
            base(tileMap)
        {
            Name = mapName;

            const int LayersCount = 11;
            var childs = new NodeLayerViewModel[LayersCount];
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = new NodeLayerViewModel(tileMap, LayersCount - i - 1);
            }
            Childs = childs;
        }
    }
}
