using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Sequences;

namespace Xe.Tools.Components.SequenceEditor
{
	public interface IController
	{
		Sequence Sequence { get; }

		void AddSequenceOperator();

		void RemoveSequenceOperator(Sequence.Entry entry);

		void MoveSequenceUp(Sequence.Entry entry);

		void MoveSequenceDown(Sequence.Entry entry);
	}
}
