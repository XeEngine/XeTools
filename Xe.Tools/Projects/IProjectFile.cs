using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xe.Tools.Projects
{
    public interface IProjectFile : IProjectEntry
    {
        Stream Open(FileAccess access);
    }
}
