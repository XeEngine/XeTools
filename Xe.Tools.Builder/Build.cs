using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Builder
{
    public static partial class Program
    {
        private class Entry
        {
            public Project Project { get; set; }
            public Project.Container Container { get; set; }
            public Project.Item Item { get; set; }
        }

        public static void Build(Project project, string outputFolder)
        {
            Program.OnProgress?.Invoke($"Building {project.FileName}...", 0, 1, false);
            ShowInfo();

            Log.Message($"Processing {project.Name} - Developed by {project.Producer}");
            Log.Message($"Version {project.Version}");
            Log.Message($"{project.Copyright} {project.Year}");

            var entries = new List<Entry>();
            foreach (var container in project.Containers)
            {
                entries.AddRange(container.Items.Select(item => new Entry()
                {
                    Project = project,
                    Container = container,
                    Item = item
                }));
            }

#if DEBUG
            int filesProcessed = 0;
            int filesCount = entries.Count;
            foreach (var entry in entries)
            {
                Program.OnProgress?.Invoke($"Processing {entry.Item.RelativeFileNameInput}...", filesProcessed, filesCount, false);
                ProcessEntry(entry, outputFolder);
                Program.OnProgress?.Invoke($"Processed {entry.Item.RelativeFileNameInput}!", filesProcessed, filesCount, false);
            }
#else
            int maxTasksCount = System.Environment.ProcessorCount * 2;
            var queue = new List<Task>(maxTasksCount);
            while (entries.Count > 0)
            {
                if (queue.Count >= maxTasksCount)
                {
                    if (queue.Count >= maxTasksCount)
                    {
                        Task.WaitAny(queue.ToArray());
                        foreach (var taskItem in queue)
                        {
                            if (taskItem.IsCompleted ||
                                taskItem.IsFaulted ||
                                taskItem.IsCanceled)
                            {
                                queue.Remove(taskItem);
                                break;
                            }
                        }
                    }
                }

                Task task;
                lock (queue)
                {
                    var entry = entries[0];
                    entries.RemoveAt(0);
                    task = ProcessEntryAsync(entry, outputFolder);
                    queue.Add(task);
                }
                task.Start();
            }
            Task.WaitAll(queue.ToArray());
#endif
            Program.OnProgress?.Invoke($"Build completed.", 1, 1, true);
        }

        private static Task ProcessEntryAsync(Entry entry, string outputFolder)
        {
            return new Task(() =>
            {
                ProcessEntry(entry, outputFolder);
            });
        }
        private static void ProcessEntry(Entry entry, string outputFolder)
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
                    FileName = Path.Combine(entry.Container.Name, item.Input),
                    Parameters = item.Parameters.ToArray(),
                    InputPath = entry.Project.ProjectPath,
                    OutputPath = outputFolder
                });
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} initialization exception on {item.Input}: {e.Message}");
                return;
            }

            string output = item.Output;
            if (string.IsNullOrEmpty(output))
                output = Path.Combine(Path.GetDirectoryName(item.Input), Path.GetFileNameWithoutExtension(item.Input));
            try
            {
                moduleInstance.Export();
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} export exception on {item.Input}: {e.Message}");
                return;
            }
        }
    }
}
