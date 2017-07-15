using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Components.AnimationEditor.Commands
{
    internal interface ICommand
    {
        void Execute();
        void UnExecute();
    }
}
