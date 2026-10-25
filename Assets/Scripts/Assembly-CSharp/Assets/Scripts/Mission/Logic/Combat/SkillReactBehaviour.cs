using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class SkillReactBehaviour : CombatBehaviour
	{
		public interface IParams
		{
			CombatParticipant Source { get; }

			IReactAnimation ReactAnimation { get; }

			AnimatorEventType CurrentEvent { get; }

			EStateType Condition { get; }

			bool AllowReact { get; }

			CharacterCondition.EAnimAllowed ReactAnimationType { get; }
		}

		public class Params : IParams
		{
			public CombatParticipant Source { get; set; }

			public IReactAnimation ReactAnimation { get; set; }

			public AnimatorEventType CurrentEvent { get; set; }

			public EStateType Condition { get; set; }

			public bool AllowReact { get; set; }

			public CharacterCondition.EAnimAllowed ReactAnimationType { get; set; }
		}

		private bool m_playedAnimation;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.SkillReact;
			}
		}

		private IParams SkillParams { get; set; }

		public SkillReactBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			base.Participant.KnowinglyInCombat = true;
			base.Participant.LookAtDisable();
			SkillParams = _params[0] as IParams;
			Character character = base.Participant.GameEntity.Cast<Character>();
			CharacterCondition characterCondition = character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (!characterCondition.IsAnimationAllowed(SkillParams.ReactAnimationType))
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.External);
			}
			else if (characterCondition.CurrentStateType == EStateType.CharacterCondition_Knockdowned)
			{
				SkillParams = new Params
				{
					AllowReact = SkillParams.AllowReact,
					Condition = SkillParams.Condition,
					CurrentEvent = SkillParams.CurrentEvent,
					ReactAnimation = new ReactAnimation
					{
						AttackMelee = AttackMeleeType.Invalid,
						AttackMeleeDie = AttackMeleeType.Invalid,
						Reaction = ReactionType.GetHurtOnGround,
						ActionUpperBody = ActionTypeUpperBody.Invalid,
						WaitForHash = AnimationStateMachineHashes.StateMachine_PushBack,
						ReactionEvent = AnimatorEventType.Hit,
						ReactionDie = ReactionType.GetHurtOnGround
					},
					ReactAnimationType = SkillParams.ReactAnimationType,
					Source = SkillParams.Source
				};
			}
			else if (character.GetHasZeroHealth())
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Die, SkillParams.Source, SkillParams.ReactAnimation);
			}
			else if (SkillParams.Condition != 0)
			{
				characterCondition.ExternalChange(SkillParams.Condition);
				base.Participant.SwitchBehaviour(ECombatBehaviourType.External);
			}
			else if (SkillParams.CurrentEvent == SkillParams.ReactAnimation.ReactionEvent)
			{
				PlayAnimation();
			}
			else
			{
				m_playedAnimation = true;
			}
		}

		public override void End()
		{
			base.End();
			base.Animator.speed = 1f;
			if (!m_playedAnimation)
			{
				base.Animator.SetCurrentReactionType(ReactionType.Invalid);
				base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
			}
		}

		public override bool Act()
		{
			return false;
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			if (_source != null && SkillParams.Source == _source && _source.ActiveBehaviour.BehaviourType == ECombatBehaviourType.SkillAct)
			{
				return true;
			}
			return m_playedAnimation && SkillParams.AllowReact;
		}

		public override void Update()
		{
			if (!m_playedAnimation)
			{
				PlayAnimation();
			}
			base.Participant.FollowAgent();
		}

		public override void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (!m_playedAnimation)
				{
					if (parentStateMachine.CompareNameHash(SkillParams.ReactAnimation.WaitForHash))
					{
						base.Animator.SetCurrentReactionType(ReactionType.Invalid);
						base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
						m_playedAnimation = true;
						break;
					}
				}
				else
				{
					if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatStrafe))
					{
						base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
						break;
					}
					if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Reaction))
					{
						base.Participant.SwitchBehaviour(ECombatBehaviourType.External);
						break;
					}
				}
			}
		}

		private void PlayAnimation()
		{
			m_playedAnimation = false;
			base.Participant.AttackWasKillMove = false;
			base.Participant.AttackHasHit = false;
			base.Agent.ResetPath();
			Vector3 vector = CombatHelper.ReactionDirectionAngle(SkillParams.Source.Character, base.Participant.Character);
			if (vector.z < 0f)
			{
				base.Participant.FacePosition(base.Participant.Position + (base.Participant.Position - SkillParams.Source.Character.Position), 0f);
			}
			base.Animator.SetCurrentReactionType(SkillParams.ReactAnimation.Reaction);
			base.Animator.SetCurrentAttackMeleeType(SkillParams.ReactAnimation.AttackMelee);
			base.Animator.SetCurrentActionTypeUpperBody(SkillParams.ReactAnimation.ActionUpperBody);
			base.Animator.SetCurrentReactionDirectionX(vector.x);
			base.Animator.SetCurrentReactionDirectionZ(vector.z);
			base.Animator.SetCurrentMovementState(MovementState.Idle);
		}
	}
}
