using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class PlayerViewModel
    {
        public Player Player { get; private set; }

        public MessagesViewModel Messages { get; private set; }

        public TabSkillsViewModel SkillsViewModel { get; private set; }

        public string Id
        {
            get => Player.Id;
            set => Player.Id = value;
        }

        public Guid NameId
        {
            get => Player.Name;
            set => Player.Name = value;
        }

        public Guid DescriptionId
        {
            get => Player.Description;
            set => Player.Description = value;
        }

        public string Name
        {
            get => Messages?[NameId]?.English ?? "NOMSG";
            set => Messages[NameId].English = value;
        }

        public string Description
        {
            get => Messages?[DescriptionId]?.English ?? "NOMSG";
            set => Messages[DescriptionId].English = value;
        }

        public bool Enabled
        {
            get => Player.Enabled;
            set => Player.Enabled = value;
        }

        public bool Locked
        {
            get => Player.Locked;
            set => Player.Locked = value;
        }

        public int Level
        {
            get => Player.Level;
            set => Player.Level = value;
        }

        public int Experience
        {
            get => Player.Level;
            set => Player.Level = value;
        }

        public int HealthCurrent
        {
            get => Player.HealthCurrent;
            set => Player.HealthCurrent = value;
        }

        public int ManaCurrent
        {
            get => Player.ManaCurrent;
            set => Player.ManaCurrent = value;
        }

        public ObservableCollection<SkillUsageViewModel> SkillsUsage { get; private set; }

        public IEnumerable<Skill> Skills { get; set; }

        public PlayerViewModel(Player player, IEnumerable<Skill> skills, MessagesViewModel messages)
        {
            Player = player;
            Messages = messages;
            Skills = skills;
        }

        public void SaveChanges()
        {
            Player.Skills = SkillsUsage
                .Select(x => x.SkillUsage)
                .ToList();
        }
    }
}
