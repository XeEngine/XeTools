using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Modules;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.TileCollisionEditor.ViewModels
{
    public class CollisionTypeEditViewModel : BaseNotifyPropertyChanged
    {
        private readonly Effect[] EFFECTS = new Effect[]
        {
            new Effect()
			{
				Id = TileCollision.Effects.None,
				Name = "None",
                Description = "No effect applied.",
                HasParameter = false,
            },
            new Effect() {
				Id = TileCollision.Effects.Solid,
				Name = "Solid",
                Description = "Collision will be a rectangular solid, unshaped.",
                HasParameter = false,
            },
            new Effect()
			{
				Id = TileCollision.Effects.SolidShaped,
				Name = "Solid shaped",
                Description = "Collision is solid, but it has a custom shape.",
                HasParameter = false,
            },
            new Effect()
            {
                Id = TileCollision.Effects.LayerChangeAbsolute,
                Name = "Layer change absolute",
                Description = "The entity that touches the collision will change the draw layer by the specified parameter.",
                ParameterName = "New layer index",
                HasParameter = true,
                MinimumValue = -127,
                MaximumValue = +127,
                ParameterStringFunc = x => $"change draw layer to {x}"
            },
            new Effect()
            {
                Id = TileCollision.Effects.LayerChangeRelative,
                Name = "Layer change relative",
                Description = "The entity that touches the collision will move the draw layer by the specified parameter.",
                ParameterName = "Move layer by",
                HasParameter = true,
                MinimumValue = -127,
                MaximumValue = +127,
                ParameterStringFunc = x => $"change draw layer by {x.ToString("+00;-00;+00")}"
            },
            new Effect()
            {
                Id = TileCollision.Effects.DepthChangeAbsolute,
                Name = "Depth change absolute",
                Description = "The entity that touches the collision will change the depth by the specified parameter.",
                ParameterName = "New depth value",
                HasParameter = true,
                MinimumValue = -127,
                MaximumValue = +127,
                ParameterStringFunc = x => $"change depth to {x}"
            },
			new Effect()
			{
				Id = TileCollision.Effects.DepthChangeRelative,
				Name = "Depth change relative",
				Description = "The entity that touches the collision will move the depth by the specified parameter.",
				ParameterName = "Move depth by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeAbsoluteDepthLS,
				Name = "Change draw layer absolute on depth less",
				Description = "The entity that touches the collision will change the draw layer if destination depth is less than the current one (for example, if entity is falling due to gravity).",
				ParameterName = "New layer index",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change draw layer to {x}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeAbsoluteDepthLE,
				Name = "Change draw layer absolute on depth less or equal",
				Description = "The entity that touches the collision will change the draw layer if destination depth is less or equal than the current one.",
				ParameterName = "New layer index",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change draw layer to {x}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeAbsoluteDepthGT,
				Name = "Change draw layer absolute on depth greater",
				Description = "The entity that touches the collision will change the draw layer if destination depth is greater than the current one.",
				ParameterName = "New layer index",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change draw layer to {x}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeAbsoluteDepthGE,
				Name = "Change draw layer absolute on depth greater or equal",
				Description = "The entity that touches the collision will change the draw layer if destination depth is greater or equal than the current one.",
				ParameterName = "New layer index",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change draw layer to {x}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeRelativeDepthLS,
				Name = "Change draw layer relative on depth less",
				Description = "The entity that touches the collision will move the draw layer if destination depth is less than the current one (for example if entity is falling due to gravity).",
				ParameterName = "Move layer by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeRelativeDepthLE,
				Name = "Change draw layer relative on depth less or equal",
				Description = "The entity that touches the collision will move the draw layer if destination depth is less or equal than the current one.",
				ParameterName = "Move layer by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeRelativeDepthGT,
				Name = "Change draw layer relative on depth greater",
				Description = "The entity that touches the collision will move the draw layer if destination depth is greater than the current one.",
				ParameterName = "Move layer by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeRelativeDepthGE,
				Name = "Change draw layer relative on depth greater or equal",
				Description = "The entity that touches the collision will move the draw layer if destination depth is greater or equal than the current one.",
				ParameterName = "Move layer by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeAbsoluteDepthReach,
				Name = "Change draw layer absolute on depth reach",
				Description = "The entity that touches the collision will change the draw layer only if destination depth is reached by the entity (for example when it ends to fall, touching the ground).",
				ParameterName = "New layer index",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change draw layer to {x}"
			},
			new Effect()
			{
				Id = TileCollision.Effects.LayerChangeRelativeDepthReach,
				Name = "Change draw layer relative on depth reach",
				Description = "The entity that touches the collision will move the draw layer only if destination depth is reached by the entity (for example when it ends to fall, touching the ground).",
				ParameterName = "Move layer by",
				HasParameter = true,
				MinimumValue = -127,
				MaximumValue = +127,
				ParameterStringFunc = x => $"change depth by {x.ToString("+00;-00;+00")}"
			},
			new Effect()
            {
                Id = TileCollision.Effects.Climb,
                Name = "Climb",
                Description = "Describe a climb, specifying a moving angle. Depth value will be affected.",
                ParameterName = "Angle",
                HasParameter = true,
                MinimumValue = 0,
                MaximumValue = 255,
                ParameterStringFunc = x => $"{(x / 256.0 * 360.0).ToString("0.##")}°"
            },
            new Effect()
            {
                Id = TileCollision.Effects.WalkEffect,
                Name = "Walk effect",
                Description = "Specify what kind of terrain the collision is",
                HasParameter = true,
                Parameters = new EffectParameter[]
                {
                    new EffectParameter() { Id = TileCollision.Walks.Default, Name = "Default" },
                    new EffectParameter() { Id = TileCollision.Walks.Grass, Name = "Grass" },
                    new EffectParameter() { Id = TileCollision.Walks.Wood, Name = "Wood" },
                    new EffectParameter() { Id = TileCollision.Walks.LowWater, Name = "Low water" },
                    new EffectParameter() { Id = TileCollision.Walks.Snow, Name = "Snow" },
                    new EffectParameter() { Id = TileCollision.Walks.Ice, Name = "Ice" },
                }
            },
            new Effect()
            {
                Id = TileCollision.Effects.Behavior,
                Name = "Behavior",
                Description = "Specify what happens on entity that touches the collision",
                HasParameter = true,
                Parameters = new EffectParameter[]
                {
                    new EffectParameter() { Id = TileCollision.Behaviors.None, Name = "Default" },
                    new EffectParameter() { Id = TileCollision.Behaviors.Damage005, Name = "Damage x0.05" },
                    new EffectParameter() { Id = TileCollision.Behaviors.Damage010, Name = "Damage x0.10" },
                    new EffectParameter() { Id = TileCollision.Behaviors.HurtNoDamage, Name = "Hurt but no damage" },
                    new EffectParameter() { Id = TileCollision.Behaviors.Slowdown, Name = "Slowdown" },
                    new EffectParameter() { Id = TileCollision.Behaviors.Skid, Name = "Skid" },
                }
            },
        };

        public CollisionType CollisionType { get; }

        public IEnumerable<Effect> Effects => EFFECTS;

        public Guid Id => CollisionType?.Id ?? Guid.Empty;

        public string Name
        {
            get => CollisionType?.Name;
            set
            {
                CollisionType.Name = value;
                OnPropertyChanged();
            }
        }

        public Guid Effect
        {
            get => CollisionType?.Effect ?? Guid.Empty;
            set
            {
                CollisionType.Effect = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedEffect));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(HasParameters));
                OnPropertyChanged(nameof(IntegerParameterVisibility));
                OnPropertyChanged(nameof(ListParameterVisibility));
                OnPropertyChanged(nameof(ParameterValue));
                OnPropertyChanged(nameof(MinimumValue));
                OnPropertyChanged(nameof(MaximumValue));
                OnPropertyChanged(nameof(ParameterDescription));
                OnPropertyChanged(nameof(ParameterList));
                OnPropertyChanged(nameof(ParameterValueId));
            }
        }

        public Effect SelectedEffect => EFFECTS.FirstOrDefault(x => x.Id == Effect);

        public string Description => SelectedEffect?.Description;

        public string ParameterName => SelectedEffect?.ParameterName;

        public Visibility HasParameters => (SelectedEffect?.HasParameter ?? true) ? Visibility.Visible : Visibility.Collapsed;

        public Visibility IntegerParameterVisibility => SelectedEffect?.Parameters == null ? Visibility.Visible : Visibility.Collapsed;

        public Visibility ListParameterVisibility => SelectedEffect?.Parameters != null ? Visibility.Visible : Visibility.Collapsed;

        public int ParameterValue
        {
            get => CollisionType?.EffectParamValue ?? 0;
            set
            {
                CollisionType.EffectParamValue = value;
                OnPropertyChanged(nameof(ParameterValue));
                OnPropertyChanged(nameof(ParameterDescription));
            }
        }

		// HACK
		public int MinimumValue => -127;//SelectedEffect?.MinimumValue ?? 0;

		// HACK
		public int MaximumValue => 127;// SelectedEffect?.MaximumValue ?? 0;

        public string ParameterDescription => SelectedEffect?.ParameterStringFunc?.Invoke(ParameterValue);

        public IEnumerable<EffectParameter> ParameterList => SelectedEffect?.Parameters;

        public Guid ParameterValueId
        {
            get => CollisionType?.EffectParamId ?? Guid.Empty;
            set
            {
                CollisionType.EffectParamId = value;
                OnPropertyChanged(nameof(ParameterValueId));
            }
        }


        public CollisionTypeEditViewModel(CollisionType collisionType)
        {
            CollisionType = collisionType;
        }
    }
}
