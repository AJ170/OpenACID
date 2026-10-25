using System;

namespace UbiCore.Utils
{
	[Serializable]
	public sealed class IntRange : Range<int>
	{
		public IntRange(int _min, int _max)
			: base(_min, _max)
		{
		}
	}
}
