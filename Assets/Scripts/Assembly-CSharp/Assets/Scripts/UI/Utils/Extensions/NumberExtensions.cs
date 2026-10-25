namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class NumberExtensions
	{
		public static int GetActiveBitCount(long _lValue)
		{
			int num = 0;
			while (_lValue != 0L)
			{
				_lValue &= _lValue - 1;
				num++;
			}
			return num;
		}

		public static bool IsBitSet(this int _value, int _pos)
		{
			return (_value & _pos) != 0;
		}
	}
}
