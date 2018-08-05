using System;

namespace Xe
{

    public class UniqueObject : IComparable, IComparable<UniqueObject>
    {
        public Guid UID { get; set; }

        public int CompareTo(object obj)
        {
            if (obj is UniqueObject)
                return CompareTo(obj as UniqueObject);
            return int.MaxValue;
        }

        public int CompareTo(UniqueObject other)
        {
            return UID.CompareTo(other.UID);
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return UID.GetHashCode();
        }
    }
}
