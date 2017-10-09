using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xe.Tools.Projects
{
    public interface IProjectFile : IProjectEntry
    {
        string Format { get; set; }

        IEnumerable<KeyValuePair<string, string>> Parameters { get; }

        Stream Open(FileAccess access);
        
        void CreateParameter(string key, string value);

        string GetParameter(string key);

        void UpdateParameter(string key, string value);

        bool RemoveParameter(string key);
    }
}
