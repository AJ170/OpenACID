using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class BlockingSkillActiveTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		public BlockingSkillActiveTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(ETriggerType.BlockingSkillActive, _owner, _frequency, _categoryFlags, _targetSource)
		{
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && TargetEntityId.GetEntity<ISkillsCarrier>().Skills.IsBlockingSkillActive;
		}
	}
}
