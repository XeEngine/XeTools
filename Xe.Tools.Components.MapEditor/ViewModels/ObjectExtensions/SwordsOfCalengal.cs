using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels.ObjectExtensions.SwordsOfCalengal
{
    public class ExtensionBaseViewModel<T> : BaseNotifyPropertyChanged where T : class, IObjectExtension
    {
        protected T _extension;

        public bool IsLoaded => _extension != null;

        public Visibility Visibility => IsLoaded ? Visibility.Visible : Visibility.Collapsed;

        public ExtensionBaseViewModel(IObjectExtension objectExtension)
        {
            _extension = objectExtension as T;
        }
    }

    public class PlayerViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Player>
    {
        public int Entry
        {
            get => _extension?.Entry ?? 0;
            set
            {
                _extension.Entry = value;
                OnPropertyChanged();
            }
        }

        public PlayerViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }

    public class EnemyViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Enemy>
    {
        public int ArtificialIntelligence
        {
            get => _extension?.ArtificialIntelligence ?? 0;
            set
            {
                _extension.ArtificialIntelligence = value;
                OnPropertyChanged();
            }
        }

        public int Variant
        {
            get => _extension?.Variant ?? 0;
            set
            {
                _extension.Variant = value;
                OnPropertyChanged();
            }
        }

        public EnemyViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }

    public class NpcViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Npc>
    {
        public NpcViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }

    public class MapChangeViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.MapChange>
    {
        public int Zone
        {
            get => _extension?.Zone ?? 0;
            set
            {
                _extension.Zone = value;
                OnPropertyChanged();
            }
        }

        public int Map
        {
            get => _extension?.Map ?? 0;
            set
            {
                _extension.Map = value;
                OnPropertyChanged();
            }
        }

        public int Entry
        {
            get => _extension?.Entry ?? 0;
            set
            {
                _extension.Entry = value;
                OnPropertyChanged();
            }
        }

        public MapChangeViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }

    public class ChestViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Chest>
    {
        public ChestViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }

    public class EventViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Event>
    {
        public EventViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }
}
