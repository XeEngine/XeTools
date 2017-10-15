using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.Builder
{
    public partial class Builder
    {
        public void Clean()
        {
            OnProgress?.Invoke($"Cleaning {Project.Name}...", 0, 1, false);

            var entries = Project.GetFiles()
                .Select(x => new Entry()
                {
                    Project = Project,
                    ProjectFile = x
                })
                .ToList();

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
            var file = entry.ProjectFile;
            var type = file.Format;
            if (!Modules.TryGetValue(type, out var module))
                return;

            Modules.IModule moduleInstance;
            try
            {
                moduleInstance = module.CreateInstance(new Modules.ModuleInit()
                {
                    FileName = file.Name,
                    OutputFileName = null,
                    Parameters = file.Parameters.ToArray(),
                    InputPath = file.FullPath,
                    OutputPath = Path.Combine(outputFolder, file.Path)
                });
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} initialization exception on {file.Path}: {e.Message}");
                return;
            }

            try
            {
                moduleInstance.Clean();
            }
            catch (Exception e)
            {
                Log.Error($"Module {type} export exception on {file.Path}: {e.Message}");
                return;
            }
        }
    }
}
