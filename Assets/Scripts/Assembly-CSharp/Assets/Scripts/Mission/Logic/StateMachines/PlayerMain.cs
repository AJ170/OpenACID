using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Selection;
using SharedBaseLib.System;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerMain : StateMachineAnimListeningBase<AssassinCharacter, PlayerMain>
	{
		public class InteractingWithEntityState : PredicateCallbackDelayState<AssassinCharacter>
		{
			public class InteractingWithEntityPayload : PredicateCallbackPayload
			{
				public EntityId EntityId { get; set; }

				public Vector3 Destination { get; set; }

				public AnimationPlotSequence Plot { get; set; }

				public InteractingWithEntityPayload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					base.Reset();
					EntityId = EntityId.InvalidId;
				}
			}

			public InteractingWithEntityState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public InteractingWithEntityState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new InteractingWithEntityPayload(_stateMachine);
			}
		}

		public interface IHidingPayload : IPayload
		{
			bool ShouldExit { get; set; }

			EntityId HidingSpotId { get; }
		}

		public class HidingInObjectState : InteractingWithEntityState
		{
			public class HidingInObjectPayload : InteractingWithEntityPayload, IHidingPayload, IPayload
			{
				public bool ShouldExit { get; set; }

				public bool WillHidingLimitFailOnIncrease
				{
					get
					{
						EHotspot _hotspotType;
						ELimitType _limitType;
						return Assets.Scripts.Mission.Logic.Components.MissionResult.ConvertTrackedHotspotType(base.EntityId.Entity as InteractableObject, out _hotspotType, out _limitType) && Assets.Scripts.Mission.Logic.Components.MissionResult.WillHidingLimitFailOnIncrease(_limitType);
					}
				}

				public EntityId HidingSpotId
				{
					get
					{
						return base.EntityId;
					}
				}

				public HidingInObjectPayload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					base.Reset();
					ShouldExit = false;
				}
			}

			public HidingInObjectState(EStateType _stateType)
				: base(_stateType)
			{
			}

			public HidingInObjectState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new HidingInObjectPayload(_stateMachine);
			}
		}

		public class HidingInPatrolState : CallbackDelayState<AssassinCharacter>
		{
			public class HidingInPatrolPayload : CallbackPayload, IHidingPayload, IPayload
			{
				public float m_startTime;

				public float m_endTime;

				public bool m_idling;

				public bool WillHidingLimitFailOnIncrease
				{
					get
					{
						Assets.Scripts.Mission.Logic.Entities.Patrol patrol = ((PatrolId != EntityId.InvalidId) ? (PatrolId.Entity as Assets.Scripts.Mission.Logic.Entities.Patrol) : null);
						return patrol != null && PatrolData.WillHidingLimitFailOnIncrease(patrol.PatrolDefinition.Definition.Type);
					}
				}

				public EntityId PatrolId { get; set; }

				public EntityId PrevPatrolId { get; set; }

				public bool ShouldExit { get; set; }

				public EntityId HidingSpotId
				{
					get
					{
						return PatrolId;
					}
				}

				public HidingInPatrolPayload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					base.Reset();
					EntityId patrolId = (PrevPatrolId = EntityId.InvalidId);
					PatrolId = patrolId;
					m_startTime = 0f;
					m_endTime = 0f;
					ShouldExit = false;
				}
			}

			public HidingInPatrolState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public HidingInPatrolState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new HidingInPatrolPayload(_stateMachine);
			}
		}

		public class ParcourState : CallbackDelayState<AssassinCharacter>
		{
			public class ParcourPayload : CallbackPayload
			{
				public Vector3 ParcourDestination { get; set; }

				public ParcourPayload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					base.Reset();
					ParcourDestination = Vector3.zero;
				}
			}

			public ParcourState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public ParcourState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new ParcourPayload(_stateMachine);
			}
		}

		public enum EFloorType
		{
			Invalid = -1,
			Ground = 0,
			Roof = 1
		}

		public class CombatState : CallbackDelayState<AssassinCharacter>
		{
			public new class Payload : CallbackPayload
			{
				public Character Instigator { get; set; }

				public Character Target { get; set; }

				public Payload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					Target = null;
				}
			}

			public CombatState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new Payload(_stateMachine);
			}
		}

		public class TargetState : CallbackDelayState<AssassinCharacter>
		{
			public new class Payload : CallbackPayload
			{
				public EnemyCharacter Target { get; set; }

				public EInteractionReason InteractionReason { get; set; }

				public Payload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public override void Reset()
				{
					base.Reset();
					Target = null;
					InteractionReason = EInteractionReason.Invalid;
				}
			}

			public TargetState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new Payload(_stateMachine);
			}
		}

		public class AssassinationState : TargetState
		{
			public new class Payload : TargetState.Payload, IAnimatorEventListener
			{
				public AssassinCharacter AssassinCharacter { get; private set; }

				public Payload(IStateMachine<AssassinCharacter> _stateMachine)
					: base(_stateMachine)
				{
				}

				public void RegisterForCameraEvent(AssassinCharacter _assassinCharacter)
				{
					AssassinCharacter = _assassinCharacter;
					AssassinCharacter.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
				}

				public void UnregisterEventListener()
				{
					if (AssassinCharacter != null && AssassinCharacter.UnityComponentCache != null && !(AssassinCharacter.UnityComponentCache.AnimationController == null) && !(AssassinCharacter.UnityComponentCache.AnimationController.DataController == null))
					{
						AssassinCharacter.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
					}
				}

				public override void Reset()
				{
					base.Reset();
					UnregisterEventListener();
					AssassinCharacter = null;
				}

				public bool OnAnimatorEvent(AnimatorEvent _event)
				{
					AnimatorEventType type = _event.Type;
					if (type == AnimatorEventType.Hit || type == AnimatorEventType.CameraStart)
					{
						AssassinCharacter.AssassinCharacterStateMachines.CharacterDrawMeleeWeapon.ForceInstantSheathedWeapon();
						return true;
					}
					return false;
				}
			}

			public AssassinationState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<AssassinCharacter> _stateMachine)
			{
				return new Payload(_stateMachine);
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerMain;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CombatCharacterMain_Roaming;
			}
		}

		public int LastStateChangeTurn { get; set; }

		public EFloorType FloorType { get; private set; }

		public PlayerMain(AssassinCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerMain()
		{
			StateMachineAnimListeningBase<AssassinCharacter, PlayerMain>.InitStateMachineAnimListeningBase();
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<State<AssassinCharacter>>(EStateType.CombatCharacterMain_Roaming, OnEnterRoaming, OnExitRoaming, OnUpdateRoaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<State<AssassinCharacter>>(EStateType.CombatCharacterMain_Combat, OnEnterCombat, OnExitCombat, OnUpdateCombat);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<HidingInObjectState>(EStateType.PlayerMain_HidingInObject, OnEnterHidingInObject, OnExitHidingInObject, OnUpdateHidingInObject);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<InteractingWithEntityState>(EStateType.PlayerMain_HidingInObjectExitAndMoveTo, OnEnterHidingInObjectExitAndMoveTo, null, null);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<HidingInPatrolState>(EStateType.PlayerMain_HidingInPatrol, OnEnterHidingInPatrol, OnExitHidingInPatrol, OnUpdateHidingInPatrol);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<InteractingWithEntityState>(EStateType.PlayerMain_NpcInteraction, OnEnterNpcInteraction, OnExitNpcInteraction, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<State<AssassinCharacter>>(EStateType.PlayerMain_BeingChased, OnEnterBeingChased, OnExitBeingChased, OnUpdateBeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<AssassinationState>(EStateType.PlayerMain_AssassinationStealth, OnEnterAssassinationStealth, OnExitAssassinationStealth, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<AssassinationState>(EStateType.PlayerMain_AssassinationLeap, OnEnterAssassinationLeap, OnExitAssassinationLeap, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<AssassinationState>(EStateType.PlayerMain_AssassinationAir, OnEnterAssassinationAir, OnExitAssassinationAir, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<InteractingWithEntityState>(EStateType.PlayerMain_ObjectInteraction, OnEnterInteractWithObject, OnExitInteractWithObject, OnUpdateInteractWithObject);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<ParcourState>(EStateType.CombatCharacterMain_Parcour, OnEnterParcour, OnExitParcour, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<CallbackDelayState<AssassinCharacter>>(EStateType.Generic_WaitingForAnim, OnEnterWaitingForAnim, null, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddState<CallbackDelayState<AssassinCharacter>>(EStateType.Generic_WaitingForAnimCancelable, null, null, OnUpdateGeneral);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_HidingInObject);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_HidingInPatrol);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_AssassinationStealth);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_AssassinationLeap);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_AssassinationAir);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.Generic_WaitingForAnim);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.Generic_WaitingForAnimCancelable);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Combat, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Combat, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_AssassinationStealth, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_AssassinationLeap, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_AssassinationAir, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInObject, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInObject, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInObject, EStateType.Generic_WaitingForAnim);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInObjectExitAndMoveTo, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.PlayerMain_AssassinationStealth);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.PlayerMain_ObjectInteraction);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_HidingInPatrol, EStateType.PlayerMain_HidingInPatrol);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.CombatCharacterMain_Parcour);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.CombatCharacterMain_Combat);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.PlayerMain_HidingInPatrol);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.PlayerMain_AssassinationLeap);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.PlayerMain_AssassinationAir);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.Generic_WaitingForAnim);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_BeingChased, EStateType.Generic_WaitingForAnimCancelable);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_NpcInteraction);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_NpcInteraction, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Roaming, EStateType.PlayerMain_ObjectInteraction);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_ObjectInteraction, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.PlayerMain_NpcInteraction, EStateType.Generic_WaitingForAnim);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_HidingInObject);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_ObjectInteraction);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_HidingInObjectExitAndMoveTo);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnimCancelable, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.Generic_WaitingForAnimCancelable, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Parcour, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddTransition(EStateType.CombatCharacterMain_Parcour, EStateType.PlayerMain_BeingChased);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddUndesiredTransition(EStateType.PlayerMain_HidingInObject, EStateType.PlayerMain_HidingInObjectExitAndMoveTo);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddUndesiredTransition(EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_AssassinationStealth, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_AssassinationLeap, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_AssassinationAir, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.CombatCharacterMain_Combat, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_HidingInPatrol, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_ObjectInteraction, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.AssassinApproachingTarget, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.CombatCharacterMain_Roaming, EStateMachineType.CharacterInteraction, EStateType.CharacterInteraction_OfferRejected);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.CharacterInteraction, EStateType.CharacterInteraction_OfferAccepted);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.Generic_WaitingForAnim, EStateMachineType.ProjectileFlight, EStateType.Projectile_Arrived);
			StateMachineBase<AssassinCharacter, PlayerMain>.AddControlling(EStateType.PlayerMain_NpcInteraction, EStateMachineType.CharacterInteraction, EStateType.CharacterInteraction_OfferOpen);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			FloorType = EFloorType.Invalid;
			LastStateChangeTurn = 0;
			StateChanged += OnStateChanged;
		}

		public override void OnUnregistered()
		{
			StateChanged -= OnStateChanged;
			base.OnUnregistered();
		}

		private void OnStateChanged(IStateMachine _stateMachine, EStateType _from, EStateType _to)
		{
			LastStateChangeTurn = LogicMain.CurrentTurn;
		}

		private static void OnUpdateGeneral(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			if (!(_stateMachine.Entity is PlayerCharacter))
			{
				return;
			}
			if (LogicMain.FrameTime > 0f)
			{
				Vector3 desiredVelocity = _stateMachine.Entity.UnityComponentCache.NavMeshAgent.desiredVelocity;
				if (ParcourHelper.IsLeavingMissionArea(_stateMachine.Entity.Position, desiredVelocity.normalized))
				{
					_stateMachine.Entity.StopMoving();
				}
			}
			NavMeshHit hit;
			if (!NavMesh.SamplePosition(_stateMachine.Entity.Position, out hit, 1f, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return;
			}
			PlayerMain playerMain = (PlayerMain)_stateMachine;
			if ((hit.mask & NavMeshLayers.GroundSurfaceLayerMask) == NavMeshLayers.GroundSurfaceLayerMask)
			{
				if (playerMain.FloorType != 0)
				{
					LogicMain.MissionResult.IncreaseEnterGroundCount();
				}
				playerMain.FloorType = EFloorType.Ground;
				LogicMain.MissionResult.IncreaseGroundTurnCount();
				return;
			}
			LogicMain.MissionResult.ResetGroundTurnCount();
			if ((hit.mask & NavMeshLayers.RoofSurfaceLayerMask) == NavMeshLayers.RoofSurfaceLayerMask)
			{
				if (playerMain.FloorType != EFloorType.Roof)
				{
					LogicMain.MissionResult.IncreaseEnterRooftopCount();
				}
				playerMain.FloorType = EFloorType.Roof;
			}
		}

		private static void OnEnterRoaming(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			if (_stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				_stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
		}

		private static void OnExitRoaming(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> currentState, IState<AssassinCharacter> nextState)
		{
			switch (nextState.StateType)
			{
			case EStateType.CombatCharacterMain_Parcour:
			case EStateType.PlayerMain_AssassinationLeap:
			case EStateType.PlayerMain_AssassinationStealth:
			case EStateType.PlayerMain_AssassinationAir:
				return;
			}
			stateMachine.Entity.StopApproaching();
		}

		private static void OnUpdateRoaming(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
			if (_stateMachine.ScheduledStateType == EStateType.Invalid && ((PlayerMain)_stateMachine).CheckBeingChased())
			{
				_stateMachine.ScheduleStateChange(EStateType.PlayerMain_BeingChased);
			}
		}

		private static void OnEnterCombat(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			CombatController.Instance.EnterCombat();
		}

		private static void OnUpdateCombat(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
		}

		private static void OnExitCombat(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			if (!_stateMachine.Entity.GetHasZeroHealth())
			{
				_stateMachine.Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Sheathed);
			}
		}

		private static void RetractHiddenBlades(IStateMachine<AssassinCharacter> _stateMachine)
		{
			Effects effects = _stateMachine.Entity.UnityComponentCache.AnimationController.EntityId.TryGetEntity<IEffectsCarrier>().Effects;
			effects.OnEffectOff("RightHiddenBlade");
			effects.OnEffectOff("LeftHiddenBlade");
		}

		private static void OnEnterAssassinationStealth(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			TargetState.Payload payload = (TargetState.Payload)_payload;
			PrepareAssassinationTarget(payload);
			payload.Target.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateStealthTarget(_stateMachine.Entity, payload.Target);
			_stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateStealthInstigator(_stateMachine.Entity.GameObject, payload.Target.GameObject);
			_stateMachine.EnterCallbackDelayState(EStateType.PlayerMain_AssassinationStealth, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming);
		}

		private static void OnExitAssassinationStealth(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			TargetState.Payload payload = (TargetState.Payload)_payload;
			payload.Target.CanBePooled = true;
			payload.Target.KillCharacter(EKillType.AssassinationStealth);
			RetractHiddenBlades(_statemachine);
		}

		private static void OnEnterAssassinationLeap(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			AssassinationState.Payload payload = (AssassinationState.Payload)_payload;
			PrepareAssassinationTarget(payload);
			payload.RegisterForCameraEvent(_stateMachine.Entity);
			payload.Target.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateLeapTarget(_stateMachine.Entity, payload.Target);
			_stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateLeapInstigator(_stateMachine.Entity.GameObject, payload.Target.GameObject, _stateMachine.Entity == LogicMain.PlayerEntity);
			_stateMachine.EnterCallbackDelayState(EStateType.PlayerMain_AssassinationLeap, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming);
		}

		private static void OnExitAssassinationLeap(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			AssassinationState.Payload payload = (AssassinationState.Payload)_payload;
			payload.Target.CanBePooled = true;
			payload.Target.KillCharacter(EKillType.AssassinationLeap);
			RetractHiddenBlades(_statemachine);
			payload.UnregisterEventListener();
		}

		private static void OnEnterAssassinationAir(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			AssassinationState.Payload payload = (AssassinationState.Payload)_payload;
			PrepareAssassinationTarget(payload);
			payload.RegisterForCameraEvent(_stateMachine.Entity);
			payload.Target.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateAirTarget(_stateMachine.Entity, payload.Target);
			_stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAssassinateAirInstigator(_stateMachine.Entity.GameObject, payload.Target.GameObject, _stateMachine.Entity == LogicMain.PlayerEntity);
			_stateMachine.EnterCallbackDelayState(EStateType.PlayerMain_AssassinationAir, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming);
		}

		private static void OnExitAssassinationAir(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			AssassinationState.Payload payload = (AssassinationState.Payload)_payload;
			payload.Target.CanBePooled = true;
			payload.Target.KillCharacter(EKillType.AssassinationAir);
			RetractHiddenBlades(_statemachine);
			payload.UnregisterEventListener();
		}

		private static void PrepareAssassinationTarget(TargetState.Payload payload)
		{
			payload.Target.CharacterAi.IsActive = false;
			payload.Target.CanBePooled = false;
			payload.Target.HeadIcon.IsVisible = false;
			payload.Target.CharacterStats.Set_bool(StatTypes.EStatType.Character_Conflict_AboutToDie, true);
			payload.Target.DropUnsheathedWeapons();
		}

		private static void OnEnterHidingInObject(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			_stateMachine.Entity.Highlight.SetMode(Highlight.EHighlightLayer.Hiding, Highlight.ESelectionMode.Hiding);
			HidingInObjectState.HidingInObjectPayload hidingInObjectPayload = (HidingInObjectState.HidingInObjectPayload)_payload;
			if (hidingInObjectPayload.EntityId.Type == EntityTypes.EType.Haystack || hidingInObjectPayload.EntityId.Type == EntityTypes.EType.RooftopGarden)
			{
				AssassinRendering.DisableOutline();
			}
		}

		private static void OnExitHidingInObject(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			HidingInObjectState.HidingInObjectPayload hidingInObjectPayload = (HidingInObjectState.HidingInObjectPayload)_payload;
			hidingInObjectPayload.ShouldExit = false;
			UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = null;
			_statemachine.Entity.Highlight.RemoveMode(Highlight.EHighlightLayer.Hiding);
			if (hidingInObjectPayload.EntityId.Type == EntityTypes.EType.Haystack || hidingInObjectPayload.EntityId.Type == EntityTypes.EType.RooftopGarden)
			{
				AssassinRendering.EnableOutline();
			}
		}

		private static void OnUpdateHidingInObject(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
			KillFromHideout activeKillFromHideout = _stateMachine.Entity.GetActiveKillFromHideout();
			activeKillFromHideout.Update();
			HidingInObjectState.HidingInObjectPayload hidingInObjectPayload = (HidingInObjectState.HidingInObjectPayload)_payload;
			if (activeKillFromHideout.IsIdling && hidingInObjectPayload.ShouldExit)
			{
				_stateMachine.Entity.ExitHideout(hidingInObjectPayload.Destination);
			}
		}

		private static void OnEnterHidingInObjectExitAndMoveTo(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = (InteractingWithEntityState.InteractingWithEntityPayload)_payload;
			NavMeshPath path = new NavMeshPath();
			if (_stateMachine.Entity.UnityComponentCache.NavMeshAgent.CalculatePath(interactingWithEntityPayload.Destination, path))
			{
				Messenger.Instance.Broadcast(new MoveToEvent(path, MoveToEvent.EInputType.GroundTap, -1f, null, EDispatcherType.Tap));
			}
			_stateMachine.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}

		private static void OnEnterHidingInPatrol(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevState, IState<AssassinCharacter> _currentState)
		{
			HidingInPatrolState.HidingInPatrolPayload hidingInPatrolPayload = (HidingInPatrolState.HidingInPatrolPayload)_payload;
			hidingInPatrolPayload.m_startTime = LogicMain.CurrentTime;
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = hidingInPatrolPayload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			if (!entity.AddAtHidingSpot(_stateMachine.Entity.Id))
			{
				_stateMachine.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
			}
			else
			{
				UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection = null;
			}
		}

		private static void OnExitHidingInPatrol(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			HidingInPatrolState.HidingInPatrolPayload hidingInPatrolPayload = (HidingInPatrolState.HidingInPatrolPayload)_payload;
			hidingInPatrolPayload.ShouldExit = false;
			hidingInPatrolPayload.m_endTime = LogicMain.CurrentTime;
			EntityId entityId = hidingInPatrolPayload.PrevPatrolId;
			hidingInPatrolPayload.PrevPatrolId = EntityId.InvalidId;
			if (!entityId.IsValid)
			{
				entityId = hidingInPatrolPayload.PatrolId;
				hidingInPatrolPayload.PatrolId = EntityId.InvalidId;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = entityId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			bool flag = entity.RemoveFromHidingSpot(_stateMachine.Entity.Id);
		}

		private static void OnUpdateHidingInPatrol(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
			HidingInPatrolState.HidingInPatrolPayload hidingInPatrolPayload = (HidingInPatrolState.HidingInPatrolPayload)_payload;
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = hidingInPatrolPayload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			if ((_stateMachine.Entity.GetIsApproaching() || entity.HidingEntityShouldExit() || hidingInPatrolPayload.ShouldExit) && _stateMachine.Entity.AssassinCharacterStateMachines.PlayerMain.ScheduledStateType == EStateType.Invalid)
			{
				_stateMachine.Entity.EnterRoaming();
			}
		}

		private static void OnEnterBeingChased(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			_statemachine.Entity.AssassinCharacterStateMachines.PlayerProfile.ScheduleStateChangeForced(EStateType.PlayerProfile_Chase);
			AnimationTaskManager taskManager = _statemachine.Entity.UnityComponentCache.AnimationController.TaskManager;
			if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.HoldCombatIdle)
			{
				taskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base, true);
			}
			if (_statemachine.Entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				_statemachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
		}

		private static void OnUpdateBeingChased(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
			if (!((PlayerMain)_stateMachine).CheckBeingChased())
			{
				_stateMachine.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
			}
		}

		private static void OnExitBeingChased(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			if (_statemachine.Entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType != EStateType.PlayerProfile_High)
			{
				_statemachine.Entity.AssassinCharacterStateMachines.PlayerProfile.ScheduleStateChangeForced(EStateType.PlayerProfile_High);
			}
		}

		private bool CheckBeingChased()
		{
			return (Entity.CheckBeingChased() || Entity.GetIsExposed()) && !Entity.GetIsHiding();
		}

		private static void OnEnterInteractWithObject(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = _payload.Cast<InteractingWithEntityState.InteractingWithEntityPayload>();
			InteractableObject entity = interactingWithEntityPayload.EntityId.GetEntity<InteractableObject>();
			if (entity is Pigeonry)
			{
				Pigeonry pigeonry = entity.Cast<Pigeonry>();
				pigeonry.PigeonryComponent.OnStartInteraction();
				AnimationTaskManager taskManager = _statemachine.Entity.UnityComponentCache.AnimationController.TaskManager;
				taskManager.CreateAnimationTaskPigeonDelivery(pigeonry);
				_statemachine.EnterCallbackDelayState(EStateType.PlayerMain_ObjectInteraction, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming);
			}
			else if (entity is Codex)
			{
				entity.Cast<Codex>().CodexEntry.OnStartInteraction();
			}
			if (entity.InteractableObjectDefinition.Definition.InteractionIsSafe && _statemachine.Entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType != EStateType.Character_Immortal)
			{
				_statemachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Immortal);
			}
		}

		private static void OnUpdateInteractWithObject(IStateMachine<AssassinCharacter> _stateMachine, IPayload _payload, IState<AssassinCharacter> _state)
		{
			OnUpdateGeneral(_stateMachine, _payload, _state);
			InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = _payload.Cast<InteractingWithEntityState.InteractingWithEntityPayload>();
			InteractableObject entity = interactingWithEntityPayload.EntityId.GetEntity<InteractableObject>();
			if (entity is Pigeonry)
			{
				Pigeonry pigeonry = entity.Cast<Pigeonry>();
				pigeonry.PigeonryComponent.Update();
			}
		}

		private static void OnExitInteractWithObject(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> currentState, IState<AssassinCharacter> nextState)
		{
			InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = payload.Cast<InteractingWithEntityState.InteractingWithEntityPayload>();
			InteractableObject entity = interactingWithEntityPayload.EntityId.GetEntity<InteractableObject>();
			if (entity is Pigeonry)
			{
				entity.Cast<Pigeonry>().PigeonryComponent.OnEndInteraction();
			}
			if (!interactingWithEntityPayload.HasBeenCanceled)
			{
				entity.InteractableObjectStateMachines.ObjectInteraction.ScheduleStateChange(EStateType.Object_Disabled);
			}
			AnimationTaskManager taskManager = stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager;
			taskManager.CreateAnimationTaskIdle(taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.Idle && interactingWithEntityPayload.HasBeenCanceled);
		}

		private static void OnEnterNpcInteraction(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _prevstate, IState<AssassinCharacter> _currentstate)
		{
			AutoAnimationRotationToTarget autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(_statemachine.Entity, _payload.Cast<InteractingWithEntityState.InteractingWithEntityPayload>().EntityId.GameEntity);
			autoAnimationRotationToTarget.FireAndForget = true;
			_statemachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Immortal);
		}

		private static void OnExitNpcInteraction(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> currentState, IState<AssassinCharacter> nextState)
		{
			InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = payload.Cast<InteractingWithEntityState.InteractingWithEntityPayload>();
			interactingWithEntityPayload.Plot = null;
			InteractionCharacter entity = interactingWithEntityPayload.EntityId.GetEntity<InteractionCharacter>();
			NpcData.NpcDefinition.ENpcInteractionType interactionType = entity.NpcDefinition.Definition.InteractionType;
			if (interactionType == NpcData.NpcDefinition.ENpcInteractionType.Mission && !interactingWithEntityPayload.HasBeenCanceled)
			{
				entity.InteractionCharacterStateMachines.CharacterInteraction.ScheduleStateChange(EStateType.CharacterInteraction_MissionDeliver);
			}
			stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle(interactingWithEntityPayload.HasBeenCanceled);
		}

		private static void OnEnterParcour(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> prevState, IState<AssassinCharacter> currentState)
		{
			if (stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType == EStateType.Character_Immortal)
			{
				stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			}
		}

		private static void OnExitParcour(IStateMachine<AssassinCharacter> _statemachine, IPayload _payload, IState<AssassinCharacter> _currentstate, IState<AssassinCharacter> _nextstate)
		{
			_statemachine.Entity.PlayerStats.Set_int(StatTypes.EStatType.Player_ClimbingHitCount, 0);
		}

		private static void OnEnterWaitingForAnim(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> prevState, IState<AssassinCharacter> currentState)
		{
			if (stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.CurrentStateType != EStateType.Character_Immortal)
			{
				stateMachine.Entity.AssassinCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Immortal);
			}
		}
	}
}
