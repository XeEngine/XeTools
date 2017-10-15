using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.Builder
{
    public partial class Builder
    {
        public void Build()
        {
            OnProgress?.Invoke($"Building {Project.Name}...", 0, 1, false);

            Log.Message($"Processing {Project.Name} - Developed by {Project.Producer}");
            Log.Message($"Version {Project.Version}");
            Log.Message($"{Project.Copyright} {Project.Year}");

            var entries = Project.GetFiles()
                .Select(x => new Entry()
                {
                    Project = Project,
                    ProjectFile = x
                })
                .ToList();

            FileNames.Clear();
#if DEBUG
            int filesProcessed = 0;
            int filesCount = entries.Count;
            foreach (var entry in entries)
            {
                OnProgress?.Invoke($"Processing {entry.ProjectFile.Name}...", filesProcessed, filesCount, false);
                ProcessEntry(entry, OutputFolder);
                OnProgress?.Invoke($"Processed {entry.ProjectFile.Name}!", filesProcessed, filesCount, false);
            }
            OnProgress?.Invoke($"Build completed.", 1, 1, true);
#else
            var dispatcher = new Dispatcher<Entry>(entries);
            dispatcher.Process((e) => ProcessEntryAsync(e, OutputFolder));
            OnProgress?.Invoke($"Build completed in {dispatcher.ElapsedMilliseconds / 1000.0} seconds.", 1, 1, true);
#endif

            var fileSystemName = Path.Combine(OutputFolder, Path.Combine("data", "filesystem.bin"));
            ExportFileSystem(fileSystemName);
        }

        private Task ProcessEntryAsync(Entry entry, string outputFolder)
        {
            return new Task(() =>
            {
                ProcessEntry(entry, outputFolder);
            });
        }
        private void ProcessEntry(Entry entry, string outputFolder)
        {
            var file = entry.ProjectFile;
            var type = file.Format;
            if (!Modules.TryGetValue(type, out var module))
            {
                Log.Error($"Module {type} not found for {file.Path} item.");
                return;
            }

            Modules.IModule moduleInstance;
            try
            {
                moduleInstance = module.CreateInstance(new Modules.ModuleInit()
                {
                    FileName = file.Path,
                    OutputFileName = null,
                    Parameters = file.Parameters.ToArray(),
                    InputPath = Project.WorkingDirectory,
                    OutputPath = outputFolder
                });
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} initialization exception on {file.Path}: {e.Message}");
                return;
            }

            lock (FileNames)
            {
                FileNames.AddRange(moduleInstance.OutputFileNames);
            }
            
            try
            {
                moduleInstance.Build();
            }
            catch (Exception e)
            {
                moduleInstance.Clean();
                Log.Error($"Module {type} export exception on {file.Path}: {e.Message}");
                return;
            }
        }
    }
}
