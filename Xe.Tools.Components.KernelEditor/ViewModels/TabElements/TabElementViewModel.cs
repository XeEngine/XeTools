using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabElements
{
	public class TabElementViewModel : BaseNotifyPropertyChanged
	{
		private const int ItemsCount = 32;
		private ElementViewModel _selectedItem;

		public ElementViewModel[] Items { get; set; }

		public ElementViewModel SelectedItem
		{
			get => _selectedItem;
			set
			{
				_selectedItem = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsItemSelected));
			}
		}

		public bool IsItemSelected => _selectedItem != null;

		public TabElementViewModel(KernelData kData)
		{
			Items = new ElementViewModel[ItemsCount];

			// Initialize if necessary
			if (kData.Elements == null)
				kData.Elements = new List<Element>(ItemsCount);
			for (int i = kData.Elements.Count; i < ItemsCount; i++)
			{
				kData.Elements.Add(new Element()
				{
					Id = Guid.NewGuid(),
					VirtualIndex = i
				});
			}

			foreach (var item in kData.Elements)
			{
				if (item.VirtualIndex >= 0 && item.VirtualIndex < ItemsCount)
					Items[item.VirtualIndex] = new ElementViewModel(item);
			}
			SelectedItem = Items.FirstOrDefault();
		}
	}
}
