using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Xe.Tools.Components.TileCollisionEditor.Models;
using Xe.Tools.Wpf;

namespace Xe.Tools.Components.TileCollisionEditor.ViewModels
{
    public class CollisionTypeEditViewModel : BaseNotifyPropertyChanged
    {
        private readonly Effect[] EFFECTS = new Effect[]
        {
            new Effect()
            {
                Id = Guid.Empty,
                Name = "None",
                Description = "No effect applied.",
                HasParameter = false,
            },
            new Effect() {
                Id = new Guid(0x4a986433, 0x9311, 0x41bb, 0x8b, 0x23, 0xe1, 0x42, 0xed, 0xfd, 0x90, 0xa3),
                Name = "Solid",
                Description = "Collision will be a rectangular solid, unshaped.",
                HasParameter = false,
            },
            new Effect()
            {
                Id = new Guid(0x82877ad2, 0xfe94, 0x4223, 0x93, 0x44, 0x8d, 0xea, 0x6d, 0x09, 0xf3, 0xfc),
                Name = "Solid shaped",
                Description = "Collision has a custom shape.",
                HasParameter = false,
            },
            new Effect()
            {
                Id = new Guid(0x3a9f7524, 0xe1d2, 0x49c2, 0x9a, 0xbf, 0xf3, 0x5b, 0x75, 0x9a, 0x3b, 0x2e),
                Name = "Layer change absolute",
                Description = "The entity that will touch the collision will change the layer by the specified parameter.",
                ParameterName = "New layer index",
                HasParameter = true,
                MinimumValue = -127,
                MaximumValue = +127,
                ParameterStringFunc = x => $"layer = {x}"
            },
            new Effect()
            {
                Id = new Guid(0x423152d2, 0xcd10, 0x4e57, 0x9a, 0x5d, 0x64, 0x38, 0xcc, 0x01, 0xbf, 0x26),
                Name = "Layer change relative",
                Description = "The entity that will touch the collision will move the layer by the specified parameter.",
                ParameterName = "Move layer by",
                HasParameter = true,
                MinimumValue = -16,
                MaximumValue = +15,
                ParameterStringFunc = x => $"layer {x.ToString("+00;-00;+00")}"
            },
            new Effect()
            {
                Id = new Guid(0xc2485aa3, 0x32a9, 0x40ee, 0xa5, 0xc0, 0x43, 0x12, 0x50, 0xe9, 0xd1, 0x1a),
                Name = "Depth change absolute",
                Description = "The entity that will touch the collision will change the depth by the specified parameter.",
                ParameterName = "New depth value",
                HasParameter = true,
                MinimumValue = -127,
                MaximumValue = +127,
                ParameterStringFunc = x => $"depth = {x}"
            },
            new Effect()
            {
                Id = new Guid(0x15e1086a, 0xe205, 0x4745, 0x8f, 0xb0, 0x11, 0x52, 0x56, 0xc7, 0xc2, 0x7f),
                Name = "Depth change relative",
                Description = "The entity that will touch the collision will move the depth by the specified parameter.",
                ParameterName = "Move depth by",
                HasParameter = true,
                MinimumValue = -16,
                MaximumValue = +15,
                ParameterStringFunc = x => $"depth {x.ToString("+00;-00;+00")}"
            },
            new Effect()
            {
                Id = new Guid(0x4c84e981, 0xcc4e, 0x41e9, 0x95, 0x5b, 0xd4, 0x34, 0xa1, 0x3d, 0xc0, 0x7d),
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
                Id = new Guid(0x280f67c3, 0x6446, 0x4b53, 0xa0, 0x93, 0xdc, 0xfd, 0xd2, 0xff, 0x0d, 0x77),
                Name = "Walk effect",
                Description = "Specify what kind of terrain the collision is",
                HasParameter = true,
                Parameters = new EffectParameter[]
                {
                    new EffectParameter() { Id = Guid.Empty, Name = "Default" },
                    new EffectParameter() { Id = new Guid(0x9181e583, 0x2522, 0x4d60, 0xae, 0xc9, 0xf8, 0xe4, 0xe0, 0x82, 0xab, 0x11), Name = "Grass" },
                    new EffectParameter() { Id = new Guid(0x867c248a, 0xb753, 0x46a7, 0x99, 0x33, 0xf7, 0x8c, 0xb1, 0x5c, 0x27, 0xbd), Name = "Wood" },
                    new EffectParameter() { Id = new Guid(0x41a1afe8, 0x7d86, 0x47a9, 0x9f, 0xbe, 0x30, 0x85, 0x9d, 0xfa, 0x0a, 0x33), Name = "Low water" },
                    new EffectParameter() { Id = new Guid(0x121a2020, 0xe4b3, 0x44a6, 0x83, 0x7c, 0x10, 0x76, 0x85, 0x18, 0x8b, 0xc5), Name = "Snow" },
                    new EffectParameter() { Id = new Guid(0xa9c286f1, 0x6097, 0x4645, 0xbe, 0xc9, 0x86, 0xe7, 0x22, 0x18, 0x3f, 0x33), Name = "Ice" },
                }
            },
            new Effect()
            {
                Id = new Guid(0x74f93026, 0x025b, 0x490e, 0xa6, 0xd9, 0xb5, 0xd8, 0xcc, 0xc1, 0x5d, 0x8b),
                Name = "Behavior",
                Description = "Specify what happens on entity that touches the collision",
                HasParameter = true,
                Parameters = new EffectParameter[]
                {
                    new EffectParameter() { Id = Guid.Empty, Name = "Default" },
                    new EffectParameter() { Id = new Guid(0x12b45365, 0x8d60, 0x4a7e, 0xbc, 0x07, 0xe1, 0x45, 0x5c, 0xcf, 0x1f, 0x2c), Name = "Damage x0.05" },
                    new EffectParameter() { Id = new Guid(0x2742ff15, 0xba99, 0x45eb, 0xbb, 0x0b, 0xb0, 0xb2, 0x8d, 0xcd, 0xb4, 0xd3), Name = "Damage x0.10" },
                    new EffectParameter() { Id = new Guid(0xa4389de9, 0xcda3, 0x497d, 0xae, 0x67, 0x7d, 0x66, 0x2e, 0x26, 0x2b, 0xaf), Name = "Hurt but no damage" },
                    new EffectParameter() { Id = new Guid(0xbdee0b9e, 0x8215, 0x4333, 0x95, 0x0a, 0xfd, 0xba, 0xdf, 0x26, 0x9e, 0xb3), Name = "Slowdown" },
                    new EffectParameter() { Id = new Guid(0xfa5ad3cf, 0x4381, 0x49d7, 0x97, 0x61, 0x29, 0x1e, 0x71, 0xa3, 0x15, 0x62), Name = "Skid" },
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

        public int MinimumValue => SelectedEffect?.MinimumValue ?? 0;

        public int MaximumValue => SelectedEffect?.MaximumValue ?? 0;

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
