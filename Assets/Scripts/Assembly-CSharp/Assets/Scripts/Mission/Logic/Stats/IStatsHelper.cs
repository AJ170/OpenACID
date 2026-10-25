using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatsHelper
	{
		StatTypes.StatsRange ImmutableRange { get; }

		StatTypes.StatsRange MutableRange { get; }

		StatTypes.StatsRange ComputedRange { get; }

		bool Contains(StatTypes.EStatType _type);

		void UpdateComputedFrame(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, float _timeDelta);

		void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns);

		void UpdateComputedExplicit(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier);

		void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier);
	}
}
