using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xe.Game.Kernel;
using Xe.Game.Messages;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class SkillViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Skill Skill { get; private set; }

        public MessagesViewModel Messages { get; private set; }

        public string Name
        {
            get => Skill.Name;
            set => Skill.Name = value;
        }

        public Guid MessageNameId
        {
            get => Skill.MsgName;
            set
            {
                if (Skill.MsgName != value)
                {
                    Skill.MsgName = value;
                    OnPropertyChanged(nameof(MessageName));
                }
            }
        }

        public Guid MessageDescriptionId
        {
            get => Skill.MsgDescription;
            set
            {
                if (Skill.MsgDescription != value)
                {
                    Skill.MsgDescription = value;
                    OnPropertyChanged(nameof(MessageDescription));
                }
            }
        }

        public string MessageName
        {
            get => Messages?[MessageNameId]?.English ?? "NOMSG";
            set => Messages[MessageNameId].English = value;
        }

        public string MessageDescription
        {
            get => Messages?[MessageDescriptionId]?.English ?? "NOMSG";
            set => Messages[MessageDescriptionId].English = value;
        }

        public string GfxName
        {
            get => Skill.GfxName;
            set => Skill.GfxName = value;
        }

        public string GfxAnimation
        {
            get => Skill.GfxAnimation;
            set => Skill.GfxAnimation = value;
        }

        public string Sfx
        {
            get => Skill.Sfx;
            set => Skill.Sfx = value;
        }

        public Tuple<DamageFormula, string> DamageFormula
        {
            get => new Tuple<DamageFormula, string>(Skill.DamageFormula, Skill.DamageFormula.ToString());
            set => Skill.DamageFormula = value.Item1;
        }

        public Tuple<TargetType, string> Target
        {
            get => new Tuple<TargetType, string>(Skill.Target, Skill.Target.ToString());
            set => Skill.Target = value.Item1;
        }

        public int Damage
        {
            get => Skill.Damage;
            set => Skill.Damage = value;
        }


        public SkillViewModel(Skill skill, MessagesViewModel messages)
        {
            Skill = skill;
            Messages = messages;
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
