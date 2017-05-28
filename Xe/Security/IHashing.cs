using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Security
{
    public interface IHashing<T>
    {
		void WriteByte(byte b);
		void Write(byte[] data, uint offset, uint size);

		T GetDigest();
    }
}
