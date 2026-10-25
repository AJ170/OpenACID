using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EnemyKilledTrigger<T> : CategoryTriggerBase<T> where T : IRulesCarrier
	{
		public EnemyKilledTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(ETriggerType.EnemyKilled, _owner, _frequency, _categoryFlags)
		{
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && LogicMain.CurrentTurn == LogicMain.GameStats.Get_int(StatTypes.EStatType.Game_RecentlyKilledEnemyTurn) + 1;
		}
	}
}
