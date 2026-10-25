using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.System;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class HirelingMain : StateMachineUnityComponentListeningBase<HirelingCharacter, HirelingMain>, ISelectionProvider
	{
		public const float CloseRangeDistance = 1f;

		private ISelectable m_Selection;

		private NavMeshPath m_Path;

		private Vector3 m_TargetPos;

		private SkillData.IApproachSkillDefinition m_SkillDefinition;

		public override EStateType InitialState
		{
			get
			{
				return EStateType.HirelingMain_ExecutingTasks;
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.HirelingMain;
			}
		}

		public EntityId TargetEntityId { get; set; }

		public bool IsInCloseRange
		{
			get
			{
				return (TargetEntityId.GameEntity.Position - Entity.Position).sqrMagnitude < 1f;
			}
		}

		public bool TaskExecuted { get; private set; }

		public ISelectable Selection
		{
			get
			{
				return m_Selection;
			}
			set
			{
				GameEntity gameEntity = m_Selection as GameEntity;
				if (gameEntity != null)
				{
					gameEntity.CanBePooled = true;
				}
				m_Selection = value;
				gameEntity = m_Selection as GameEntity;
				if (gameEntity != null)
				{
					gameEntity.CanBePooled = false;
				}
			}
		}

		public HirelingMain(HirelingCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitHirelingMain()
		{
			StateMachineUnityComponentListeningBase<HirelingCharacter, HirelingMain>.InitStateMachineUnityComponentListeningBase();
			StateMachineBase<HirelingCharacter, HirelingMain>.AddState<State<HirelingCharacter>>(EStateType.HirelingMain_ExecutingTasks, null, null, OnUpdateExecutingTasks);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddState<State<HirelingCharacter>>(EStateType.HirelingMain_TasksFinished, null, null, OnUpdateTasksFinished);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddState<State<HirelingCharacter>>(EStateType.HirelingMain_Leaving, OnEnterLeaving, null, OnUpdateLeaving);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddState<State<HirelingCharacter>>(EStateType.HirelingMain_Despawned, null, null, null);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddTransition(EStateType.HirelingMain_ExecutingTasks, EStateType.HirelingMain_TasksFinished);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddTransition(EStateType.HirelingMain_TasksFinished, EStateType.HirelingMain_Leaving);
			StateMachineBase<HirelingCharacter, HirelingMain>.AddTransition(EStateType.HirelingMain_Leaving, EStateType.HirelingMain_Despawned);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			Entity.Skills.OnSkillEvent += SkillsOnOnSkillEvent;
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			Selection = null;
			Entity.Skills.OnSkillEvent -= SkillsOnOnSkillEvent;
		}

		public override void OnAttachedAndInitialized()
		{
			base.OnAttachedAndInitialized();
			TryApproachTarget();
		}

		private static void OnUpdateExecutingTasks(IStateMachine<HirelingCharacter> _stateMachine, IPayload _payload, IState<HirelingCharacter> _currentState)
		{
			if (!_stateMachine.Entity.GetIsApproachingTarget() && _stateMachine.Entity.Skills.GetTriggeredSlotSkillsCount(SkillTypes.Invalid, ESkillActivationType.All) == 0)
			{
				HirelingMain hirelingMain = (HirelingMain)_stateMachine;
				hirelingMain.TryApproachTarget();
			}
		}

		private static void OnUpdateTasksFinished(IStateMachine<HirelingCharacter> _stateMachine, IPayload _payload, IState<HirelingCharacter> _currentState)
		{
			if (!_stateMachine.Entity.AssassinCharacterStateMachines.AssassinApproachingTarget.TargetId.IsValid && !_stateMachine.Entity.GetIsWaitingForAnim() && _stateMachine.Entity.AssassinCharacterStateMachines.PlayerMain.ScheduledStateType == EStateType.Invalid && _stateMachine.Entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.CombatCharacterMain_Roaming)
			{
				_stateMachine.ScheduleStateChange(EStateType.HirelingMain_Leaving);
			}
		}

		private static void OnEnterLeaving(IStateMachine<HirelingCharacter> _stateMachine, IPayload _payload, IState<HirelingCharacter> _prevState, IState<HirelingCharacter> _currentState)
		{
			HirelingMain hirelingMain = _stateMachine.Cast<HirelingMain>();
			hirelingMain.m_TargetPos = SpawnHelper.ComputeSpawnPos(_stateMachine.Entity.Position, Quaternion.LookRotation(_stateMachine.Entity.Position), 30f, 0f, NavMeshLayers.WalkableLayerMaskPreset, false, out hirelingMain.m_Path);
			if (hirelingMain.m_Path == null)
			{
				hirelingMain.Despawn();
			}
			else
			{
				hirelingMain.MoveToDespawnPos();
			}
		}

		private static void OnUpdateLeaving(IStateMachine<HirelingCharacter> _stateMachine, IPayload _payload, IState<HirelingCharacter> _currentState)
		{
			if ((_stateMachine.Entity.Position - _stateMachine.Cast<HirelingMain>().m_TargetPos).sqrMagnitude <= 25f)
			{
				_stateMachine.Cast<HirelingMain>().Despawn();
			}
			else if (_stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.PlayerMoveTo)
			{
				_stateMachine.Cast<HirelingMain>().MoveToDespawnPos();
			}
		}

		private void SkillsOnOnSkillEvent(SkillEvent _event)
		{
			if (_event.SubType == ESkillEventType.ActivityEnd)
			{
				TaskExecuted = true;
				StartLeaving();
			}
		}

		private void MoveToDespawnPos()
		{
			MoveToEvent eventMoveTo = new MoveToEvent(m_Path, m_TargetPos - Entity.Position, MoveToEvent.EInputType.GroundTap, -1f, null, EDispatcherType.Tap);
			Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskPlayerMoveTo(eventMoveTo);
		}

		private void StartLeaving()
		{
			Selection = null;
			Entity.Skills.OnSkillEvent -= SkillsOnOnSkillEvent;
			ScheduleStateChange(EStateType.HirelingMain_TasksFinished);
		}

		private void ApproachTarget(EInteractionReason _reason, EMovementSpeed _speed)
		{
			EnemyCharacter enemyCharacter = TargetEntityId.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter == null)
			{
				StartLeaving();
				return;
			}
			Selection = enemyCharacter;
			Entity.AssassinCharacterStateMachines.PlayerProfile.ChosenSpeed = _speed;
			Entity.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(enemyCharacter.Id, _reason);
		}

		private void TryApproachTarget()
		{
			EnemyCharacter enemyCharacter = TargetEntityId.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter == null || enemyCharacter.GetDeadOrAboutToDie())
			{
				StartLeaving();
			}
			else if (enemyCharacter.EnemyCharacterStateMachines.EnemyMain.CurrentStateType != EStateType.CombatCharacterMain_Parcour)
			{
				ESkillAvailabilityFlags _availabilityFlags;
				m_SkillDefinition = Entity.Skills.GetAvailableApproachSkill(new EntityIdSkillPayload(TargetEntityId), null, false, out _availabilityFlags);
				if (m_SkillDefinition != null)
				{
					ApproachTarget(m_SkillDefinition.InteractionReason, (m_SkillDefinition.Speed != EMovementSpeed.Invalid_EMovementSpeed) ? m_SkillDefinition.Speed : EMovementSpeed.Run);
				}
				else
				{
					StartLeaving();
				}
			}
		}

		private void Despawn()
		{
			SpawnHelper.DespawnHireling(Entity);
			ScheduleStateChange(EStateType.HirelingMain_Despawned);
		}
	}
}
