using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Xe.Tools.Project;

namespace Xe.Tools.Services
{
    public class ProjectService
    {
        public Project Project { get; private set; }

        public Container Container { get; private set; }

        public string WorkingDirectory { get; private set; }

        public IEnumerable<Item> Items => Container.Items;

        public AnimationService AnimationService { get; private set; }

        public ProjectService(Project project, Container container)
        {
            Project = project;
            Container = container;
            WorkingDirectory = Path.Combine(Project.ProjectPath, Container.Name);

            AnimationService = new AnimationService(this);
        }

        public T DeserializeItem<T>(Item item)
        {
            var filePath = Path.Combine(Project.ProjectPath, Path.Combine(Container.Name, item.Input));
            if (File.Exists(filePath))
            {
                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Unable to process item {item.Input}: {e.Message}");
                }
            }
            else
            {
                Log.Warning($"File {filePath} does not exists.");
            }
            return default(T);
        }
    }
}
