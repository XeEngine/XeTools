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
        public ILayerTilemap LayerTilemap { get; }

        public NodeEntryTilemapViewModel(ITileMap tileMap, ILayerTilemap layerTilemap) :
            base(tileMap, layerTilemap)
        {
            LayerTilemap = layerTilemap;
        }
    }
}
