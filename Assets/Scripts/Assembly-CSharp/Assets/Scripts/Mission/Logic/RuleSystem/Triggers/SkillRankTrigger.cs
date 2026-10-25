using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class SkillRankTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		public SkillRankTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(ETriggerType.SkillRank, _owner, _frequency, _categoryFlags, _targetSource)
		{
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			SkillData.ISkillDefinition definition = Owner.Definition.SkillType.GetDefinition();
			return base.IsActive(_entity, _stats) && TargetEntityId.GetEntity<ISkillsCarrier>().Skills.GetBuffedRank(definition.Type) == definition.Rank;
		}
	}
}
