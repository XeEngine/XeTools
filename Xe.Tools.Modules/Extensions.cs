using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public static class Extensions
    {
        public static Stream OpenModuleSettings(this Project project, string moduleName, FileAccess access)
        {
            var path = Path.Combine(project.ProjectPath, ".settings/modules/");
            if (!Directory.Exists(path))
            {
                switch (access)
                {
                    case FileAccess.Read:
                        throw new DirectoryNotFoundException(path);
                    case FileAccess.Write:
                    case FileAccess.ReadWrite:
                        Directory.CreateDirectory(path);
                        break;
                }
            }

            var fileName = Path.Combine(path, $"{moduleName}.json");

            FileMode mode;
            FileShare share;
            switch (access)
            {
                case FileAccess.Read:
                    mode = FileMode.Open;
                    share = FileShare.ReadWrite | FileShare.Delete;
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
                    throw new ArgumentOutOfRangeException(nameof(access), "Invalid parameter");
            }

            return new FileStream(path, mode, access, share);
        }
    }
}
