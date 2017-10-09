using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Xe.Tools
{
	class VariableNotFoundException : Exception
	{
		public VariableNotFoundException(string variable) :
			base($"Variable $({variable}) was not found.")
		{ }
	}

	public partial class Project : IInfoLastEdit
    {
        private string _filename;
        private string _projectFileName;
        private string _projectPath;
        private Version _version = new Version();
        private List<Container> _containers = new List<Container>();
		
		[JsonIgnore]
		public string FileName
		{
			get { return _filename; }
			private set
			{
				_filename = value;
                _projectFileName = Path.GetFileName(_filename);
                _projectPath = Path.GetDirectoryName(Path.GetFullPath(_filename));
			}
		}

		[JsonIgnore]
		public string ProjectFileName { get => _projectFileName; }
		
		[JsonIgnore]
		public string ProjectPath { get => _projectPath; }

		public string Name { get; set; }
		public string ShortName { get; set; }
		public Version Version
		{
            get => _version;
            set { _version = value != null ? _version : new Version(); }
		}
		public string Company { get; set; }
		public string Producer { get; set; }
		public string Copyright { get; set; }
		public int Year { get; set; }

        public List<Container> Containers
        {
            get => _containers;
            set { _containers = value != null ? _containers : new List<Container>(); }
        }

        public string ProcessString(string str)
		{
			int i = 0;
			while (i < str.Length - 2)
			{
				if (str[i + 0] == '$' && str[i + 1] == '(')
				{
					int finish = str.IndexOf(')', i + 2);
					if (finish > 0)
					{
						var id = str.Substring(i, finish - i);
						if (Environment.Variables.TryGetValue(id, out object value))
							str = $"{str.Substring(0, i)}{value}{str.Substring(finish + i, str.Length)}";
						else
							throw new VariableNotFoundException(id);
					}
					else
						return str;
				}
				else i++;
			}
			return str;
		}

		public DateTime? GetInfoLastEdit()
		{
			if (!File.Exists(_filename))
				return null;
			return File.GetLastWriteTimeUtc(_filename);
		}

		public DateTime? GetInfoLastEditRecursive()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			Save(FileName);
		}
		public void Save(string filename)
		{
            foreach (var container in Containers)
                container.Items = container.Items.OrderBy(x => x.Input).ToList();
            Containers = Containers.OrderBy(x => x.Name).ToList();
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                Save(stream);
            }
        }
        public void Save(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }

		public static Project Open(string filename)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException();

			Project project;
			switch (Path.GetExtension(filename))
			{
				case ".xml": project = OpenXml(filename); break;
				case ".json": project = OpenJson(filename); break;
				default:
					throw new ArgumentException($"{filename} seems to not be a xml or json file.");
			}
			project.FileName = filename;
			return project;
		}

		private static Project OpenXml(string filename)
		{
			throw new NotImplementedException();
		}
		private static Project OpenJson(string filename)
		{
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				using (var reader = new StreamReader(file))
				{
					return JsonConvert.DeserializeObject<Project>(reader.ReadToEnd());
				}
			}
		}
	}
}
