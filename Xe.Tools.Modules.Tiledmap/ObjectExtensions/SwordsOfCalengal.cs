using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Game.Tilemaps;

namespace Xe.Tools.Modules.ObjectExtensions
{
    public static class SwordsOfCalengal
    {
        public static readonly ObjectExtensionDefinition[] Extensions = {
            new ObjectExtensionDefinition() { Id = Player.ID, Name = nameof(Player), Type = typeof(Player) },
            new ObjectExtensionDefinition() { Id = Enemy.ID, Name = nameof(Enemy), Type = typeof(Enemy) },
            new ObjectExtensionDefinition() { Id = Npc.ID, Name = nameof(Npc), Type = typeof(Npc) },
            new ObjectExtensionDefinition() { Id = MapChange.ID, Name = nameof(MapChange), Type = typeof(MapChange) },
            new ObjectExtensionDefinition() { Id = Chest.ID, Name = nameof(Chest), Type = typeof(Chest) },
            new ObjectExtensionDefinition() { Id = Event.ID, Name = nameof(Event), Type = typeof(Event) },
        };

        public class Player : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0x920d9124, 0x1908, 0x4ace, 0x8d, 0xfd, 0x81, 0x65, 0x0a, 0xb9, 0x98, 0x81);

            public Guid Id => ID;

            public int Entry { get; set; }

            public void Write(BinaryWriter writer)
            {
                writer.Write((byte)Entry);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((uint)0);
            }
        }

        public class Enemy : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0xd2605fb9, 0x8b86, 0x4fb9, 0xb9, 0x76, 0x57, 0x2d, 0x3c, 0x53, 0x7c, 0x00);

            public Guid Id => ID;

            public int ArtificialIntelligence { get; set; }

            public int Variant { get; set; }

            public void Write(BinaryWriter writer)
            {
                writer.Write((ushort)ArtificialIntelligence);
                writer.Write((ushort)Variant);
                writer.Write((uint)0);
            }
        }

        public class Npc : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0xee6c4e94, 0x412b, 0x41f8, 0x96, 0x30, 0x48, 0xd9, 0x27, 0xce, 0x4d, 0x6d);

            public Guid Id => ID;

            public void Write(BinaryWriter writer)
            {
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((uint)0);
            }
        }

        public class MapChange : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0x0addfab8, 0xbb1a, 0x4409, 0x8a, 0xc4, 0xf1, 0xf5, 0x62, 0x2b, 0x34, 0xf3);

            public Guid Id => ID;

            public int Zone { get; set; }

            public int Map { get; set; }

			public int Entry { get; set; }

			public Guid ZoneId { get; set; }

			public void Write(BinaryWriter writer)
            {
                writer.Write((byte)Zone);
                writer.Write((byte)Map);
                writer.Write((byte)Entry);
                writer.Write((byte)0);
                writer.Write((uint)0);
            }
        }

        public class Chest : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0x51aebd17, 0xfcc3, 0x4610, 0xbd, 0xa4, 0x64, 0xfa, 0x1c, 0xf6, 0xc5, 0xa9);

            public Guid Id => ID;

            public void Write(BinaryWriter writer)
            {
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((uint)0);
            }
        }

        public class Event : IObjectExtension
        {
            public static readonly Guid ID = new Guid(0x4f8e6d55, 0x3a51, 0x4527, 0xa3, 0x1a, 0xea, 0x09, 0x6b, 0x28, 0xc5, 0x48);

            public Guid Id => ID;

			public int Category { get; set; }

			public int Index { get; set; }

			public int Flags { get; set; }

			public void Write(BinaryWriter writer)
            {
                writer.Write((byte)Category);
                writer.Write((byte)Index);
                writer.Write((byte)Flags);
                writer.Write((byte)0);
                writer.Write((uint)0);
            }
        }
    }
}
