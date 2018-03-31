using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class MainWindowViewModel : BaseNotifyPropertyChanged
    {
        #region Main properties

        /// <summary>
        /// Main view model
        /// </summary>
        public MapEditorViewModel MapEditor { get; }

        private NodeMapViewModel _masterNode;
        /// <summary>
        /// Node that contains the map, layers, tilemaps and objects groups
        /// </summary>
        public NodeMapViewModel MasterNode
        {
            get => _masterNode;
            set
            {
                _masterNode = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Item selection

        private enum PropertyBar
        {
            None,
            Map,
            Layer,
            Tilemap,
            ObjectsGroup,
            ObjectEntry
        }

        private PropertyBar _propertyBar;

        #region Node selection

        private NodeBaseViewModel _selectedNode;
        public NodeBaseViewModel SelectedNode
        {
            get => _selectedNode;
            set
            {
                _selectedNode = value;
                OnPropertyChanged();

                if (value != null)
                {
                    if (value is NodeMapViewModel mastreNode)
                    {
                        SetPropertyBar(PropertyBar.Map);
                    }
                    else if (value is NodeLayerViewModel node)
                    {
                        NodeLayerViewModel = node;
                        SetPropertyBar(PropertyBar.Layer);
                    }
                    else if (value is NodeEntryTilemapViewModel tileMap)
                    {
                        NodeEntryTilemapViewModel = tileMap;
                        SetPropertyBar(PropertyBar.Tilemap);
                    }
                    else if (value is NodeObjectsGroupViewModel objGroup)
                    {
                        NodeObjectsGroupViewModel = objGroup;
                        SetPropertyBar(PropertyBar.ObjectsGroup);
                    }
                    else
                    {
                        SetPropertyBar(PropertyBar.None);
                    }
                }
            }
        }

        private NodeMapViewModel _nodeMapViewModel;
        private NodeLayerViewModel _nodeLayerViewModel;
        private NodeEntryTilemapViewModel _nodeEntryTilemapViewModel;
        private NodeObjectsGroupViewModel _nodeObjectsGroupViewModel;

        public NodeMapViewModel NodeMapViewModel
        {
            get => _nodeMapViewModel;
            set
            {
                _nodeMapViewModel = value;
                OnPropertyChanged();
            }
        }

        public NodeLayerViewModel NodeLayerViewModel
        {
            get => _nodeLayerViewModel;
            set
            {
                _nodeLayerViewModel = value;
                OnPropertyChanged();
            }
        }

        public NodeEntryTilemapViewModel NodeEntryTilemapViewModel
        {
            get => _nodeEntryTilemapViewModel;
            set
            {
                _nodeEntryTilemapViewModel = value;
                OnPropertyChanged();
            }
        }

        public NodeObjectsGroupViewModel NodeObjectsGroupViewModel
        {
            get => _nodeObjectsGroupViewModel;
            set
            {
                _nodeObjectsGroupViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public Visibility PropertyMapVisibility => _propertyBar == PropertyBar.Map ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyLayerVisibility => _propertyBar == PropertyBar.Layer ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyTilemapVisibility => _propertyBar == PropertyBar.Tilemap ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyObjectsGroupVisibility => _propertyBar == PropertyBar.ObjectsGroup ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PropertyObjectEntryVisibility => _propertyBar == PropertyBar.ObjectEntry ? Visibility.Visible : Visibility.Collapsed;

        private void SetPropertyBar(PropertyBar propertyBar)
        {
            _propertyBar = propertyBar;
            OnPropertyChanged(nameof(PropertyMapVisibility));
            OnPropertyChanged(nameof(PropertyLayerVisibility));
            OnPropertyChanged(nameof(PropertyTilemapVisibility));
            OnPropertyChanged(nameof(PropertyObjectsGroupVisibility));
            OnPropertyChanged(nameof(PropertyObjectEntryVisibility));
        }

        #endregion
        
        public ObjectPropertiesViewModel ObjectPropertiesViewModel { get; }

        private ObjectEntry _selectedObjectEntry;
        public ObjectEntry SelectedObjectEntry
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

        #region Commands

        public ICommand CommandSaveMap { get; }

		public ICommand CommandEditEvents { get; }

		public ICommand CommandEditEventsListeners { get; }

		public ICommand GroupLayersByPriorityCommand { get; }

        public ICommand UngroupLayersCommand { get; }

        #endregion

        private bool _isRedrawingNeeded;
        private object _lockIsRedrawingNeeded = new object();
        public bool IsRedrawingNeeded
        {
            get => _isRedrawingNeeded;
            set
            {
                lock (_lockIsRedrawingNeeded)
                {
                    _isRedrawingNeeded = value;
                }
            }
        }

        public MainWindowViewModel(MapEditorViewModel vm)
        {
            MapEditor = vm;
            MasterNode = new NodeMapViewModel(this, MapEditor.MapName);
            
            ObjectPropertiesViewModel = new ObjectPropertiesViewModel(this);
            SetPropertyBar(PropertyBar.None);

            MapEditor.OnTilemapChanged += (sender, tileMap) =>
            {
                OnPropertyChanged(nameof(MasterNode));
            };

            CommandSaveMap = new RelayCommand(o => MapEditor.Save());
			CommandEditEvents = new RelayCommand(o =>
			{
				new Windows.EventsPropertiesWindow(this).ShowDialog();
				// TODO aggiornare la lista degli eventi
			});
			CommandEditEventsListeners = new RelayCommand(o =>
			{
				//new Windows.EventsListenersWindow(this).ShowDialog();
			});

			GroupLayersByPriorityCommand = new RelayCommand(o => ChangeLayerOrder(NodeOrderMode.GroupByPriority));
            UngroupLayersCommand = new RelayCommand(o => ChangeLayerOrder(NodeOrderMode.OriginalOrder));
        }

        private void ChangeLayerOrder(NodeOrderMode order)
        {
            var masterNode = MasterNode;
            masterNode.NodeOrderMode = order;

            MasterNode = null;
            OnPropertyChanged(nameof(MasterNode));
            MasterNode = masterNode;
            OnPropertyChanged(nameof(MasterNode));
        }

        private void OnTilemapChanged(object sender, Map tilemap)
        {
            MasterNode = new NodeMapViewModel(this, MapEditor.MapName);
        }
    }
}
