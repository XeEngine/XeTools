using System.Windows.Media;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class LayerTilemapPropertiesViewModel : NodeBaseViewModel
    {
        MainWindowViewModel _vm;

        private ILayerTilemap _layerTilemap;
        public  ILayerTilemap LayerTilemap
        {
            get => _layerTilemap;
            set
            {
                _layerTilemap = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(Priority));
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(nameof(MultiplyColor));
                OnPropertyChanged(nameof(MultiplyColorBrush));
                OnPropertyChanged(nameof(ColorText));
            }
        }

        public new string Name
        {
            get => _layerTilemap?.Name;
            set
            {
                _layerTilemap.Name = value;
                OnPropertyChanged();
            }
        }

        public int Type
        {
            get => _layerTilemap?.Type ?? int.MinValue;
            set => _layerTilemap.Type = value;
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

        public int Priority
        {
            get => _layerTilemap?.Priority ?? int.MinValue;
            set
            {
                _layerTilemap.Priority = value;
                _vm.IsRedrawingNeeded = true;
                OnPropertyChanged();
            }
        }

        public int Opacity
        {
            get => (int)((_layerTilemap?.Opacity ?? 0) * 255.0);
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
                    color.ScR.ToString("N03"),
                    color.ScG.ToString("N03"),
                    color.ScB.ToString("N03"),
                    _layerTilemap.Opacity.ToString("N03"));
            }
        }

        public LayerTilemapPropertiesViewModel(MainWindowViewModel vm) :
            base(vm)
        {
            _vm = vm;
        }
    }
}
