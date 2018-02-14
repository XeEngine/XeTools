using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Sequences;
using Xe.Tools.Services;

namespace Xe.Tools.Components.SequenceEditor
{
	public interface IController
	{
		Sequence Sequence { get; }

		ProjectService ProjectService { get; }

		double RenderTime { get; set; }

		double ExecutionTime { get; set; }

		double TimeMultiplier { get; set; }

		int CurrentOperationIndex { set; }

		bool IsSequenceFinished { set; }

		void AddSequenceOperator();

		void RemoveSequenceOperator(Sequence.Entry entry);

		void MoveSequenceUp(Sequence.Entry entry);

		void MoveSequenceDown(Sequence.Entry entry);
	}
}
