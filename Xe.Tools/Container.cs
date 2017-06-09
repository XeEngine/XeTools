using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Xe.Tools
{
	public partial class Project
	{
		public class Container
		{
			[JsonIgnore]
			public Project Parent;

			public string Name { get; set; }

			public List<Item> Items;

			public string ProcessPath(string filename, bool relative)
			{
				if (filename == null) return "";
                if (Parent == null) relative = true;

				if (filename.Contains("$(InputDir)"))
				{
					var basepath = Name;
					if (!relative) basepath = System.IO.Path.Combine(Parent.ProjectPath, basepath);
					filename = filename.Replace("$(InputDir)", basepath);
				}
				if (filename.Contains("$(TempDir)"))
				{
					var basepath = System.IO.Path.Combine(Name, ".tmp");
					if (!relative)
						basepath = System.IO.Path.Combine(Parent.ProjectPath, basepath);
					filename = filename.Replace("$(TempDir)", basepath);
				}
				if (filename.Contains("$(OutputDir)"))
				{
					string path = Environment.Variables["ProjectFileOutputDir"].ToString();
					var basepath = Name;
					if (!relative) basepath = System.IO.Path.Combine(path, Name);
					filename = filename.Replace("$(OutputDir)", basepath);
				}
				return Parent?.ProcessString(filename) ?? filename;
			}

			/// <summary>
			/// Ottiene una lista di tutti gli oggetti del tipo specificato.
			/// </summary>
			/// <param name="name">Tipo da specificare.</param>
			/// <returns>Enumeratore con tutti gli oggetti richiesti.</returns>
			public IEnumerable<Item> GetItemsFromType(string name)
			{
				//return Items.Where(x => string.Compare(x.Type, name, true) == 0).Distinct(new NameEqualityComparer());
				return Items.Where(x => string.Compare(x.Type, name, true) == 0);
			}

			public override string ToString()
			{
				return Name;
			}
		}
	}
}
