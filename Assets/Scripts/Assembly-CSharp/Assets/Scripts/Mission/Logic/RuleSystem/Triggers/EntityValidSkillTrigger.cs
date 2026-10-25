using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityValidSkillTrigger : CategorySkillTriggerBase<PlayerCharacter, IEntityIdSkillPayload>
	{
		public EntityValidSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(ETriggerType.EntityValid, _owner, _frequency, _categoryFlags, _targetSource)
		{
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return TargetEntityId.IsValid;
		}
	}
}
