using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class CombatTargetedSkillEffect : ParticipantCarrierSkillEffectBase<PlayerCharacter, IEntityIdSkillPayload>
	{
		protected ICombatAnimationControllingRule m_rule;

		public CombatTargetedSkillEffect(IRule _owner, ICombatAnimationControllingRule _rule)
			: base(EEffectType.CombatSwitchBehaviour, _owner, ETargetSource.Payload)
		{
			m_rule = _rule;
		}

		protected override IEffectInstance Apply(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			base.SourceParticipant.SwitchBehaviour(ECombatBehaviourType.SkillAct, m_rule.SkillParams);
			return result;
		}
	}
}
