using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class Player : StatisticsBase
    {
        public Guid MsgName { get; set; }

        public Guid MsgDescription { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        public int HealthCurrent { get; set; }

        public int ManaCurrent { get; set; }

        public List<SkillUsage> Skills { get; set; }
    }
}
