using System;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AwareEnemiesCountSkillTrigger<T> : CategoryTriggerBase<T> where T : AssassinCharacter
	{
		private readonly Predicate<int> m_CountPredicate;

		public AwareEnemiesCountSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, Predicate<int> _countPredicate)
			: base(ETriggerType.AwareEnemiesCount, _owner, _frequency, _categoryFlags)
		{
			m_CountPredicate = _countPredicate;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && m_CountPredicate(_entity.EntityStats.Get_int(StatTypes.EStatType.Player_AwareEnemies));
		}
	}
}
