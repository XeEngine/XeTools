using System.Collections.Generic;
using System.Linq;

namespace Xe.Tools.Components.KernelEditor.Models
{
	public class BitmaskSelectionModel : BaseNotifyPropertyChanged
	{
		private uint value;

		public BitmaskSelectionModel(IEnumerable<string> names, uint value)
		{
			this.value = value;

			var properties = GetType()
				.GetProperties()
				.Where(x => x.Name.IndexOf("Name") == 0)
				.ToDictionary(x => x.Name, x => x);

			foreach (var property in properties)
			{
				if (int.TryParse(property.Key.Substring(4), out var index))
				{
					property.Value.SetValue(this, index.ToString("X02"));
				}
			}

			var i = 0;
			foreach (var name in names.Take(properties.Count))
			{
				if (properties.TryGetValue($"Name{i++}", out var property))
				{
					property.SetValue(this, name);
				}
			}
		}

		public uint Value
		{
			get => value;
			set
			{
				this.value = value;
				OnAllPropertiesChanged();
			}
		}

		public string Name0 { get; private set; }
		public string Name1 { get; private set; }
		public string Name2 { get; private set; }
		public string Name3 { get; private set; }
		public string Name4 { get; private set; }
		public string Name5 { get; private set; }
		public string Name6 { get; private set; }
		public string Name7 { get; private set; }
		public string Name8 { get; private set; }
		public string Name9 { get; private set; }
		public string Name10 { get; private set; }
		public string Name11 { get; private set; }
		public string Name12 { get; private set; }
		public string Name13 { get; private set; }
		public string Name14 { get; private set; }
		public string Name15 { get; private set; }
		public string Name16 { get; private set; }
		public string Name17 { get; private set; }
		public string Name18 { get; private set; }
		public string Name19 { get; private set; }
		public string Name20 { get; private set; }
		public string Name21 { get; private set; }
		public string Name22 { get; private set; }
		public string Name23 { get; private set; }
		public string Name24 { get; private set; }
		public string Name25 { get; private set; }
		public string Name26 { get; private set; }
		public string Name27 { get; private set; }
		public string Name28 { get; private set; }
		public string Name29 { get; private set; }
		public string Name30 { get; private set; }
		public string Name31 { get; private set; }
		public bool Value0 { get => (value & (1 << 0)) != 0; set => this.value = value ? this.value | (1 << 0) : (uint)(this.value & ~(1 << 0)); }
		public bool Value1 { get => (value & (1 << 1)) != 0; set => this.value = value ? this.value | (1 << 1) : (uint)(this.value & ~(1 << 1)); }
		public bool Value2 { get => (value & (1 << 2)) != 0; set => this.value = value ? this.value | (1 << 2) : (uint)(this.value & ~(1 << 2)); }
		public bool Value3 { get => (value & (1 << 3)) != 0; set => this.value = value ? this.value | (1 << 3) : (uint)(this.value & ~(1 << 3)); }
		public bool Value4 { get => (value & (1 << 4)) != 0; set => this.value = value ? this.value | (1 << 4) : (uint)(this.value & ~(1 << 4)); }
		public bool Value5 { get => (value & (1 << 5)) != 0; set => this.value = value ? this.value | (1 << 5) : (uint)(this.value & ~(1 << 5)); }
		public bool Value6 { get => (value & (1 << 6)) != 0; set => this.value = value ? this.value | (1 << 6) : (uint)(this.value & ~(1 << 6)); }
		public bool Value7 { get => (value & (1 << 7)) != 0; set => this.value = value ? this.value | (1 << 7) : (uint)(this.value & ~(1 << 7)); }
		public bool Value8 { get => (value & (1 << 8)) != 0; set => this.value = value ? this.value | (1 << 8) : (uint)(this.value & ~(1 << 8)); }
		public bool Value9 { get => (value & (1 << 9)) != 0; set => this.value = value ? this.value | (1 << 9) : (uint)(this.value & ~(1 << 9)); }
		public bool Value10 { get => (value & (1 << 10)) != 0; set => this.value = value ? this.value | (1 << 10) : (uint)(this.value & ~(1 << 10)); }
		public bool Value11 { get => (value & (1 << 11)) != 0; set => this.value = value ? this.value | (1 << 11) : (uint)(this.value & ~(1 << 11)); }
		public bool Value12 { get => (value & (1 << 12)) != 0; set => this.value = value ? this.value | (1 << 12) : (uint)(this.value & ~(1 << 12)); }
		public bool Value13 { get => (value & (1 << 13)) != 0; set => this.value = value ? this.value | (1 << 13) : (uint)(this.value & ~(1 << 13)); }
		public bool Value14 { get => (value & (1 << 14)) != 0; set => this.value = value ? this.value | (1 << 14) : (uint)(this.value & ~(1 << 14)); }
		public bool Value15 { get => (value & (1 << 15)) != 0; set => this.value = value ? this.value | (1 << 15) : (uint)(this.value & ~(1 << 15)); }
		public bool Value16 { get => (value & (1 << 16)) != 0; set => this.value = value ? this.value | (1 << 16) : (uint)(this.value & ~(1 << 16)); }
		public bool Value17 { get => (value & (1 << 17)) != 0; set => this.value = value ? this.value | (1 << 17) : (uint)(this.value & ~(1 << 17)); }
		public bool Value18 { get => (value & (1 << 18)) != 0; set => this.value = value ? this.value | (1 << 18) : (uint)(this.value & ~(1 << 18)); }
		public bool Value19 { get => (value & (1 << 19)) != 0; set => this.value = value ? this.value | (1 << 19) : (uint)(this.value & ~(1 << 19)); }
		public bool Value20 { get => (value & (1 << 20)) != 0; set => this.value = value ? this.value | (1 << 20) : (uint)(this.value & ~(1 << 20)); }
		public bool Value21 { get => (value & (1 << 21)) != 0; set => this.value = value ? this.value | (1 << 21) : (uint)(this.value & ~(1 << 21)); }
		public bool Value22 { get => (value & (1 << 22)) != 0; set => this.value = value ? this.value | (1 << 22) : (uint)(this.value & ~(1 << 22)); }
		public bool Value23 { get => (value & (1 << 23)) != 0; set => this.value = value ? this.value | (1 << 23) : (uint)(this.value & ~(1 << 23)); }
		public bool Value24 { get => (value & (1 << 24)) != 0; set => this.value = value ? this.value | (1 << 24) : (uint)(this.value & ~(1 << 24)); }
		public bool Value25 { get => (value & (1 << 25)) != 0; set => this.value = value ? this.value | (1 << 25) : (uint)(this.value & ~(1 << 25)); }
		public bool Value26 { get => (value & (1 << 26)) != 0; set => this.value = value ? this.value | (1 << 26) : (uint)(this.value & ~(1 << 26)); }
		public bool Value27 { get => (value & (1 << 27)) != 0; set => this.value = value ? this.value | (1 << 27) : (uint)(this.value & ~(1 << 27)); }
		public bool Value28 { get => (value & (1 << 28)) != 0; set => this.value = value ? this.value | (1 << 28) : (uint)(this.value & ~(1 << 28)); }
		public bool Value29 { get => (value & (1 << 29)) != 0; set => this.value = value ? this.value | (1 << 29) : (uint)(this.value & ~(1 << 29)); }
		public bool Value30 { get => (value & (1 << 30)) != 0; set => this.value = value ? this.value | (1 << 30) : (uint)(this.value & ~(1 << 30)); }
		public bool Value31 { get => (value & (1 << 31)) != 0; set => this.value = value ? (uint)(this.value | (1 << 31)) : (uint)(this.value & ~(1 << 31)); }

	}
}
