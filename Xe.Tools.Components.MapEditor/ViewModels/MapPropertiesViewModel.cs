using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class MapPropertiesViewModel : BaseNotifyPropertyChanged
    {
        private MainWindowViewModel _mainWindow;

        private ITileMap _tileMap;
        public ITileMap TileMap
        {
            get => _tileMap;
            set
            {
                _tileMap = value;
                OnPropertyChanged(nameof(BgmField));
                OnPropertyChanged(nameof(BgmBattle));
            }
        }

        public string BgmField
        {
            get => _tileMap?.BgmField;
            set
            {
                _tileMap.BgmField = value;
                OnPropertyChanged();
            }
        }

        public string BgmBattle
        {
            get => _tileMap?.BgmBattle;
            set
            {
                _tileMap.BgmBattle = value;
                OnPropertyChanged();
            }
        }

        public MapPropertiesViewModel(MainWindowViewModel vm)
        {
            _mainWindow = vm;
        }
    }
}
