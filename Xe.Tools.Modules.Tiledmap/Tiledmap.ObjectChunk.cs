using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Tilemaps;
using Xe.Security;

namespace Xe.Tools.Modules
{
    public partial class Tiledmap
    {
        private const int Alignment = 8;

        private string WriteObjectsChunk(Map tileMap, BinaryWriter w)
        {
            var extBuffer = new MemoryStream(256);
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
                flags = AddFlag(flags, 0, o.Visible);
                flags = AddFlag(flags, 1, o.HasShadow);
                flags = AddFlag(flags, 2, (int)o.Flip);
                flags = AddFlag(flags, 4, (int)o.Direction);

                w.Write(Crc32.CalculateDigestAscii(o.Name));
                w.Write(Crc32.CalculateDigestAscii(o.Type));
                w.Write((uint)CheckEntry(animDictionary, o.AnimationData)); // To replace with Crc32 in future
                w.Write(Crc32.CalculateDigestAscii(o.AnimationName));
                w.Write((short)(o.X + o.Width / 2));
                w.Write((short)(o.Y + o.Height / 2));
                w.Write((short)o.Z);
                w.Write((byte)0); // depth
                w.Write((byte)entry.Layer.GetPriority());
                w.Write((short)o.Width);
                w.Write((short)o.Height);
                w.Write((byte)flags);
                w.Write((byte)0);

                var extensionId = o.Extension != null ? IndexOf(ObjectExtensionDefinitions, o.Extension.Id) : 0;
                if (extensionId > 0)
                {
                    extBuffer.SetLength(0);
                    o.Extension.Write(new BinaryWriter(extBuffer));
                    if ((extBuffer.Length % Alignment) == 0)
                    {
                        Log.Warning($"Extension {extensionId} {o.Extension.GetType().Name} is not aligned!");
                        var diff = Alignment - (extBuffer.Length % Alignment);
                        extBuffer.SetLength(extBuffer.Length + diff);
                    }
                    w.Write((byte)extensionId);
                    w.Write((byte)(extBuffer.Length / Alignment));
                    w.Write(extBuffer.GetBuffer(), 0, (int)extBuffer.Length);
                }
                else
                {
                    w.Write((byte)0); // Extension ID
                    w.Write((byte)0); // Extension length
                }
            }

            // Write animation names
            var animationNames = animDictionary
                .OrderBy(x => x.Value)
                .Select(x =>
                {
                    var str = x.Key;
                    if (Path.GetExtension(str) == ".json")
                        str = str.Substring(0, str.IndexOf(".json"));
                    var data = System.Text.Encoding.UTF8.GetBytes(str);
                    return new
                    {
                        Name = str,
                        Data = data,
                        Length = data.Length
                    };
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
            value = dictionary.Count;
            dictionary.Add(key, value);
            return value;
        }
        private static int AddFlag(int data, int shift, bool value)
        {
            return value ? (data |= (1 << shift)) : (data &= ~(1 << shift));
        }
        private static int AddFlag(int data, int shift, int value)
        {
            return data |= (value << shift);
        }

        private static int IndexOf(ObjectExtensionDefinition[] array, Guid id)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Id == id)
                    return i + 1;
            }
            return 0;
        }
    }
}
