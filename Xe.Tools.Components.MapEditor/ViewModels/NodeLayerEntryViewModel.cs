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
        public ILayerEntry LayerEntry { get; }

        public string Name
        {
            get => LayerEntry.Name;
            set => LayerEntry.Name = value;
        }

        public int Priority
        {
            get => LayerEntry.Priority;
            set => LayerEntry.Priority = value;
        }

        public bool IsVisible
        {
            get => LayerEntry.Visible;
            set => LayerEntry.Visible = value;
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

        public NodeLayerEntryViewModel(ITileMap tileMap, ILayerEntry layerEntry) :
            base(tileMap)
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
