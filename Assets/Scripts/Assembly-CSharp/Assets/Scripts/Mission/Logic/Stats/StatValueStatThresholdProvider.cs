using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueStatThresholdProvider<T> : IStatValueThresholdProvider<T> where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		private readonly StatTypes.EStatType m_StatType;

		private readonly T m_ValueWrapper = new T();

		public StatValueStatThresholdProvider(StatTypes.EStatType _statType)
		{
			m_StatType = _statType;
		}

		public T GetThreshold(StatValueClamped<T> _statValue)
		{
			if (m_StatType == StatTypes.EStatType.Invalid)
			{
				return (T)null;
			}
			StatValue<T> statValue = (StatValue<T>)_statValue.StatValueCollection.GetReadonlyStatValueBase(m_StatType);
			statValue.CopyTo(m_ValueWrapper);
			return m_ValueWrapper;
		}

		public IStatValueThresholdProvider<T> Clone()
		{
			return new StatValueStatThresholdProvider<T>(m_StatType);
		}
	}
}
