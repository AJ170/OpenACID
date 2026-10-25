using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatValueThresholdProvider<T> where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		T GetThreshold(StatValueClamped<T> _statValue);

		IStatValueThresholdProvider<T> Clone();
	}
}
