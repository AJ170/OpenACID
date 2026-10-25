using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class MaximumTriggeredSkillsTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly int m_MaximumTriggeredSkillCount;

		private readonly ESkillActivationType m_ActivationTypeFlags;

		public MaximumTriggeredSkillsTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _maximumTriggeredSkillCount, ESkillActivationType _activationTypeFlags)
			: base(ETriggerType.MaximumTriggeredSkills, _owner, _frequency, _categories, _targetSource)
		{
			m_MaximumTriggeredSkillCount = _maximumTriggeredSkillCount;
			m_ActivationTypeFlags = _activationTypeFlags;
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && TargetEntityId.GetEntity<ISkillsCarrier>().Skills.GetTriggeredSlotSkillsCount(((ISkillRule)Owner).SkillType, m_ActivationTypeFlags) <= m_MaximumTriggeredSkillCount;
		}
	}
}
