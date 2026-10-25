using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class AssassinExtensions
	{
		private class EnemyEntry
		{
			public float MinDistance { get; set; }

			public EnemyCharacter Enemycharacter { get; set; }

			public EnemyEntry(float _minDistance, EnemyCharacter _enemyCharacter)
			{
				MinDistance = _minDistance;
				Enemycharacter = _enemyCharacter;
			}
		}

		private const float CloseDistanceOfHidingSpot = 5f;

		public static int LastUsedLinkID = -1;

		private static BBList<EnemyEntry> m_closeEnemies = new BBList<EnemyEntry>();

		private static readonly NavMeshPath m_dummyPath = new NavMeshPath();

		private static NavMeshHit m_dummyHit;

		public static bool EnterHideout(this AssassinCharacter _entity, HideoutObject _hideout, InteractionLocators.Locator _locator)
		{
			AnimationTaskType taskType = AnimationTaskType.Unknown;
			EntityTypes.EType type = _hideout.Type;
			bool flag = false;
			AnimationTaskManager taskManager = _entity.UnityComponentCache.AnimationController.TaskManager;
			switch (type)
			{
			case EntityTypes.EType.RooftopGarden:
				taskType = AnimationTaskType.RooftopGardenEnter;
				UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current = (RooftopGarden)_hideout;
				flag = taskManager.CreateAnimationTaskRooftopGardenEnter((RooftopGarden)_hideout, _locator);
				break;
			case EntityTypes.EType.Bench:
				taskType = AnimationTaskType.BenchEnter;
				UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.Current = (Bench)_hideout;
				flag = taskManager.CreateAnimationTaskBenchEnter((Bench)_hideout, _locator);
				break;
			case EntityTypes.EType.Haystack:
				if (_locator != null)
				{
					taskType = AnimationTaskType.HayStackEnter;
					UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current = (Haystack)_hideout;
					flag = taskManager.CreateAnimationTaskHayStackEnter((Haystack)_hideout, _locator);
				}
				else
				{
					taskType = AnimationTaskType.LeapOfFaith;
					UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current = (Haystack)_hideout;
					flag = taskManager.CreateAnimationTaskLeapOfFaith((Haystack)_hideout);
				}
				break;
			}
			if (!flag)
			{
				return false;
			}
			PlayerMain playerMain = _entity.PlayerMainCarrier.PlayerMain;
			playerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_HidingInObject, EStateType.PlayerMain_HidingInObject, taskType);
			PlayerMain.HidingInObjectState.HidingInObjectPayload payload = playerMain.GetPayload<PlayerMain.HidingInObjectState.HidingInObjectPayload>(EStateType.PlayerMain_HidingInObject);
			payload.EntityId = _hideout.Id;
			payload.ShouldExit = payload.WillHidingLimitFailOnIncrease || (_entity.GetIsExposed() && (!_entity.GetIsUnseen() || AreAwareCharacterClose(_hideout)));
			payload.Destination = ((_locator != null) ? _locator.Position.transform.position : _hideout.Position);
			return true;
		}

		public static bool EnterHidingInPatrol(this AssassinCharacter _entity, EntityId _patrolId)
		{
			bool flag = false;
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			switch (playerMain.ScheduledStateType)
			{
			case EStateType.CombatCharacterMain_Roaming:
			case EStateType.PlayerMain_BeingChased:
				playerMain.ChangeStateForced(playerMain.ScheduledStateType);
				ScheduleHidingInPatrol(_patrolId, _entity, playerMain);
				flag = true;
				break;
			case EStateType.Invalid:
				ScheduleHidingInPatrol(_patrolId, _entity, playerMain);
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				_entity.RegisterInteraction(EInteractionReason.HidingInPatrol);
			}
			return flag;
		}

		public static bool FreeCaptives(this AssassinCharacter _entity, EntityId _patrolId)
		{
			_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle();
			_entity.RegisterInteraction(EInteractionReason.FreeCapturedNpcs);
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _patrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			entity.PatrolComponent.MissionData.SetMapIconType(EMapIconType.Invalid_EMapIconType);
			if (entity.CaptiveComponent == null)
			{
				return _entity.FinalizeFreeCaptives(_patrolId);
			}
			return entity.CaptiveComponent.Liberate();
		}

		public static bool FinalizeFreeCaptives(this AssassinCharacter _entity, EntityId _patrolId)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _patrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			BBList<MemberInfo> entities = entity.PatrolComponent.Formation.Entities;
			if (entities.Count == 0)
			{
				return false;
			}
			LogicMain.MissionResult.IncreaseUsedHotspotCount(EHotspot.EHotspot_Captives);
			entity.PatrolStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
			Vector3 position = entity.PatrolComponent.Formation.Entities[0].m_id.GameEntity.Position;
			for (int i = 0; i < entities.Count; i++)
			{
				NpcCharacter npcCharacter = entities[i].m_id.TryGetEntity<NpcCharacter>();
				if (npcCharacter != null)
				{
					npcCharacter.NpcStats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, position);
				}
			}
			return true;
		}

		public static void SetLastFallHeight(this AssassinCharacter _entity, float _height)
		{
			EntityStats entityStats = _entity.EntityStats;
			if (entityStats.HasStat(StatTypes.EStatType.Player_LastFallingHeight))
			{
				entityStats.Set_float(StatTypes.EStatType.Player_LastFallingHeight, _height);
			}
		}

		public static float GetLastFallHeight(this AssassinCharacter _entity)
		{
			EntityStats entityStats = _entity.EntityStats;
			if (entityStats.HasStat(StatTypes.EStatType.Player_LastFallingHeight))
			{
				return entityStats.Get_float(StatTypes.EStatType.Player_LastFallingHeight);
			}
			return 0f;
		}

		public static bool EnterParcour(this AssassinCharacter _entity, ParcourEvent _event)
		{
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			if (playerMain.ScheduledStateType != 0 && playerMain.ScheduledStateType != EStateType.CombatCharacterMain_Roaming && playerMain.ScheduledStateType != EStateType.PlayerMain_BeingChased)
			{
				return false;
			}
			if (!playerMain.IsCurrentOrScheduled(EStateType.CombatCharacterMain_Roaming) && !playerMain.IsCurrentOrScheduled(EStateType.PlayerMain_BeingChased))
			{
				return false;
			}
			AnimationTaskType animationTaskType = AnimationTaskType.Unknown;
			Vector3 parcourDestination;
			switch (_event.Type)
			{
			case ParcourEvent.EParcourType.UseLink:
			{
				UseLinkEvent useLinkEvent = _event as UseLinkEvent;
				if (useLinkEvent == null)
				{
					return false;
				}
				OffMeshLinkEntity link = useLinkEvent.Link;
				parcourDestination = link.EndPosition;
				LastUsedLinkID = link.OffMeshLink.GetComponent<OffMeshLinkMeta>().UniqueID;
				switch (link.LinkType)
				{
				case eLinkType.Climb:
					animationTaskType = AnimationTaskType.Climb;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskClimb(link);
					break;
				case eLinkType.Balance:
					animationTaskType = AnimationTaskType.Balance;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskBalance(link);
					break;
				case eLinkType.JumpHorizontal:
					animationTaskType = AnimationTaskType.JumpHorizontal;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskJumpHorizontal(link);
					break;
				case eLinkType.JumpDown:
					animationTaskType = AnimationTaskType.JumpDown;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskJumpDown(link);
					break;
				case eLinkType.RunPassOver:
					animationTaskType = AnimationTaskType.RunPassOver;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskRunPassOver(link);
					break;
				case eLinkType.RunPassOverDown:
					animationTaskType = AnimationTaskType.RunPassOverDown;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskRunPassOverDown(link);
					break;
				case eLinkType.ParkourSequence:
					animationTaskType = AnimationTaskType.ParkourSequence;
					_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskParkourSequence(link);
					break;
				default:
					return false;
				}
				break;
			}
			case ParcourEvent.EParcourType.JumpHorizontalToWall:
			{
				JumpHorizontalToWallEvent jumpHorizontalToWallEvent = _event as JumpHorizontalToWallEvent;
				if (jumpHorizontalToWallEvent == null)
				{
					return false;
				}
				animationTaskType = AnimationTaskType.Climb;
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskClimb(jumpHorizontalToWallEvent);
				parcourDestination = jumpHorizontalToWallEvent.TargetPosition;
				break;
			}
			case ParcourEvent.EParcourType.JumpAnywhere:
			{
				JumpAnywhereEvent jumpAnywhereEvent = _event as JumpAnywhereEvent;
				if (jumpAnywhereEvent == null)
				{
					return false;
				}
				animationTaskType = AnimationTaskType.JumpDownAnywhere;
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskJumpDownAnywhere(jumpAnywhereEvent);
				parcourDestination = jumpAnywhereEvent.LandPosition;
				break;
			}
			case ParcourEvent.EParcourType.ClimbRebound:
			{
				ClimbReboundEvent climbReboundEvent = _event as ClimbReboundEvent;
				if (climbReboundEvent == null)
				{
					return false;
				}
				animationTaskType = AnimationTaskType.ClimbRebound;
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskClimbRebound(climbReboundEvent);
				parcourDestination = _entity.Position;
				break;
			}
			default:
				return false;
			}
			PlayerMain.ParcourState.ParcourPayload payload = playerMain.GetPayload<PlayerMain.ParcourState.ParcourPayload>(EStateType.CombatCharacterMain_Parcour);
			payload.ParcourDestination = parcourDestination;
			EStateType eStateType = playerMain.CurrentStateType;
			if (eStateType == EStateType.PlayerMain_HidingInPatrol)
			{
				eStateType = EStateType.CombatCharacterMain_Roaming;
			}
			playerMain.EnterCallbackDelayState(EStateType.CombatCharacterMain_Parcour, eStateType, eStateType, animationTaskType, AnimationTaskManager.ELayer.Base, true);
			return true;
		}

		public static PlayerMain.HidingInObjectState.HidingInObjectPayload GetActiveHidingInObjectPayload(this AssassinCharacter _entity)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
			{
				return _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject) as PlayerMain.HidingInObjectState.HidingInObjectPayload;
			}
			return null;
		}

		public static EntityTypes.EType GetActiveHideoutType(this AssassinCharacter _entity)
		{
			PlayerMain.HidingInObjectState.HidingInObjectPayload activeHidingInObjectPayload = _entity.GetActiveHidingInObjectPayload();
			return (activeHidingInObjectPayload != null) ? activeHidingInObjectPayload.EntityId.Type : EntityTypes.EType.Invalid;
		}

		public static KillFromHideout GetActiveKillFromHideout(this AssassinCharacter _entity)
		{
			EntityTypes.EType activeHideoutType = _entity.GetActiveHideoutType();
			KillFromHideout result = null;
			switch (activeHideoutType)
			{
			case EntityTypes.EType.Haystack:
				result = _entity.KillFromHaystack;
				break;
			case EntityTypes.EType.RooftopGarden:
				result = _entity.KillFromRooftopGarden;
				break;
			case EntityTypes.EType.Bench:
				result = _entity.KillFromBench;
				break;
			}
			return result;
		}

		public static bool ExitHideout(this AssassinCharacter _entity, Vector3 _destination)
		{
			switch (_entity.GetActiveHideoutType())
			{
			case EntityTypes.EType.Haystack:
				return UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.LeaveInteraction(_entity, _destination);
			case EntityTypes.EType.RooftopGarden:
				return UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.LeaveInteraction(_entity, _destination);
			case EntityTypes.EType.Bench:
				return UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.LeaveInteraction(_entity, _destination);
			default:
				return false;
			}
		}

		public static bool ExitBench(this AssassinCharacter _entity, Vector3 _destination, BenchManager.ELeaveReason _reason)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType != EStateType.PlayerMain_HidingInObject)
			{
				return false;
			}
			AnimationTaskType currentTaskType = _entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			if (!Assets.Scripts.Engine.Helpers.Verify(currentTaskType == AnimationTaskType.BenchIdle || currentTaskType == AnimationTaskType.BenchKillInstigator, "Expecting BenchIdle or BenchKillInstigator, got {0}", currentTaskType))
			{
				return false;
			}
			switch (_reason)
			{
			case BenchManager.ELeaveReason.StandUpFromBench:
			{
				bool flag = false;
				bool immediateStateChange = false;
				EStateType nextStateType = EStateType.CombatCharacterMain_Roaming;
				if (_destination != Vector3.zero && Vector3.Distance(_entity.Position, _destination) > 2f)
				{
					NavMeshHit _hit = default(NavMeshHit);
					if (NavigationHelper.CascadingSamplePosition(_destination, out _hit, _entity.UnityComponentCache.NavMeshAgent.areaMask))
					{
						PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObjectExitAndMoveTo);
						payload.Destination = _hit.position;
						nextStateType = EStateType.PlayerMain_HidingInObjectExitAndMoveTo;
						immediateStateChange = true;
						flag = true;
					}
				}
				if (!flag)
				{
					flag = _entity.GetIsNavigating();
				}
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskBenchExit(flag, _destination);
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, nextStateType, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.BenchExit, AnimationTaskManager.ELayer.Base, immediateStateChange);
				break;
			}
			case BenchManager.ELeaveReason.KillFromBench:
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.BenchKillInstigator);
				break;
			}
			_entity.AssassinCharacterStateMachines.PlayerMain.SetDelayedTriggerAction(EStateType.Generic_WaitingForAnim, UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.OnHideoutExited);
			return true;
		}

		public static bool ExitHaystack(this AssassinCharacter _entity, Vector3 _destination)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType != EStateType.PlayerMain_HidingInObject)
			{
				return false;
			}
			AnimationTaskType currentTaskType = _entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			if (!Assets.Scripts.Engine.Helpers.Verify(currentTaskType == AnimationTaskType.HayStackIdle, "Expecting HayStackIdle, got {0}", currentTaskType))
			{
				return false;
			}
			PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = (PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload)_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStatePayload;
			HideoutObject entity = interactingWithEntityPayload.EntityId.GetEntity<HideoutObject>();
			InteractionLocators.Locator _locator = null;
			Vector3 _exitTarget = Vector3.zero;
			if (DetermineHideoutExitTarget(_destination, _entity.UnityComponentCache.AnimationController.Agent, entity, out _exitTarget, out _locator))
			{
				bool flag = false;
				bool immediateStateChange = false;
				EStateType nextStateType = EStateType.CombatCharacterMain_Roaming;
				if (_destination != Vector3.zero && Vector3.Distance(_entity.Position, _destination) > 2f)
				{
					NavMeshHit _hit = default(NavMeshHit);
					if (NavigationHelper.CascadingSamplePosition(_destination, out _hit, _entity.UnityComponentCache.NavMeshAgent.areaMask))
					{
						PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObjectExitAndMoveTo);
						payload.Destination = _hit.position;
						nextStateType = EStateType.PlayerMain_HidingInObjectExitAndMoveTo;
						immediateStateChange = true;
						flag = true;
					}
				}
				if (!flag)
				{
					flag = _entity.GetIsNavigating();
				}
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskHayStackExit(_exitTarget, entity, flag);
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, nextStateType, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.HayStackExit, AnimationTaskManager.ELayer.Base, immediateStateChange);
				_entity.AssassinCharacterStateMachines.PlayerMain.SetDelayedTriggerAction(EStateType.Generic_WaitingForAnim, UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.OnHideoutExited);
				return true;
			}
			return false;
		}

		public static bool ExitRooftopGarden(this AssassinCharacter _entity, Vector3 _destination)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType != EStateType.PlayerMain_HidingInObject)
			{
				return false;
			}
			AnimationTaskType currentTaskType = _entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			if (!Assets.Scripts.Engine.Helpers.Verify(currentTaskType == AnimationTaskType.RooftopGardenIdle, "Expecting RooftopGardenIdle, got {0}", currentTaskType))
			{
				return false;
			}
			PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload interactingWithEntityPayload = (PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload)_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStatePayload;
			HideoutObject entity = interactingWithEntityPayload.EntityId.GetEntity<HideoutObject>();
			InteractionLocators.Locator _locator = null;
			Vector3 _exitTarget = Vector3.zero;
			if (DetermineHideoutExitTarget(_destination, _entity.UnityComponentCache.AnimationController.Agent, entity, out _exitTarget, out _locator))
			{
				bool flag = false;
				bool immediateStateChange = false;
				EStateType nextStateType = EStateType.CombatCharacterMain_Roaming;
				if (_destination != Vector3.zero && Vector3.Distance(_entity.Position, _destination) > 2f)
				{
					NavMeshHit _hit = default(NavMeshHit);
					if (NavigationHelper.CascadingSamplePosition(_destination, out _hit, _entity.UnityComponentCache.NavMeshAgent.areaMask))
					{
						PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObjectExitAndMoveTo);
						payload.Destination = _hit.position;
						nextStateType = EStateType.PlayerMain_HidingInObjectExitAndMoveTo;
						immediateStateChange = true;
						flag = true;
					}
				}
				if (!flag)
				{
					flag = _entity.GetIsNavigating();
				}
				_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskRooftopGardenExit(entity, _locator, _exitTarget, flag);
				_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, nextStateType, EStateType.CombatCharacterMain_Roaming, AnimationTaskType.RooftopGardenExit, AnimationTaskManager.ELayer.Base, immediateStateChange);
				_entity.AssassinCharacterStateMachines.PlayerMain.SetDelayedTriggerAction(EStateType.Generic_WaitingForAnim, UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.OnHideoutExited);
				return true;
			}
			return false;
		}

		public static bool CancelMovement(this AssassinCharacter _entity, bool _startIdle)
		{
			if (!_entity.GetIsRoaming() && !_entity.GetIsBeingChased())
			{
				return false;
			}
			return _entity.CancelMovement(_startIdle, false);
		}

		public static bool EnterBeingPushed(this AssassinCharacter _entity, EntityId _instigator)
		{
			if (!_entity.AssassinCharacterStateMachines.PlayerMain.IsCurrentOrScheduled(EStateType.CombatCharacterMain_Roaming))
			{
				return false;
			}
			if (!_entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTasksPushTarget(_instigator))
			{
				return false;
			}
			EStateType currentStateType = _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType;
			_entity.AssassinCharacterStateMachines.PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnimCancelable, currentStateType, currentStateType, AnimationTaskType.PushTarget);
			return true;
		}

		public static bool StopMoving(this AssassinCharacter _entity)
		{
			if (!_entity.GetIsRoaming() && !_entity.GetIsBeingChased())
			{
				return false;
			}
			if (_entity.StopApproaching())
			{
				return true;
			}
			return _entity.CancelMovement(true);
		}

		public static bool StopApproaching(this AssassinCharacter _entity, bool _stopMoving = true)
		{
			bool flag = _entity.AssassinInteraction.StopApproaching(_stopMoving);
			IPlayerEventHandlerCarrier playerEventHandlerCarrier = _entity as IPlayerEventHandlerCarrier;
			if (playerEventHandlerCarrier != null)
			{
				flag |= playerEventHandlerCarrier.PlayerEventHandler.StopApproaching(_stopMoving);
			}
			return flag;
		}

		public static bool CheckBeingChased(this AssassinCharacter _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			return playerCharacter != null && playerCharacter.CheckBeingChased();
		}

		public static EnemyCharacter GetClosestEnemyThreateningOrNormal(this AssassinCharacter _entity, float _closestDistance)
		{
			EntityId entityId = _entity.EntityStats.Get_EntityId(StatTypes.EStatType.Player_ClosestEnemyThreateningOrNormal);
			if (entityId.IsValid)
			{
				float num = _entity.EntityStats.Get_float(StatTypes.EStatType.Player_ClosestEnemyDistanceSqr);
				if (num <= _closestDistance * _closestDistance)
				{
					return entityId.GetEntity<EnemyCharacter>();
				}
			}
			return null;
		}

		public static void EnterRoaming(this AssassinCharacter _entity)
		{
			_entity.AssassinCharacterStateMachines.PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}

		public static bool CanTriggerSelectedSkill(this AssassinCharacter _entity, ISkillPayload _payload, out ESkillAvailabilityFlags _availabilityFlags)
		{
			if (_entity.Skills.SelectedSkill.IsValid)
			{
				return _entity.Skills.CanBeTriggered(_entity.Skills.SelectedSkill, _payload, out _availabilityFlags);
			}
			_availabilityFlags = ESkillAvailabilityFlags.Unknown;
			return false;
		}

		public static bool TriggerSelectedSkill(this AssassinCharacter _entity, ISkillPayload _payload)
		{
			if (_entity.Skills.SelectedSkill.IsValid)
			{
				return _entity.Skills.Trigger(_entity.Skills.SelectedSkill, _payload);
			}
			return false;
		}

		public static bool GetIsMovementAllowed(this AssassinCharacter _entity)
		{
			return !_entity.EntityStats.Get_bool(StatTypes.EStatType.Player_MovementPrevented);
		}

		public static void RegisterInteraction(this AssassinCharacter _entity, EInteractionReason _interactionReason)
		{
			IPlayerAlertlevelCarrier playerAlertlevelCarrier = _entity.StateMachines as IPlayerAlertlevelCarrier;
			if (playerAlertlevelCarrier != null)
			{
				playerAlertlevelCarrier.PlayerAlertLevel.RegisterInteraction(_interactionReason);
			}
		}

		public static void RegisterExposingInteraction(this AssassinCharacter _entity)
		{
			IPlayerAlertlevelCarrier playerAlertlevelCarrier = _entity.StateMachines as IPlayerAlertlevelCarrier;
			if (playerAlertlevelCarrier != null)
			{
				playerAlertlevelCarrier.PlayerAlertLevel.RegisterExposingInteraction();
			}
		}

		public static void RegisteIncreaseToNotoriety(this AssassinCharacter _entity, float _minNotoriety)
		{
			IPlayerAlertlevelCarrier playerAlertlevelCarrier = _entity.StateMachines as IPlayerAlertlevelCarrier;
			if (playerAlertlevelCarrier != null)
			{
				playerAlertlevelCarrier.PlayerAlertLevel.RegisterIncreaseToNotoriety(_minNotoriety);
			}
		}

		public static void RegisterDecreaseToNotoriety(this AssassinCharacter _entity, float _maxNotoriety)
		{
			IPlayerAlertlevelCarrier playerAlertlevelCarrier = _entity.StateMachines as IPlayerAlertlevelCarrier;
			if (playerAlertlevelCarrier != null)
			{
				playerAlertlevelCarrier.PlayerAlertLevel.RegisterDecreaseToNotoriety(_maxNotoriety);
			}
		}

		public static EMovementSpeed GetSpeed(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType.ToPlayerSpeed(_entity);
		}

		public static void SetSpeed(this AssassinCharacter _entity, EMovementSpeed _speed)
		{
			_entity.AssassinCharacterStateMachines.PlayerProfile.ChosenSpeed = _speed;
		}

		public static bool GetIsImmortal(this AssassinCharacter _entity)
		{
			IStateMachine<ICharacterVitalityCarrierEntity> characterVitality = _entity.AssassinCharacterStateMachines.CharacterVitality;
			return characterVitality.CurrentStateType == EStateType.Character_Immortal;
		}

		public static bool GetIsExposed(this AssassinCharacter _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			return playerCharacter != null && playerCharacter.GetIsExposed();
		}

		public static bool GetIsAnonymous(this AssassinCharacter _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			return playerCharacter == null || playerCharacter.GetIsAnonymous();
		}

		public static bool GetIsStalking(this AssassinCharacter _entity)
		{
			bool result = false;
			if (_entity.AssassinCharacterStateMachines.PlayerStalkingZone.CurrentStateType == EStateType.PlayerStalkingZone_Stalking)
			{
				result = true;
			}
			return result;
		}

		public static bool GetIsInRooftopGarden(this AssassinCharacter _entity)
		{
			bool result = false;
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject);
				result = payload.EntityId.Type == EntityTypes.EType.RooftopGarden;
			}
			return result;
		}

		public static bool GetIsInHaystack(this AssassinCharacter _entity)
		{
			bool result = false;
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject);
				result = payload.EntityId.Type == EntityTypes.EType.Haystack;
			}
			return result;
		}

		public static bool GetIsOnBench(this AssassinCharacter _entity)
		{
			bool result = false;
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject);
				result = payload.EntityId.Type == EntityTypes.EType.Bench;
			}
			return result;
		}

		public static Assets.Scripts.Mission.Logic.Entities.Patrol GetHidingPatrol(this AssassinCharacter _entity)
		{
			if (_entity.GetIsHidingInPatrol())
			{
				PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
				return payload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			}
			return null;
		}

		public static bool GetIsHidingInPatrol(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInPatrol;
		}

		public static bool GetHidingInPatrolStartTime(this AssassinCharacter _entity, out float _startTime)
		{
			_startTime = 0f;
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInPatrol)
			{
				PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
				_startTime = payload.m_startTime;
				return true;
			}
			return false;
		}

		public static float GetHidingInPatrolEndTime(this AssassinCharacter _entity)
		{
			PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
			return payload.m_endTime;
		}

		public static bool GetIsHidingInCourtesans(this AssassinCharacter _entity)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol hidingPatrol = _entity.GetHidingPatrol();
			return hidingPatrol != null && hidingPatrol.PatrolComponent.MissionData.Data.Type == EPatrolType.CourtesanHidingSpot;
		}

		public static bool GetIsHiding(this AssassinCharacter _entity)
		{
			return _entity.PlayerStats.Get_bool(StatTypes.EStatType.Player_IsHiding);
		}

		public static bool GetIsHidingInObject(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject;
		}

		public static bool GetIsRoaming(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.CombatCharacterMain_Roaming;
		}

		public static bool GetIsInDisguise(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerDisguise.CurrentStateType == EStateType.PlayerDisguise_Disguised;
		}

		public static bool GetIsBeingChased(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_BeingChased;
		}

		public static bool GetIsHidingInObjectExitAndMoveTo(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_HidingInObjectExitAndMoveTo;
		}

		public static bool GetIsInParcour(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.CombatCharacterMain_Parcour;
		}

		public static bool GetIsInParcourOrScheduled(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.IsCurrentOrScheduled(EStateType.CombatCharacterMain_Parcour);
		}

		public static bool GetIsInObjectInteraction(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_ObjectInteraction;
		}

		public static bool GetIsUnseen(this AssassinCharacter _entity)
		{
			return _entity.EntityStats.Get_int(StatTypes.EStatType.Player_AwareEnemies) == 0;
		}

		public static bool GetIsApproachingTarget(this AssassinCharacter _entity)
		{
			return _entity.AssassinCharacterStateMachines.AssassinApproachingTarget.CurrentStateType == EStateType.AssassinApproachingTarget_Approaching;
		}

		public static bool GetIsExposedAndSeen(this AssassinCharacter _entity)
		{
			return _entity.GetIsExposed() && !_entity.GetIsUnseen();
		}

		public static bool GetIsApproaching(this AssassinCharacter _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			if (playerCharacter != null && playerCharacter.GetIsApproachingPosition())
			{
				return true;
			}
			return _entity.GetIsApproachingTarget();
		}

		public static Vector3 GetApproachPosition(this AssassinCharacter _entity)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			if (playerCharacter != null && playerCharacter.GetIsApproachingPosition())
			{
				return playerCharacter.PlayerEventHandler.TargetPosition;
			}
			if (_entity.GetIsApproachingTarget())
			{
				return _entity.AssassinCharacterStateMachines.AssassinApproachingTarget.TargetId.GetEntity<IPositionEntity>().Position;
			}
			return Vector3.zero;
		}

		public static bool GetIsNavigating(this AssassinCharacter _entity, bool _recompute = false)
		{
			PlayerCharacter playerCharacter = _entity as PlayerCharacter;
			if (playerCharacter != null)
			{
				if (playerCharacter.GetIsApproachingPosition(_recompute))
				{
					return true;
				}
				if ((AcierInputHandler.Instance.HasTouchInput(0) && !AcierInputHandler.Instance.HasTouchInput(1, false)) || AcierInputHandler.Instance.HasGamepadMoveInput)
				{
					return true;
				}
			}
			return _entity.GetIsApproachingTarget();
		}

		public static EntityId GetCurrentInteractionObject(this AssassinCharacter _entity)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_ObjectInteraction)
			{
				return _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction).EntityId;
			}
			return EntityId.InvalidId;
		}

		public static EntityId GetCurrentInteractionNpc(this AssassinCharacter _entity)
		{
			if (_entity.AssassinCharacterStateMachines.PlayerMain.CurrentStateType == EStateType.PlayerMain_NpcInteraction)
			{
				return _entity.AssassinCharacterStateMachines.PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_NpcInteraction).EntityId;
			}
			return EntityId.InvalidId;
		}

		public static bool GetIsRunning(this AssassinCharacter _entity)
		{
			NavMeshAgent navMeshAgent = _entity.UnityComponentCache.NavMeshAgent;
			return _entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType >= EStateType.PlayerProfile_High && navMeshAgent.velocity.sqrMagnitude > 0f;
		}

		public static bool GetIsPerformingAssassination(this AssassinCharacter _entity)
		{
			return _entity.GetIsPerformingLeapAssasination() || _entity.GetIsPerformingStealthAssasination() || _entity.GetIsPerformingAirAssasination();
		}

		public static bool GetIsPerformingStrikingAssassination(this AssassinCharacter _entity)
		{
			return _entity.GetIsPerformingLeapAssasination() || _entity.GetIsPerformingAirAssasination();
		}

		public static bool GetIsPerformingLeapAssasination(this AssassinCharacter _entity)
		{
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			return playerMain.CurrentStateType == EStateType.PlayerMain_AssassinationLeap || playerMain.ScheduledStateType == EStateType.PlayerMain_AssassinationLeap;
		}

		public static bool GetIsPerformingStealthAssasination(this AssassinCharacter _entity)
		{
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			return playerMain.CurrentStateType == EStateType.PlayerMain_AssassinationStealth || playerMain.ScheduledStateType == EStateType.PlayerMain_AssassinationStealth;
		}

		public static bool GetIsPerformingAirAssasination(this AssassinCharacter _entity)
		{
			PlayerMain playerMain = _entity.AssassinCharacterStateMachines.PlayerMain;
			return playerMain.CurrentStateType == EStateType.PlayerMain_AssassinationAir || playerMain.ScheduledStateType == EStateType.PlayerMain_AssassinationAir;
		}

		public static void ConfigureAnimationController(this IPlayerProfileCarrierEntity _entity, EStateType _profile)
		{
			AnimationController animationController = _entity.UnityComponentCache.AnimationController;
			EMovementSpeed speed = (animationController.ReferenceMovementSpeed = _profile.ToPlayerSpeed(_entity as IPlayerStatsCarrier));
			animationController.Agent.speed = animationController.CalculateSpeed(speed);
			AnimatorExtensions.SetCurrentIdleType(_type: animationController.ReferenceIdleType = _profile.ToIdleType(), _animator: animationController.Animator);
		}

		public static EnemyCharacter ComputeEnemyInformation(this AssassinCharacter _entity, out float _distance, out int _awareCount, out int _chasingCount)
		{
			Profiling.Start("ComputeEnemyInformation()", float.MaxValue);
			_distance = 25f;
			_awareCount = 0;
			_chasingCount = 0;
			Vector3 position = LogicMain.PlayerEntity.Position;
			if (NavigationHelper.CascadingSamplePosition(position, out m_dummyHit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				position = m_dummyHit.position;
			}
			m_closeEnemies.Clear();
			EnemyCharacter result = null;
			int enemyCharacterCount = LogicMain.EntityManager.EnemyCharacterCount;
			for (int i = 0; i < enemyCharacterCount; i++)
			{
				EnemyCharacter enemyCharacter = LogicMain.EntityManager.GetEnemyCharacter(i);
				if (enemyCharacter.GetIsDead() || !enemyCharacter.IsValid)
				{
					continue;
				}
				float magnitude = (enemyCharacter.Position - position).magnitude;
				if (!(magnitude > 25f))
				{
					float num = enemyCharacter.EntityStats.Get_float(StatTypes.EStatType.Character_Detection_Distance);
					if (magnitude < _distance && num < _distance && enemyCharacter.GetIsThreateningOrNormal())
					{
						m_closeEnemies.Add(new EnemyEntry(magnitude, enemyCharacter));
					}
					EStateType currentStateType = enemyCharacter.EnemyCharacterStateMachines.CharacterAwareness.CurrentStateType;
					if (currentStateType == EStateType.CharacterAwareness_Low || currentStateType == EStateType.CharacterAwareness_High || (currentStateType == EStateType.CharacterAwareness_Alarmed && enemyCharacter.EnemyNpcStats.Get_int(StatTypes.EStatType.Character_Detection_Range) <= 2))
					{
						_awareCount++;
					}
					if (enemyCharacter.GetIsChasing())
					{
						_chasingCount++;
					}
				}
			}
			m_closeEnemies.Sort((EnemyEntry a, EnemyEntry b) => a.MinDistance.CompareTo(b.MinDistance));
			foreach (EnemyEntry closeEnemy in m_closeEnemies)
			{
				EnemyCharacter enemycharacter = closeEnemy.Enemycharacter;
				float minDistance = closeEnemy.MinDistance;
				if (minDistance > _distance)
				{
					break;
				}
				Profiling.Start("CalculatePath()", float.MaxValue);
				int areaMask = ((!enemycharacter.IsAttached) ? enemycharacter.NavAgentSurrogate.WalkableMask : enemycharacter.UnityComponentCache.NavMeshAgent.areaMask);
				if (NavMesh.CalculatePath(enemycharacter.Position, position, areaMask, m_dummyPath) && m_dummyPath.status == NavMeshPathStatus.PathComplete)
				{
					float num2 = m_dummyPath.CalculateLength();
					if (num2 < _distance)
					{
						_distance = num2;
						result = enemycharacter;
					}
				}
				Profiling.Stop();
			}
			Profiling.Stop();
			return result;
		}

		private static void ScheduleHidingInPatrol(EntityId _patrolId, AssassinCharacter _assassinCharacter, PlayerMain _playerMain)
		{
			PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = _playerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
			payload.PrevPatrolId = payload.PatrolId;
			payload.PatrolId = _patrolId;
			payload.ShouldExit = payload.WillHidingLimitFailOnIncrease || _assassinCharacter.GetIsExposedAndSeen();
			_playerMain.ScheduleStateChange(EStateType.PlayerMain_HidingInPatrol);
		}

		private static bool DetermineHideoutExitTarget(Vector3 _clickTarget, NavMeshAgent _agent, HideoutObject _hideout, out Vector3 _exitTarget, out InteractionLocators.Locator _locator)
		{
			bool result = false;
			_exitTarget = Vector3.zero;
			_locator = null;
			Vector3 position = _agent.GetComponent<Transform>().position;
			Vector3 vector = _clickTarget - position;
			vector.Normalize();
			vector *= 3f;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(position + vector, out hit, 5f, _agent.areaMask) && (_hideout is Haystack || _hideout is RooftopGarden))
			{
				InteractionLocators.Locator locator = null;
				float num = float.MaxValue;
				for (int i = 0; i < _hideout.InteractionLocators.Locators.Count; i++)
				{
					InteractionLocators.Locator locator2 = _hideout.InteractionLocators.Locators[i];
					if (locator2.Enabled)
					{
						float num2 = Vector3.Distance(locator2.Position.transform.position, hit.position);
						if (num2 < num)
						{
							locator = locator2;
							num = num2;
						}
					}
				}
				_exitTarget = ((locator == null) ? Vector3.zero : locator.Position.transform.position);
				_locator = locator;
				result = locator != null;
			}
			return result;
		}

		private static bool AreAwareCharacterClose(HideoutObject _hideout)
		{
			IEnumerable<NpcCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<NpcCharacter>(_hideout.Position, 5f, FilterAwareCharcter);
			return enumerable.GetEnumerator().MoveNext();
		}

		private static bool FilterAwareCharcter(NpcCharacter _npc)
		{
			return _npc.StateMachines is ICharacterAwarenessCarrier && !_npc.GetIsDead() && !_npc.GetIsUnconscious();
		}
	}
}
