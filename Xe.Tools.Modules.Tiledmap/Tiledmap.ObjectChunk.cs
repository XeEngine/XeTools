using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Security;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private static string WriteObjectsChunk(Map tileMap, BinaryWriter w)
        {
            var animDictionary = new Dictionary<string, int>();

            var objects = tileMap.Layers
                .Where(x => x is LayerObjects)
                .Select(x => x as LayerObjects)
                .SelectMany(x => x.Objects, (x, o) => new
                {
                    Layer = x,
                    Object = o
                });


            w.Write((short)objects.Count());
            var headerPos = w.BaseStream.Position;
            w.Write((short)0); // Write it later
            w.Write((uint)0); // Write it later
            foreach (var entry in objects)
            {
                var o = entry.Object;

                int flags = 0;
                AddFlag(flags, 0, o.Visible);
                AddFlag(flags, 1, o.HasShadow);
                AddFlag(flags, 2, (int)o.Flip);
                AddFlag(flags, 4, (int)o.Direction);

                w.Write(Crc32.CalculateDigestAscii(o.Name));
                w.Write(Crc32.CalculateDigestAscii(o.Type));
                w.Write((short)o.X);
                w.Write((short)o.Y);
                w.Write((short)o.Z);
                w.Write((short)o.Width);
                w.Write((short)o.Height);
                w.Write((byte)0); // depth
                w.Write((byte)flags);
                w.Write((byte)entry.Layer.Priority);
                w.Write((byte)0); // RESERVED

                w.Write((uint)CheckEntry(animDictionary, o.AnimationData)); // To replace with Crc32
                w.Write(Crc32.CalculateDigestAscii(o.AnimationName));
            }

            var animationsPos = w.BaseStream.Position;
            var count = animDictionary.Count;
            w.BaseStream.Position = w.BaseStream.Position += count * 4;
            var pointersList = new List<int>(count);
            foreach (var item in animDictionary
                .OrderBy(x => x.Value)
                .Select(x => x.Key))
            {
                pointersList.Add((int)w.BaseStream.Position);
                w.Write(System.Text.Encoding.UTF8.GetBytes(item));
            }
            w.BaseStream.Position = headerPos;
            w.Write((short)count);
            w.Write((uint)animationsPos);

            return "OBJ\x01";
        }

        private static int CheckEntry(Dictionary<string, int> dictionary, string key)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;
            value = dictionary.Count + 1;
            dictionary.Add(key, value);
            return value;
        }
        private static int AddFlag(int data, int shift, bool value)
        {
            return value ? (data |= (1 << shift)) : (data &= ~(1 << shift));
        }
        private static int AddFlag(int data, int shift, int value)
        {
            return data |= (1 << shift);
        }
    }
}
