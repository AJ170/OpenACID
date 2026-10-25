using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Network.Missions.Mission.Enums;
using UbiCore.Utils;
using UnityEngine;

public class AnimationStateLootChest : AnimationStateBase, IAnimatorEventListener
{
	private readonly EObjectCategory m_TargetMissionCategory;

	private EObjectSubCategory m_targetMissionSubCategory;

	private bool m_startLootingChest;

	private bool m_updateRotation = true;

	private bool m_updateTranslation = true;

	private Vector3 m_distance = Vector3.zero;

	private Vector3 m_direction = Vector3.zero;

	private float m_speed;

	private float m_angularSpeed;

	private int m_loopCount;

	private readonly Vector3 m_targetPosition;

	private readonly Quaternion m_targetOrientation;

	public AnimationStateLootChest(AnimationController _animationController, EObjectCategory _targetMissionCategory, EObjectSubCategory _targetMissionSubCategory, InteractionLocators.Locator _locator, int _loopCount)
		: base(AnimationStateType.ApproachLink, _animationController)
	{
		m_TargetMissionCategory = _targetMissionCategory;
		m_targetMissionSubCategory = _targetMissionSubCategory;
		m_loopCount = _loopCount;
		m_targetPosition = _locator.Position.transform.position;
		m_targetOrientation = _locator.Position.transform.rotation;
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.DataController.RegisterEventListener(this);
	}

	public override void OnDrawGizmos()
	{
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.ApplyRootMotion = false;
		AnimationController.Agent.Stop();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		m_distance = m_targetPosition - AnimationController.NextPosition;
		m_direction = m_distance.normalized;
		m_speed = Mathf.Max(AnimationController.Velocity.magnitude, 1f);
		m_angularSpeed = Mathf.Max(AnimationController.Agent.angularSpeed, 1f);
		AnimationController.SetLookAtWeight(0f, 0f, 0f);
		AnimationController.SetLookAtPosition(m_targetPosition);
	}

	public override void OnUpdate()
	{
		if (base.IsFinished || m_startLootingChest)
		{
			return;
		}
		if (m_updateRotation || m_updateTranslation)
		{
			if (m_updateTranslation)
			{
				m_distance = m_targetPosition - AnimationController.NextPosition;
				m_direction = m_distance.normalized;
				Vector3 vector = m_direction * m_speed;
				AnimationController.Animator.SetCurrentSpeed(AnimationController.GetSpeed(vector));
				Vector3 vector2 = vector * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
				if (m_distance.sqrMagnitude <= vector2.sqrMagnitude)
				{
					AnimationController.NextPosition = m_targetPosition;
					m_updateTranslation = false;
				}
				else
				{
					AnimationController.NextPosition += vector2;
				}
				m_updateTranslation = (m_targetPosition - AnimationController.NextPosition).sqrMagnitude > AnimationController.TranslateMargin * AnimationController.TranslateMargin;
			}
			if (m_updateRotation)
			{
				AnimationController.NextRotation = Quaternion.RotateTowards(AnimationController.NextRotation, m_targetOrientation, m_angularSpeed * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
				m_updateRotation = Quaternion.Angle(AnimationController.NextRotation, m_targetOrientation) > AnimationController.RotationMargin;
			}
		}
		if (!m_updateRotation && !m_updateTranslation)
		{
			StartLootChest();
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (!base.IsFinished)
		{
			AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
		}
	}

	public override bool CanCancel()
	{
		return true;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.CameraStart:
			CameraStateHelper.OnCameraStart();
			break;
		case AnimatorEventType.CameraEnd:
			CameraStateHelper.OnCameraEnd();
			break;
		}
		return false;
	}

	private void StartLootChest()
	{
		if (!m_startLootingChest)
		{
			m_startLootingChest = true;
			AnimationController.NextRotation = m_targetOrientation;
			AnimationController.NextPosition = m_targetPosition;
			ActionType actionType = ActionType.Invalid;
			EObjectCategory targetMissionCategory = m_TargetMissionCategory;
			if (targetMissionCategory == EObjectCategory.Mission_EObjectCategory && m_targetMissionSubCategory.HasFlag(EObjectSubCategory.Chest))
			{
				actionType = ((!m_targetMissionSubCategory.HasFlag(EObjectSubCategory.Elevated)) ? ActionType.LootChestGround : ActionType.LootChestTable);
			}
			AnimationController.Animator.SetCurrentActionType(actionType);
		}
	}

	private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_LootChest))
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (!_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_LootChest))
		{
			return;
		}
		if (m_loopCount >= 0)
		{
			m_loopCount--;
			if (m_loopCount <= 0)
			{
				base.IsFinished = true;
				AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
				return;
			}
		}
		m_startLootingChest = false;
	}
}
