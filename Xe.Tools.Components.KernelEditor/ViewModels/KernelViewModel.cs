using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Game.Messages;
using Xe.Tools.Projects;
using Xe.Tools.Services;
using static Xe.Tools.Project;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class KernelViewModel
    {
        public ProjectService ProjectService { get; private set; }
        public MessageService MessageService { get; private set; }

        public IProjectFile ProjectFile { get; private set; }

        public KernelData Kernel { get; private set; }
        private string WorkingFileName { get; set; }
        private string BasePath { get => Path.GetDirectoryName(WorkingFileName); }

        public AnimationGroupsViewModel AnimationGroups { get; private set; }
        public TabSkillsViewModel Skills { get; private set; }
        public TabPlayersViewModel Players { get; private set; }
        public TabMessagesViewModel Messages { get; private set; }
		public TabBgm.TabBgmViewModel Bgms { get; private set; }
		public TabSfx.TabSfxViewModel Sfxs { get; private set; }

		public KernelViewModel(IProject project, IProjectFile file)
        {
            ProjectService = new ProjectService(project);
            MessageService = new MessageService(ProjectService);
            ProjectFile = file;

            WorkingFileName = file.FullPath;

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
                Log.Error($"Error while opening {ProjectFile.Path}: {e.Message}");
            }
            
            Skills = new TabSkillsViewModel(Kernel.Skills, MessageService, ProjectService.AnimationService);
            Players = new TabPlayersViewModel(Kernel.Players, Kernel.Skills, MessageService, ProjectService.AnimationService);
            Messages = new TabMessagesViewModel(MessageService);
			Bgms = new TabBgm.TabBgmViewModel(Kernel);
			Sfxs = new TabSfx.TabSfxViewModel(Kernel);

		}

        public void SaveChanges()
        {
            MessageService.SaveChanges();
            Skills.SaveChanges();
            Players.SaveChanges();
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
                Log.Error($"Error while saving changes on {ProjectFile.Path}: {e.Message}");
            }
        }
    }
}
