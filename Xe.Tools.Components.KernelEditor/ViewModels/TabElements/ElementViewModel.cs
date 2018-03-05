using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels.TabElements
{
	public class ElementViewModel : BaseNotifyPropertyChanged
	{
		public Element Element { get; }

		public Guid Id => Element?.Id ?? Guid.Empty;

		public int VirtualIndex => Element?.VirtualIndex ?? -1;

		public string DisplayName => !string.IsNullOrEmpty(Name) ? Name : $"<{Element.VirtualIndex.ToString("X02")}>";

		public string Name
		{
			get => Element.Name;
			set
			{
				Element.Name = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(DisplayName));
			}
		}

		public ElementViewModel(Element element)
		{
			Element = element;
		}
	}
}
