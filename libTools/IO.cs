using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libTools
{
    [Serializable]
    class InvalidFileFormatException : Exception
    {
        public InvalidFileFormatException()
        {
        }

        public InvalidFileFormatException(string message)
        : base(message)
        {
        }

        public InvalidFileFormatException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    public abstract class IO<T> : IItem
    {
        private string filename;
        private string filepath;
        private string filenamenoext;
        private string fileext;
        [JsonIgnore]
        public string FileName {
            get { return filename; }
            set
            {
                filename = value;
                filepath = Path.GetDirectoryName(value);
                filenamenoext = Path.GetFileNameWithoutExtension(value);
                fileext = Path.GetExtension(value);
            }
        }
        [JsonIgnore]
        public string FilePath { get { return filepath; } }
        [JsonIgnore]
        public string FileNameNoExt { get { return filenamenoext; } }
        [JsonIgnore]
        public string Extension { get { return fileext; } }

        protected IO() { }
        public IO(string filename)
        {
            OpenFile(filename);
        }
        public IO(FileStream stream)
        {
            OpenFile(stream);
        }

        private void ProcessFilename(string filename)
        {
            FileName = filename;
            if (string.Compare(fileext, ".json") == 0)
                filenamenoext = Path.GetFileNameWithoutExtension(filenamenoext);
        }
        private void OpenFile(string filename)
        {
            OpenFile(OpenRead(filename));
        }
        private void OpenFile(FileStream stream)
        {
            ProcessFilename(stream.Name);
            if (string.Compare(Path.GetExtension(filename), ".json") == 0)
            {
                using (var streamReader = new StreamReader(stream))
                {
                    string json = streamReader.ReadToEnd();
                    var obj = JsonConvert.DeserializeObject<T>(json);
                    MyLoad(obj);
                }
            }
            else
            {
                Import(new BinaryReader(stream));
            }
        }
        public void Save()
        {
            Save(FileName);
        }
        public void Save(string filename)
        {
            this.filename = filename;
            string outFilename;
            if (string.Compare(Path.GetExtension(filename), ".json") == 0)
                outFilename = filename;
            else
                outFilename = filename + ".json";
            using (var sw = new StreamWriter(outFilename))
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        public void Export(string filename)
        {
            using (var writer = new BinaryWriter(OpenWrite(filename)))
            {
                Export(writer);
                writer.Flush();
            }
        }

        protected abstract void MyLoad(T item);
        protected abstract void Import(BinaryReader reader);
        protected abstract void Export(BinaryWriter writer);

        protected static string ReadString(BinaryReader r)
        {
            var len = r.ReadByte();
            if (len == 0) return "";
            var bytes = r.ReadBytes(len);
            return Encoding.ASCII.GetString(bytes);
        }
        protected static int WriteString(BinaryWriter w, string str)
        {
            if (str == null) str = "";
            var bytes = Encoding.ASCII.GetBytes(str);
            byte len = (byte)Math.Min(bytes.Length, byte.MaxValue);
            w.Write(len);
            w.Write(bytes, 0, len);
            return len;
        }
        private FileStream OpenRead(string filename)
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read);
        }
        private FileStream OpenWrite(string filename)
        {
            return new FileStream(filename, FileMode.Create, FileAccess.Write);
        }
    }
}
