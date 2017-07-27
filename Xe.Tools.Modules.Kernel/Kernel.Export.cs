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
            writer.Write(index);
        }
    }

    public partial class Kernel
    {
        struct Header
        {
            public uint MagicCode;
            public short GfxsCount;
            public short SfxsCount;
            public short SkillsCount;
            public short AbilitiesCount;
            public short EnemiesCoumt;
            public short PlayersCount;
        }
        
        private Dictionary<uint, int> _gfxs;
        private Dictionary<uint, int> _sfxs;
        private Dictionary<uint, int> _skills;

        private void Export(BinaryWriter w)
        {
            var gfxNames = KernelData.Skills.Select(x => x.GfxName).ToList();
            var sfxNames = KernelData.Skills.Select(x => x.Sfx).ToList();
            
            _gfxs = GetHashList(w, gfxNames);
            _sfxs = GetHashList(w, sfxNames);
            _skills = GetHashList(KernelData.Skills, x =>
            {
                ExportSkill(w, x);
                return x.Name.GetXeHash();
            });
        }

        private void ExportSkill(BinaryWriter w, Skill skill)
        {
            w.Write(skill.Name.GetXeHash());
            w.Write(skill.MsgName.ToInt());
            w.Write(skill.MsgDescription.ToInt());
            w.Write(_gfxs, skill.GfxName);
            w.Write(_gfxs, skill.GfxAnimation);
            w.Write(_gfxs, skill.Sfx);
            w.Write((byte)skill.DamageFormula);
            w.Write((byte)skill.Damage);
            w.Write((ushort)0);
        }

        private Dictionary<uint, int> GetHashList<T>(List<T> items, Func<T, uint> func)
        {
            var count = items.Count;
            var dictionary = new Dictionary<uint, int>(count);
            for (int i = 0; i < count; i++)
            {
                var hash = func.Invoke(items[i]);
                dictionary.Add(hash, i);
            }
            return dictionary;
        }
        private Dictionary<uint, int> GetHashList(BinaryWriter w, List<string> strings)
        {
            var count = strings.Count;
            var dictionary = new Dictionary<uint, int>(count);
            for (int i = 0; i < count; i++)
            {
                var hash = strings[i].GetXeHash();
                w.Write(hash);
                dictionary.Add(hash, i);
            }
            return dictionary;
        }
    }
}
