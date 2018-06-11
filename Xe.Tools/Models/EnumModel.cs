using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xe.Tools.Models
{
	public class EnumItemModel<T>
	{
		public T Value { get; set; }

		public string Name { get; set; }
	}

	public class EnumModel<T> : IEnumerable<EnumItemModel<T>>
		 where T : struct, IConvertible
	{
		private readonly IEnumerable<EnumItemModel<T>> items;

		public EnumModel()
		{
			var type = typeof(T);
			if (type.IsEnum == false)
			{
				throw new InvalidOperationException($"{type} is not an enum.");
			}

			items = Enum.GetValues(type)
				.Cast<T>()
				.Select(e => new EnumItemModel<T>()
				{
					Value = e,
					Name = e.ToString()
				});
		}

		public IEnumerator<EnumItemModel<T>> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}
	}
}
