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

		public ZonesModel Zones { get; private set; }
		public BgmsModel Bgms { get; private set; }
		public SfxsModel Sfxs { get; private set; }
		public ElementsModel Elements { get; private set; }
		public StatusesModel Statuses { get; set; }
		public InventoryEntriesModel Inventory { get; set; }
		public SkillsModel Skills { get; private set; }

        public PlayersModel Actors { get; private set; }

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
                    if (Kernel.Actors == null) Kernel.Actors = new List<Actor>();

                    Log.Message($"Kernel file {WorkingFileName} opened.");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error while opening {ProjectFile.Path}: {e.Message}");
            }

			Zones = new ZonesModel(Kernel?.Zones?.Select(x => new ZoneModel(x, MessageService)), MessageService);
			Bgms = new BgmsModel(Kernel?.Bgms?.Select(x => new BgmModel(x)));
			Sfxs = new SfxsModel(Kernel?.Sfxs?.Select(x => new SfxModel(x)));
			Elements = new ElementsModel(Kernel.Elements, MessageService);
			Statuses = new StatusesModel(Kernel.Status, MessageService);
			Inventory = new InventoryEntriesModel(Kernel.InventoryItems, Kernel, MessageService);
			Skills = new SkillsModel(Kernel.Skills, Elements, Statuses, MessageService, ProjectService.AnimationService);
            Actors = new PlayersModel(Kernel.Actors, MessageService, ProjectService.AnimationService);
		}

        public void SaveChanges()
        {
			Kernel.Zones = Zones.Items.Select(x => x.Item).ToList();
			Kernel.Bgms = Bgms.Items.Select(x =>
			{
				x.Item.Loops = x.Loops.ToList();
				x.Item.Starts = x.Starts.ToList();
				return x.Item;
			}).ToList();
			Kernel.Sfxs = Sfxs.Items.Select(x => x.Item).ToList();
			Kernel.Elements = Elements.Items.Select(x => x.Item).ToList();
			Kernel.InventoryItems = Inventory.Items.Select(x => x.Item).ToList();
			Kernel.Skills = Skills.Items.Select(x => x.Item).ToList();
			Kernel.Actors = Actors.Items.Select(x => x.Item).ToList();

			MessageService.SaveChanges();

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
