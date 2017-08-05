using Xe.Game.Kernel;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class SkillUsageViewModel
    {
        public SkillUsage SkillUsage { get; private set; }
        
        public string Animation
        {
            get => SkillUsage.Animation;
            set => SkillUsage.Animation = value;
        }

        public string Skill
        {
            get => SkillUsage.Skill;
            set => SkillUsage.Skill = value;
        }

        public bool Enabled
        {
            get => SkillUsage.Enabled;
            set => SkillUsage.Enabled = value;
        }

        public bool Visible
        {
            get => SkillUsage.Visible;
            set => SkillUsage.Visible = value;
        }

        public SkillUsageViewModel(SkillUsage skillUsage)
        {
            SkillUsage = skillUsage;
        }
    }
}
