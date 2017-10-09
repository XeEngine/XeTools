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

        public bool TryOpen(Stream stream)
        {
            try
            {
                Project model;
                using (var reader = new StreamReader(stream))
                {
                    model = JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryOpen(string directory)
        {
            return false;
        }

        public IProject Open(Stream stream)
        {
            Project model;
            using (var reader = new StreamReader(stream))
            {
                model = JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
            }
            return new MyProject(model);
        }

        public IProject Open(string directory)
        {
            return null;
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
                return null;
            }
        }

        private class ProjectContainer : IProjectDirectory
        {
            private Project.Container _container;

            private List<IProjectEntry> _entries = new List<IProjectEntry>();

            public string Name { get => _container.Name; set => _container.Name = value; }

            public bool CanRename => throw new System.NotImplementedException();

            internal ProjectContainer(Project.Container container)
            {
                _container = container;
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

                    int index = 0;
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
                        var fileName = strPath[index];
                        projDir.AddFile(fileName);
                    }
                }
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return _entries;
            }

            public IProjectDirectory AddDirectory(string name)
            {
                var entry = new ProjectDirectory(null, name);
                _entries.Add(entry);
                return entry;
            }

            public IProjectFile AddFile(string name)
            {
                return AddFile(CreateDefaultItem(name));
            }

            public bool Remove(bool delete)
            {
                bool r = false;
                foreach (var entry in _entries)
                    r |= entry.Remove(delete);
                return r;
            }

            private IProjectFile AddFile(Project.Item item)
            {
                var entry = new ProjectFile(null, item);
                _entries.Add(entry);
                return entry;
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
            protected ProjectEntry Parent { get; set; }

            public abstract string Name { get; set; }

            public abstract bool CanRename { get; }

            public string Path => Parent != null ? System.IO.Path.Combine(Parent.Name, Name) : Name;

            internal ProjectEntry(ProjectEntry parent)
            {
                Parent = parent;
            }

            public abstract bool Remove(bool delete);
        }

        private class ProjectDirectory : ProjectEntry, IProjectDirectory
        {
            public override string Name { get; set; }

            public override bool CanRename => true;

            private List<IProjectEntry> _entries = new List<IProjectEntry>();

            internal ProjectDirectory(ProjectEntry parent, string name) :
                base(parent)
            {
                Name = name;
            }

            public IEnumerable<IProjectEntry> GetEntries()
            {
                return _entries;
            }

            public IProjectDirectory AddDirectory(string name)
            {
                var entry = new ProjectDirectory(this, name);
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
                return false;
            }

            private IProjectFile AddFile(Project.Item item)
            {
                var entry = new ProjectFile(this, item);
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

            internal ProjectFile(ProjectEntry parent, Project.Item item) :
                base(parent)
            {
                _item = item;
                _name = System.IO.Path.GetFileName(item.Input);
                Format = item.Type;
                _parameters = item.Parameters.ToDictionary(x => x.Item1, x => x.Item2);
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
