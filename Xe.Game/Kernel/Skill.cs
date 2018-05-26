﻿using System;

namespace Xe.Game.Kernel
{
    public class Skill
    {
		/// <summary>
		/// Internal name
		/// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Shown translated name
		/// </summary>
        public string TagName { get; set; }

		/// <summary>
		/// Shown translated description
		/// </summary>
        public string TagDescription { get; set; }

		/// <summary>
		/// Name of the used effect.
		/// </summary>
        public string GfxName { get; set; }

		/// <summary>
		/// Name of the animation of selected GFX.
		/// </summary>
        public string GfxAnimation { get; set; }

		/// <summary>
		/// Name of the sound effect.
		/// </summary>
        public string Sfx { get; set; }

        public DamageFormula DamageFormula { get; set; }

        public TargetType Target { get; set; }

        public int Damage { get; set; }
    }
}
