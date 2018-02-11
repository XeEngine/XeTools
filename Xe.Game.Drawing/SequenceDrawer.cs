using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Drawing;
using Xe.Game.Sequences;
using Xe.Tools.Services;

namespace Xe.Game.Drawing
{
	public class SequenceDrawer : MapDrawer
	{
		private Sequence _sequence;

		public Sequence Sequence
		{
			get => _sequence;
			set
			{
				_sequence = value;
				EntryIndex = 0;
			}
		}

		public int EntryIndex { get; set; }

		public SequenceDrawer(ProjectService projService, IDrawing drawing) :
			base(projService, drawing)
		{ }

		Sequence.Entry PeekEntry()
		{
			return _sequence.Entries[EntryIndex];
		}

		public void Step()
		{
			var entry = _sequence.Entries[EntryIndex++];
			Execute(entry);
		}
		
		private void Execute(Sequence.Entry entry)
		{

		}
	}
}
