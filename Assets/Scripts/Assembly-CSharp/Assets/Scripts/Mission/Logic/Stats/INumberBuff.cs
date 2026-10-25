using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface INumberBuff : ICloneable, IComparable<IBuff>, IBuff
	{
		float Magnitude { get; }
	}
}
