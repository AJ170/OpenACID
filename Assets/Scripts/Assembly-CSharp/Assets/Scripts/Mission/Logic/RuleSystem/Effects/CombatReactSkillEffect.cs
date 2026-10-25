using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class CombatReactSkillEffect : ParticipantCarrierSkillEffectBase<PlayerCharacter, IEntityIdSkillPayload>
	{
		public EStateType Condition { get; private set; }

		public AnimatorEventType AnimatorEvent { get; private set; }

		public bool AllowReact { get; private set; }

		public CharacterCondition.EAnimAllowed ReactAnimType { get; private set; }

		public CombatReactSkillEffect(IRule _owner, AnimatorEventType _animatorEvent, bool _allowReact, CharacterCondition.EAnimAllowed _reactAnimType, EStateType _condition = EStateType.Invalid)
			: base(EEffectType.CombatReactSkill, _owner, ETargetSource.Payload)
		{
			AnimatorEvent = _animatorEvent;
			AllowReact = _allowReact;
			ReactAnimType = _reactAnimType;
			Condition = _condition;
		}

		protected override IEffectInstance Apply(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			SkillActBehaviour skillActBehaviour = base.SourceParticipant.ActiveBehaviour as SkillActBehaviour;
			if (Assets.Scripts.Engine.Helpers.Verify(skillActBehaviour != null, "Participant not in SkillActBehaviour as expected."))
			{
				skillActBehaviour.OnReactSkillEffectEvent(this);
			}
			return result;
		}
	}
}
