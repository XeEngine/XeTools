using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class KernelData
    {
        public List<Skill> Skills { get; set; }

        public List<Ability> Abilities { get; set; }

        public List<Player> Players { get; set; }

        public List<Enemy> Enemies { get; set; }
    }
}
