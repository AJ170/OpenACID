using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterInteraction : StateMachineAnimListeningBase<InteractionCharacter, CharacterInteraction>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterInteraction;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				NpcData.NpcDefinition.ENpcInteractionType interactionType = Entity.NpcDefinition.Definition.InteractionType;
				if (interactionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant)
				{
					return EStateType.CharacterInteraction_Enabled;
				}
				return EStateType.CharacterInteraction_Disabled;
			}
		}

		public IdleType IdleTypeOnTransition
		{
			get
			{
				if (Entity.NpcDefinition.Definition.InteractionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant && CurrentStateType == EStateType.CharacterInteraction_Disabled)
				{
					return IdleType.Neutral;
				}
				return Entity.CharacterDefinition.Definition.TransitionIdleType;
			}
		}

		public CharacterInteraction(InteractionCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitCharacterInteraction()
		{
			StateMachineAnimListeningBase<InteractionCharacter, CharacterInteraction>.InitStateMachineAnimListeningBase();
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_Despawned, null, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_Enabled, null, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_Disabled, OnEnterDisabled, null, OnUpdateDisabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_MissionDeliver, OnEnterDeliver, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_OfferOpen, OnEnterOfferOpen, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_OfferAccepted, OnEnterOfferAccepted, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteraction_OfferRejected, OnEnterOfferRejected, null, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddState<CallbackDelayState<InteractionCharacter>>(EStateType.Generic_WaitingForAnim, null, OnExitWaitingForAnim, null);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_Enabled, EStateType.CharacterInteraction_Disabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_Disabled, EStateType.CharacterInteraction_Enabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_Enabled, EStateType.CharacterInteraction_OfferOpen);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferOpen, EStateType.CharacterInteraction_OfferAccepted);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferOpen, EStateType.CharacterInteraction_OfferRejected);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferAccepted, EStateType.CharacterInteraction_Enabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferRejected, EStateType.CharacterInteraction_Enabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_Enabled, EStateType.CharacterInteraction_MissionDeliver);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_MissionDeliver, EStateType.CharacterInteraction_Enabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_MissionDeliver, EStateType.CharacterInteraction_Disabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferAccepted, EStateType.Generic_WaitingForAnim);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_OfferRejected, EStateType.Generic_WaitingForAnim);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CharacterInteraction_Enabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CharacterInteraction_Disabled);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddTransition(EStateType.CharacterInteraction_Disabled, EStateType.CharacterInteraction_Despawned);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddControlling(EStateType.CharacterInteraction_OfferOpen, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<InteractionCharacter, CharacterInteraction>.AddControlling(EStateType.CharacterInteraction_MissionDeliver, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
		}

		private static void OnUpdateDisabled(IStateMachine<InteractionCharacter> _stateMachine, IPayload _payload, IState<InteractionCharacter> _currentState)
		{
			if (_stateMachine.Entity.NpcDefinition.Definition.InteractionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant && (_stateMachine.Entity.Patrol.PatrolComponent.CurrentRoute[_stateMachine.Entity.Patrol.PatrolComponent.CurrentRoute.Count - 1].Position - _stateMachine.Entity.Position).sqrMagnitude < 1f)
			{
				CharacterInteraction characterInteraction = (CharacterInteraction)_stateMachine;
				characterInteraction.Despawn();
				_stateMachine.ScheduleStateChange(EStateType.CharacterInteraction_Despawned);
			}
		}

		public override void Attach()
		{
			base.Attach();
			TryStartToLeave();
		}

		private static void OnEnterDisabled(IStateMachine<InteractionCharacter> _stateMachine, IPayload _payload, IState<InteractionCharacter> _prevState, IState<InteractionCharacter> _currentState)
		{
			CharacterInteraction characterInteraction = (CharacterInteraction)_stateMachine;
			characterInteraction.TryStartToLeave();
		}

		private void TryStartToLeave()
		{
			if (CurrentStateType == EStateType.CharacterInteraction_Disabled && Entity.NpcDefinition.Definition.InteractionType == NpcData.NpcDefinition.ENpcInteractionType.Merchant)
			{
				NavMeshPath _path;
				Vector3 target = SpawnHelper.ComputeSpawnPos(Entity.Position, Quaternion.LookRotation(Entity.Position), 30f, 0f, NavMeshLayers.WalkableLayerMaskPreset, false, out _path);
				if (_path == null)
				{
					Despawn();
				}
				else if (Entity.IsAttached)
				{
					Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle(IdleType.Neutral);
					Entity.Patrol.PatrolComponent.StartSidetrack(target, false, false, -1, true);
				}
			}
		}

		private void Despawn()
		{
			Entity.Despawn();
		}

		private static void OnExitWaitingForAnim(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> currentState, IState<InteractionCharacter> nextState)
		{
			if (stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.CurrentStateType != EStateType.CharacterInteractionMain_Idle)
			{
				stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.ScheduleStateChange(EStateType.CharacterInteractionMain_Idle);
			}
		}

		private static void OnEnterOfferOpen(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> prevState, IState<InteractionCharacter> currentState)
		{
			AutoAnimationRotationToTarget autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(stateMachine.Entity, PlayerHelper.Entity);
			autoAnimationRotationToTarget.FireAndForget = true;
			stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.ScheduleStateChange(EStateType.CharacterInteractionMain_Busy);
			PlayerHelper.PlayerMain.ScheduleStateChange(EStateType.PlayerMain_NpcInteraction);
			PlayerHelper.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_NpcInteraction).EntityId = stateMachine.Entity.Id;
		}

		private static void OnEnterDeliver(IStateMachine<InteractionCharacter> _stateMachine, IPayload _payload, IState<InteractionCharacter> _prevState, IState<InteractionCharacter> _currentState)
		{
			_stateMachine.Entity.InteractionCharacterStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
			_stateMachine.ScheduleStateChange(EStateType.CharacterInteraction_Disabled);
			_stateMachine.Entity.Patrol.EntityStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
			if (_stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.CurrentStateType != EStateType.CharacterInteractionMain_Idle)
			{
				_stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.ScheduleStateChange(EStateType.CharacterInteractionMain_Idle);
			}
		}

		private static void OnEnterOfferRejected(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> prevState, IState<InteractionCharacter> currentState)
		{
			stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskDoctor(AnimationTaskDoctor.EType.Reject, PlayerHelper.Entity.Id);
			stateMachine.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CharacterInteraction_Enabled, EStateType.CharacterInteraction_Enabled, AnimationTaskType.Doctor);
			PlayerHelper.PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}

		private static void OnEnterOfferAccepted(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> prevState, IState<InteractionCharacter> currentState)
		{
			if (PlayerHelper.AssassinInteraction.ApplyMerchantEffect(stateMachine.Entity.Id))
			{
				LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskHealAtDoctor(stateMachine.Entity.Id);
				PlayerHelper.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.PlayerHealAtDoctor);
				stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskDoctor(AnimationTaskDoctor.EType.Accept, PlayerHelper.Entity.Id);
				stateMachine.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CharacterInteraction_Disabled, EStateType.CharacterInteraction_Disabled, AnimationTaskType.Doctor);
			}
			else
			{
				PlayerHelper.PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
				stateMachine.ScheduleStateChange(EStateType.CharacterInteraction_Enabled);
				stateMachine.Entity.InteractionCharacterStateMachines.InteractionCharacterMain.ScheduleStateChange(EStateType.CharacterInteractionMain_Idle);
			}
		}
	}
}
