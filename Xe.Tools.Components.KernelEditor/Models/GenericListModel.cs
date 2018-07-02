﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public abstract class GenericListModel<T> : BaseNotifyPropertyChanged
	{
		private T selectedItem;
		private int selectedIndex;
		protected readonly ObservableCollection<T> list;

		public GenericListModel(IEnumerable<T> list)
		{
			Items = this.list = list != null ?
				new ObservableCollection<T>(list) :
				new ObservableCollection<T>();

			AddCommand = new RelayCommand(x =>
			{
				var item = OnNewItem();
				this.list.Add(OnNewItem());
				OnPropertyChanged(nameof(Items));
			}, x => this.list != null);

			RemoveCommand = new RelayCommand(x =>
			{
				var index = Items.IndexOf(SelectedItem);
				Items.RemoveAt(index);
				this.list.RemoveAt(index);
			}, x => x != null && x is T);

			MoveUpCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(--selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => x != null && x is T);

			MoveDownCommand = new RelayCommand(x =>
			{
				var item = Items[selectedIndex];
				Items.RemoveAt(selectedIndex);
				Items.Insert(++selectedIndex, item);
				SelectedIndex = selectedIndex;
			}, x => x != null && x is T);
		}

		public ObservableCollection<T> Items { get; private set; }

		public T SelectedItem
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

		private void NotifyItemSelected()
		{
			OnPropertyChanged(nameof(SelectedItem));
			OnPropertyChanged(nameof(SelectedIndex));
			OnPropertyChanged(nameof(IsItemSelected));
			OnPropertyChanged(nameof(RemoveCommand));
			OnPropertyChanged(nameof(MoveUpCommand));
			OnPropertyChanged(nameof(MoveDownCommand));
		}

		public void Filter(Func<T, bool> selector = null)
		{
			Items = selector != null ?
				new ObservableCollection<T>(list.Where(selector)) : list;
		}

		protected abstract T OnNewItem();

		protected abstract void OnSelectedItem(T item);
	}
}
