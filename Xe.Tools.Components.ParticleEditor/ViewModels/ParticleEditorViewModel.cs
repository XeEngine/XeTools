using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Animations;
using Xe.Game.Particles;
using Xe.Tools.Components.ParticleEditor.Service;
using Xe.Tools.Services;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.ParticleEditor.ViewModels
{
    public class ParticleEditorViewModel : BaseNotifyPropertyChanged
	{
		private IController _controller;
		private ParticlesData _particlesData = new ParticlesData();
		private ParticleSystem _particleSystem;
		private ObservableCollection<ParticleGroup> _particleGroupsCollection
			= new ObservableCollection<ParticleGroup>();

		#region Basic properties

		public ProjectService ProjectService { get; }

		public AnimationService AnimationService { get; }

		public ParticleSystem ParticleSystem
		{
			get => _particleSystem;
			set
			{
				_particleSystem = value;
				_particleSystem.ParticleGroups = ParticleGroups;
				ParticlesData = _controller.ParticleData;
			}
		}

		public IEnumerable<string> AnimationDataList { get; }

		public AnimationData AnimationData { get; private set; }

		public ParticlesData ParticlesData
		{
			get => _particlesData;
			set
			{
				_particlesData = value;
				AnimationDataName = _particlesData.AnimationDataName;
				ParticleGroups = new ObservableCollection<ParticleGroup>(
					_particlesData.Groups
						.Select(x => new ParticleGroup(x)
						{
							AnimationDrawer = ParticleSystem.AnimationDrawer
						})
					);
			}
		}

		public string AnimationDataName
		{
			get => _particlesData?.AnimationDataName;
			set
			{
				_particlesData.AnimationDataName = value;
				if (ParticleSystem.LoadAnimation($"{value}.json"))
				{
					foreach (var particleGroup in ParticleGroups)
					{
						particleGroup.AnimationDrawer = ParticleSystem.AnimationDrawer;
					}
					AnimationNames = ParticleSystem.AnimationData.AnimationDefinitions.Select(x => x.Name);
					OnPropertyChanged(nameof(AnimationNames));
				}
			}
		}

		public double Timer
		{
			get => ParticleSystem.Timer;
			set
			{
				ParticleSystem.Timer = value;
				OnPropertyChanged();
			}
		}

		public double TimerTotal { get; private set; }

		public RelayCommand ResetTimerCommand { get; }

		#endregion

		#region Particle groups management

		private ParticleGroup _selectedParticleGroup;

		public ObservableCollection<ParticleGroup> ParticleGroups
		{
			get => _particleGroupsCollection;
			set
			{
				_particleGroupsCollection = value;
				if (_particleSystem != null)
				{
					_particleSystem.ParticleGroups = value;
				}
				OnPropertyChanged();
			}
		}

		public ParticleGroup SelectedParticleGroup
		{
			get => _selectedParticleGroup;
			set
			{
				_selectedParticleGroup = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(AnimationName));
				OnPropertyChanged(nameof(IsParticleGroupSelected));
				OnPropertyChanged(nameof(ParticlesCount));
				OnPropertyChanged(nameof(GlobalDelay));
				OnPropertyChanged(nameof(GlobalDuration));
				OnPropertyChanged(nameof(DelayBetweenParticles));
				_controller.RefreshEffectsList();
			}
		}

		public bool IsParticleGroupSelected => SelectedParticleGroup != null;

		public RelayCommand AddParticleGroup { get; set; }

		public RelayCommand RemoveParticleGroup { get; set; }

		public RelayCommand DuplicateParticleGroup { get; set; }

		#endregion

		#region Particle group effects management

		public IEnumerable<string> AnimationNames { get; private set; }

		public string AnimationName
		{
			get => SelectedParticleGroup?.AnimationName;
			set
			{
				SelectedParticleGroup.AnimationName = value;
				OnPropertyChanged();
			}
		}

		public int ParticlesCount
		{
			get => SelectedParticleGroup?.Count ?? 0;
			set
			{
				SelectedParticleGroup.Count = value;
				OnPropertyChanged();
			}
		}

		public double GlobalDelay
		{
			get => SelectedParticleGroup?.GlobalDelay ?? 0;
			set
			{
				SelectedParticleGroup.GlobalDelay = value;
				OnPropertyChanged();
			}
		}

		public double GlobalDuration
		{
			get => SelectedParticleGroup?.GlobalDuration ?? 0;
			set
			{
				SelectedParticleGroup.GlobalDuration = value;
				OnPropertyChanged();
			}
		}

		public double DelayBetweenParticles
		{
			get => SelectedParticleGroup?.DelayBetweenParticles ?? 0;
			set
			{
				SelectedParticleGroup.DelayBetweenParticles = value;
				OnPropertyChanged();
			}
		}

		#endregion

		public ParticleEditorViewModel(IController controller, ProjectService projectService)
		{
			_controller = controller;
			ProjectService = projectService;
			AnimationService = new AnimationService(ProjectService);
			AnimationDataList = AnimationService.AnimationFilesData;

			AddParticleGroup = new RelayCommand(x =>
			{
				var particlesGroup = new ParticlesGroup()
				{
					
				};
				_particlesData.Groups.Add(particlesGroup);

				ParticleGroups.Add(new ParticleGroup(particlesGroup)
				{
					AnimationDrawer = ParticleSystem.AnimationDrawer
				});
			}, x => true);

			RemoveParticleGroup = new RelayCommand(x =>
			{
				if (IsParticleGroupSelected)
				{
					_particlesData.Groups.Remove(SelectedParticleGroup.ParticlesGroup);
					ParticleGroups.Remove(SelectedParticleGroup);
				}
			}, x => true);

			DuplicateParticleGroup = new RelayCommand(x =>
			{
				if (!IsParticleGroupSelected)
					return;
				var cur = SelectedParticleGroup;
				var particlesGroup = new ParticlesGroup()
				{
					AnimationName = cur.AnimationName.Substring(0),
					 ParticlesCount = cur.Count,
					 GlobalDelay = cur.GlobalDelay,
					 GlobalDuration = cur.GlobalDuration,
					 Delay = cur.DelayBetweenParticles,
					 Effects = cur.Effects
						.Select(effect => new Effect
						{
							Ease = effect.Ease,
							Parameter = effect.Parameter,
							Speed = effect.Speed,
							FixStep = effect.FixStep,
							Sum = effect.Sum,
							Multiplier = effect.Multiplier,
							Delay = effect.Delay,
							Duration = effect.Duration
						})
						.ToList()
				};
				_particlesData.Groups.Add(particlesGroup);

				ParticleGroups.Add(new ParticleGroup(particlesGroup)
				{
					AnimationDrawer = ParticleSystem.AnimationDrawer
				});
			}, x => true);

			ResetTimerCommand = new RelayCommand(x =>
			{
				ParticleSystem.Timer = 0.0;
			}, x => true);
		}

		public void SetTimer(double timer)
		{
			TimerTotal = _particlesData.Groups.Max(x => x.GlobalDelay + x.GlobalDuration);
			Timer = Math.Min(timer, TimerTotal);
			OnPropertyChanged(nameof(TimerTotal));
		}

		public void SaveChanges()
		{
		}
	}
}
