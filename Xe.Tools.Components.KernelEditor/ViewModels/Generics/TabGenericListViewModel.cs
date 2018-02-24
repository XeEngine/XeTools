using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.KernelEditor.ViewModels.Generics
{
	public abstract class TabGenericListViewModel<T, TViewModel> : BaseNotifyPropertyChanged
	{
		private readonly List<T> _list;
		private TViewModel _selectedItem;

		public ObservableCollection<TViewModel> Items =>
			new ObservableCollection<TViewModel>(Query());

		public TViewModel SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;
				RemoveItem.CanExecute(value);
				OnSelectedItem(value);
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsItemSelected));
			}
		}

		public bool IsItemSelected => _selectedItem != null;

		public RelayCommand AddItem { get; }

		public RelayCommand RemoveItem { get; }

		public TabGenericListViewModel(List<T> list)
		{
			_list = list;
			AddItem = new RelayCommand(x =>
			{
				var item = NewItem();
				_list.Add(item);
				OnPropertyChanged(nameof(Items));
			}, x => _list != null);
			RemoveItem = new RelayCommand(x =>
			{
				var index = Items.IndexOf(SelectedItem);
				Items.RemoveAt(index);
				_list.RemoveAt(index);
			}, x => x != null && x is TViewModel);
		}

		private IEnumerable<TViewModel> Query()
		{
			return _list?.Select(x => NewViewModel(x)) ?? new TViewModel[0];
		}

		protected abstract TViewModel NewViewModel(T item);

		protected abstract T NewItem();

		protected abstract void OnSelectedItem(TViewModel item);
	}
}
