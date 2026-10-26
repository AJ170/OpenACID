using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public abstract class AnimationTaskHideoutExitBase : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Vector3 Destination { get; set; }

		public HideoutObject Hideout { get; set; }

		public bool StartRunning { get; set; }

		public InteractionLocators.Locator Locator { get; set; }

		public Parameters(AnimationTaskType _taskType)
			: base(_taskType)
		{
		}
	}

	private bool m_RootMotion;

	private Vector3 m_Destination = Vector3.zero;

	protected HideoutObject m_hideoutObject;

	private static MatchTargetWeightMask s_WeightMask = new MatchTargetWeightMask(Vector3.one, 0f);

	public bool StartRunning { get; private set; }

	protected AnimationTaskHideoutExitBase(Parameters _parameters)
		: base(_parameters)
	{
		m_hideoutObject = _parameters.Hideout;
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_Destination = _parameters.Destination;
		StartRunning = _parameters.StartRunning;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.StateMachine.StateExited += OnStateExited;
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawCube(m_Destination, new Vector3(0.25f, 0.25f, 0.25f));
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		PlayerHelper.Entity.SetLastFallHeight(0.75f);
		AnimationController.Animator.SetCurrentActionType(ActionType.FromHayStack);
		AnimationController.Animator.SetCurrentMovementState(StartRunning ? MovementState.Movement : MovementState.Idle);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		Vector3 forward = m_Destination - AnimationController.NextPosition;
		forward.y = 0f;
		AnimationController.NextRotation = Quaternion.LookRotation(forward);
		m_RootMotion = AnimationController.ApplyRootMotion;
		AnimationController.Agent.enabled = true;
		AnimationController.ApplyRootMotion = true;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.Warp(m_Destination);
		AnimationController.Agent.ResetPath();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		float _matchStart = 0f;
		float _matchEnd = 0f;
		if (AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0))
		{
			AnimationController.MatchTarget(m_Destination, Quaternion.identity, AvatarTarget.LeftFoot, s_WeightMask, _matchStart, _matchEnd);
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = m_RootMotion;
	}

	private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_HayStack))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		}
	}

	private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_LeaveHaystack))
		{
			AnimationController.Agent.Resume();
			AnimationController.StateMachine.StateExited -= OnStateExited;
		}
	}

	public static void SetPlayerCondition(EffectManager.ECondition _condition, bool _activate)
	{
		GameObject player = LogicMain.Player;
		if (!(player != null))
		{
			return;
		}
		EffectController componentInChildren = player.GetComponentInChildren<EffectController>();
		if (componentInChildren != null)
		{
			if (_activate)
			{
				componentInChildren.ActivateCondition(_condition);
			}
			else
			{
				componentInChildren.RemoveCondition(_condition);
			}
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
		AnimationController.StateMachine.StateExited -= OnStateExited;
	}
}
