using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class ClosestEnemyInRangeTrigger : CategoryTriggerBase<PlayerCharacter>
	{
		public delegate float RangeGetter(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats);

		private readonly RangeGetter m_RangeGetter;

		public ClosestEnemyInRangeTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, RangeGetter _rangeGetter)
			: base(ETriggerType.ClosestEnemyInRange, _owner, _frequency, _categories)
		{
			m_RangeGetter = _rangeGetter;
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			float closestDistance = m_RangeGetter(_entity, _stats);
			return _entity.GetClosestEnemyThreateningOrNormal(closestDistance) != null;
		}
	}
}
