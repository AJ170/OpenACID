using System;

namespace SharedBaseLib.Core
{
	public sealed class StringCRC32
	{
		public const uint DefaultPolynomial = 3988292384u;

		public const uint DefaultSeed = uint.MaxValue;

		private static uint[] s_defaultTable;

		private readonly uint m_Seed;

		private readonly uint[] m_Table;

		private uint m_Hash;

		static StringCRC32()
		{
			InitializeTable(uint.MaxValue);
		}

		private StringCRC32()
			: this(3988292384u, uint.MaxValue)
		{
		}

		private StringCRC32(uint _polynomial, uint _seed)
		{
			m_Table = InitializeTable(_polynomial);
			m_Seed = (m_Hash = _seed);
		}

		public static uint ComputeLowerCase(string _string)
		{
			return Compute(uint.MaxValue, _string, true);
		}

		public static uint Compute(string _string)
		{
			return Compute(uint.MaxValue, _string, false);
		}

		public static uint Compute(uint _seed, string _string, bool _lowercase)
		{
			return Compute(3988292384u, _seed, _string, _lowercase);
		}

		public static uint Compute(uint _polynomial, uint _seed, string _string, bool _lowercase)
		{
			return ~CalculateHash(InitializeTable(_polynomial), _seed, _string, _lowercase);
		}

		private static uint[] InitializeTable(uint _polynomial)
		{
			if (_polynomial == 3988292384u && s_defaultTable != null)
			{
				return s_defaultTable;
			}
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint num = (uint)i;
				for (int j = 0; j < 8; j++)
				{
					num = (((num & 1) != 1) ? (num >> 1) : ((num >> 1) ^ _polynomial));
				}
				array[i] = num;
			}
			if (_polynomial == 3988292384u)
			{
				s_defaultTable = array;
			}
			return array;
		}

		private static uint CalculateHash(uint[] _table, uint _seed, string _string, bool _lowercase)
		{
			if (_string == null)
			{
				throw new ArgumentNullException("_string");
			}
			uint num = _seed;
			for (int i = 0; i < _string.Length; i++)
			{
				uint num2 = ((_lowercase ? char.ToLowerInvariant(_string[i]) : _string[i]) ^ num) & 0xFFu;
				if (num2 >= _table.Length)
				{
					throw new ArgumentOutOfRangeException(string.Format("{0} is larger that {1}", num2, _table.Length));
				}
				num = (num >> 8) ^ _table[num2];
			}
			return num;
		}

		private static byte[] UInt32ToBigEndianBytes(uint _uint32)
		{
			byte[] bytes = BitConverter.GetBytes(_uint32);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			return bytes;
		}
	}
}
