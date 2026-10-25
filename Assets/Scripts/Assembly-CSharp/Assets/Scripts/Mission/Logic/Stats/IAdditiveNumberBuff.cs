using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IAdditiveNumberBuff : ICloneable, IComparable<IBuff>, IBuff, INumberBuff
	{
	}
}
