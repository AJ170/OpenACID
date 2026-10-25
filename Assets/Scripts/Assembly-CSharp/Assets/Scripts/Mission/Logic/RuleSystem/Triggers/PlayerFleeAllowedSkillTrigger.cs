using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class PlayerFleeAllowedSkillTrigger : CategorySkillTriggerBase<PlayerCharacter, ISkillPayload>
	{
		public PlayerFleeAllowedSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories)
			: base(ETriggerType.CombatPlayerFleeAllowed, _owner, _frequency, _categories, ETargetSource.Player)
		{
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return CombatController.IsPlayerEscapeAllowed;
		}
	}
}
