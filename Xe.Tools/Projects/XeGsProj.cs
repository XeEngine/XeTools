using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xe.Tools.Projects
{
    public partial class XeGsProj : IProjectFactory
    {
        public string Name => "XeEngine Game Studio Project";

        public bool IsDirectory => false;

        public IEnumerable<string> SupportedExtensions => new string[] { "*.game.proj.json" };
        
        public bool TryOpen(string fileName)
        {
            return Project.Open(fileName) != null;
        }

        public IProject Open(string fileName)
        {
            return new MyProject(Project.Open(fileName));
        }

        private static Project.Item CreateDefaultItem(string name)
        {
            return new Project.Item()
            {
                Input = name,
                Type = "copy"
            };
        }

        private class MyProject : IProject
        {
            private Project _project;
            private IEnumerable<IProjectEntry> _root;

            public string Name { get => _project.Name; set => _project.Name = value; }
            public string WorkingDirectory => _project.ProjectPath;
            public string FileName => _project.FileName;
            public string FullPath => Path.Combine(WorkingDirectory, FileName);

            public string ShortName { get => _project.ShortName; set => _project.ShortName = value; }
            public string Company { get => _project.Company; set => _project.Company = value; }
            public string Producer { get => _project.Producer; set => _project.Producer = value; }
            public string Copyright { get => _project.Copyright; set => _project.Copyright = value; }
            public int Year { get => _project.Year; set => _project.Year = value; }
            public Version Version { get => _project.Version; set => _project.Version = value; }
            
            internal MyProject(Project project)
            {
                _project = project;

                var root = new List<IProjectEntry>(_project.Containers.Count);
                foreach (var container in _project.Containers)
                {
                    root.Add(ProcessContainer(container));
                }
                _root = root;
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return _root;
            }

            public void SaveChanges()
            {
                _project.Save();
            }

            public void SaveChanges(Stream stream)
            {
                _project.Save(stream);
            }

            private void WriteChanges()
            {
                _project.Containers = _root
                    .Where(x => x is ProjectContainer)
                    .Select(x => (x as ProjectContainer).AsContainer())
                    .ToList();
            }

            private IProjectEntry ProcessContainer(Project.Container container)
            {
                return new ProjectContainer(this, container);
            }
        }

        private class ProjectContainer : ProjectDirectory, IProjectContainer
        {
            private Project.Container _container;

            internal ProjectContainer(MyProject project, Project.Container container)
                : base(project, null, container.Name)
            {
                _container = container;
                Populate();
            }

            private void Populate()
            {
                var split = new char[] { '/', '\\' };
                foreach (var item in _container.Items)
                {
                    var path = item.Input;
                    var strPath = path.Split(split);
                    if (strPath.Length <= 0)
                        continue;
                    
                    IProjectDirectory projDir = this;
                    for (int i = 0; i < strPath.Length - 1 && projDir != null; i++)
                    {
                        var directory = strPath[i];

                        // Search for existing directory
                        var curProjDir = projDir.GetEntries()
                            .SingleOrDefault(x => x is IProjectDirectory && string.Compare(x.Name, directory, true) == 0)
                            as IProjectDirectory;

                        if (curProjDir == null)
                        {
                            // Directory not found, create it!
                            curProjDir = projDir.AddDirectory(directory);
                        }
                        projDir = curProjDir;
                    }

                    if (projDir != null)
                    {
                        var fileName = strPath[strPath.Length - 1];
                        (projDir as ProjectDirectory).AddFile(item);
                    }
                }
            }

            internal Project.Container AsContainer()
            {
                _container.Items.Clear();
                _container.Items.AddRange(GetLeafs()
                    .Select(x => x.AsItem()));
                return _container;
            }
            internal List<ProjectFile> GetLeafs()
            {
                var list = new List<ProjectFile>();
                foreach (var entry in GetEntries())
                {
                    if (entry is ProjectFile)
                        list.Add(entry as ProjectFile);
                    else if (entry is ProjectDirectory)
                        list.AddRange((entry as ProjectContainer).GetLeafs());
                }
                return list;
            }
        }

        private abstract class ProjectEntry : IProjectEntry
        {
            protected MyProject Project { get; }

            protected ProjectEntry Parent { get; set; }

            public abstract string Name { get; set; }

            public abstract bool CanRename { get; }

            public string Path => Parent != null ? System.IO.Path.Combine(Parent.Path, Name).Replace('\\', '/') : Name;

            public string FullPath => System.IO.Path.Combine(Project.WorkingDirectory, Path).Replace('\\', '/');

            internal ProjectEntry(MyProject project, ProjectEntry parent)
            {
                Project = project;
                Parent = parent;
            }

            public abstract bool Remove(bool delete);
        }

        private class ProjectDirectory : ProjectEntry, IProjectDirectory
        {
            public override string Name { get; set; }

            public override bool CanRename => true;

            protected List<IProjectEntry> _entries = new List<IProjectEntry>();

            internal ProjectDirectory(MyProject project, ProjectEntry parent, string name) :
                base(project, parent)
            {
                Name = name;
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return _entries;
            }

            public IProjectDirectory AddDirectory(string name)
            {
                var entry = new ProjectDirectory(Project, this, name);
                Directory.CreateDirectory(entry.FullPath);
                _entries.Add(entry);
                return entry;
            }

            public IProjectFile AddFile(string name)
            {
                return AddFile(CreateDefaultItem(name));
            }

            public override bool Remove(bool delete)
            {
                foreach (var entry in _entries)
                    entry.Remove(delete);
                if (Directory.Exists(FullPath))
                {
                    Directory.Delete(FullPath, true);
                }
                return false;
            }

            internal IProjectFile AddFile(Project.Item item)
            {
                var entry = new ProjectFile(Project, this, item);
                var fullPath = entry.FullPath;
                if (!File.Exists(fullPath))
                {
                    File.Create(fullPath, 0, FileOptions.None).Close();
                }
                _entries.Add(entry);
                return entry;
            }
        }

        private class ProjectFile : ProjectEntry, IProjectFile
        {
            private Project.Item _item;
            private Dictionary<string, string> _parameters = new Dictionary<string, string>();
            private string _name;

            public override string Name { get => _name; set => _name = value; }

            public override bool CanRename => true;

            public string Format { get; set; }

            public IEnumerable<KeyValuePair<string, string>> Parameters => _parameters;

            internal ProjectFile(MyProject project, ProjectEntry parent, Project.Item item) :
                base(project, parent)
            {
                _item = item;
                _name = System.IO.Path.GetFileName(item.Input);
                Format = item.Type;
                _parameters = item.Parameters?.ToDictionary(x => x.Item1, x => x.Item2)
                    ?? new Dictionary<string, string>();
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
                return false;
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

            internal Project.Item AsItem()
            {
                _item.Input = Path;
                _item.Type = Format;
                _item.Parameters = _parameters
                    .Select(x => new Tuple<string, string>(x.Key, x.Value))
                    .ToList();
                return _item;
            }
        }
    }
}
