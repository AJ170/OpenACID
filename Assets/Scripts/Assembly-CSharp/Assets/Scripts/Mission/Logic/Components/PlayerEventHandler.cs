using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using UbiCore.Acier;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PlayerEventHandler : UnityComponentListeningTurnUpdateableComponent, IAnimatorEventListener
	{
		private readonly TargetApproacher m_TargetApproacher = new TargetApproacher
		{
			UpdateInterval = 0.5f
		};

		public PlayerCharacter PlayerCharacter
		{
			get
			{
				return (PlayerCharacter)Entity;
			}
		}

		public override int Priority
		{
			get
			{
				return 20;
			}
		}

		public bool ApproachPositionWasTapped { get; private set; }

		public int PositionArrivalTurn { get; private set; }

		public Vector3 TargetPosition
		{
			get
			{
				return m_TargetApproacher.ActualTargetPos;
			}
		}

		public bool IsApproachingPosition
		{
			get
			{
				return m_TargetApproacher.Status == TargetApproacher.EStatus.Running;
			}
		}

		public PlayerEventHandler(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			ApproachPositionWasTapped = false;
			PositionArrivalTurn = 0;
			m_TargetApproacher.Reset();
			Messenger.Instance.AddListener<MoveToEvent>(OnMoveTo);
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			Messenger.Instance.RemoveListener<MoveToEvent>(OnMoveTo);
		}

		public override void OnAttachedAndInitialized()
		{
			base.OnAttachedAndInitialized();
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationStateEntered += TaskManagerOnAnimationStateEntered;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationStateExited += TaskManagerOnAnimationStateExited;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskEntered += TaskManagerOnAnimationTaskEntered;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited += TaskManagerOnAnimationTaskExited;
			PlayerCharacter.PlayerCharacterStateMachines.AssassinApproachingTarget.StateChanged += AssassinApproachingTargetOnStateChanged;
			GameObject.GetComponent<AnimatorDataController>().RegisterEventListener(this);
			Messenger.Instance.AddListener<ParcourInputEvent>(OnParcourInput);
			Messenger.Instance.AddListener<ParcourCancelEvent>(OnParcourCanceled);
		}

		public override void Detach()
		{
			base.Detach();
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationStateEntered -= TaskManagerOnAnimationStateEntered;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationStateExited -= TaskManagerOnAnimationStateExited;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskEntered -= TaskManagerOnAnimationTaskEntered;
			base.GameObjectEntity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited -= TaskManagerOnAnimationTaskExited;
			PlayerCharacter.PlayerCharacterStateMachines.AssassinApproachingTarget.StateChanged -= AssassinApproachingTargetOnStateChanged;
			GameObject.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
			Messenger.Instance.RemoveListener<ParcourInputEvent>(OnParcourInput);
			Messenger.Instance.RemoveListener<ParcourCancelEvent>(OnParcourCanceled);
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			if (!PlayerCharacter.GetIsApproachingTarget() && !PlayerCharacter.GetIsHidingInObject())
			{
				TargetApproacher.EStatus status = m_TargetApproacher.Status;
				m_TargetApproacher.Update();
				if (m_TargetApproacher.Status != TargetApproacher.EStatus.Running)
				{
					MoveInterpreterDecals.Instance.Enabled = false;
				}
				if (status == TargetApproacher.EStatus.Running && m_TargetApproacher.Status != TargetApproacher.EStatus.Running)
				{
					PositionArrivalTurn = LogicMain.CurrentTurn;
					if (PlayerCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) == null)
					{
						PlayerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle();
					}
				}
			}
			base.DoTurnUpdate(_deltaTurns);
		}

		public bool CheckIsApproachingPosition()
		{
			return m_TargetApproacher.CheckIsRunning();
		}

		public bool StopApproaching(bool _stopMoving = true)
		{
			if (m_TargetApproacher.Status == TargetApproacher.EStatus.Running)
			{
				m_TargetApproacher.Reset();
				PositionArrivalTurn = LogicMain.CurrentTurn;
				MoveInterpreterDecals.Instance.Enabled = false;
				PlayerCharacter.CancelMovement(_stopMoving);
				return true;
			}
			return false;
		}

		public void OverrideResetPathOnCancel()
		{
			m_TargetApproacher.RetainPathOnCancel = false;
			AnimationTaskPlayerMoveTo animationTaskPlayerMoveTo = PlayerCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) as AnimationTaskPlayerMoveTo;
			if (animationTaskPlayerMoveTo != null)
			{
				animationTaskPlayerMoveTo.OverrideResetAgentOnCancel();
			}
		}

		private void OnParcourInput(ParcourInputEvent _message)
		{
			if (_message.Direction != Vector3.zero)
			{
				PlayerCharacter.StopApproaching(false);
			}
		}

		private void OnParcourCanceled(ParcourCancelEvent _message)
		{
			PlayerCharacter.StopApproaching(false);
		}

		private void TaskManagerOnAnimationStateEntered(object _sender, AnimationTaskManager.AnimationStateEventArgs _eventArgs)
		{
			switch (_eventArgs.AnimationState.StateType)
			{
			case AnimationStateType.Balance:
			{
				AnimationStateBalance animationStateBalance = (AnimationStateBalance)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkStartedEvent(animationStateBalance.Destination));
				break;
			}
			case AnimationStateType.JumpHorizontal:
			{
				AnimationStateJumpHorizontal animationStateJumpHorizontal = (AnimationStateJumpHorizontal)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkStartedEvent(animationStateJumpHorizontal.Destination));
				break;
			}
			case AnimationStateType.JumpDown:
			{
				AnimationStateJumpDown animationStateJumpDown = (AnimationStateJumpDown)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkStartedEvent(animationStateJumpDown.Destination));
				break;
			}
			}
		}

		private void TaskManagerOnAnimationStateExited(object _sender, AnimationTaskManager.AnimationStateEventArgs _eventArgs)
		{
			switch (_eventArgs.AnimationState.StateType)
			{
			case AnimationStateType.JumpDown:
			{
				AnimationStateJumpDown animationStateJumpDown = (AnimationStateJumpDown)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkFinishedEvent(animationStateJumpDown.Destination));
				break;
			}
			case AnimationStateType.JumpHorizontal:
			{
				AnimationStateJumpHorizontal animationStateJumpHorizontal = (AnimationStateJumpHorizontal)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkFinishedEvent(animationStateJumpHorizontal.Destination));
				break;
			}
			case AnimationStateType.Balance:
			{
				AnimationStateBalance animationStateBalance = (AnimationStateBalance)_eventArgs.AnimationState;
				Messenger.Instance.Broadcast(new TraverseLinkFinishedEvent(animationStateBalance.Destination));
				break;
			}
			}
		}

		private void TaskManagerOnAnimationTaskEntered(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
		{
			AnimationTaskType taskType = _eventArgs.AnimationTask.TaskType;
			if (taskType == AnimationTaskType.Climb)
			{
				AnimationTaskClimb animationTaskClimb = (AnimationTaskClimb)_eventArgs.AnimationTask;
				Messenger.Instance.Broadcast(new TraverseLinkStartedEvent(animationTaskClimb.Destination));
			}
		}

		private void TaskManagerOnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
		{
			AnimationTaskType taskType = _eventArgs.AnimationTask.TaskType;
			if (taskType == AnimationTaskType.Climb)
			{
				AnimationTaskClimb animationTaskClimb = (AnimationTaskClimb)_eventArgs.AnimationTask;
				Messenger.Instance.Broadcast(new TraverseLinkFinishedEvent(animationTaskClimb.Destination));
			}
		}

		private void OnMoveTo(MoveToEvent _event)
		{
			float _startTime;
			if (((!PlayerCharacter.GetHidingInPatrolStartTime(out _startTime) || LogicMain.CurrentTime < _startTime + 1.5f) && !PlayerCharacter.GetIsRoaming() && !PlayerCharacter.GetIsBeingChased() && !PlayerCharacter.GetIsHidingInObjectExitAndMoveTo()) || PlayerCharacter.PlayerCharacterStateMachines.PlayerMain.ScheduledStateType != 0 || PlayerCharacter.GetIsApproachingTarget())
			{
				return;
			}
			PlayerCharacter playerCharacter = (PlayerCharacter)Entity;
			if (_event.Type != MoveToEvent.EInputType.GroundDrag && _event.Type != MoveToEvent.EInputType.GroundTap)
			{
				return;
			}
			if (_event.Path.corners.Length > 0)
			{
				m_TargetApproacher.SetData(Entity.Id, _event.Path.corners[_event.Path.corners.Length - 1], MapSpeed(_event.SpeedFactor), float.MaxValue);
				m_TargetApproacher.RetainPathOnCancel = _event.Type == MoveToEvent.EInputType.GroundDrag;
				m_TargetApproacher.Approach();
				ApproachPositionWasTapped = _event.Type == MoveToEvent.EInputType.GroundTap;
				if (_event.Type == MoveToEvent.EInputType.GroundTap && !_event.HideDecals)
				{
					MoveInterpreterDecals.Instance.Enabled = true;
				}
			}
			else
			{
				StopApproaching();
				playerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskPlayerMoveTo(_event);
			}
		}

		private EMovementSpeed MapSpeed(float _speed)
		{
			if (_speed <= 0f)
			{
				return EMovementSpeed.Invalid_EMovementSpeed;
			}
			if (_speed <= 0.85f)
			{
				return EMovementSpeed.WalkFast;
			}
			return EMovementSpeed.Invalid_EMovementSpeed;
		}

		private void AssassinApproachingTargetOnStateChanged(IStateMachine _stateMachine, EStateType _from, EStateType _to)
		{
			if (_to == EStateType.AssassinApproachingTarget_Approaching)
			{
				MoveInterpreterDecals.Instance.Enabled = false;
			}
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (_event.Type == AnimatorEventType.FallDamageHit)
			{
				float lastFallHeight = PlayerCharacter.GetLastFallHeight();
				if (lastFallHeight > ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage)
				{
					EntityStats entityStats = PlayerCharacter.EntityStats;
					float num = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
					float num2 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health);
					float num3 = Mathf.Clamp01((lastFallHeight - ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage) / (ParcourHelper.Acrobatics.JumpMaxFallHeightDamaged - ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage));
					float num4 = Mathf.Max(num * num3, 0f);
					if (num4 > 0f)
					{
						float num5 = entityStats.Get_float(StatTypes.EStatType.Player_FallingDamageReductionFactor);
						num4 *= Mathf.Max(1f - num5, 0f);
					}
					float num6 = num2 - num4;
					if (num6 <= 0f)
					{
						entityStats.Set_bool(StatTypes.EStatType.Player_ProtectedFromDeath, false);
						num6 = 1f;
						num4 = num2 - num6;
					}
					entityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, num6);
					if (num4 > 0f)
					{
						LogicMain.LogicInterface.Combat_Hit(PlayerCharacter, PlayerCharacter, num4, LogicInterface.ECombatEventModifierFlags.FallDamage);
					}
				}
			}
			return false;
		}
	}
}
