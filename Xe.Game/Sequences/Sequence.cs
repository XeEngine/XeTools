using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xe.Game.Sequences
{
	public enum ParameterType
	{
		Boolean,
		Integer,
		Double,
		String,
		Color,
		Point2f,
		Vector2f
	}

	public class OperationEntry
	{
		public Operation Operation { get; }

		public string Description { get; }

		public IOperationParameter[] Parameters { get; }

		public OperationEntry(FieldInfo field)
		{
			Operation = (Operation)field.GetValue(null);

			var parameters = new List<IOperationParameter>(8);
			foreach (var attribute in field.GetCustomAttributes(false))
			{
				if (attribute is ParameterAttribute _parameter)
				{
					parameters.Add(_parameter);
				}
				else if (attribute is DescriptionAttribute _description)
				{
					Description = _description.Description;
				}
			}
			Parameters = parameters.ToArray();
		}
	}

	public interface IOperationParameter
	{
		ParameterType Type { get; }

		Type ValueType { get; }

		object DefaultValue { get; }

		object MinimumValue { get; }

		object MaximumValue { get; }

		string Description { get; }
	}

	public partial class Sequence
	{
		public static readonly Dictionary<Operation, OperationEntry> OPERATIONS =
			typeof(Operation).GetFields()
			.Where(x => x.IsLiteral)
			.Select(x => new OperationEntry(x))
			.ToDictionary(x => x.Operation, x => x);

		public List<Entry> Entries { get; set; } = new List<Entry>();
	}
}
