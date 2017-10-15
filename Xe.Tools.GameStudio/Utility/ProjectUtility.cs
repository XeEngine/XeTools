using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xe.Tools.Projects;

namespace Xe.Tools.GameStudio.Utility
{
    public static class ProjectUtility
    {
        public static void Run(this IProject project)
        {
            var config = Settings.GetProjectConfiguration(project);
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

        public static void Build(this IProject project, Action callback = null)
        {
            var config = Settings.GetProjectConfiguration(project);
            if (string.IsNullOrEmpty(config.OutputDirectory))
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

        public static void Clean(this IProject project)
        {
            var config = Settings.GetProjectConfiguration(project);
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
    }
}
