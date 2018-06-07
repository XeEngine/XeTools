using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Messages;
using Xe.Tools.Projects;

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

		public IEnumerable<string> Tags => MessageContainers
			.SelectMany(x => x.Item2.Messages)
			.GroupBy(x => x.Tag)
			.Select(x => x.Key);


		public Language Language { get; set; }

		public MessageService(ProjectService projectService)
        {
            ProjectService = projectService;
            Items = ProjectService.Items
                .Where(x => x.Format == "message")
                .Distinct(new ItemMessageEqualityComparer());

            MessageContainers = Items.Select(x => new Tuple<IProjectFile, MessageContainer> (
                x, ProjectService.DeserializeItem<MessageContainer>(x)
            )).ToList();
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

		public Message SetMessage(string file, string tag, Language language, string text)
		{
			foreach (var entry in MessageContainers)
			{
				if (entry.Item1.Name == file)
				{
					var msg = entry.Item2.GetMessage(language, tag);
					if (msg == null)
					{
						entry.Item2.Messages.Add(new Message()
						{
							Language = language,
							Tag = tag,
							Text = text
						});
					}
					else
					{
						msg.Text = text;
					}

					return msg;
				}
			}

			return null;
		}

		public void RemoveMessage(string file, string tag, Language language)
		{
			foreach (var entry in MessageContainers)
			{
				if (entry.Item1.Name == file)
				{
					var msg = entry.Item2.Messages
						.FirstOrDefault(x => x.Tag == tag && x.Language == Language);

					if (msg != null)
					{
						entry.Item2.Messages.Remove(msg);
					}
				}
			}
		}

		public void SaveChanges()
        {
			foreach (var entry in MessageContainers)
			{
				using (var stream = File.CreateText(entry.Item1.FullPath))
				{
					var data = entry.Item2.Messages.OrderBy(x => x.Tag).ToList();
					entry.Item2.Messages = entry.Item2.Messages.OrderBy(x => x.Tag).ToList();
					stream.Write(JsonConvert.SerializeObject(entry.Item2, Formatting.Indented));
				}
			}
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
