using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class NodeEntryTilemapViewModel : NodeLayerEntryViewModel
    {
        public LayerTilemap LayerTilemap { get; }

        public int ProcessingMode
        {
            get => (int)(LayerTilemap?.ProcessingMode ?? LayerProcessingMode.Tilemap);
            set => LayerTilemap.ProcessingMode = (LayerProcessingMode)value;
        }

        public NodeEntryTilemapViewModel(MainWindowViewModel vm, LayerTilemap layerTilemap) :
            base(vm, layerTilemap)
        {
            LayerTilemap = layerTilemap;
        }
    }
}
