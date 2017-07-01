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
            ShowInfo();

            Log.Message($"Processing {project.Name} - Developed by {project.Producer}");
            Log.Message($"Version {project.Version}");
            Log.Message($"{project.Copyright} {project.Company}");

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
            foreach (var entry in entries)
            {
                ProcessEntry(entry, outputFolder);
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
            var moduleInstance = module.CreateInstance(new Modules.ModuleSettings()
            {
                FileName = Path.Combine(entry.Container.Name, item.Input),
                Parameters = item.Parameters.ToArray(),
                InputPath = entry.Project.ProjectPath,
                OutputPath = outputFolder
            });
            string output = item.Output;
            if (string.IsNullOrEmpty(output))
                output = Path.Combine(Path.GetDirectoryName(item.Input), Path.GetFileNameWithoutExtension(item.Input));
             moduleInstance.Export();
        }
    }
}
