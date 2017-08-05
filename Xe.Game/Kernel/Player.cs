using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class Player : StatisticsBase
    {
        /// <summary>
        /// Player id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Message id for player's name
        /// </summary>
        public Guid Name { get; set; }

        /// <summary>
        /// Message id for player's description
        /// </summary>
        public Guid Description { get; set; }

        /// <summary>
        /// If the player is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If the player is locked
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// Character's level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Character's experience
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Current health
        /// </summary>
        public int HealthCurrent { get; set; }
        
        /// <summary>
        /// Current mana
        /// </summary>
        public int ManaCurrent { get; set; }

        /// <summary>
        /// List of learned skills
        /// </summary>
        public List<SkillUsage> Skills { get; set; }
    }
}
