using System.Xml.Serialization;

namespace libTools
{
    public partial class Project
    {
        /// <summary>
        /// Parametri che descrivono uno specifico oggetto.
        /// </summary>
        public class Parameter
        {
            [XmlAttribute]
            public string Name;
            [XmlText]
            public string Value;
        }
    }
}
