using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Xe.Tools.Builder
{
    public partial class Builder
    {
        public void Build()
        {
            OnProgress?.Invoke($"Building {Project.FileName}...", 0, 1, false);

            Log.Message($"Processing {Project.Name} - Developed by {Project.Producer}");
            Log.Message($"Version {Project.Version}");
            Log.Message($"{Project.Copyright} {Project.Year}");

            var entries = new List<Entry>();
            foreach (var container in Project.Containers)
            {
                entries.AddRange(container.Items.Select(item => new Entry()
                {
                    Project = Project,
                    Container = container,
                    Item = item
                }));
            }

            FileNames.Clear();
#if DEBUG
            int filesProcessed = 0;
            int filesCount = entries.Count;
            foreach (var entry in entries)
            {
                OnProgress?.Invoke($"Processing {entry.Item.RelativeFileNameInput}...", filesProcessed, filesCount, false);
                ProcessEntry(entry, OutputFolder);
                OnProgress?.Invoke($"Processed {entry.Item.RelativeFileNameInput}!", filesProcessed, filesCount, false);
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
            var item = entry.Item;
            var type = item.Type;
            if (!Modules.TryGetValue(type, out var module))
            {
                Log.Error($"Module {type} not found for {item.Input} item.");
                return;
            }

            Modules.IModule moduleInstance;
            try
            {
                moduleInstance = module.CreateInstance(new Modules.ModuleInit()
                {
                    FileName = item.Input,
                    OutputFileName = item.Output,
                    Parameters = item.Parameters?.ToArray() ?? new Tuple<string, string>[0],
                    InputPath = Path.Combine(entry.Project.ProjectPath, entry.Container.Name),
                    OutputPath = Path.Combine(outputFolder, entry.Container.Name)
                });
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} initialization exception on {item.Input}: {e.Message}");
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
                Log.Error($"Module {type} export exception on {item.Input}: {e.Message}");
                return;
            }
        }
    }
}
