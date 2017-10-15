using System.Collections.Generic;
using System.Linq;
using Xe.Tools.Modules;
using Xe.Tools.Projects;

namespace Xe.Tools.Builder
{
    public partial class Builder
    {
        private class Entry
        {
            public IProject Project { get; set; }
            public IProjectFile ProjectFile { get; set; }
        }

        public delegate void ProgressFunc(string message, int filesProcessed, int filesToProcess, bool hasFinish);

        public event ProgressFunc OnProgress;

        public IProject Project { get; }
        public string OutputFolder { get; }

        private Dictionary<string, Module> Modules { get; }

        private List<string> FileNames { get; }

        public Builder(IProject project, string outputFolder)
        {
            Project = project;
            OutputFolder = outputFolder;
            Modules = Module.GetModules().ToDictionary(x => x.Name.ToLower(), x => x);
            FileNames = new List<string>();
        }
    }
}
