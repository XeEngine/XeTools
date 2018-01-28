using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.MapEditor.ViewModels
{
    public class EventsEditorViewModel : BaseNotifyPropertyChanged
    {
        public class EventDefinitionModel : BaseNotifyPropertyChanged
        {
            public EventDefinition EventDefinition { get; }

            public int Index
            {
                get => EventDefinition.Index;
                set => EventDefinition.Index = value;
            }

            public string Name
            {
                get => EventDefinition.Name;
                set
                {
                    EventDefinition.Name = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayMember));
                }
            }

            public string DisplayMember => string.IsNullOrEmpty(Name) ? Index.ToString("X02") : Name;

            public EventDefinitionModel(EventDefinition eventDefinition)
            {
                EventDefinition = eventDefinition;
            }
        }

        private List<EventDefinition> _listEventDefinitions;
        private EventDefinitionModel _selectedEventDefinition;
        private bool _isSelectedEventExists;

        public ObservableCollection<EventDefinitionModel> EventDefinitions
        {
            get
            {
                var eventDefsModel = new EventDefinitionModel[128];
                for (int i = 0; i < eventDefsModel.Length; i++)
                {
                    var eventDef = _listEventDefinitions
                        .FirstOrDefault(x => x.Index == i) ??
                        new EventDefinition()
                        {
                            Index = i
                        };
                    eventDefsModel[i] = new EventDefinitionModel(eventDef);
                }
                return new ObservableCollection<EventDefinitionModel>(eventDefsModel);
            }
        }

        public EventDefinitionModel SelectedEventDefinition
        {
            get => _selectedEventDefinition;
            set
            {
                _selectedEventDefinition = value;
                _isSelectedEventExists = _listEventDefinitions
                    .Any(x => x.Index == _selectedEventDefinition.Index);
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEventDefintionSelected));
                OnPropertyChanged(nameof(EventDefinitionName));
            }
        }

        public bool IsEventDefintionSelected => SelectedEventDefinition != null;

        public string EventDefinitionName
        {
            get => SelectedEventDefinition?.Name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (_isSelectedEventExists)
                    {
                        _listEventDefinitions.Remove(SelectedEventDefinition.EventDefinition);
                        _isSelectedEventExists = false;
                    }
                }
                else
                {
                    if (!_isSelectedEventExists)
                    {
                        _listEventDefinitions.Add(SelectedEventDefinition.EventDefinition);
                        _isSelectedEventExists = true;
                    }
                }
                SelectedEventDefinition.Name = value;
                OnPropertyChanged();
            }
        }

        public EventsEditorViewModel(MainWindowViewModel vm)
        {
            _listEventDefinitions = vm.MapEditor.TileMap.EventDefinitions;
        }
    }
}
