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
        public void Clean()
        {
            OnProgress?.Invoke($"Cleaning {Project.FileName}...", 0, 1, false);

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

            var dispatcher = new Dispatcher<Entry>(entries);
            dispatcher.Process((e) => CleanEntryAsync(e, OutputFolder));

            OnProgress?.Invoke($"Clean completed in {dispatcher.ElapsedMilliseconds / 1000.0} seconds.", 1, 1, true);
        }

        private Task CleanEntryAsync(Entry entry, string outputFolder)
        {
            return new Task(() =>
            {
                CleanEntry(entry, outputFolder);
            });
        }
        private void CleanEntry(Entry entry, string outputFolder)
        {
            var item = entry.Item;
            var type = item.Type;
            if (!Modules.TryGetValue(type, out var module))
                return;

            Modules.IModule moduleInstance;
            try
            {
                moduleInstance = module.CreateInstance(new Modules.ModuleInit()
                {
                    FileName = Path.Combine(entry.Container.Name, item.Input),
                    OutputFileName = item.Output != null ? Path.Combine(entry.Container.Name, item.Output) : null,
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
                moduleInstance.Clean();
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} export exception on {item.Input}: {e.Message}");
                return;
            }
        }
    }
}
