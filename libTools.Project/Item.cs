using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace libTools
{
    public partial class Project
    {
        /// <summary>
        /// Descrive un singolo oggetto.
        /// </summary>
        public class Item
        {
            private Parameter[] _tmpParameters = new Parameter[0];
            private Dictionary<string, List<string>> _parameters = new Dictionary<string, List<string>>();

            [XmlIgnore]
            public Container Parent;

            /// <summary>
            /// Ogni oggetto ha un proprio tipo che ne descriverà il contenuto.
            /// </summary>
            [XmlAttribute]
            public string Type;
            /// <summary>
            /// Nome human-friendly da dare ad un oggetto.
            /// </summary>
            [XmlAttribute]
            public string Alias;
            /// <summary>
            /// Nome del file o percorso da caricare in input.
            /// Il percorso non è processato.
            /// </summary>
            [XmlAttribute()]
            public string Input;
            /// <summary>
            /// Nome del file o percorso usato come output.
            /// </summary>
            [XmlAttribute()]
            public string Output;

            /// <summary>
            /// Lista dei parametri per modificare il comporamento di un oggetto.
            /// </summary>
            [XmlElement("Parameter")]
            public Parameter[] Parameters
            {
                get { return _tmpParameters; }
                set
                {
                    _parameters.Clear();
                    if (value == null) return;
                    foreach (var item in value)
                    {
                        List<string> items;
                        if (_parameters.TryGetValue(item.Name, out items))
                            items.Add(item.Value);
                        else
                            _parameters.Add(item.Name, new List<string>() { item.Value });
                    }
                    _tmpParameters = value;
                }
            }

            /// <summary>
            /// Lista dei parametri per modificare il comporamento di un oggetto.
            /// </summary>
            [XmlIgnore]
            private Dictionary<string, List<string>> Parameters2
            {
                get { return _parameters; }
            }
            //public Dictionary<string, string> Parameters2 = new Dictionary<string, string>();

            /// <summary>
            /// Come Input, ma fornisce un percorso concreto del file.
            /// </summary>
            public string FileNameInput
            { get { return Parent.ProcessPath(Input, false); } }
            /// <summary>
            /// Come Output, ma fornisce un percorso concreto del file.
            /// </summary>
            public string FileNameOutput
            { get { return Parent.ProcessPath(Output, false); } }

            public string RelativeFileNameInput
            { get { return Parent.ProcessPath(Input, true); } }
            public string RelativeFileNameOutput
            { get { return Parent.ProcessPath(Output, true); } }

            public string ProcessPath(string path, bool relative)
            {
                return Parent.ProcessPath(path, relative);
            }

            public override string ToString()
            {
                if (Alias != null && Alias.Length > 0) return Alias;
                if (Input == null) return "";
                return System.IO.Path.GetFileName(Input);
            }
        }
    }
}
