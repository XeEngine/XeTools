using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools.Projects
{
    public interface IProjectDirectory : IProjectEntry
    {
        IEnumerable<IProjectEntry> GetEntries();

        IProjectDirectory AddDirectory(string name);

        IProjectFile AddFile(string name);
    }
}
