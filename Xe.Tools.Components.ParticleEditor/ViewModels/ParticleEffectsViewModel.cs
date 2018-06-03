using Xe.Game.Particles;
using Xe.Tools.Components.ParticleEditor.Service;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.ParticleEditor.ViewModels
{
    public class ParticleEffectsViewModel : BaseNotifyPropertyChanged
    {
		public ParticleSystem ParticleSystem { get; }

		public ParticleGroup ParticleGroup { get; }

		public Effect Effect { get; }

		public EnumViewModel<Ease> AlgorithmTypes { get; } = new EnumViewModel<Ease>();

		public EnumViewModel<ParameterType> ParameterTypes { get; } = new EnumViewModel<ParameterType>();

		public ParameterType Parameter
		{
			get => Effect.Parameter;
			set
			{
				Effect.Parameter = value;
				OnPropertyChanged();
			}
		}

		public Ease Algorithm
		{
			get => Effect.Ease;
			set
			{
				Effect.Ease = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(EaseName));
			}
		}

		public double FixStep
		{
			get => Effect.FixStep;
			set
			{
				Effect.FixStep = value;
				OnPropertyChanged();
			}
		}

		public double Speed
		{
			get => Effect.Speed;
			set
			{
				Effect.Speed = value;
				OnPropertyChanged();
			}
		}

		public double Multiplier
		{
			get => Effect.Multiplier;
			set
			{
				Effect.Multiplier = value;
				OnPropertyChanged();
			}
		}

		public double Delay
		{
			get => Effect.Delay;
			set
			{
				Effect.Delay = value;
				OnPropertyChanged();
			}
		}

		public double Duration
		{
			get => Effect.Duration;
			set
			{
				Effect.Duration = value;
				OnPropertyChanged();
			}
		}

		public string ParameterName => Parameter.ToString();

		public string EaseName => Algorithm.ToString();

		public ParticleEffectsViewModel(ParticleSystem particleSystem, ParticleGroup particleGroup, Effect effect)
		{
			ParticleSystem = particleSystem;
			ParticleGroup = particleGroup;
			Effect = effect;
		}
    }
}
