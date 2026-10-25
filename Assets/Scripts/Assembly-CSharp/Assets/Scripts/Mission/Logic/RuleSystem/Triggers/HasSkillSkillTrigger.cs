using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class HasSkillSkillTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly Enum<SkillTypes> m_skill;

		public HasSkillSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, Enum<SkillTypes> _skill)
			: base(ETriggerType.SkillRank, _owner, _frequency, _categoryFlags, _targetSource)
		{
			m_skill = _skill;
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && TargetEntityId.GetEntity<ISkillsCarrier>().Skills.Has(m_skill);
		}
	}
}
