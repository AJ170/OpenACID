using System;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class KilledEnemiesCountTrigger : CategoryTriggerBase<PlayerCharacter>
	{
		private readonly Predicate<int> m_CountPredicate;

		public KilledEnemiesCountTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, Predicate<int> _countPredicate)
			: base(ETriggerType.KilledEnemiesCount, _owner, _frequency, _categoryFlags)
		{
			m_CountPredicate = _countPredicate;
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			int obj = LogicMain.GameStats.Get_int(StatTypes.EStatType.Game_RecentlyKilledEnemyCount);
			return base.IsActive(_entity, _stats) && m_CountPredicate(obj);
		}
	}
}
