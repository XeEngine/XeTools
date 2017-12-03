﻿using System.Collections.Generic;
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
                .FlatterLayers<LayerObjects>()
                .SelectMany(x => x.Objects, (x, o) => new
                {
                    Layer = x,
                    Object = o
                });
            var objectsCount = objects.Count();
            if (objectsCount == 0)
                return null;

            w.Write((short)objectsCount);
            var headerPos = w.BaseStream.Position;
            w.Write((short)0); // RESERVED
            w.Write((uint)0); // RESERVED
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
                w.Write((short)0); // RESERVED
                w.Write((short)o.Width);
                w.Write((short)o.Height);
                w.Write((byte)0); // depth
                w.Write((byte)flags);
                w.Write((byte)entry.Layer.Priority);
                w.Write((byte)0); // RESERVED

                w.Write((uint)CheckEntry(animDictionary, o.AnimationData)); // To replace with Crc32
                w.Write(Crc32.CalculateDigestAscii(o.AnimationName));
            }

            // Write animation names
            var animationNames = animDictionary
                .OrderBy(x => x.Value)
                .Select(x => new
                {
                    Name = x.Key,
                    Data = System.Text.Encoding.UTF8.GetBytes(x.Key)
                })
                .Select(x => new
                {
                    Name = x.Name,
                    Data = x.Data,
                    Length = x.Data.Length
                });
            var animationNamesLength = animationNames.Sum(x => x.Length + 1);
            w.Write((ushort)animDictionary.Count);
            w.Write((ushort)animationNamesLength);
            foreach (var item in animationNames)
            {
                w.Write(item.Data);
                w.Write((byte)0);
            }
            w.Align(8);

            return "OBJ\x01";
        }

        private static int CheckEntry(Dictionary<string, int> dictionary, string key)
        {
            if (key == null)
                return -1;
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
