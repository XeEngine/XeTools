using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Sequences
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
    {
		public string Description { get; }

		public DescriptionAttribute(string description)
		{
			Description = description;
		}

		public override string ToString()
		{
			return Description;
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class ParameterAttribute : Attribute, IOperationParameter
	{
		public ParameterType Type { get; }

		public Type ValueType { get; }

		public object DefaultValue { get; }

		public object MinimumValue { get; }

		public object MaximumValue { get; }

		public string Description { get; }


		public ParameterAttribute(int defaultValue, int minimum, int maximum, string description)
		{
			Type = ParameterType.Integer;
			ValueType = typeof(int);
			DefaultValue = defaultValue;
			MinimumValue = minimum;
			MaximumValue = maximum;
			Description = description;
		}

		public ParameterAttribute(float defaultValue, float minimum, float maximum, string description)
		{
			Type = ParameterType.Float;
			ValueType = typeof(float);
			DefaultValue = defaultValue;
			MinimumValue = minimum;
			MaximumValue = maximum;
			Description = description;
		}

		public ParameterAttribute(bool defaultValue, string description)
		{
			Type = ParameterType.Boolean;
			ValueType = typeof(bool);
			DefaultValue = defaultValue;
			Description = description;
		}

		public ParameterAttribute(string defaultValue, string description)
		{
			Type = ParameterType.String;
			ValueType = typeof(string);
			DefaultValue = defaultValue;
			Description = description;
		}

		public override string ToString()
		{
			return DefaultValue?.ToString() ?? "null";
		}
	}
}
