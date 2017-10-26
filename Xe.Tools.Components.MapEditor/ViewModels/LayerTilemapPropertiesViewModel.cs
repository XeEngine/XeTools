using System.Windows.Media;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class LayerTilemapPropertiesViewModel : BaseNotifyPropertyChanged
    {
        private ILayerTilemap _layerTilemap;
        public  ILayerTilemap LayerTilemap
        {
            get => _layerTilemap;
            set
            {
                _layerTilemap = value;
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(nameof(MultiplyColor));
                OnPropertyChanged(nameof(ColorText));
            }
        }

        public bool IsVisible
        {
            get => _layerTilemap?.Visible ?? false;
            set
            {
                if (_layerTilemap == null) return;
                _layerTilemap.Visible = value;
            }
        }

        public int Opacity
        {
            get => (int)(_layerTilemap?.Opacity ?? 0 * 255);
            set
            {
                if (_layerTilemap == null) return;
                _layerTilemap.Opacity = value / 255.0;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ColorText));
            }
        }

        public Color MultiplyColor => Colors.White;

        public SolidColorBrush MultiplyColorBrush => new SolidColorBrush(MultiplyColor);

        public string ColorText {
            get
            {
                if (_layerTilemap == null) return "N/A";
                var color = MultiplyColor;
                return string.Format("{0}, {1}, {2}, {3}",
                    color.ScR.ToString("N02"),
                    color.ScG.ToString("N02"),
                    color.ScB.ToString("N02"),
                    _layerTilemap.Opacity.ToString("N02"));
            }
        }
    }
}
