using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class StatsHelperBase : IStatsHelper
	{
		private readonly bool[] m_ContainsCache = new bool[387];

		public StatTypes.StatsRange ImmutableRange { get; private set; }

		public StatTypes.StatsRange MutableRange { get; private set; }

		public StatTypes.StatsRange ComputedRange { get; private set; }

		protected StatsHelperBase(StatTypes.StatsRange _immutableRange, StatTypes.StatsRange _mutableRange, StatTypes.StatsRange _computedRange)
		{
			ImmutableRange = _immutableRange;
			MutableRange = _mutableRange;
			ComputedRange = _computedRange;
			for (int i = 0; i < 387; i++)
			{
				m_ContainsCache[i] = ImmutableRange.Contains((StatTypes.EStatType)i) || MutableRange.Contains((StatTypes.EStatType)i) || ComputedRange.Contains((StatTypes.EStatType)i);
			}
		}

		public bool Contains(StatTypes.EStatType _type)
		{
			return m_ContainsCache[(int)_type];
		}

		public virtual void UpdateComputedFrame(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, float _timeDelta)
		{
		}

		public virtual void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
		}

		public virtual void UpdateComputedExplicit(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
		}

		public virtual void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
		}
	}
}
