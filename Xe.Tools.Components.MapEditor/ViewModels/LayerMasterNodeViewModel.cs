using System.Collections.Generic;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class LayerMasterNodeViewModel : BaseNotifyPropertyChanged
    {
        public ITileMap TileMap { get; }

        public string Name { get; }

        public IEnumerable<LayerNodeViewModel> Childs { get; }

        public LayerMasterNodeViewModel(ITileMap tileMap)
        {
            TileMap = tileMap;
            Name = "blabla";
            var childs = new LayerNodeViewModel[11];
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = new LayerNodeViewModel(tileMap, i);
            }
            Childs = childs;
        }
    }
}
