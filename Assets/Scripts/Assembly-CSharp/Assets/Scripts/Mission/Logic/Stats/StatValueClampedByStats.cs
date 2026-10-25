using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueClampedByStats<T> : StatValueClamped<T> where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		public StatValueClampedByStats(StatTypes.EStatType _statType, StatValueCollection _statValueCollection, StatTypes.EStatType _minValueStatType, StatTypes.EStatType _maxValueStatType)
			: base(_statType, _statValueCollection, (IStatValueThresholdProvider<T>)new StatValueStatThresholdProvider<T>(_minValueStatType), (IStatValueThresholdProvider<T>)new StatValueStatThresholdProvider<T>(_maxValueStatType))
		{
		}
	}
}
