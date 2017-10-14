using System;

namespace Tiled
{
    public class Version : IComparable<Version>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Revision { get; }

        public Version(int major, int minor, int revision)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
        }

        public int CompareTo(Version other)
        {
            int r;
            if (((r = Major - other.Major) != 0) ||
                ((r = Minor - other.Minor) != 0) ||
                ((r = Revision - other.Revision) != 0))
                return r;
            return 0;
        }

        public override string ToString()
        {
            return Revision > 0 ? $"{Major}.{Minor}.{Revision}" : $"{Major}.{Minor}";
        }

        public static bool TryParse(string str, out Version version)
        {
            if (string.IsNullOrEmpty(str))
            {
                version = null;
                return false;
            }
            var strs = str.Split('.');
            if (strs.Length < 2 ||
                !int.TryParse(strs[0], out var major) ||
                !int.TryParse(strs[1], out var minor))
            {
                version = null;
                return false;
            }
            if (strs.Length < 3 || !int.TryParse(strs[2], out var revision))
            {
                revision = 0;
            }
            version = new Version(major, minor, revision);
            return true;
        }
    }
}
