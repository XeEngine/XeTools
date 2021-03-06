﻿using System;
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
        public MapEditorViewModel ViewModel { get; }

        private LayerEntry _layerEntry;
        public LayerEntry LayerEntry
        {
            get => _layerEntry;
            set
            {
                _layerEntry = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(DefinitionId));
            }
        }

        public new string Name
        {
            get => LayerEntry.Name;
            set
            {
                LayerEntry.Name = value;
                OnPropertyChanged();
            }
        }

        public Guid DefinitionId
        {
            get => LayerEntry.DefinitionId;
            set
            {
                LayerEntry.DefinitionId = value;
                MainWindow.IsRedrawingNeeded = true;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get => LayerEntry.Visible;
            set
            {
                LayerEntry.Visible = value;
                OnPropertyChanged();
				MainWindow.IsRedrawingNeeded = true;
            }
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

        public NodeLayerEntryViewModel(MainWindowViewModel vm, LayerEntry layerEntry) :
            base(vm)
        {
            LayerEntry = layerEntry;
            if (layerEntry is LayerTilemap)
                LayerType = LayerType.Tilemap;
            else if (layerEntry is LayerObjects)
                LayerType = LayerType.ObjectGroup;
            else
                LayerType = LayerType.Unknown;
        }
    }
}
