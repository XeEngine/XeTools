using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Game.Kernel;

namespace Xe.Tools.Modules.Kernel
{
    internal static class Extensions
    {
        public static void Write(this BinaryWriter writer, Dictionary<uint, int> dictionary, string data)
        {
            uint hash = data.GetXeHash();
            if (!dictionary.TryGetValue(hash, out int index))
                index = -1;
            writer.Write((ushort)index);
        }
    }

    public partial class Kernel
    {
        #region configuration

        const int PLAYER_SKILLSCOUNT = 32;

        const int SIZEOF_HEADER = 8;
        const int SIZEOF_ENTRY = 8;
        const int SIZEOF_SKILL = 0x18;
        const int SIZEOF_SKILLUSAGE = 0;
        const int SIZEOF_PLAYER = 0x20 + PLAYER_SKILLSCOUNT * SIZEOF_SKILLUSAGE;

        #endregion

        /// <summary>
        /// Type of content entries
        /// </summary>
        [Flags]
        private enum Content
        {
            GfxTable = 1 << 0,
            SfxTable = 1 << 1,
            Skills = 1 << 8,
            Abilities = 1 << 9,
            Enemies = 1 << 10,
            Players = 1 << 11,
        }

        /// <summary>
        /// Description of file itselfs
        /// </summary>
        struct Header
        {
            /// <summary>
            /// Identifier of file's header
            /// </summary>
            public byte MagicCode;

            /// <summary>
            /// Current version of the file
            /// </summary>
            public byte Version;

            /// <summary>
            /// How big is the file itself
            /// </summary>
            public byte HeaderSize;

            /// <summary>
            /// Bytes alignment
            /// </summary>
            public byte Align;

            /// <summary>
            /// What kind of content will be loaded
            /// </summary>
            public Content Content;
        }

        /// <summary>
        /// Description for each content entry
        /// </summary>
        struct Entry
        {
            /// <summary>
            /// Where the acutal content starts
            /// </summary>
            public int Offset { get; set; }

            /// <summary>
            /// How elements are contained
            /// </summary>
            public short Count { get; set; }

            /// <summary>
            /// Optionally describe how bytes there are for each element
            /// </summary>
            public short Stride { get; set; }
        }
        
        private Dictionary<uint, int> _gfxs;
        private Dictionary<uint, int> _sfxs;
        private Dictionary<uint, int> _skills;

        private void Export(BinaryWriter w)
        {
            // Populate the header
            var header = new Header()
            {
                MagicCode = 0x4B,
                Version = 1,
                HeaderSize = SIZEOF_HEADER,
                Align = 8,
                Content = Content.GfxTable | Content.SfxTable | Content.Skills |
                    Content.Abilities | Content.Enemies | Content.Players
            };

            // Check how entries there are
            int bitsCount = CountBits((int)header.Content);
            // Jump the header and entries descriptions
            w.BaseStream.Position = AlignOffset(header.HeaderSize + bitsCount * SIZEOF_ENTRY, header.Align);
            // Prepare a list of entries to populate during exporting
            var entries = new List<Entry>();

            var gfxNames = KernelData.Skills.Select(x => x.GfxName).ToList();
            var sfxNames = KernelData.Skills.Select(x => x.Sfx).ToList();

            _gfxs = header.Content.HasFlag(Content.GfxTable) ? WriteHashList(w, gfxNames) : GetHashList(gfxNames);
            _sfxs = header.Content.HasFlag(Content.SfxTable) ? WriteHashList(w, sfxNames) : GetHashList(sfxNames);
            
            if (header.Content.HasFlag(Content.Skills))
            {
                entries.Add(new Entry()
                {
                    Offset = (int)w.BaseStream.Position,
                    Count = (short)KernelData.Skills.Count,
                    Stride = SIZEOF_SKILL
                });
                _skills = GetHashList(KernelData.Skills, x =>
                {
                    ExportSkill(w, x);
                    return x.Name.GetXeHash();
                });
            }
            if (header.Content.HasFlag(Content.Abilities))
            {

            }
            if (header.Content.HasFlag(Content.Enemies))
            {

            }
            if (header.Content.HasFlag(Content.Players))
            {
                entries.Add(new Entry()
                {
                    Offset = (int)w.BaseStream.Position,
                    Count = (short)KernelData.Players.Count,
                    Stride = SIZEOF_PLAYER
                });
                foreach (var player in KernelData.Players)
                {
                    ExportPlayer(w, player);
                }
            }

            // Finally write the header
            w.BaseStream.Position = 0;
            ExportHeader(w, header);
            foreach (var entry in entries)
            {
                ExportEntry(w, entry);
            }
        }

