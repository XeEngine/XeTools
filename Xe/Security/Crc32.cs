using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Security
{
	public class Crc32 : IHashing<uint>
	{
		private static Crc32 Instance = new Crc32();

		private readonly uint[] _table;
		private uint _value;

		public Crc32(uint key = 0x9d7f97d6)
		{
			_table = new uint[256];
			for (uint i = 0; i < 256; i++)
			{
				var r = i;
				for (var j = 0; j < 8; j++)
					if ((r & 1) != 0)
						r = (r >> 1) ^ key;
					else
						r >>= 1;
				_table[i] = r;
			}
			Init();
		}
		public Crc32(Crc32 crc)
		{
			_table = crc._table;
			Init();
		}

		public void WriteByte(byte b)
		{
			_value = _table[(byte)(_value) ^ b] ^ (_value >> 8);
		}

		public void Write(byte[] data, uint offset, uint size)
		{
			for (uint i = 0; i < size; i++)
				_value = _table[(((byte)(_value)) ^ data[offset + i])] ^ (_value >> 8);
		}

		public uint GetDigest() { return _value ^ uint.MaxValue; }

		private void Init()
		{
			_value = uint.MaxValue;
		}

        public static uint CalculateDigestAscii(string str)
        {
            var data = Encoding.ASCII.GetBytes(str);
            return CalculateDigest(data, 0, (uint)data.Length);
        }
		public static uint CalculateDigest(byte[] data, uint offset, uint size)
		{
			Instance.Init();
			Instance.Write(data, offset, size);
			return Instance.GetDigest();
		}
	}
}
