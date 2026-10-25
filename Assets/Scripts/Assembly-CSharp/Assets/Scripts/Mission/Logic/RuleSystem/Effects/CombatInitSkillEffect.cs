using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class CombatInitSkillEffect : ParticipantCarrierSkillEffectBase<PlayerCharacter, IEntityIdSkillPayload>
	{
		public CombatInitSkillEffect(IRule _owner)
			: base(EEffectType.CombatInitSkill, _owner, ETargetSource.Payload)
		{
		}

		protected override IEffectInstance Apply(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			if (CombatController.IsActive)
			{
				return result;
			}
			AssassinApproachingTarget assassinApproachingTarget = _entity.AssassinCharacterStateMachines.AssassinApproachingTarget;
			assassinApproachingTarget.SetTarget(TargetEntityId, EInteractionReason.Combat);
			return result;
		}
	}
}
