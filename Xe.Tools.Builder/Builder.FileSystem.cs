using System.IO;
using System.Linq;
using System.Text;

namespace Xe.Tools.Builder
{
    public partial class Builder
    {
		public void ExportFileSystem(string fileName)
        {
            using (var fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fStream))
                {
                    var count = FileNames.Count;
                    writer.Write(count);

                    var entries = FileNames
                        .Select(x => x.Replace('\\', '/'))
                        .Distinct()
                        .Select(x => new
                        {
                            Hash = x.GetXeHash(),
                            Name = x,
                            Data = Encoding.UTF8.GetBytes(x)
                        })
                        .OrderBy(x => x.Hash);

                    foreach (var item in entries)
                    {
                        writer.Write(item.Hash);
                    }

                    int offset = 4 + count * 8;
                    foreach (var item in entries)
                    {
                        writer.Write(offset);
                        offset += item.Data.Length + 1;
                    }

                    foreach (var item in entries)
                    {
                        writer.Write(item.Data);
                        writer.Write('\0');
                    }
                }
            }
        }
    }
}
