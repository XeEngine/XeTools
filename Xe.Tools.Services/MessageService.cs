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

		public delegate void LanguageChanged(Language language);
		public delegate void MessageChanged(string tag);
		public event LanguageChanged OnLanguageChanged;
		public event MessageChanged OnMessageChanged;

        public ProjectService ProjectService { get; private set; }

        public IEnumerable<IProjectFile> Items { get; private set; }

        public IEnumerable<Tuple<IProjectFile, MessageContainer>> MessageContainers { get; private set; }

		public Language Language { get; set; }

		public MessageService(ProjectService projectService)
        {
            ProjectService = projectService;
            Items = ProjectService.Items
                .Where(x => x.Format == "message")
                .Distinct(new ItemMessageEqualityComparer());

            MessageContainers = Items.Select(x => new Tuple<IProjectFile, MessageContainer> (
                x, ProjectService.DeserializeItem<MessageContainer>(x)
            ));
		}

		public Message GetMessage(string tag)
		{
			return GetMessage(tag, Language);
		}

        public Message GetMessage(string tag, Language language)
		{
			foreach (var container in MessageContainers)
			{
				var message = container.Item2.GetMessage(language, tag);
				if (message != null)
				{
					return message;
				}
			}

			return null;
		}

		public void SaveChanges()
        {

        }

		public string this[string tag]
		{
			get
			{
				var msg = GetMessage(tag);
				return msg != null ? msg.Text : tag;
			}
		}
    }
}
