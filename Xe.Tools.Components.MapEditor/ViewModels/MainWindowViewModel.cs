﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class MainWindowViewModel : BaseNotifyPropertyChanged
    {
        private enum PropertyBar
        {
            None,
            Tilemap,
            ObjectsGroup,
            ObjectEntry
        }

        private PropertyBar _propertyBar;

        public MapEditorViewModel MapEditor { get; }
        public MapPropertiesViewModel MapPropertiesViewModel { get; }
        public LayerTilemapPropertiesViewModel LayerTilemapPropertiesViewModel { get; }
        public ObjectPropertiesViewModel ObjectPropertiesViewModel { get; }

        public IEnumerable<LayerEntryViewModel> Layers => MapEditor.TileMap.Layers.Reverse<ILayerEntry>()
            .Select(x => new LayerEntryViewModel(x));

        private LayerEntryViewModel _selectedLayerEntry;
        public LayerEntryViewModel SelectedLayerEntry
        {
            get => _selectedLayerEntry;
            set
            {
                _selectedLayerEntry = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedLayerTilemap));

                if (value != null)
                {
                    var layerEntry = value.LayerEntry;
                    if (layerEntry is ILayerTilemap tileMap)
                    {
                        LayerTilemapPropertiesViewModel.LayerTilemap = tileMap;
                        SetPropertyBar(PropertyBar.Tilemap);
                    }
                    else if (layerEntry is ILayerObjects)
                    {
                        SetPropertyBar(PropertyBar.ObjectsGroup);
                    }
                    else
                    {
                        MapPropertiesViewModel.TileMap = MapEditor.TileMap;
                        SetPropertyBar(PropertyBar.None);
                    }
                }
            }
        }

        private IObjectEntry _selectedObjectEntry;
        public IObjectEntry SelectedObjectEntry
        {
            get => _selectedObjectEntry;
            set
            {
                _selectedObjectEntry = value;
                ObjectPropertiesViewModel.ObjectEntry = value;
                OnPropertyChanged();
                SetPropertyBar(PropertyBar.ObjectEntry);
            }
        }

        public ILayerTilemap SelectedLayerTilemap => SelectedLayerEntry as ILayerTilemap;
        public ILayerObjects SelectedLayerObjectsGroup => SelectedLayerEntry as ILayerObjects;

        public Visibility PropertyMapVisibility => _propertyBar == PropertyBar.None ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyTilemapVisibility => _propertyBar == PropertyBar.Tilemap ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyObjectGroupVisibility => _propertyBar == PropertyBar.ObjectsGroup ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyObjectEntryVisibility => _propertyBar == PropertyBar.ObjectEntry ? Visibility.Visible : Visibility.Collapsed;

        #region Status bar

        private double _lastRenderingTime;
        private double _maxRenderingTime;
        private double _avgRenderingTime;
        public int _rendersCount = 0;
        public double LastRenderingTime
        {
            get => _lastRenderingTime;
            set
            {
                _lastRenderingTime = value;
                if (_lastRenderingTime > _maxRenderingTime)
                {
                    _maxRenderingTime = _lastRenderingTime;
                    OnPropertyChanged(nameof(MaxRenderingTime));
                }
                _rendersCount++;
                _avgRenderingTime += -(_avgRenderingTime / _rendersCount) + (value / _rendersCount);
                OnPropertyChanged();
                OnPropertyChanged(nameof(AvgRenderingTime));
            }
        }
        public double MaxRenderingTime => _maxRenderingTime;
        public double AvgRenderingTime => _avgRenderingTime;

        #endregion


        public MainWindowViewModel(MapEditorViewModel vm)
        {
            MapEditor = vm;
            MapPropertiesViewModel = new MapPropertiesViewModel(this);
            LayerTilemapPropertiesViewModel = new LayerTilemapPropertiesViewModel(this);
            ObjectPropertiesViewModel = new ObjectPropertiesViewModel(this);
            SetPropertyBar(PropertyBar.None);
        }

        private void SetPropertyBar(PropertyBar propertyBar)
        {
            _propertyBar = propertyBar;
            OnPropertyChanged(nameof(PropertyMapVisibility));
            OnPropertyChanged(nameof(PropertyTilemapVisibility));
            OnPropertyChanged(nameof(PropertyObjectGroupVisibility));
            OnPropertyChanged(nameof(PropertyObjectEntryVisibility));
        }
    }
}
