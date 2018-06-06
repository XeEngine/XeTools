using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public abstract class GenericListModel<T, TModel> : BaseNotifyPropertyChanged
	{
		private readonly List<T> list;
		private TModel selectedItem;
		private int selectedIndex;

		public ObservableCollection<TModel> Items =>
			new ObservableCollection<TModel>(Query());

		public TModel SelectedItem
		{
			get => selectedItem;
			set
			{
				selectedItem = value;
				OnSelectedItem(value);

				NotifyItemSelected();
			}
		}

		public int SelectedIndex
		{
			get => selectedIndex;
			set
			{
				selectedIndex = value;
				
				NotifyItemSelected();
			}
		}

		public bool IsItemSelected => selectedItem != null;

		public RelayCommand AddCommand { get; }

		public RelayCommand RemoveCommand { get; }

		public RelayCommand MoveUpCommand { get; }

		public RelayCommand MoveDownCommand { get; }

		public GenericListModel(List<T> list)
		{
			this.list = list;

			AddCommand = new RelayCommand(x =>
			{
				var item = NewItem();
				this.list.Add((T)item);
				OnPropertyChanged(nameof(Items));
			}, x => this.list != null);

			RemoveCommand = new RelayCommand(x =>
			{
				var index = Items.IndexOf(SelectedItem);
				Items.RemoveAt(index);
				this.list.RemoveAt(index);
			}, x => x != null && x is TModel);

			MoveUpCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(--selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => x != null && x is TModel);

			MoveDownCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(++selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => x != null && x is TModel);
		}

		private void NotifyItemSelected()
		{
			OnPropertyChanged(nameof(SelectedItem));
			OnPropertyChanged(nameof(SelectedIndex));
			OnPropertyChanged(nameof(IsItemSelected));
			OnPropertyChanged(nameof(RemoveCommand));
			OnPropertyChanged(nameof(MoveUpCommand));
			OnPropertyChanged(nameof(MoveDownCommand));
		}

		private IEnumerable<TModel> Query()
		{
			return list?.Select(x => NewViewModel(x)) ?? new TModel[0];
		}

		protected abstract TModel NewViewModel(T item);

		protected abstract T NewItem();

		protected abstract void OnSelectedItem(TModel item);
	}
}
