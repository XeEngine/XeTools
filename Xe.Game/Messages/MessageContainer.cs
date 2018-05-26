using System;
using System.Collections.Generic;
using System.Linq;

namespace Xe.Game.Messages
{
    public class MessageContainer
    {
		private List<Message> messages = new List<Message>();
		private Dictionary<Guid, Message> idMessages = new Dictionary<Guid, Message>();
		private Dictionary<Language, List<Message>> langMessages = new Dictionary<Language, List<Message>>();
		private Dictionary<string, List<Message>> tagMessages = new Dictionary<string, List<Message>>();

		public List<Message> Messages
		{
			get => messages;
			set
			{
				messages = value;
				idMessages = messages.ToDictionary(x => x.UID, x => x);
				langMessages = messages
					.GroupBy(x => x.Language)
					.ToDictionary(x => x.Key, x => x.ToList());
				tagMessages = messages
					.GroupBy(x => x.Tag)
					.ToDictionary(x => x.Key, x => x.ToList());
			}
		}

		public Message GetMessage(Guid id)
		{
			idMessages.TryGetValue(id, out var messages);
			return messages;
		}

		public Message GetMessage(Language language, string tag)
		{
			return GetMessagesByTag(tag)?.FirstOrDefault(x => x.Language == language);
		}

		public IEnumerable<Message> GetMessagesByLanguage(Language language)
		{
			return langMessages.TryGetValue(language, out var messages) ? messages : new List<Message>();
		}

		public IEnumerable<Message> GetMessagesByTag(string tag)
		{
			return tagMessages.TryGetValue(tag, out var messages) ? messages : new List<Message>();
		}

		public Message this[Guid id] => GetMessage(id);

		public Message this[Language language, string tag] => GetMessage(language, tag);
	}
}
