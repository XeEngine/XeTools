using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Xe.Tools
{
	public partial class Project
	{
		/// <summary>
		/// Descrive un singolo oggetto.
		/// </summary>
		public class Item
		{
			[JsonIgnore]
			public Container Parent;

			/// <summary>
			/// Ogni oggetto ha un proprio tipo che ne descriverà il contenuto.
			/// </summary>
			public string Type { get; set; }

            /// <summary>
            /// Nome human-friendly da dare ad un oggetto.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
			public string Alias { get; set; }

            /// <summary>
            /// Nome del file o percorso da caricare in input.
            /// Il percorso non è processato.
            /// </summary>
            public string Input { get; set; }

            /// <summary>
            /// Nome del file o percorso usato come output.
            /// </summary>
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Output { get; set; }

			/// <summary>
			/// Lista dei parametri per modificare il comporamento di un oggetto.
			/// </summary>
			public List<Tuple<string, string>> Parameters { get; set; }
            
            /// <summary>
            /// Come Input, ma fornisce un percorso concreto del file.
            /// </summary>
            [JsonIgnore]
            public string FileNameInput
			{ get { return Parent?.ProcessPath(Input, false); } }

            /// <summary>
            /// Come Output, ma fornisce un percorso concreto del file.
            /// </summary>
            [JsonIgnore]
            public string FileNameOutput
			{ get { return Parent?.ProcessPath(Output, false); } }

            [JsonIgnore]
            public string RelativeFileNameInput
			{ get { return Parent?.ProcessPath(Input, true); } }

            [JsonIgnore]
            public string RelativeFileNameOutput
			{ get { return Parent?.ProcessPath(Output, true); } }

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
