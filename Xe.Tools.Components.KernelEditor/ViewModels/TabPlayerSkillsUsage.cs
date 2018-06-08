using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class TabPlayerSkillsUsage : BaseNotifyPropertyChanged
    {
        private Actor _player;
        private SkillUsage _skillUsage;
        private IEnumerable<string> _skillNames;
        private IEnumerable<string> _castAnimations;

        public Actor Player
        {
            get => _player;
            set
            {
                _player = value;
                if (_player.Skills == null)
                {
                    _player.Skills = new List<SkillUsage>(32);
                    for (int i = 0; i < 32; i++)
                    {
                        _player.Skills.Add(new SkillUsage()
                        {
                            Skill = "<new skill usage>"
                        });
                    }
                }
                OnPropertyChanged(nameof(CastAnimations));
                OnPropertyChanged(nameof(Skills));
            }
        }

        public IEnumerable<string> SkillNames
        {
            get => _skillNames;
            set
            {
                _skillNames = value;
                OnPropertyChanged(nameof(SkillNames));
            }
        }

        public IEnumerable<string> CastAnimations
        {
            get => _castAnimations;
            set
            {
                _castAnimations = value;
                OnPropertyChanged(nameof(CastAnimations));
            }
        }

        public IEnumerable<SkillUsage> Skills => _player?.Skills ?? new List<SkillUsage>();

        public SkillUsage SelectedSkill
        {
            get => _skillUsage;
            set
            {
                _skillUsage = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(Skill));
                OnPropertyChanged(nameof(Animation));
                OnPropertyChanged(nameof(Enabled));
                OnPropertyChanged(nameof(Visible));
            }
        }

        public bool IsSelected => SelectedSkill != null;

        public string Skill
        {
            get => SelectedSkill?.Skill;
            set => SelectedSkill.Skill = value;
        }

        public string Animation
        {
            get => SelectedSkill?.Animation;
            set => SelectedSkill.Animation = value;
        }

        public bool Enabled
        {
            get => SelectedSkill?.Enabled ?? false;
            set => SelectedSkill.Enabled = value;
        }

        public bool Visible
        {
            get => SelectedSkill?.Visible ?? false;
            set => SelectedSkill.Visible = value;
        }
    }
}
