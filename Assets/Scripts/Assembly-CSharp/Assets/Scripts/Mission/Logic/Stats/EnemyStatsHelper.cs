using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class EnemyStatsHelper : StatsHelper<EnemyStatsHelper>
	{
		public EnemyStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Enemy, StatTypes.EStatType.__End_Immutable_Enemy), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Enemy, StatTypes.EStatType.__End_Mutable_Enemy), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_Enemy, StatTypes.EStatType.__End_Computed_Enemy))
		{
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			_stats.Set_bool(StatTypes.EStatType.Enemy_Silenced, false);
			_stats.Set_float(StatTypes.EStatType.Enemy_AttackDelayFactor, 1f);
			_stats.Set_float(StatTypes.EStatType.Enemy_FearsomeFactor, 1f);
		}
	}
}
