using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
    public class ProjectService
    {
        public IProject Project { get; private set; }

        public string WorkingDirectory { get; private set; }

        public IEnumerable<IProjectFile> Items { get; private set; }

        public AnimationService AnimationService { get; private set; }

        public ProjectService(IProject project)
        {
            Project = project;
            WorkingDirectory = Path.GetDirectoryName(Project.WorkingDirectory);
            Items = project.GetFiles();

            AnimationService = new AnimationService(this);
        }

        public T DeserializeItem<T>(IProjectFile item)
        {
            var filePath = item.FullPath;
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
                    Log.Error($"Unable to process item {item.Path}: {e.Message}");
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
