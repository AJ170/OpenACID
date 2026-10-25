using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class TargetPredicateSkillTrigger<T, TPayload> : SkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public delegate bool TargetPredicate(T _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetId);

		private readonly TargetPredicate m_Predicate;

		public TargetPredicateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, TargetPredicate _predicate)
			: base(ETriggerType.TargetPredicate, _owner, _targetSource)
		{
			base.Frequency = _frequency;
			Categories = _categories;
			m_Predicate = _predicate;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return m_Predicate(_entity, _stats, TargetEntityId);
		}
	}
}
