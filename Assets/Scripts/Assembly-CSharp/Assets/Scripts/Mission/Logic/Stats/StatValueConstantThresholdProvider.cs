using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueConstantThresholdProvider<T> : IStatValueThresholdProvider<T> where T : class, IStatValueWrapper, IComparable<T>, new()
	{
		private readonly T m_Threshold;

		public StatValueConstantThresholdProvider(T _threshold)
		{
			m_Threshold = (T)_threshold.Clone();
		}

		public T GetThreshold(StatValueClamped<T> _statValue)
		{
			return m_Threshold;
		}

		public IStatValueThresholdProvider<T> Clone()
		{
			return new StatValueConstantThresholdProvider<T>(m_Threshold);
		}
	}
}
