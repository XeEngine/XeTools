using System.IO;
using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
    public class FileWatched
    {
		private readonly FileWatcherService fileWatcherService;

		public IProjectFile File { get; }

		public FileWatched(
			FileWatcherService fileWatcherService,
			IProjectFile projectFile)
		{
			this.fileWatcherService = fileWatcherService;

			if (!System.IO.File.Exists(projectFile.FullPath))
				throw new FileNotFoundException("File not found.", projectFile.FullPath);

			File = projectFile;
		}

		internal void OnChanged()
		{

		}
    }
}
