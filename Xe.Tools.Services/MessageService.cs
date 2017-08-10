using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Messages;
using static Xe.Tools.Project;

namespace Xe.Tools.Services
{
    public class MessageService
    {
        private class ItemMessageEqualityComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                return x.Input == y.Input;
            }

            public int GetHashCode(Item obj)
            {
                return obj.Input.GetHashCode();
            }
        }

        public ProjectService ProjectService { get; private set; }

        public IEnumerable<Item> Items { get; private set; }

        public IEnumerable<MessageContainer> MessageContainers { get; private set; }

        public IDictionary<Guid, Message> Messages { get; private set; }

        public IDictionary<string, IDictionary<Guid, string>> Categories { get; private set; }

        public MessageService(ProjectService projectService)
        {
            ProjectService = projectService;
            Items = ProjectService.Items
                .Where(x => x.Type == "message")
                .Distinct(new ItemMessageEqualityComparer());
            MessageContainers = Items.Select(x => ProjectService.DeserializeItem<MessageContainer>(x));
            Messages = MessageContainers.SelectMany(x => x.Segments
                .SelectMany(m => m.Messages))
                .ToDictionary(x => x.UID, x => x);
            Categories = MessageContainers.SelectMany(x => x.Segments)
                .ToDictionary(x => x.Name, x => x.Messages.ToDictionary(m => m.UID, m => m.En) as IDictionary<Guid, string>);
        }

        public IDictionary<Guid, string> GetMessages(string category)
        {
            Categories.TryGetValue(category, out var dic);
            return dic;
        }

        public Message GetMessage(Guid id)
        {
            Messages.TryGetValue(id, out var message);
            return message;
        }

        public string GetString(Guid id)
        {
            return GetMessage(id)?.En;
        }

        public void SaveChanges()
        {

        }
    }
}
