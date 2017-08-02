using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class SkillUsage
    {
        /// <summary>
        /// Animation that the character will use to perform the specified skill
        /// </summary>
        public string Animation { get; set; }

        /// <summary>
        /// Name of the skill to use
        /// <see cref="Skill"/>
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// If the skill is enabled / acquired
        /// </summary>
        public bool Enabled { get; set; }
    }
}
