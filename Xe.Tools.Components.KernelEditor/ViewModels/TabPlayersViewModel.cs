using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Kernel;
using Xe.Tools.Services;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.KernelEditor.ViewModels
{
    public class TabPlayersViewModel : BaseNotifyPropertyChanged
    {
        private List<Player> _playersList;

        private Player _player;

        public MessageService MessageService { get; private set; }

        public AnimationService AnimationService { get; private set; }

        public ObservableCollection<Player> Players { get; private set; }

        public IEnumerable<Skill> Skills { get; private set; }

        public IEnumerable<string> AnimationFileNames => AnimationService.AnimationFilesData;

        public TabPlayerSkillsUsage PlayerSkillUsage { get; private set; }

        public Player SelectedPlayer
        {
            get => _player;
            set
            {
                _player = value;
                PlayerSkillUsage.Player = value;
                PlayerSkillUsage.SkillNames = Skills.Select(x => x.Name);
                PlayerSkillUsage.CastAnimations = AnimationService.GetAnimationDefinitions(Animation);
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(Id));
                OnPropertyChanged(nameof(Animation));
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(Enabled));
                OnPropertyChanged(nameof(Locked));
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(Experience));
                OnPropertyChanged(nameof(HealthCurrent));
                OnPropertyChanged(nameof(ManaCurrent));
            }
        }

        public bool IsSelected => SelectedPlayer != null;

        public string Id
        {
            get => SelectedPlayer.Id;
            set => SelectedPlayer.Id = value;
        }

        public string Animation
        {
            get => _player?.Animation;
            set
            {
                _player.Animation = value;
                PlayerSkillUsage.CastAnimations = AnimationService.GetAnimationDefinitions(value);
            }
        }

        public string Name => SelectedPlayer != null ? MessageService.GetString(SelectedPlayer.Name) : "<null>";

        public string Description => SelectedPlayer != null ? MessageService.GetString(SelectedPlayer.Description) : "<null>";

        public bool Enabled
        {
            get => SelectedPlayer?.Enabled ?? false;
            set => SelectedPlayer.Enabled = value;
        }

        public bool Locked
        {
            get => SelectedPlayer?.Locked ?? false;
            set => SelectedPlayer.Locked = value;
        }

        public int Level
        {
            get => SelectedPlayer?.Level ?? 0;
            set => SelectedPlayer.Level = value;
        }

        public int Experience
        {
            get => SelectedPlayer?.Level ?? 0;
            set => SelectedPlayer.Level = value;
        }

        public int HealthCurrent
        {
            get => SelectedPlayer?.HealthCurrent ?? 0;
            set => SelectedPlayer.HealthCurrent = value;
        }

        public int ManaCurrent
        {
            get => SelectedPlayer?.ManaCurrent ?? 0;
            set => SelectedPlayer.ManaCurrent = value;
        }

        public int Health => 0;
        public int Mana => 0;
        public int Attack => 0;
        public int Defense => 0;
        public int AttackSpecial => 0;
        public int DefenseSpecial => 0;

        public ObservableCollection<SkillUsageViewModel> SkillsUsage { get; private set; }

        public TabPlayersViewModel(List<Player> players,
            IEnumerable<Skill> skills,
            MessageService messageService,
            AnimationService animationService)
        {
            MessageService = messageService;
            AnimationService = animationService;

            _playersList = players;
            Players = new ObservableCollection<Player>(players);
            Skills = skills;
            PlayerSkillUsage = new TabPlayerSkillsUsage()
            {
                SkillNames = Skills.Select(x => x.Name),
                CastAnimations = AnimationService.GetAnimationDefinitions(Animation)
            };
        }

        public void SaveChanges()
        {
            _playersList.Clear();
            _playersList.AddRange(Players);
        }
    }
}
