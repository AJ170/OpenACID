using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueClamped<T> : StatValue<T>, IStatValueCollectionCarrier, IReadonlyStatValueBase where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		private readonly IStatValueThresholdProvider<T> m_MinValueThresholdProvider;

		private readonly IStatValueThresholdProvider<T> m_MaxValueThresholdProvider;

		private readonly StatValueCollection m_StatValueCollection;

		public override StatValueCollection StatValueCollection
		{
			get
			{
				return m_StatValueCollection;
			}
		}

		public T MinValue
		{
			get
			{
				if (m_MinValueThresholdProvider == null)
				{
					return (T)null;
				}
				return m_MinValueThresholdProvider.GetThreshold(this);
			}
		}

		public T MaxValue
		{
			get
			{
				if (m_MaxValueThresholdProvider == null)
				{
					return (T)null;
				}
				return m_MaxValueThresholdProvider.GetThreshold(this);
			}
		}

		public StatValueClamped(StatTypes.EStatType _statType, StatValueCollection _statValueCollection, IStatValueThresholdProvider<T> _minValueThresholdProvider, IStatValueThresholdProvider<T> _maxValueThresholdProvider)
			: base(_statType, _statValueCollection)
		{
			m_MinValueThresholdProvider = _minValueThresholdProvider;
			m_MaxValueThresholdProvider = _maxValueThresholdProvider;
			m_StatValueCollection = _statValueCollection;
		}

		protected override void FixValue(T _statValueWrapper, out bool _changed)
		{
			_changed = false;
			T minValue = MinValue;
			if (minValue != null && _statValueWrapper.CompareTo(minValue) < 0)
			{
				_changed = true;
				_statValueWrapper.CopyFrom(minValue);
			}
			T maxValue = MaxValue;
			if (maxValue != null && _statValueWrapper.CompareTo(maxValue) > 0)
			{
				_changed = true;
				_statValueWrapper.CopyFrom(maxValue);
			}
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatTypes.EStatType statType = StatType;
			IStatValueThresholdProvider<T> minValueThresholdProvider;
			if (m_MinValueThresholdProvider == null)
			{
				IStatValueThresholdProvider<T> statValueThresholdProvider = null;
				minValueThresholdProvider = statValueThresholdProvider;
			}
			else
			{
				minValueThresholdProvider = m_MinValueThresholdProvider.Clone();
			}
			IStatValueThresholdProvider<T> maxValueThresholdProvider;
			if (m_MaxValueThresholdProvider == null)
			{
				IStatValueThresholdProvider<T> statValueThresholdProvider = null;
				maxValueThresholdProvider = statValueThresholdProvider;
			}
			else
			{
				maxValueThresholdProvider = m_MaxValueThresholdProvider.Clone();
			}
			StatValueClamped<T> statValueClamped = new StatValueClamped<T>(statType, _newStatValueCollection, minValueThresholdProvider, maxValueThresholdProvider);
			T valueWrapper = statValueClamped.m_ValueWrapper;
			valueWrapper.CopyFrom(m_ValueWrapper);
			return statValueClamped;
		}
	}
}
