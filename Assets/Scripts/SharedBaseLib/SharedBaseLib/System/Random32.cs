namespace SharedBaseLib.System
{
	public class Random32
	{
		private const int RandPower = 32;

		private const uint RandMax = uint.MaxValue;

		private uint m_RandTmp;

		private uint m_RandOut;

		public Random32(uint _seed)
		{
			m_RandOut = _seed;
			m_RandTmp = m_RandOut ^ 0x67452301u;
		}

		public uint Rand()
		{
			m_RandTmp ^= m_RandOut;
			m_RandOut += m_RandTmp;
			return m_RandOut;
		}

		public uint Rand(uint _Max)
		{
			return Rand() % (_Max + 1);
		}

		public uint Rand(uint _Min, uint _Max)
		{
			return _Min + Rand(_Max - _Min);
		}

		public int Rand(int _Min, int _Max)
		{
			return (int)(_Min + Rand((uint)(_Max - _Min)));
		}

		public float RandF()
		{
			return (float)(Rand() >> 8) / 16777215f;
		}

		public float Rand(float _Max)
		{
			return RandF() * _Max;
		}

		public float Rand(float _Min, float _Max)
		{
			return _Min + Rand(_Max - _Min);
		}

		public float RandX()
		{
			return (float)(Rand() >> 8) / 16777216f;
		}

		public float RandX(float _Max)
		{
			return RandX() * _Max;
		}

		public float RandX(float _Min, float _Max)
		{
			return _Min + RandX(_Max - _Min);
		}

		public bool Bernoulli(float _P = 0.5f)
		{
			return RandX() < _P;
		}
	}
}
