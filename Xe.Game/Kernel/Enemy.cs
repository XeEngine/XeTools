using System;
using System.Collections.Generic;

namespace Xe.Game.Kernel
{
    public class Enemy : StatisticsBase
    {
        public string Name { get; set; }

        public Guid MsgName { get; set; }

        public Guid MsgDescription { get; set; }

        public List<EnemyAction> Actions { get; set; }

        public List<SkillUsage> Skills { get; set; }
    }
}
