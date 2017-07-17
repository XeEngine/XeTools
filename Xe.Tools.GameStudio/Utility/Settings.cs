﻿using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using Xe.Tools.GameStudio.Models;

namespace Xe.Tools.GameStudio.Utility
{
    public static class Settings
    {
        public static WindowPropertiesModel WindowProperties
        {
            get
            {
                var o = Properties.Settings.Default.WindowProperties;
                if (o == null)
                {
                    o = Properties.Settings.Default.WindowProperties = new WindowPropertiesModel();
                    Properties.Settings.Default.Save();
                }
                return o;
            }
            set
            {
                Properties.Settings.Default.WindowProperties = value;
                Properties.Settings.Default.Save();
            }
        }

        public static int OutputPageHeight
        {
            get
            {
                var height = Properties.Settings.Default.OutputPageHeight;
                if (height <= 20)
                    height = 20;
                return height;
            }
            set => Properties.Settings.Default.OutputPageHeight = value;
        }

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
