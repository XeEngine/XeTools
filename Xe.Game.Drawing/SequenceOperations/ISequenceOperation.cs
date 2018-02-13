using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Drawing.SequenceOperations
{
	internal interface ISequenceOperation
	{
		bool IsFinished { get; }

		double TimeDiscarded { get; }

		bool IsAsynchronous { get; }

		void Update(double deltaTime);
	}
}
