using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class TauntBehaviour : CombatBehaviour
	{
		private const int InvalidAnimationIndex = -1;

		private bool m_playedAnimation;

		private int m_animationIndex = -1;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.Taunt;
			}
		}

		protected IActAnimation ActAnimation { get; set; }

		protected IReactAnimation ReactAnimation { get; set; }

		public TauntBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			m_playedAnimation = false;
			if (m_animationIndex < 1)
			{
				m_animationIndex = LogicMain.Random.Rand(1, 3);
			}
			Taunt();
		}

		public override void End()
		{
			base.End();
			base.Animator.speed = 1f;
			base.Animator.SetCurrentActionType(ActionType.Invalid);
		}

		public override void Update()
		{
			if (!m_playedAnimation)
			{
				PlayTauntAnimation();
			}
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null)
			{
				Vector3 position = currentTarget.Position;
				base.Participant.FacePosition(position);
			}
			base.Participant.LookAtDisable();
		}

		public override bool Act()
		{
			return false;
		}

		public override void OnAnimatorTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _transition)
		{
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _transition.Transition.TargetState.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (!m_playedAnimation && parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Threaten))
				{
					m_playedAnimation = true;
					base.Animator.SetCurrentActionType(ActionType.Invalid);
					m_animationIndex = CombatHelper.RandomWithoutRepetition(1, 3, m_animationIndex);
					break;
				}
				if (m_playedAnimation && (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatStrafe)))
				{
					base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
					break;
				}
			}
		}

		private void Taunt()
		{
			CombatParticipant nextTarget = base.Participant.NextTarget;
			if (nextTarget == null)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
				return;
			}
			base.Participant.CurrentTarget = nextTarget;
			base.Agent.ResetPath();
			base.Animator.speed = 1f;
			PlayTauntAnimation();
		}

		private void PlayTauntAnimation()
		{
			base.Animator.SetCurrentMovementState(MovementState.Idle);
			base.Animator.SetCurrentReactionType(ReactionType.Invalid);
			Animator animator = base.AnimationController.Animator;
			animator.SetCurrentNpcId(m_animationIndex);
			animator.SetCurrentActionType(ActionType.Threat);
			m_playedAnimation = false;
		}
	}
}
