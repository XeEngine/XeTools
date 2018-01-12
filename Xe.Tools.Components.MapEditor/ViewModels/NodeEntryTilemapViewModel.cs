using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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

		public int Opacity
		{
			get => (int)((LayerTilemap?.Opacity ?? 0) * 255.0);
			set
			{
				if (LayerTilemap == null) return;
				LayerTilemap.Opacity = value / 255.0;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ColorText));
				MainWindow.IsRedrawingNeeded = true;
			}
		}

		public Color MultiplyColor => Colors.White;

		public SolidColorBrush MultiplyColorBrush => new SolidColorBrush(MultiplyColor);

		public string ColorText
		{
			get
			{
				if (LayerTilemap == null) return "N/A";
				var color = MultiplyColor;
				return string.Format("{0}, {1}, {2}, {3}",
					color.ScR.ToString("N03"),
					color.ScG.ToString("N03"),
					color.ScB.ToString("N03"),
					LayerTilemap.Opacity.ToString("N03"));
			}
		}

		public NodeEntryTilemapViewModel(MainWindowViewModel vm, LayerTilemap layerTilemap) :
            base(vm, layerTilemap)
        {
            LayerTilemap = layerTilemap;
        }
    }
}
