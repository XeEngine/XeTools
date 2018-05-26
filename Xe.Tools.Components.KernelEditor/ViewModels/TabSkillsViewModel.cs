using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Game.Messages;
using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class TabSkillsViewModel : BaseNotifyPropertyChanged
    {
        private List<Skill> _skills;
        private Skill _selectedItem;
        private AnimationService _animationService;

        public MessageService MessageService { get; private set; }

        /// <summary>
        /// Bind to the ListBox
        /// </summary>
        public ObservableCollection<Skill> Skills { get; private set; }

        /// <summary>
        /// Used to display the skill's messages
        /// </summary>
        //public MessagesViewModel Messages { get; private set; }

        /// <summary>
        /// Used to display the animation files's list
        /// </summary>
        public IEnumerable<string> AnimationFileNames => _animationService.AnimationFilesData;

        /// <summary>
        /// Used to display the DamageFormuls enumeration values
        /// </summary>
        public EnumViewModel<DamageFormula> DamageFormulaType { get; private set; } = new EnumViewModel<DamageFormula>();

        /// <summary>
        /// Used to display the TargetType enumeration values
        /// </summary>
        public EnumViewModel<TargetType> TargetType { get; private set; } = new EnumViewModel<TargetType>();

        public Skill SelectedItem
        {
            get => _selectedItem ?? new Skill();
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(IsItemSelected));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(GfxName));
                OnPropertyChanged(nameof(GfxAnimation));
                OnPropertyChanged(nameof(Sfx));
                OnPropertyChanged(nameof(DamageFormula));
                OnPropertyChanged(nameof(Target));
                OnPropertyChanged(nameof(Damage));
            }
        }

        #region skill view

        public IEnumerable<string> Animations => _animationService.GetAnimationDefinitions(GfxName);

        public bool IsItemSelected => SelectedItem != null;

        public string Name => "WIP";

        public string Description => "WIP";


        public string GfxName
        {
            get => SelectedItem.GfxName;
            set
            {
                SelectedItem.GfxName = value;
                OnPropertyChanged(nameof(Animations));
            }
        }

        public string GfxAnimation
        {
            get => SelectedItem.GfxAnimation;
            set => SelectedItem.GfxAnimation = value;
        }

        public string Sfx
        {
            get => SelectedItem.Sfx;
            set => SelectedItem.Sfx = value;
        }

        public Tuple<DamageFormula, string> DamageFormula
        {
            get => new Tuple<DamageFormula, string>(SelectedItem.DamageFormula, SelectedItem.DamageFormula.ToString());
            set => SelectedItem.DamageFormula = value.Item1;
        }

        public Tuple<TargetType, string> Target
        {
            get => new Tuple<TargetType, string>(SelectedItem.Target, SelectedItem.Target.ToString());
            set => SelectedItem.Target = value.Item1;
        }

        public int Damage
        {
            get => SelectedItem.Damage;
            set => SelectedItem.Damage = value;
        }

        #endregion

        public TabSkillsViewModel(List<Skill> skills,
            MessageService messageService,
            AnimationService animationService)
        {
            _skills = skills;
            Skills = new ObservableCollection<Skill>(_skills);
            MessageService = messageService;
            _animationService = animationService;
        }

        public void Add()
        {
            Skills.Add(new Skill()
            {
                Name = "<new skill>"
            });
        }
        public void RemoveAt(int index)
        {
            if (index < 0) return;
            Skills.RemoveAt(index);
        }

        public void SaveChanges()
        {
            _skills.Clear();
            _skills.AddRange(Skills);
        }
    }
}
