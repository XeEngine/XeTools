using System.Collections.Generic;

namespace Xe.Game.PalAnimations
{
	public class PalCommand
    {
		public CommandType Command { get; set; }

		public Ease Ease { get; set; }

		public bool InvertedTimer { get; set; }

		public float Start { get; set; }

		public float End { get; set; }

		public float Loop { get; set; }

		public List<object> Parameters { get; set; }
	}
}
