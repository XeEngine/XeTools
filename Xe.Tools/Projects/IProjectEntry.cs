using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools.Projects
{
    public interface IProjectEntry
    {
        string Name { get; set; }

        bool CanRename { get; }

        bool Remove(bool delete);
    }
}
