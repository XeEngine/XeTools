using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xe.Tools.Projects
{
    public class FileSystem : IProjectFactory
    {
        public string Name => "File system";

        public bool IsDirectory => true;

        public IEnumerable<string> SupportedExtensions => new string[0];
        
        public bool TryOpen(string directory)
        {
            return Directory.Exists(directory);
        }
        
        public IProject Open(string directory)
        {
            return new Project(directory);
        }

        private class Project : IProject
        {
            public string Name { get => System.IO.Path.GetFileName(Path); set { } }
            public string Path { get; }

            public string ShortName { get => System.IO.Path.GetFullPath(Path); set { } }

            public Version Version => new Version();

            public string Company { get => null; set { } }
            public string Producer { get => null; set { } }
            public string Copyright { get => null; set { } }
            public int Year { get => 0; set { } }

            public Project(string directory)
            {
                Path = directory;
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return FileSystem.GetEntries(Path);
            }

            public void SaveChanges()
            {

            }

            public void SaveChanges(Stream stream)
            {

            }
        }

        private abstract class ProjectEntry : IProjectEntry
        {
            public string Name
            {
                get => System.IO.Path.GetDirectoryName(Path);
                set
                {
                    var newPath = GetPath(value);
                    Move(Path, newPath);
                    Path = newPath;
                }
            }

            public string Path { get; private set; }

            public bool CanRename => true;

            protected ProjectEntry(string path)
            {
                Path = path;
            }

            public abstract bool Remove(bool delete);

            protected abstract void Move(string oldPath, string newPath);

            protected string GetPath(string name)
            {
                return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Path), name);
            }
        }

        private class ProjectDirectory : ProjectEntry, IProjectDirectory
        {
            public ProjectDirectory(string path) : base(path)
            {
            }

            public IProjectDirectory AddDirectory(string name)
            {
                var entry = GetEntries()
                    .SingleOrDefault(x => x is IProjectDirectory && string.Compare(x.Name, name, true) == 0)
                    as IProjectDirectory;
                if (entry != null)
                {
                    var path = GetPath(name);
                    Directory.CreateDirectory(path);
                    entry = new ProjectDirectory(path);
                }
                return entry;
            }

            public IProjectFile AddFile(string name)
            {
                var entry = GetEntries()
                    .SingleOrDefault(x => x is IProjectFile && string.Compare(x.Name, name, true) == 0)
                    as IProjectFile;
                if (entry != null)
                {
                    var path = GetPath(name);
                    File.Create(path).Dispose();
                    entry = new ProjectFile(path);
                }
                return entry;
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return FileSystem.GetEntries(Path);
            }

            public override bool Remove(bool delete)
            {
                if (delete)
                {
                    Directory.Delete(Path, true);
                }
                return delete;
            }

            protected override void Move(string oldPath, string newPath)
            {
                Directory.Move(oldPath, newPath);
            }
        }

        private class ProjectFile : ProjectEntry, IProjectFile
        {
            private Dictionary<string, string> _parameters = new Dictionary<string, string>();

            public string Format { get; set; }

            public IEnumerable<KeyValuePair<string, string>> Parameters => _parameters;

            public ProjectFile(string path) : base(path)
            {
            }

            public Stream Open(FileAccess access)
            {
                FileMode mode;
                FileShare share;
                switch (access)
                {
                    case FileAccess.Read:
                        mode = FileMode.Open;
                        share = FileShare.Read;
                        break;
                    case FileAccess.Write:
                        mode = FileMode.Create;
                        share = FileShare.Read;
                        break;
                    case FileAccess.ReadWrite:
                        mode = FileMode.OpenOrCreate;
                        share = FileShare.Read;
                        break;
                    default:
                        mode = 0;
                        share = 0;
                        break;
                }
                return new FileStream(Path, mode, access, share);
            }

            public override bool Remove(bool delete)
            {
                if (delete)
                {
                    File.Delete(Path);
                }
                return delete;
            }

            public void CreateParameter(string key, string value)
            {
                _parameters.Add(key, value);
            }

            public string GetParameter(string key)
            {
                return _parameters.TryGetValue(key, out var value) ? value : null;
            }

            public void UpdateParameter(string key, string value)
            {
                _parameters[key] = value;
            }

            public bool RemoveParameter(string key)
            {
                return _parameters.Remove(key);
            }

            protected override void Move(string oldPath, string newPath)
            {
                File.Move(oldPath, newPath);
            }
        }

        private static IEnumerable<IProjectEntry> GetEntries(string path)
        {
            var directories = Directory.GetDirectories(path)
                .Select(x => new ProjectDirectory(x) as IProjectEntry);
            var files = Directory.GetFiles(path)
                .Select(x => new ProjectFile(x) as IProjectEntry);
            return directories.Concat(files);
        }
    }
}
