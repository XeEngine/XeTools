using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.GameStudio.Models;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Utility
{
    public static class ProjectUtility
    {
        public static void Run(this IProject project, string configurationName)
        {
			var config = project.GetConfiguration(configurationName);
            if (string.IsNullOrEmpty(config.Executable) ||
                string.IsNullOrEmpty(config.WorkingDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else if (!File.Exists(config.Executable) ||
                !Directory.Exists(config.WorkingDirectory))
            {
                Helpers.ShowMessageBoxWarning($"{config.Executable} not found.");
            }
            else
            {
                Helpers.RunApplication(config.Executable, config.WorkingDirectory);
            }
        }

        public static void Build(this IProject project, string configurationName, Action callback = null)
		{
			var config = project.GetConfiguration(configurationName);

			if (string.IsNullOrEmpty(config?.OutputDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else
            {
                Task.Run(() =>
                {
                    Common.ProjectBuild(project, config.OutputDirectory);
                    callback?.Invoke();
                });
            }
        }

        public static void Clean(this IProject project, string configurationName)
		{
			var config = project.GetConfiguration(configurationName);
			if (string.IsNullOrEmpty(config.OutputDirectory))
            {
                Helpers.ShowMessageBoxWarning("Please review your project configuration before to continue.");
            }
            else
            {
                Task.Run(() =>
                {
                    Common.ProjectClean(project, config.OutputDirectory);
                });
            }
        }

		public static ProjectConfiguration GetConfiguration(this IProject project, string configurationName)
		{
			if (string.IsNullOrEmpty(configurationName))
				return null;

			return Settings.GetProjectConfiguration(project)?
				.Configurations.FirstOrDefault(x => x.Name == configurationName);
		}
    }
}
