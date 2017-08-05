using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xe.Game.Messages;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class MessagesViewModel
    {
        private MessageContainer _messageContainer;

        public Project Project { get; private set; }
        public Project.Container Container { get; private set; }
        public Project.Item PItem { get; private set; }

        private string _workingFileName;
        
        public MessageContainer MessageContainer
        {
            get => _messageContainer;
            set
            {
                _messageContainer = value;
                Messages = new ObservableCollection<MessageViewModel>(
                    MessageContainer.Segments.SelectMany(x => x.Messages,
                        (seg, msg) => new MessageViewModel(seg.Name, msg)
                    ));
            }
        }

        public ObservableCollection<MessageViewModel> Messages { get; set; }

        public MessagesViewModel(Project project, Project.Container container, Project.Item item)
        {
            Project = project;
            Container = container;
            PItem = item;

            _workingFileName = Path.Combine(Path.Combine(project.ProjectPath, container.Name), item.Input);
            try
            {
                using (var reader = File.OpenText(_workingFileName))
                {
                    MessageContainer = JsonConvert.DeserializeObject<MessageContainer>(reader.ReadToEnd());

                    Log.Message($"Message file {_workingFileName} opened.");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while opening {item.Input}: {e.Message}");
            }
        }
        protected MessagesViewModel(MessageContainer messages)
        {
            MessageContainer = messages;
        }

        public MessageViewModel this[Guid id] => Messages.FirstOrDefault(x => x.Id == id);

        public Guid AddMessage(string category)
        {
            var id = Guid.NewGuid();
            Messages.Add(new MessageViewModel(category, new Message()
            {
                UID = id
            }));
            return id;
        }

        public void RemoveMessage(Guid id)
        {
            var message = this[id];
            if (message != null)
            {
                Messages.Remove(message);
            }
        }

        public void SaveChanges()
        {
            MessageContainer.Segments = Messages
                .GroupBy(x => x.Category)
                .Select(seg => new Segment()
                {
                    Id = 0,
                    Name = seg.Key,
                    Messages = seg.Select(msg => msg.Message).ToList()
                }).ToList();

            try
            {
                using (var writer = File.CreateText(_workingFileName))
                {
                    var str = JsonConvert.SerializeObject(MessageContainer, Formatting.Indented);
                    writer.Write(str);
                }
                Log.Message($"Message file {_workingFileName} saved.");
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving {PItem.Input}: {e.Message}");
            }
        }
    }
}