        private static void ExportHeader(BinaryWriter w, Header header)
        {
            w.Write(header.MagicCode);
            w.Write(header.Version);
            w.Write(header.HeaderSize);
            w.Write(header.Align);
            w.Write((uint)header.Content);
        }
        private static void ExportEntry(BinaryWriter w, Entry entry)
        {
            w.Write(entry.Offset);
            w.Write(entry.Count);
            w.Write(entry.Stride);
        }
        private void ExportSkill(BinaryWriter w, Skill skill)
        {
            w.Write(skill.Name.GetXeHash());                // 0x00
            w.Write(skill.MsgName.ToInt());                 // 0x04
            w.Write(skill.MsgDescription.ToInt());          // 0x08
            w.Write(_gfxs, skill.GfxName);                  // 0x0c
            w.Write(_gfxs, skill.GfxAnimation);             // 0x0e
            w.Write(_gfxs, skill.Sfx);                      // 0x10
            w.Write((byte)skill.DamageFormula);             // 0x12
            w.Write((byte)skill.Damage);                    // 0x14
            w.Write((ushort)0);                             // 0x16
        }
        private void ExportPlayer(BinaryWriter w, Player player)
        {
            byte properties = (byte)((player.Enabled ? 1 : 0) |
                (player.Locked ? 1 : 0));

            w.Write(player.MsgName.ToInt());                // 0x00
            w.Write(player.MsgDescription.ToInt());         // 0x04
            w.Write((byte)player.Id);                       // 0x08
            w.Write(properties);                            // 0x09
            w.Write((byte)player.Level);                    // 0x0A
            w.Write((byte)0);                               // 0x0B     UNUSED
            w.Write(player.Experience);                     // 0x0C
            w.Write((short)player.Health);                  // 0x10
            w.Write((short)player.Mana);                    // 0x12
            w.Write((byte)player.Attack);                   // 0x14
            w.Write((byte)player.AttackSpecial);            // 0x15
            w.Write((byte)player.Defense);                  // 0x16
            w.Write((byte)player.DefenseSpecial);           // 0x17
            w.Write((short)player.HealthCurrent);           // 0x18
            w.Write((short)player.ManaCurrent);             // 0x1A
            w.Write(0);                                     // 0x1C     UNUSED

            WriteElements(w, player.Skills, SIZEOF_SKILLUSAGE, PLAYER_SKILLSCOUNT, (writer, item) => ExportSkillUsage(writer, item));
        }
        private void ExportSkillUsage(BinaryWriter w, SkillUsage skillUsage)
        {
            w.Write(skillUsage.Animation.GetXeHash());
            w.Write(_skills, skillUsage.Skill);
        }

        #region utilities

        private static int CountBits(int data)
        {
            int bitsCount = 0;
            for (int i = 0; i < 32; i++)
            {
                bitsCount += (data & (1 << i)) != 0 ? 1 : 0;
            }
            return bitsCount;
        }
        private static int AlignOffset(int offset, int alignment)
        {
            int diff = offset % alignment;
            return diff > 0 ? offset + alignment - diff : offset;
        }
        private static Dictionary<uint, int> GetHashList<T>(List<T> items, Func<T, uint> func)
        {
            var index = 0;
            var uniqueList = items.Distinct();
            var dictionary = new Dictionary<uint, int>(uniqueList.Count());
            foreach (var item in uniqueList)
            {
                var hash = func.Invoke(item);
                dictionary.Add(hash, index++);
            }
            return dictionary;
        }
        private static Dictionary<uint, int> GetHashList(List<string> items)
        {
            return GetHashList(items, x => x.GetXeHash());
        }
        private static Dictionary<uint, int> WriteHashList(BinaryWriter w, List<string> items)
        {
            return GetHashList(items, x =>
            {
                var hash = x.GetXeHash();
                w.Write(hash);
                return hash;
            });
        }

        private static void WriteElements<T>(BinaryWriter w, List<T> elements, int stride, int count, Action<BinaryWriter, T> func)
        {
            int index, toWrite = elements.Count;
            if (toWrite > count)
            {
                Log.Warning($"There are too many {typeof(T)} elements to write: {toWrite} with a maximum of {count}.");
                toWrite = count;
            }

            for (index = 0; index < count; index++)
            {
                func.Invoke(w, elements[index]);
            }
            if (index < count)
            {
                w.BaseStream.Position += (count - index) * stride;
            }
        }

        #endregion
    }
}
