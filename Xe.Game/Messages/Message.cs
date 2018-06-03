using System;

namespace Xe.Game.Messages
{
	public class Message : IDeepCloneable
	{
		public Language Language { get; set; }

		public string Tag { get; set; }

		public string Text { get; set; }

		public object DeepClone()
		{
			return new Message()
			{
				Language = Language,
				Tag = Tag,
				Text = Text
			};
		}

		public override string ToString()
		{
			return string.IsNullOrEmpty(Text) ? Tag : Text;
		}
	}
}
