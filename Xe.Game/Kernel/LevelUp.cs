using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class LevelUp : StatisticsBase
    {
        public Skill NewSkill { get; set; }

        public Ability NewAbility { get; set; }
    }
}
