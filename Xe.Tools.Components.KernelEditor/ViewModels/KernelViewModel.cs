using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;
using Xe.Tools.Components.KernelEditor.Models;
using Xe.Tools.Projects;
using Xe.Tools.Services;

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

		public ZonesModel Zones { get; set; }
		public BgmsModel Bgms { get; set; }
		public SfxsModel Sfxs { get; set; }

		public AnimationGroupsViewModel AnimationGroups { get; private set; }
		public TabElements.TabElementViewModel Elements { get; private set; }
		public TabSkillsViewModel Skills { get; private set; }
        public TabPlayersViewModel Players { get; private set; }

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

			Elements = new TabElements.TabElementViewModel(Kernel);
			Skills = new TabSkillsViewModel(Kernel.Skills, MessageService, ProjectService.AnimationService);
            Players = new TabPlayersViewModel(Kernel.Players, Kernel.Skills, MessageService, ProjectService.AnimationService);
			Zones = new ZonesModel(Kernel?.Zones?.Select(x => new ZoneModel(x, MessageService)), MessageService);
			Bgms = new BgmsModel(Kernel?.Bgms?.Select(x => new BgmModel(x)));
			Sfxs = new SfxsModel(Kernel?.Sfxs?.Select(x => new SfxModel(x)));
		}

        public void SaveChanges()
        {
			Kernel.Zones = Zones.Items.Select(x => x.Zone).ToList();
			Kernel.Bgms = Bgms.Items.Select(x => x.Bgm).ToList();
			Kernel.Sfxs = Sfxs.Items.Select(x => x.Sfx).ToList();

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
