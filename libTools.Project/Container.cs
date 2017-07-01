using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace libTools
{
    public partial class Project
    {
        /// <summary>
        /// Descrive un contenitore di oggetti.
        /// </summary>
        [Serializable]
        public class Container
        {
            [XmlIgnore]
            public Project Parent;

            /// <summary>
            /// Nome del contenitore e cartella dove si troveranno tutti gli oggetti.
            /// </summary>
            [XmlAttribute]
            public string Name;
            /// <summary>
            /// Indica se usare un contenitore binario come output.
            /// </summary>
            [XmlAttribute]
            public bool UseContainer;
            /// <summary>
            /// Indica se è necessario offuscare il file binario.
            /// </summary>
            [XmlAttribute]
            public bool UseObfuscation;
            /// <summary>
            /// Lista degli oggetti contenuti nel contenitore corrente.
            /// </summary>
            [XmlArray]
            public List<Item> Items;

            public string ProcessPath(string filename, bool relative)
            {
                if (filename == null) return "";
                if (filename.Contains("$(InputDir)"))
                {
                    string path;
                    if (!Parent.GetVariable("Path", out path))
                        throw new VariableNotFoundException("Path");
                    var basepath = Name;
                    if (!relative) basepath = System.IO.Path.Combine(path, basepath);
                    filename = filename.Replace("$(InputDir)", basepath);
                }
                if (filename.Contains("$(TempDir)"))
                {
                    string path;
                    if (!Parent.GetVariable("Path", out path))
                        throw new VariableNotFoundException("Path");
                    var basepath = System.IO.Path.Combine(Name, ".tmp");
                    if (!relative) basepath = System.IO.Path.Combine(path, basepath);
                    filename = filename.Replace("$(TempDir)", basepath);
                }
                if (filename.Contains("$(OutputDir)"))
                {
                    string path;
                    if (!Parent.GetVariable("OutputPath", out path))
                        throw new VariableNotFoundException("OutputPath");
                    var basepath = Name;
                    if (!relative) basepath = System.IO.Path.Combine(path, basepath);
                    filename = filename.Replace("$(OutputDir)", basepath);
                }
                try
                {
                    return Parent.ProcessString(filename);
                }
                catch (VariableNotFoundException)
                {
                    return filename;
                }
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
