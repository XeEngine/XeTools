using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xe.Game.Animations;
using Xe.Tools.Components.ParticleEditor.Service;
using Xe.Tools.Services;
using Xe.Tools.Wpf;
using Xe.Tools.Wpf.Commands;

namespace Xe.Tools.Components.ParticleEditor.ViewModels
{
    public class ParticleEditorViewModel : BaseNotifyPropertyChanged
	{
		private string _animationDataName;
		private ParticleSystem _particleSystem;

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
			}
		}

		public IEnumerable<string> AnimationDataList { get; }

		public AnimationData AnimationData { get; private set; }

		public string AnimationDataName
		{
			get => _animationDataName;
			set
			{
				_animationDataName = value;
				if (ParticleSystem.LoadAnimation($"{_animationDataName}.json"))
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

		public RelayCommand ResetTimerCommand { get; }

		#endregion

		#region Particle groups management

		private ParticleGroup _selectedParticleGroup;

		public ObservableCollection<ParticleGroup> ParticleGroups { get; set; } = new ObservableCollection<ParticleGroup>();

		public ParticleGroup SelectedParticleGroup
		{
			get => _selectedParticleGroup;
			set
			{
				_selectedParticleGroup = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(IsParticleGroupSelected));
			}
		}

		public bool IsParticleGroupSelected => SelectedParticleGroup != null;

		public RelayCommand AddParticleGroup { get; set; }

		public RelayCommand RemoveParticleGroup { get; set; }

		#endregion

		#region Particle gruop effects management

		public IEnumerable<string> AnimationNames { get; private set; }

		public string AnimationName
		{
			get => SelectedParticleGroup?.AnimationName;
			set => SelectedParticleGroup.AnimationName = value;
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

		public ParticleEditorViewModel(ProjectService projectService)
		{
			ProjectService = projectService;
			AnimationService = new AnimationService(ProjectService);
			AnimationDataList = AnimationService.AnimationFilesData;

			AddParticleGroup = new RelayCommand(x =>
			{
				ParticleGroups.Add(new ParticleGroup()
				{
					AnimationDrawer = ParticleSystem.AnimationDrawer
				});
			}, x => true);

			RemoveParticleGroup = new RelayCommand(x =>
			{
				if (IsParticleGroupSelected)
					ParticleGroups.Remove(SelectedParticleGroup);
			}, x => true);

			ResetTimerCommand = new RelayCommand(x =>
			{
				ParticleSystem.Timer = 0.0;
			}, x => true);
		}
	}
}
