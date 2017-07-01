using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace libTools
{
    [Serializable]
    class VariableNotFoundException : Exception
    {
        public VariableNotFoundException(string variable) :
            base(string.Format("Variable $({0}) is not registered.", variable))
        { }
    }

    /// <summary>
    /// Gestione di un progetto contenente files da processare.
    /// </summary>
    [Serializable]
    public partial class Project
    {
        class NameEqualityComparer : IEqualityComparer<Item>
        {
            public bool Equals(Item x, Item y)
            {
                return string.Compare(x.FileNameInput, y.FileNameInput, true) == 0;
            }

            public int GetHashCode(Item obj)
            {
                return obj.FileNameInput.GetHashCode();
            }
        }

        private string mName;
        private string mFullName;
        private string mCompany;
        private string mYear;
        private int mVersionMajor;
        private int mVersionMinor;
        private int mVersionRevision;

        private string mFileName;
        private string mPath;
        private string mFullPath;
        private string mOutputPath;

        [XmlAttribute]
        public string Name
        {
            get { return mName; }
            set { mVars["Name"] = mName = value; }
        }
        [XmlAttribute]
        public int VersionMajor
        {
            get { return mVersionMajor; }
            set { mVersionMajor = value; }
        }
        [XmlAttribute]
        public int VersionMinor
        {
            get { return mVersionMinor; }
            set { mVersionMinor = value; }
        }
        [XmlAttribute]
        public int VersionRevision
        {
            get { return mVersionRevision; }
            set { mVersionRevision = value; }
        }
        [XmlAttribute]
        public string FullName
        {
            get { return mFullName; }
            set { mVars["FullName"] = mFullName = value; }
        }
        [XmlAttribute]
        public string Company
        {
            get { return mCompany; }
            set { mVars["Company"] = mCompany = value; }
        }
        [XmlAttribute]
        public string Year
        {
            get { return mYear; }
            set { mVars["Year"] = mYear = value; }
        }

        [XmlIgnore]
        public string Version
        {
            get
            {
                return string.Format("{0}.{1}.{2}",
                    mVersionMajor,
                    mVersionMinor,
                    mVersionRevision);
            }
        }

        [XmlIgnore]
        public string FileName
        {
            get { return mFileName; }
            set { mVars["FileName"] = mFileName = value; }
        }
        [XmlIgnore]
        public string Path
        {
            get { return mPath; }
            set { mVars["Path"] = mPath = value; }
        }
        [XmlIgnore]
        public string FullPath
        {
            get { return mFullPath; }
            set { mVars["FullPath"] = mFullPath = value; }
        }
        [XmlIgnore]
        public string OutputPath
        {
            get { return mOutputPath; }
            set { mVars["OutputPath"] = mOutputPath = value; }
        }

        [XmlArray]
        public Container[] Containers;

        [XmlIgnore]
        private Dictionary<string, string> mVars = new Dictionary<string, string>();

        [XmlIgnore]
        private DateTime ModifyDateTime;

        public DateTime GetDateTime()
        {
            return ModifyDateTime;
        }

        public static Project Open(string filename)
        {
            if (File.Exists(filename))
            {
                using (var reader = new StreamReader(filename))
                {
                    var project = new XmlSerializer(typeof(Project)).Deserialize(reader) as Project;
                    project.FullPath = System.IO.Path.GetFullPath(filename);
                    project.Path = System.IO.Path.GetDirectoryName(filename);
                    project.FileName = System.IO.Path.GetFileName(filename);
                    project.ModifyDateTime = File.GetLastWriteTimeUtc(filename);
                    foreach (var container in project.Containers)
                    {
                        container.Parent = project;
                        foreach (var item in container.Items)
                            item.Parent = container;
                    }

                    return project;
                }
            }
            else throw new FileNotFoundException();
        }
        public void Save(string filename)
        {
            using (var writer = XmlWriter.Create(filename, new System.Xml.XmlWriterSettings { Indent = true })) {
                new XmlSerializer(typeof(Project)).Serialize(writer, this);
                writer.Flush();
            }
        }

        public bool GetVariable(string key, out string value)
        {
            return mVars.TryGetValue(key, out value);
        }
        private void SetVariable(string key, string value)
        {
            mVars[key] = value;
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
                        string value;
                        var id = str.Substring(i, finish - i);
                        if (!GetVariable(id, out value))
                            throw new VariableNotFoundException(id);
                        str = string.Format("{0}{1}{2}",
                            str.Substring(0, i),
                            value,
                            str.Substring(finish + i, str.Length));
                    }
                    else return str;
                }
                else i++;
            }
            return str;
        }

        /// <summary>
        /// Ottiene il contenitore con il nome specificato.
        /// </summary>
        /// <param name="name">Nome del contenitore da estrarre.</param>
        /// <returns>Contenitore estratto. Può ritornare null.</returns>
        public Container GetContainer(string name)
        {
            var r = Containers.Where(x => x.Name.CompareTo(name) == 0);
            if (r.Count() == 0) return null;
            return r.First();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
