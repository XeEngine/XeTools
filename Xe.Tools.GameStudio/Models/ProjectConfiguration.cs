using System.Collections.Generic;

namespace Xe.Tools.GameStudio.Models
{
    public class ProjectConfiguration
    {
		public string Name { get; set; }

		public string Executable { get; set; }

        public string WorkingDirectory { get; set; }

        public string OutputDirectory { get; set; }
    }

	public class ProjectSettings
	{
		public string CurrentConfiguration { get; set; }

		public List<ProjectConfiguration> Configurations { get; set; }
	}
}
