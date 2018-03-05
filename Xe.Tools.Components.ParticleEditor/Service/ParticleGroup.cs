using System.Collections.Generic;
using Xe.Game.Drawing;
using Xe.Game.Particles;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.ParticleEditor.Service
{
	public class Parameters
	{
		public double X { get; set; } = 0.0;

		public double Y { get; set; } = 0.0;

		public double Z { get; set; } = 0.0;

		public double RotationX { get; set; } = 0.0;

		public double RotationY { get; set; } = 0.0;

		public double RotationZ { get; set; } = 0.0;

		public double ScaleX { get; set; } = 1.0;

		public double ScaleY { get; set; } = 1.0;

		public double ScaleZ { get; set; } = 1.0;

		public double CenterX { get; set; } = 0.0;

		public double CenterY { get; set; } = 0.0;

		public double CenterZ { get; set; } = 0.0;

		public double ColorRed { get; set; } = 1.0;

		public double ColorGreen { get; set; } = 1.0;

		public double ColorBlue { get; set; } = 1.0;

		public double ColorAlpha { get; set; } = 1.0;

		public void CopyFrom(Parameters parameters)
		{
			X = parameters.X;
			Y = parameters.Y;
			Z = parameters.Z;
			RotationX = parameters.RotationX;
			RotationY = parameters.RotationY;
			RotationZ = parameters.RotationZ;
			ScaleX = parameters.ScaleX;
			ScaleY = parameters.ScaleY;
			ScaleZ = parameters.ScaleZ;
			CenterX = parameters.CenterX;
			CenterY = parameters.CenterY;
			CenterZ = parameters.CenterZ;
			ColorRed = parameters.ColorRed;
			ColorGreen = parameters.ColorGreen;
			ColorBlue = parameters.ColorBlue;
			ColorAlpha = parameters.ColorAlpha;
		}

		public void SetValue(ParameterType parameterType, double value)
		{
			switch (parameterType)
			{
				case ParameterType.X:
					X = value;
					break;
				case ParameterType.Y:
					Y = value;
					break;
				case ParameterType.Z:
					Z = value;
					break;
				case ParameterType.RotationX:
					RotationX = value;
					break;
				case ParameterType.RotationY:
					RotationY = value;
					break;
				case ParameterType.RotationZ:
					RotationZ = value;
					break;
				case ParameterType.ScaleX:
					ScaleX = value;
					break;
				case ParameterType.ScaleY:
					ScaleY = value;
					break;
				case ParameterType.ScaleZ:
					ScaleZ = value;
					break;
				case ParameterType.CenterX:
					CenterX = value;
					break;
				case ParameterType.CenterY:
					CenterY = value;
					break;
				case ParameterType.CenterZ:
					CenterZ = value;
					break;
				case ParameterType.ColorRed:
					ColorRed = value;
					break;
				case ParameterType.ColorGreen:
					ColorGreen = value;
					break;
				case ParameterType.ColorBlue:
					ColorBlue = value;
					break;
				case ParameterType.ColorAlpha:
					ColorAlpha = value;
					break;
				case ParameterType.ScaleXYZ:
					ScaleX = value;
					ScaleY = value;
					ScaleZ = value;
					break;
				case ParameterType.ColorRGB:
					ColorRed = value;
					ColorGreen = value;
					ColorBlue = value;
					break;
			}
		}

		public void AddValue(ParameterType parameterType, double value)
		{
			switch (parameterType)
			{
				case ParameterType.X:
					X += value;
					break;
				case ParameterType.Y:
					Y += value;
					break;
				case ParameterType.Z:
					Z += value;
					break;
				case ParameterType.RotationX:
					RotationX += value;
					break;
				case ParameterType.RotationY:
					RotationY += value;
					break;
				case ParameterType.RotationZ:
					RotationZ += value;
					break;
				case ParameterType.ScaleX:
					ScaleX += value;
					break;
				case ParameterType.ScaleY:
					ScaleY += value;
					break;
				case ParameterType.ScaleZ:
					ScaleZ += value;
					break;
				case ParameterType.CenterX:
					CenterX += value;
					break;
				case ParameterType.CenterY:
					CenterY += value;
					break;
				case ParameterType.CenterZ:
					CenterZ += value;
					break;
				case ParameterType.ColorRed:
					ColorRed += value;
					break;
				case ParameterType.ColorGreen:
					ColorGreen += value;
					break;
				case ParameterType.ColorBlue:
					ColorBlue += value;
					break;
				case ParameterType.ColorAlpha:
					ColorAlpha += value;
					break;
				case ParameterType.ScaleXYZ:
					ScaleX += value;
					ScaleY += value;
					ScaleZ += value;
					break;
				case ParameterType.ColorRGB:
					ColorRed += value;
					ColorGreen += value;
					ColorBlue += value;
					break;
			}
		}
	}

	public class ParticleGroup : BaseNotifyPropertyChanged
	{
		public readonly Game.Particles.ParticlesGroup ParticlesGroup;
		private double _timer;
		private AnimationDrawer _animationDrawer;
		private List<ParticleEntity> _particleEntities = new List<ParticleEntity>();

		public ParticleGroup(Game.Particles.ParticlesGroup particlesGroup)
		{
			ParticlesGroup = particlesGroup;
			ParticlesGroup.Effects = ParticlesGroup.Effects ?? new List<Effect>();
			Count = ParticlesGroup.ParticlesCount;
		}

		public AnimationDrawer AnimationDrawer
		{
			get => _animationDrawer;
			set
			{
				_animationDrawer = value;
				if (_animationDrawer != null)
				{
					foreach (var item in _particleEntities)
					{
						item.EntityDrawer = _animationDrawer.CreateEntity();
						item.EntityDrawer.SetAnimation(AnimationName, Game.Direction.Undefined);
					}
				}
			}
		}

		public double Timer
		{
			get => _timer;
			set
			{
				_timer = value;
				Update();
			}
		}

		/// <summary>
		/// Name of the animation loaded, with Unspecified as direction
		/// </summary>
		public string AnimationName
		{
			get => ParticlesGroup.AnimationName ?? "<no anim>";
			set
			{
				ParticlesGroup.AnimationName = value;
				foreach (var item in _particleEntities)
				{
					item.EntityDrawer?.SetAnimation(AnimationName, Game.Direction.Undefined);
				}
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// Particles count
		/// </summary>
		public int Count
		{
			get => _particleEntities.Count;
			set
			{
				if (value > Count)
				{
					var itemsToAdd = value - Count;
					for (int i = 0; i < itemsToAdd; i++)
					{
						var entity = new ParticleEntity
						{
							EntityDrawer = AnimationDrawer?.CreateEntity()
						};
						if (entity.EntityDrawer != null)
						{
							entity.EntityDrawer.SetAnimation(AnimationName, Game.Direction.Undefined);
						}
						_particleEntities.Add(entity);
					}
				}
				else if (value < Count)
				{
					var itemsToRemove = Count - value;
					_particleEntities.RemoveRange(value, itemsToRemove);
				}
				ParticlesGroup.ParticlesCount = value;
			}
		}

		/// <summary>
		/// Delay for all particles
		/// </summary>
		public double GlobalDelay
		{
			get => ParticlesGroup.GlobalDelay;
			set => ParticlesGroup.GlobalDelay = value;
		}

		/// <summary>
		/// Duration timer for all particles
		/// </summary>
		public double GlobalDuration
		{
			get => ParticlesGroup.GlobalDuration;
			set => ParticlesGroup.GlobalDuration = value;
		}

		/// <summary>
		/// Delay between particles
		/// </summary>
		public double DelayBetweenParticles
		{
			get => ParticlesGroup.Delay;
			set => ParticlesGroup.Delay = value;
		}

		/// <summary>
		/// List of particle entities
		/// </summary>
		public IEnumerable<ParticleEntity> Entities => _particleEntities;

		/// <summary>
		/// List of effects
		/// </summary>
		public List<Effect> Effects => ParticlesGroup.Effects;
		
		private void Update()
		{
			var realTimer = Timer - GlobalDelay;
			if (realTimer > GlobalDuration)
				_timer = GlobalDuration;
			if (realTimer >= 0)
			{
				foreach (var entity in _particleEntities)
				{
					var parameters = new Parameters();
					foreach (var effect in Effects)
					{
						var effectTimer = realTimer - effect.Delay;
						entity.EntityDrawer.Timer = effectTimer;
						if (effectTimer > 0)
						{
							if (effectTimer > effect.Duration)
								effectTimer = effect.Duration;
							parameters.AddValue(effect.Parameter, effect.Get(effectTimer));
						}
					}
					entity.Parameters.CopyFrom(parameters);
					realTimer -= DelayBetweenParticles;
				}
			}
		}
	}

	public class ParticleEntity
	{
		public AnimationEntityDrawer EntityDrawer { get; set; }

		public Parameters Parameters { get; } = new Parameters();

		public double Priority => Parameters.Y;

		public void Draw(double x, double y)
		{
			EntityDrawer?.Draw(x + Parameters.X, y + Parameters.Y - Parameters.Z);
		}
	}
}
