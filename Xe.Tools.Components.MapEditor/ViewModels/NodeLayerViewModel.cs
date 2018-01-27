using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;
using Xe.Tools.Components.MapEditor.Services;
using System;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public abstract class NodeBaseViewModel : BaseNotifyPropertyChanged
    {
        public MainWindowViewModel MainWindow { get; }

        public Map TileMap => MainWindow.MapEditor.TileMap;

        public virtual string Name { get; }

        public NodeBaseViewModel(MainWindowViewModel vm)
        {
            MainWindow = vm;
        }
    }

    public class NodeGroupViewModel : NodeBaseViewModel
    {
        public override string Name { get; }

        public IEnumerable<NodeBaseViewModel> Childs { get; private set; }

        public NodeGroupViewModel(MainWindowViewModel vm, LayersGroup layersGroup) : base(vm)
        {
            Name = layersGroup.Name;
            Childs = NodeMapViewModel.GetLayers(vm, layersGroup.Layers);
        }
    }

    public class NodeLayerViewModel : NodeBaseViewModel
    {
        private LayerDefinition _layerDef;

        public override string Name
        {
            get => TilemapSettings.LayerNames
                .FirstOrDefault(x => x.Id == _layerDef.Id)?
                .Name ?? "<unknown>";
        }

        public bool IsVisible
        {
            get => _layerDef.IsVisible;
            set
            {
                _layerDef.IsVisible = value;
                MainWindow.IsRedrawingNeeded = true;
            }
        }

        public bool IsEnabled
        {
            get => _layerDef.IsEnabled;
            set =>_layerDef.IsEnabled = value;
        }

        public int RenderingMode
        {
            get => (int)_layerDef.RenderingMode;
            set
            {
                _layerDef.RenderingMode = (LayerRenderingMode)value;
                OnPropertyChanged();
            }
        }

        public float ParallaxHorizontalMultiplier
        {
            get => _layerDef.ParallaxHorizontalMultiplier;
            set
            {
                _layerDef.ParallaxHorizontalMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float ParallaxVerticalMultiplier
        {
            get => _layerDef.ParallaxVerticalMultiplier;
            set
            {
                _layerDef.ParallaxVerticalMultiplier = value;
                OnPropertyChanged();
            }
        }

        public float ParallaxHorizontalSpeed
        {
            get => _layerDef.ParallaxHorizontalSpeed;
            set
            {
                _layerDef.ParallaxHorizontalSpeed = value;
                OnPropertyChanged();
            }
        }

        public float ParallaxVerticalSpeed
        {
            get => _layerDef.ParallaxVerticalSpeed;
            set
            {
                _layerDef.ParallaxVerticalSpeed = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NodeBaseViewModel> Childs { get; set; }

        public NodeLayerViewModel(MainWindowViewModel vm,
            IEnumerable<LayerEntry> layers, LayerDefinition layerDef) :
            base(vm)
        {
            _layerDef = layerDef;
            Childs = new ObservableCollection<NodeBaseViewModel>(
                layers.Where(x =>
                {
                    if (x is LayerTilemap layerTilemap)
                        return layerTilemap.DefinitionId == _layerDef.Id;
                    if (x is LayerObjects objectsGroup)
                        return objectsGroup.DefinitionId == _layerDef.Id;
                    return false;
                })
                .Select(x =>
                {
                    if (x is LayerTilemap layerTilemap)
                        return new NodeEntryTilemapViewModel(vm, layerTilemap);
                    if (x is LayerObjects objectsGroup)
                        return new NodeObjectsGroupViewModel(vm, objectsGroup);
                    return (NodeBaseViewModel)null;
                })
                .Where(x => x != null)
            );
        }
    }
}
