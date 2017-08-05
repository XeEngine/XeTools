using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Game.Kernel
{
    public class Skill
    {
        public string Name { get; set; }

        public Guid MsgName { get; set; }

        public Guid MsgDescription { get; set; }

        public string GfxName { get; set; }

        public string GfxAnimation { get; set; }

        public string Sfx { get; set; }

        public DamageFormula DamageFormula { get; set; }

        public TargetType Target { get; set; }

        public int Damage { get; set; }
    }
}
