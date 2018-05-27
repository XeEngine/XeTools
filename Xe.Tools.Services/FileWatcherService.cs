using System.Collections.Generic;
using System.IO;
using Xe.Tools.Projects;

namespace Xe.Tools.Services
{
	public class FileWatcherService
	{
		private readonly FileSystemWatcher fileSystemWatcher;
		private readonly Dictionary<string, FileWatched> listFileWatched;

		public IProject Project { get; }

		public FileWatcherService(IProject project)
		{
			Project = project;
			fileSystemWatcher = new FileSystemWatcher()
			{
				Path = Path.GetDirectoryName(Project.FullPath),
				IncludeSubdirectories = true,
				
			};
			listFileWatched = new Dictionary<string, FileWatched>();

			fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
			fileSystemWatcher.Changed += FileSystemWatcher_Changed;
		}

		public FileWatched Open(IProjectFile file)
		{
			if (!listFileWatched.TryGetValue(file.Path, out var fileWatched))
			{
				fileWatched = new FileWatched(this, file);
				listFileWatched.Add(NormalizePath(file.FullPath), fileWatched);
			}

			return fileWatched;
		}

		public void Close(IProjectFile file)
		{
			listFileWatched.Remove(file.FullPath);
		}

		public void CloseAll(IProjectFile file)
		{
			listFileWatched.Clear();
		}


		private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
		{

		}

		private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			GetFileWatched(e.FullPath)?.OnChanged();
		}

		private FileWatched GetFileWatched(string path)
		{
			path = NormalizePath(path);
			listFileWatched.TryGetValue(path, out var fileWatched);
			return fileWatched;
		}

		private string NormalizePath(string path)
		{
			if (path.Length > 0)
			{
				path = Path.GetFullPath(path);
				path = path.Replace('/', '\\');
				path = path.ToLower();

				if (path[path.Length - 1] == '\\')
				{
					path = path.Substring(0, path.Length - 1);
				}
			}

			return path;
		}
	}
}
