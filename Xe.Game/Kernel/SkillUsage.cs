﻿namespace Xe.Game.Kernel
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

        /// <summary>
        /// If the skill is visible thorugh the skills menu
        /// </summary>
        public bool Visible { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Skill))
                return "<empty>";
            return Skill;
        }
    }
}
