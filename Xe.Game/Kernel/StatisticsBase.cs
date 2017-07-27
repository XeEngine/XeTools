using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class StatisticsBase
    {
        public int Health { get; set; }

        public int Mana { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int AttackSpecial { get; set; }

        public int DefenseSpecial { get; set; }
    }
}
