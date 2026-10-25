using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationStateBenchEnter : AnimationStateBase
{
	private Bench m_bench;

	private Vector3 m_initialPos;

	private Quaternion m_initialRotation;

	private bool m_alignToSeat;

	public AnimationStateBenchEnter(AnimationController _animationController, Bench _bench, InteractionLocators.Locator _selectedLocator)
		: base(AnimationStateType.BenchEnter, _animationController)
	{
		m_bench = _bench;
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.StateMachine.StateExited += OnStateExited;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Agent.Stop();
		AnimationController.Agent.enabled = false;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		Vector3 forward = AnimationController.Agent.transform.forward;
		Vector3 vector = AnimationController.NextPosition - m_bench.GetSeat(Bench.ESeats.Player).Position;
		forward.y = (vector.y = 0f);
		forward.Normalize();
		vector.Normalize();
		float num = ((!(Vector3.Cross(forward, vector).y <= 0f)) ? 1f : (-1f));
		float num2 = Mathf.Clamp(Vector3.Angle(forward, vector), -180f, 180f);
		AnimationController.Animator.SetCurrentBenchApproachAngle(num2 / 180f * num);
		AnimationController.Animator.SetCurrentBankLeftRight(0f);
		AnimationController.Animator.SetCurrentActionType(ActionType.SitOnBench);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		m_initialPos = AnimationController.NextPosition;
		m_initialRotation = AnimationController.NextRotation;
		m_alignToSeat = true;
	}

	private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_SitDownOnBench))
		{
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
			AnimationController.ApplyRootMotion = true;
		}
	}

	private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_SitDownOnBench))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateExited -= OnStateExited;
			AnimationController.NextPosition = m_bench.GetSeat(Bench.ESeats.Player).Position;
			AnimationController.NextRotation = m_bench.GetSeat(Bench.ESeats.Player).Orientation;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_alignToSeat)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.fullPathHash == AnimationStateHashes.State_SitDownOnBench.NameHash)
			{
				float num = Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime * 2f);
				Bench.Seat seat = m_bench.GetSeat(Bench.ESeats.Player);
				Vector3 vector = seat.Position - m_initialPos;
				Vector3 nextPosition = m_initialPos + vector * num;
				Quaternion nextRotation = Quaternion.Slerp(m_initialRotation, seat.Orientation, num);
				AnimationController.NextPosition = nextPosition;
				AnimationController.NextRotation = nextRotation;
			}
		}
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.StateMachine.StateEntered -= OnStateEntered;
		AnimationController.StateMachine.StateExited -= OnStateExited;
	}
}
