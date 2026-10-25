using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Actions.Parameters;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class MiscActions : NpcBehaviorComponent
	{
		private const float TurnThresholdAngle = 5f;

		private readonly int[] TurnTaskTypes = new int[1] { 34 };

		public MiscActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public BehaviorReturnCode ActionUpdateLastKnownPosition(BehaviorReturnCode _lastReturnCode)
		{
			ICharacterAwarenessCarrier characterAwarenessCarrier = base.NpcCharacter.StateMachines as ICharacterAwarenessCarrier;
			if (characterAwarenessCarrier == null)
			{
				return BehaviorReturnCode.Failure;
			}
			CharacterAwareness.AlarmedState.AlarmedPayload payload = characterAwarenessCarrier.CharacterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
			payload.UpdateLastKnownPosition(base.NpcCharacter, LogicMain.PlayerEntity);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionSetCharacterAwareness(IParameterContainer _stateMachineParameters, BehaviorReturnCode _lastReturnCode)
		{
			StateMachineParameters stateMachineParameters = _stateMachineParameters as StateMachineParameters;
			return ActionSetCharacterAwareness(stateMachineParameters.State);
		}

		public BehaviorReturnCode ActionGetExecutedOnGround(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			bool isAttached = npcCharacter.IsAttached;
			if (isAttached && _lastReturnCode != BehaviorReturnCode.Running)
			{
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetExecutedOnGround();
			}
			return (!isAttached || npcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.GetExecutedOnGround) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running;
		}

		private BehaviorReturnCode ActionSetCharacterAwareness(EStateType _stateType)
		{
			ICharacterAwarenessCarrier characterAwarenessCarrier = base.NpcCharacter.StateMachines as ICharacterAwarenessCarrier;
			CharacterAwareness characterAwareness = characterAwarenessCarrier.CharacterAwareness;
			if (characterAwareness.CurrentStateType != _stateType && characterAwareness.ScheduledStateType == EStateType.Invalid)
			{
				characterAwareness.ScheduleStateChange(_stateType);
			}
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionIncreaseToNotoriety(IParameterContainer _stateMachineParameters, BehaviorReturnCode _lastReturnCode)
		{
			FloatParameters floatParameters = _stateMachineParameters as FloatParameters;
			return ActionIncreaseToNotoriety(floatParameters.Value);
		}

		private BehaviorReturnCode ActionIncreaseToNotoriety(float _minNotoritey)
		{
			PlayerCharacter targetAsPlayer = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetAsPlayer;
			targetAsPlayer.PlayerCharacterStateMachines.PlayerAlertLevel.RegisterIncreaseToNotoriety(_minNotoritey);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionSetDisguised(IParameterContainer _stateMachineParameters, BehaviorReturnCode _lastReturnCode)
		{
			BoolParameters boolParameters = _stateMachineParameters as BoolParameters;
			if (!(base.NpcCharacter is EnemyCharacter))
			{
				return BehaviorReturnCode.Failure;
			}
			base.NpcCharacter.Cast<EnemyCharacter>().SetDisguised(boolParameters.Value);
			return BehaviorReturnCode.Success;
		}

		public void AttachIdle(IParameterContainer _idleParameters)
		{
			IdleParameters idleParameters = _idleParameters as IdleParameters;
			base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdleForcePlay(idleParameters.IdleType);
		}

		public BehaviorReturnCode ActionIdle(IParameterContainer _idleParameters, BehaviorReturnCode _lastReturnCode)
		{
			bool flag = _lastReturnCode == BehaviorReturnCode.Running;
			if (!flag)
			{
				if (base.NpcCharacter.IsAttached)
				{
					IdleParameters idleParameters = _idleParameters as IdleParameters;
					AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
					flag = taskManager.CreateAnimationTaskIdle(idleParameters.IdleType, false, idleParameters.CancelCurrentTask);
				}
				else
				{
					flag = true;
				}
			}
			return (!flag) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running;
		}

		public BehaviorReturnCode ActionGetKilled(IParameterContainer _meleeParameters, BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			bool isAttached = npcCharacter.IsAttached;
			if (isAttached && _lastReturnCode != BehaviorReturnCode.Running)
			{
				MeleeParameters meleeParameters = _meleeParameters as MeleeParameters;
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetHit(base.NpcCharacter, Vector3.zero, meleeParameters.AttackMeleeType, true, true);
			}
			return (!isAttached || npcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.GetHit) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running;
		}

		public BehaviorReturnCode ActionTurn(IParameterContainer _targetParameters, BehaviorReturnCode _lastReturnCode)
		{
			TargetParameters targetParameters = _targetParameters as TargetParameters;
			GameEntity target = targetParameters.GetTarget(base.NpcTreeHolder, base.NpcCharacter);
			if (target != null)
			{
				Vector3 forward = target.Position - base.NpcCharacter.Position;
				Quaternion quaternion = Quaternion.LookRotation(forward);
				float f = Mathf.DeltaAngle(0f, (quaternion * Quaternion.Inverse(base.NpcCharacter.Rotation)).eulerAngles.y);
				if (Mathf.Abs(f) < 5f)
				{
					return BehaviorReturnCode.Success;
				}
				if (base.NpcCharacter.IsAttached)
				{
					return AiHelper.ActionWaitForAnim(_lastReturnCode, base.NpcCharacter, PlayTurn, _targetParameters, TurnTaskTypes, false);
				}
				base.NpcCharacter.Rotation = quaternion;
				return BehaviorReturnCode.Success;
			}
			return BehaviorReturnCode.Failure;
		}

		public BehaviorReturnCode ActionRestartAlarmedActivity(BehaviorReturnCode _behaviorReturnCode)
		{
			EnemyCharacter enemy = base.NpcCharacter as EnemyCharacter;
			AiHelper.RestartAlarmedActivity(enemy);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionPlayerShouldExitHidingSpot(BehaviorReturnCode _behaviorReturnCode)
		{
			BehaviorReturnCode result = BehaviorReturnCode.Failure;
			PlayerCharacter targetAsPlayer = base.Knowledge.TargetKnowledge.TargetAsPlayer;
			GameEntity spotEntity = base.NpcRuntimeData.Chase.m_currentInspectionTarget.m_spotEntity;
			if (spotEntity != null)
			{
				if (targetAsPlayer.GetIsStalking())
				{
					PlayerStalkingZone.StalkingZoneState.StalkingZoneStatePayload payload = targetAsPlayer.PlayerCharacterStateMachines.PlayerStalkingZone.GetPayload<PlayerStalkingZone.StalkingZoneState.StalkingZoneStatePayload>(EStateType.PlayerStalkingZone_Stalking);
					StalkingZone entity = payload.StalkingZone.GetEntity<StalkingZone>();
					if (entity == spotEntity)
					{
						result = BehaviorReturnCode.Success;
					}
				}
				else
				{
					IStateMachine mainStateMachine = targetAsPlayer.GetMainStateMachine();
					PlayerMain.IHidingPayload hidingPayload = mainStateMachine.TryGetPayload<PlayerMain.IHidingPayload>(mainStateMachine.CurrentStateType);
					if (hidingPayload != null && hidingPayload.HidingSpotId == spotEntity.Id)
					{
						hidingPayload.ShouldExit = true;
						result = BehaviorReturnCode.Success;
					}
				}
			}
			return result;
		}

		private bool PlayTurn(IParameterContainer _targetParameters)
		{
			TargetParameters targetParameters = _targetParameters as TargetParameters;
			GameEntity target = targetParameters.GetTarget(base.NpcTreeHolder, base.NpcCharacter);
			if (target == null)
			{
				return false;
			}
			Vector3 position = target.Position;
			if (base.NpcCharacter.IsAttached)
			{
				return base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskNpcTurning(position);
			}
			Vector3 forward = position - base.NpcCharacter.Position;
			forward.y = 0f;
			base.NpcCharacter.Rotation = Quaternion.LookRotation(forward);
			return true;
		}
	}
}
