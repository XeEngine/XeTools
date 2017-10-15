using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Projects;
using static Xe.Tools.Project;

namespace Xe.Tools.Services
{
    public class MessageService
    {
        private class ItemMessageEqualityComparer : IEqualityComparer<IProjectFile>
        {
            public bool Equals(IProjectFile x, IProjectFile y)
            {
                return x.Path == y.Path;
            }

            public int GetHashCode(IProjectFile obj)
            {
                return obj.Path.GetHashCode();
            }
        }

        public ProjectService ProjectService { get; private set; }

        public IEnumerable<IProjectFile> Items { get; private set; }

        public IEnumerable<Tuple<IProjectFile, MessageContainer>> MessageContainers { get; private set; }

        public IDictionary<Guid, Tuple<IProjectFile, string, Message>> Messages { get; private set; }

        public IDictionary<string, IDictionary<Guid, string>> Categories { get; private set; }

        public MessageService(ProjectService projectService)
        {
            ProjectService = projectService;
            Items = ProjectService.Items
                .Where(x => x.Format == "message")
                .Distinct(new ItemMessageEqualityComparer());
            MessageContainers = Items.Select(x => new Tuple<IProjectFile, MessageContainer> (
                x, ProjectService.DeserializeItem<MessageContainer>(x)
            ));
            Messages = MessageContainers.SelectMany(x => x.Item2.Segments
                .SelectMany(m => m.Messages.Select(mm =>
                    new Tuple<IProjectFile, string, Message>(x.Item1, m.Name, mm))
                ))
                .ToDictionary(x => x.Item3.UID, x => x);
            Categories = MessageContainers.SelectMany(x => x.Item2.Segments)
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
            return message?.Item3;
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
