using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Game.Messages;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class SkillsViewModel
    {
        private List<Skill> _skills;

        public ObservableCollection<SkillViewModel> Skills { get; private set; }

        public MessagesViewModel Messages { get; private set; }

        public AnimationGroupsViewModel AnimationGroups { get; private set; }

        public EnumViewModel<DamageFormula> DamageFormula { get; private set; } = new EnumViewModel<DamageFormula>();

        public EnumViewModel<TargetType> TargetType { get; private set; } = new EnumViewModel<TargetType>();

        public SkillsViewModel(List<Skill> skills,
            MessagesViewModel messages,
            AnimationGroupsViewModel animationGroups)
        {
            _skills = skills;
            Skills = new ObservableCollection<SkillViewModel>(_skills.Select(x => new SkillViewModel(x, messages)));
            Messages = messages;
            AnimationGroups = animationGroups;
        }

        public void Add()
        {
            Skills.Add(new SkillViewModel(new Skill()
            {
                Name = "<new skill>"
            }, Messages));
        }
        public void RemoveAt(int index)
        {
            if (index < 0) return;
            Skills.RemoveAt(index);
        }

        public void SaveChanges()
        {
            _skills.Clear();
            _skills.AddRange(Skills.Select(x => x.Skill));
        }
    }
}
