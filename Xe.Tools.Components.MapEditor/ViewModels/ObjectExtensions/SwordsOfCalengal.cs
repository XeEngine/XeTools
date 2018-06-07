using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xe.Game.Kernel;
using Xe.Game.Tilemaps;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.MapEditor.ViewModels.ObjectExtensions.SwordsOfCalengal
{
    public class ExtensionBaseViewModel<T> : BaseNotifyPropertyChanged where T : class, IObjectExtension
    {
        protected T extension;

        public bool IsLoaded => extension != null;

        public Visibility Visibility => IsLoaded ? Visibility.Visible : Visibility.Collapsed;

        public ExtensionBaseViewModel(IObjectExtension objectExtension)
        {
            extension = objectExtension as T;
        }
    }

    public class PlayerViewModel : ExtensionBaseViewModel<Modules.ObjectExtensions.SwordsOfCalengal.Player>
    {
        public int Entry
        {
            get => extension?.Entry ?? 0;
            set
            {
                extension.Entry = value;
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
            get => extension?.ArtificialIntelligence ?? 0;
            set
            {
                extension.ArtificialIntelligence = value;
                OnPropertyChanged();
            }
        }

        public int Variant
        {
            get => extension?.Variant ?? 0;
            set
            {
                extension.Variant = value;
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
		public IEnumerable<Zone> Zones => MapEditorViewModel.Instance.Kernel.Zones;

		public int Zone
        {
            get => extension?.Zone ?? 0;
            set
            {
                extension.Zone = value;
                OnPropertyChanged();
            }
        }

        public int Map
        {
            get => extension?.Map ?? 0;
            set
            {
                extension.Map = value;
                OnPropertyChanged();
            }
        }

        public int Entry
        {
            get => extension?.Entry ?? 0;
            set
            {
                extension.Entry = value;
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
		public int Category
		{
			get => extension?.Category ?? 0;
			set
			{
				extension.Category = value;
				OnPropertyChanged();
			}
		}

		public int Index
		{
			get => extension?.Index ?? 0;
			set
			{
				extension.Index = value;
				OnPropertyChanged();
			}
		}

		public int Flags
		{
			get => extension?.Flags ?? 0;
			set
			{
				extension.Flags = value;
				OnPropertyChanged();
			}
		}

		public EventViewModel(IObjectExtension objectExtension) :
            base(objectExtension)
        { }
    }
}
