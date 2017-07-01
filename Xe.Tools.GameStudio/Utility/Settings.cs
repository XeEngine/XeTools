using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.GameStudio.Models;

namespace Xe.Tools.GameStudio.Utility
{
    public static class Settings
    {
        private static string GetProjectConfigurationFileName(Project project)
        {
            return Path.Combine(project.ProjectPath, project.FileName + ".config");
        }

        public static ProjectConfiguration GetProjectConfiguration(Project project)
        {
            var strFile = GetProjectConfigurationFileName(project);
            if (!File.Exists(strFile))
                return new ProjectConfiguration();
            using (var file = new FileStream(strFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(file))
                {
                    return JsonConvert.DeserializeObject<ProjectConfiguration>(reader.ReadToEnd());
                }
            }
        }

        public static void SaveProjectConfiguration(Project project, ProjectConfiguration settings)
        {
            var strFile = GetProjectConfigurationFileName(project);
            using (var file = new FileStream(strFile, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(JsonConvert.SerializeObject(settings, Formatting.Indented));
                }
            }
        }
    }
}
