using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static Xe.Math;

namespace Xe.Game.Animations
{
    public class Animation : IDeepCloneable
    {
        private ushort speed;
        private byte loop, texture;

        [JsonIgnore]
        public string Name { get; set; }

        public Hitbox FieldHitbox { get; set; }

        public List<FrameRef> Frames { get; set; }

        public int Speed
        {
            get => speed;
            set => speed = (ushort)Range(value, ushort.MinValue, ushort.MaxValue);
        }

        public int Loop
        {
            get => loop;
            set => loop = (byte)Range(value, byte.MinValue, byte.MaxValue);
        }

        public int Texture
        {
            get => texture;
            set => texture = (byte)Range(value, byte.MinValue, byte.MaxValue);
        }

        public object DeepClone()
        {
            return new Animation()
            {
                Name = Name.Clone() as string,
                FieldHitbox = FieldHitbox.DeepClone() as Hitbox,
                Frames = Frames.Select(x => x.DeepClone() as FrameRef).ToList(),
                speed = speed,
                loop = loop,
                texture = texture
            };
        }
    }
}
