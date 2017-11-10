using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Xe.Game.Tilemaps;
using Xe.Tools.Components.MapEditor.Models;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class LayerEntryViewModel
    {
        public ILayerEntry LayerEntry { get; }

        public string Name => LayerEntry.Name;

        public bool IsVisible => LayerEntry.Visible;

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

        public string Prefix
        {
            get
            {
                switch (LayerEntry.Priority)
                {
                    case 0: return "P";
                    case 1: return "B1";
                    case 2: return "B2";
                    case 3: return "L";
                    case 4: return "LS";
                    case 5: return "H";
                    case 6: return "HS";
                    case 7: return "X";
                    case 8: return "F1";
                    case 9: return "F2";
                    default: return ((byte)LayerEntry.Priority).ToString("X02");
                }
            }
        }

        public LayerEntryViewModel(ILayerEntry layerEntry)
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
