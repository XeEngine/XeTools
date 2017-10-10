using System.Collections.Generic;
using System.IO;

namespace Xe.Tools.Projects
{
    public interface IProjectFactory
    {
        string Name { get; }

        bool IsDirectory { get; }

        IEnumerable<string> SupportedExtensions { get; }

        bool TryOpen(string path);

        IProject Open(string path);
    }
}
