using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueClampedByConstants<T> : StatValueClamped<T> where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		public StatValueClampedByConstants(StatTypes.EStatType _statType, StatValueCollection _statValueCollection, T _minValue, T _maxValue)
			: base(_statType, _statValueCollection, (IStatValueThresholdProvider<T>)new StatValueConstantThresholdProvider<T>(_minValue), (IStatValueThresholdProvider<T>)new StatValueConstantThresholdProvider<T>(_maxValue))
		{
		}
	}
}
