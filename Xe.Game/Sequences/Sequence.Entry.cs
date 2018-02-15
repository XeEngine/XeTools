using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xe.Game.Sequences
{
	public partial class Sequence
	{
		public class Entry
		{
			private Operation _operation;
			private OperationEntry _entry;

			public Operation Operation
			{
				get => _entry.Operation;
				set
				{
					if (_operation == value)
						return;
					_operation = value;
					SetInternal();
				}
			}

			public object[] Parameters { get; set; } = new object[0];

			public bool IsAsynchronous { get; set; }

			public string UserDescription { get; set; }

			[JsonIgnore]
			public string Description => _entry.Description;

			[JsonIgnore]
			public IOperationParameter[] ParametersDescription => _entry.Parameters;

			public Entry(Operation operation)
			{
				Operation = operation;
			}

			public object GetValue(int index)
			{
				return Parameters[index];
			}

			public Entry SetValue(int index, object value)
			{
				var desc = ParametersDescription[index];
				if (value == null)
				{
					Parameters[index] = desc.DefaultValue;
				}
				else if (value.GetType() == desc.ValueType)
				{
					Parameters[index] = value;
				}
				else
					throw new InvalidCastException($"Value {value} of type {value.GetType()} must be of type {desc.ValueType}.");
				return this;
			}

			public Entry SetAsynchronous(bool isAsync)
			{
				IsAsynchronous = isAsync;
				return this;
			}

			private void SetInternal()
			{
				_entry = OPERATIONS[_operation];
				Parameters = _entry.Parameters
					.Select(x => x.DefaultValue)
					.ToArray();
			}
		}
	}
}
