using System;

namespace UbiCore.Utils
{
	[Serializable]
	public sealed class FloatRange : Range<float>
	{
		public FloatRange(float _min, float _max)
			: base(_min, _max)
		{
		}
	}
}
