using System;

namespace Xe
{

    public class UniqueObject : IComparable, IComparable<UniqueObject>
    {
        private Guid mUID = Guid.NewGuid();
        public Guid UID
        {
            get { return mUID; }
            set { mUID = value; }
        }

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
