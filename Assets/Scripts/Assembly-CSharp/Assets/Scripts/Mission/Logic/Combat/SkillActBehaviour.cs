using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class SkillActBehaviour : CombatBehaviour
	{
		public interface IParams
		{
			IActAnimation ActAnimation { get; }

			IReactAnimation ReactAnimation { get; }
		}

		public class Params : IParams
		{
			public IActAnimation ActAnimation { get; set; }

			public IReactAnimation ReactAnimation { get; set; }
		}

		private bool m_playedAnimation;

		private Vector3 m_baseHitPosition;

		private float m_baseHitTime;

		private float m_targetHitTime;

		public override ECombatBehaviourType BehaviourType
		{
			get
			{
				return ECombatBehaviourType.SkillAct;
			}
		}

		private IParams SkillParams { get; set; }

		public SkillActBehaviour(CombatParticipant _participant)
			: base(_participant)
		{
		}

		public override void Start(params object[] _params)
		{
			base.Start(_params);
			SkillParams = (IParams)_params[0];
			m_playedAnimation = false;
			CombatParticipant nextTarget = base.Participant.NextTarget;
			if (nextTarget == null)
			{
				base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
				return;
			}
			base.Participant.CurrentTarget = nextTarget;
			base.Participant.AttackWasKillMove = false;
			base.Participant.AttackHasHit = false;
			base.Agent.ResetPath();
			m_baseHitTime = LogicMain.CurrentTime;
			m_baseHitPosition = base.Participant.Position;
			PlayAnimation();
		}

		public override bool Act()
		{
			return false;
		}

		public override bool React(CombatParticipant _source, params object[] _params)
		{
			return false;
		}

		public override void Update()
		{
			if (!m_playedAnimation)
			{
				PlayAnimation();
			}
			if (!base.Participant.AttackHasHit && base.Participant.CurrentTarget != null)
			{
				AdjustHitPosition();
			}
			base.Participant.FollowAgent();
		}

		public void OnReactSkillEffectEvent(CombatReactSkillEffect _effect)
		{
			CombatParticipant currentTarget = base.Participant.CurrentTarget;
			if (currentTarget != null && currentTarget.ActiveBehaviour.React(base.Participant))
			{
				SkillReactBehaviour.Params @params = new SkillReactBehaviour.Params();
				@params.Source = base.Participant;
				@params.ReactAnimation = SkillParams.ReactAnimation;
				@params.CurrentEvent = _effect.AnimatorEvent;
				@params.Condition = _effect.Condition;
				@params.AllowReact = _effect.AllowReact;
				@params.ReactAnimationType = _effect.ReactAnimType;
				SkillReactBehaviour.IParams params2 = @params;
				currentTarget.SwitchBehaviour(ECombatBehaviourType.SkillReact, params2);
			}
		}

		public override void OnAnimatorTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _transition)
		{
			if (m_playedAnimation)
			{
				return;
			}
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _transition.Transition.TargetState.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (parentStateMachine.CompareNameHash(SkillParams.ActAnimation.WaitForHash))
				{
					m_playedAnimation = true;
					base.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
					base.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
					m_targetHitTime = base.CombatController.GetTargetHitTime(this, SkillParams.ActAnimation.AttackMelee);
					base.Animator.speed = 1f;
					break;
				}
			}
		}

		public override void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			if (!m_playedAnimation)
			{
				return;
			}
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HighLowProfileWait) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_CombatStrafe))
				{
					base.Participant.SwitchBehaviour(ECombatBehaviourType.Idle);
					break;
				}
			}
		}

		private void PlayAnimation()
		{
			base.Animator.SetCurrentAttackMeleeType(SkillParams.ActAnimation.AttackMelee);
			base.Animator.SetCurrentActionTypeUpperBody(SkillParams.ActAnimation.ActionUpperBody);
			base.Animator.SetCurrentComboAttack(Flag.False);
		}

		private void AdjustHitPosition()
		{
			if (LogicMain.CurrentTime < m_targetHitTime)
			{
				float num = m_targetHitTime - LogicMain.CurrentTime;
				Vector3 position = base.Participant.Position;
				Vector3 position2 = base.Participant.CurrentTarget.Position;
				Vector3 vector = position2 - base.Participant.Position;
				Vector3 normalized = vector.normalized;
				float magnitude = vector.magnitude;
				Vector3 b = position2 - normalized * Mathf.Min(magnitude, base.CombatController.HitDistance);
				Vector3 b2 = Vector3.Lerp(m_baseHitPosition, b, 1f - num / (m_targetHitTime - m_baseHitTime));
				float num2 = num * base.CombatController.HitCorrectionDamping;
				base.Participant.Position = Vector3.Lerp(position, b2, LogicMain.FrameTime / (LogicMain.FrameTime + num2));
				base.Participant.FacePosition(base.Participant.CurrentTarget.Position, num2);
				base.Participant.LookAtDisable();
			}
			else
			{
				base.Participant.FacePosition(base.Participant.CurrentTarget.Position, base.CombatController.HitCorrectionDamping);
				base.Participant.LookAtDisable();
			}
		}
	}
}
