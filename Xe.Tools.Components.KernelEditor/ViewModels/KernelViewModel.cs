using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Game.Messages;
using static Xe.Tools.Project;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class KernelViewModel
    {
        public Project Project { get; private set; }
        public Container Container { get; private set; }
        public Item Item { get; private set; }

        public KernelData Kernel { get; private set; }
        private string WorkingFileName { get; set; }
        private string BasePath { get => Path.GetDirectoryName(WorkingFileName); }

        public AnimationGroupsViewModel AnimationGroups { get; private set; }
        public MessagesViewModel Messages { get; private set; }
        public SkillsViewModel Skills { get; private set; }

        public KernelViewModel(Project project, Container container, Item item)
        {
            Project = project;
            Container = container;
            Item = item;

            WorkingFileName = Path.Combine(Path.Combine(Project.ProjectPath, Container.Name), item.Input);

            try
            {
                using (var reader = File.OpenText(WorkingFileName))
                {
                    Kernel = JsonConvert.DeserializeObject<KernelData>(reader.ReadToEnd());
                    if (Kernel.Skills == null) Kernel.Skills = new List<Skill>();
                    if (Kernel.Abilities == null) Kernel.Abilities = new List<Ability>();
                    if (Kernel.Players == null) Kernel.Players = new List<Player>();
                    if (Kernel.Enemies == null) Kernel.Enemies = new List<Enemy>();

                    Log.Message($"Kernel file {WorkingFileName} opened.");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while opening {Item.Input}: {e.Message}");
            }

            AnimationGroups = new AnimationGroupsViewModel(Project, Container);
            Messages = CreateMessagesViewModel("sys.msg");
            Skills = new SkillsViewModel(Kernel.Skills, Messages, AnimationGroups);
        }

        public void SaveChanges()
        {
            Messages.SaveChanges();
            Skills.SaveChanges();
            try
            {
                using (var writer = File.CreateText(WorkingFileName))
                {
                    var str = JsonConvert.SerializeObject(Kernel, Formatting.Indented);
                    writer.Write(str);
                }
                Log.Message($"Message file {WorkingFileName} saved.");
            }
            catch (Exception e)
            {
                Log.Error($"Error while saving changes on {Item.Input}: {e.Message}");
            }
        }

        private MessagesViewModel CreateMessagesViewModel(string fileName)
        {
            MessagesViewModel mvm;
            var item = Container.Items.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x.Input) == fileName);
            if (item != null)
            {
                mvm = new MessagesViewModel(Project, Container, item);
            }
            else
            {
                mvm = null;
                Log.Warning($"Unable to find file {fileName} for messages management");
            }
            return mvm;
        }
    }
}
