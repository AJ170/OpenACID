using System;
using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class AssassinInteraction : Component
	{
		private readonly ISelectionProvider m_SelectionProvider;

		private readonly TargetApproacher s_TargetApproacher = new TargetApproacher();

		public override int Priority
		{
			get
			{
				return 21;
			}
		}

		public bool CanStopApproaching
		{
			get
			{
				bool result = true;
				if (AssassinApproachingTarget.TargetId != EntityId.InvalidId && AssassinApproachingTarget.Reason == EInteractionReason.Combat)
				{
					Character entity = AssassinApproachingTarget.TargetId.GetEntity<Character>();
					float magnitude = (entity.Position - AssassinCharacter.Position).magnitude;
					result = magnitude > 5f;
				}
				return result;
			}
		}

		private AssassinCharacter AssassinCharacter
		{
			get
			{
				return (AssassinCharacter)Entity;
			}
		}

		private PlayerMain PlayerMain
		{
			get
			{
				return AssassinCharacter.AssassinCharacterStateMachines.PlayerMain;
			}
		}

		private AssassinApproachingTarget AssassinApproachingTarget
		{
			get
			{
				return AssassinCharacter.AssassinCharacterStateMachines.AssassinApproachingTarget;
			}
		}

		private AnimationTaskManager AnimationTaskManager
		{
			get
			{
				return AssassinCharacter.UnityComponentCache.AnimationController.TaskManager;
			}
		}

		public AssassinInteraction(IEntity _entity, ISelectionProvider _selectionProvider)
			: base(_entity)
		{
			m_SelectionProvider = _selectionProvider;
		}

		public bool CanReachEntity(EntityId _entityId)
		{
			s_TargetApproacher.SetData(AssassinCharacter.Id, _entityId, EMovementSpeed.Run, 0f);
			return s_TargetApproacher.IsTargetReachable;
		}

		public void Reevaluate()
		{
			EInteractionReason reason = AssassinApproachingTarget.Reason;
			Assets.Scripts.Mission.Definitions.SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetBuffedApproachSkillDefinition(reason, AssassinCharacter);
			if (buffedApproachSkillDefinition != null && (buffedApproachSkillDefinition.ActivationType & ESkillActivationType.Slots) == 0)
			{
				AssassinApproachingTarget.FreezeStartApproachingStatus = true;
				TapEntity(AssassinApproachingTarget.TargetId);
				AssassinApproachingTarget.FreezeStartApproachingStatus = false;
			}
		}

		public bool DoubleTapEntity(EntityId _targetId)
		{
			Character character = _targetId.TryGetEntity<Character>();
			if (character != null && !character.GetIsDead() && CanApproachNpc(_targetId, EInteractionReason.Combat) && m_SelectionProvider.Selection != null && m_SelectionProvider.Selection.Id == _targetId)
			{
				return ApproachNpc(_targetId, EInteractionReason.Combat);
			}
			return false;
		}

		public bool SelectEntity(EntityId _targetId, bool _foreceReplace = false, float _minDistanceToOtherSelection = 0f, bool _onlyIfNewIsCloser = true)
		{
			EnemyCharacter enemyCharacter = _targetId.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter != null)
			{
				if (m_SelectionProvider.Selection != null && !_foreceReplace)
				{
					float magnitude = (m_SelectionProvider.Selection.Position - LogicMain.PlayerEntity.Position).magnitude;
					float magnitude2 = (enemyCharacter.Position - LogicMain.PlayerEntity.Position).magnitude;
					float magnitude3 = (m_SelectionProvider.Selection.Position - enemyCharacter.Position).magnitude;
					if (_onlyIfNewIsCloser && magnitude < magnitude2)
					{
						return false;
					}
					if (magnitude3 < _minDistanceToOtherSelection)
					{
						return false;
					}
				}
				EInteractionReason reason = AssassinApproachingTarget.EvaluateInteractionReason(_targetId);
				if (CanApproachNpc(_targetId, reason))
				{
					m_SelectionProvider.Selection = enemyCharacter;
					if (m_SelectionProvider.Selection == enemyCharacter)
					{
						Update(enemyCharacter.Id);
					}
					return true;
				}
			}
			return false;
		}

		public bool TapEntity(EntityId _targetId)
		{
			if (AssassinCharacter.GetIsInHaystack())
			{
				return StartStealthKill(UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.OnKillFromHideout, _targetId);
			}
			if (AssassinCharacter.GetIsInRooftopGarden())
			{
				return StartStealthKill(UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.OnKillFromHideout, _targetId);
			}
			if (AssassinCharacter.GetIsOnBench())
			{
				return StartStealthKill(UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.OnKillFromHideout, _targetId);
			}
			Character character = _targetId.TryGetEntity<Character>();
			CrowdCharacter crowdCharacter = character as CrowdCharacter;
			if (crowdCharacter != null)
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = crowdCharacter.Patrol;
				if (patrol != null)
				{
					return TapEntity(patrol.Id);
				}
			}
			else if (character != null)
			{
				EInteractionReason eInteractionReason = AssassinApproachingTarget.EvaluateInteractionReason(_targetId);
				switch (eInteractionReason)
				{
				case EInteractionReason.Invalid:
					return false;
				case EInteractionReason.TriggerSelectedSkill:
					if (AssassinCharacter.TriggerSelectedSkill(new EntityIdSkillPayload(_targetId)))
					{
						return true;
					}
					break;
				}
				if (CanApproachNpc(_targetId, eInteractionReason))
				{
					EnemyCharacter enemyCharacter = character as EnemyCharacter;
					if (enemyCharacter != null)
					{
						if (m_SelectionProvider.Selection != null && m_SelectionProvider.Selection.Id == _targetId)
						{
							if (eInteractionReason == EInteractionReason.LetCourtesansSeduce)
							{
								m_SelectionProvider.Selection = null;
								EnterLetCourtesansSeduce(enemyCharacter);
								return true;
							}
							return ApproachNpc(_targetId, eInteractionReason);
						}
						m_SelectionProvider.Selection = character;
						if (m_SelectionProvider.Selection != character)
						{
							return true;
						}
						Update(character.Id);
						return true;
					}
					if (character is InteractionCharacter)
					{
						m_SelectionProvider.Selection = null;
						return ApproachNpc(_targetId, eInteractionReason);
					}
				}
				return false;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol2 = _targetId.TryGetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			if (patrol2 != null)
			{
				EInteractionReason reason = AssassinApproachingTarget.EvaluateInteractionReason(patrol2.Id);
				if (CanApproachPatrol(patrol2.Id, reason))
				{
					return ApproachPatrol(patrol2, reason);
				}
			}
			InteractableObject interactableObject = _targetId.TryGetEntity<InteractableObject>();
			if (interactableObject != null)
			{
				EInteractionReason reason2 = AssassinApproachingTarget.EvaluateInteractionReason(_targetId);
				if (CanApproachInteractableObject(_targetId, reason2))
				{
					return ApproachInteractableObject(interactableObject, reason2);
				}
			}
			return false;
		}

		public bool StopApproaching(bool _stopMoving = true)
		{
			bool result = false;
			if (AssassinApproachingTarget.TargetId != EntityId.InvalidId && CanStopApproaching)
			{
				AssassinApproachingTarget.Stop(_stopMoving);
				result = true;
			}
			return result;
		}

		public bool CanApproachInteractableObject(EntityId _objectId, EInteractionReason _reason)
		{
			return _reason != EInteractionReason.Invalid && CanInteractWithInteractableObject(_objectId) && (CanReachEntity(_objectId) || GetClosestReachableLocator(_objectId) != null);
		}

		public bool CanApproachNpc(EntityId _npcId, EInteractionReason _reason)
		{
			return _reason != EInteractionReason.Invalid && CanInteractWithNpc(_npcId) && CanReachEntity(_npcId) && (_reason != EInteractionReason.LetCourtesansSeduce || _npcId.GetEntity<EnemyCharacter>().CanBeSeduced());
		}

		public bool CanApproachPatrol(EntityId _patrolId, EInteractionReason _reason)
		{
			return _reason != EInteractionReason.Invalid && CanInteractWithPatrol(_patrolId) && _patrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>().PatrolComponent.Formation.EntityAtHidingSpot == EntityId.InvalidId && CanReachEntity(_patrolId);
		}

		public bool CanInteractWithInteractableObject(EntityId _targetId)
		{
			InteractableObject interactableObject = _targetId.TryGetEntity<InteractableObject>();
			if (((interactableObject.InteractableObjectDefinition.Definition.Category != 0) ? AssassinCanInteractPeaceful(interactableObject) : AssassinCanInteractDefault(interactableObject)) && interactableObject.CanInteract())
			{
				return true;
			}
			return false;
		}

		public bool CanInteractWithNpc(EntityId _npcId)
		{
			Character character = _npcId.TryGetEntity<Character>();
			InteractionCharacter interactionCharacter = character as InteractionCharacter;
			if (interactionCharacter != null)
			{
				return AssassinCanInteractPeaceful(interactionCharacter) && interactionCharacter.CanInteract();
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = _npcId.TryGetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			if (patrol != null)
			{
				return CanInteractWithPatrol(patrol.Id);
			}
			if (character != null)
			{
				return AssassinCanInteractDefault(character);
			}
			return false;
		}

		public bool CanInteractWithPatrol(EntityId _patrolId)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _patrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			return ((entity.PatrolDefinition.Definition.Type != EPatrolType.CapturedNpcs) ? AssassinCanInteractDefault(entity) : AssassinCanInteractPeaceful(entity)) && entity.CanInteract();
		}

		public bool EnterNpcInteraction(EntityId _npcId)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(CanInteractWithNpc(_npcId)))
			{
				return false;
			}
			InteractionCharacter entity = _npcId.GetEntity<InteractionCharacter>();
			NpcData.NpcDefinition.ENpcInteractionType interactionType = entity.NpcDefinition.Definition.InteractionType;
			bool result = false;
			switch (interactionType)
			{
			case NpcData.NpcDefinition.ENpcInteractionType.Merchant:
			{
				AssassinCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle();
				CharacterInteraction characterInteraction = entity.InteractionCharacterStateMachines.CharacterInteraction;
				if (characterInteraction.CurrentStateType != EStateType.CharacterInteraction_OfferOpen)
				{
					result = characterInteraction.ScheduleStateChange(EStateType.CharacterInteraction_OfferOpen);
				}
				break;
			}
			case NpcData.NpcDefinition.ENpcInteractionType.Mission:
				result = EnterMissionDeliver(_npcId);
				break;
			}
			RegisterConspiciousAction(EInteractionReason.InteractWithNpc);
			return result;
		}

		public bool AcceptOffer(EntityId _npcId)
		{
			InteractionCharacter entity = _npcId.GetEntity<InteractionCharacter>();
			if (entity.InteractionCharacterStateMachines.CharacterInteraction.CurrentStateType == EStateType.CharacterInteraction_OfferOpen)
			{
				return entity.InteractionCharacterStateMachines.CharacterInteraction.ScheduleStateChange(EStateType.CharacterInteraction_OfferAccepted);
			}
			return false;
		}

		public bool RejectOffer(EntityId _npcId)
		{
			InteractionCharacter entity = _npcId.GetEntity<InteractionCharacter>();
			if (entity.InteractionCharacterStateMachines.CharacterInteraction.CurrentStateType == EStateType.CharacterInteraction_OfferOpen)
			{
				return entity.InteractionCharacterStateMachines.CharacterInteraction.ScheduleStateChange(EStateType.CharacterInteraction_OfferRejected);
			}
			return false;
		}

		public bool EnterPatrol(EntityId _patrolId, EInteractionReason _reason)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(CanInteractWithPatrol(_patrolId)))
			{
				return false;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol entity = _patrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			switch (_reason)
			{
			case EInteractionReason.FreeCapturedNpcs:
				return AssassinCharacter.FreeCaptives(entity.Id);
			case EInteractionReason.HidingInPatrol:
				return AssassinCharacter.EnterHidingInPatrol(entity.Id);
			default:
				return false;
			}
		}

		public bool ExitCodex()
		{
			if (Assets.Scripts.Engine.Helpers.Verify(PlayerMain.CurrentStateType == EStateType.PlayerMain_ObjectInteraction))
			{
				Codex entity = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction).EntityId.GetEntity<Codex>();
				UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnlockEntry(entity.CodexEntry.Guid);
				return PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
			}
			return false;
		}

		public bool EnterObjectInteraction(EntityId _id, InteractionLocators.Locator _locator)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(CanInteractWithInteractableObject(_id)))
			{
				return false;
			}
			bool flag = false;
			switch (_id.Type)
			{
			case EntityTypes.EType.Haystack:
			case EntityTypes.EType.Bench:
			case EntityTypes.EType.RooftopGarden:
				flag = AssassinCharacter.EnterHideout(_id.GetEntity<HideoutObject>(), _locator);
				break;
			case EntityTypes.EType.Codex:
				flag = EnterCodex(_id.GetEntity<Codex>(), _locator);
				break;
			}
			if (!flag)
			{
				flag = EnterMissionObjectInteraction(_id, AnimationTaskType.InteractableObject);
			}
			if (flag)
			{
				RegisterConspiciousAction(EInteractionReason.InteractWithObject);
			}
			return flag;
		}

		public bool CanAbortObjectInteraction()
		{
			if (Assets.Scripts.Engine.Helpers.Verify(PlayerMain.CurrentStateType == EStateType.PlayerMain_ObjectInteraction))
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction);
				return payload.EntityId.GetEntity<InteractableObject>().InteractableObjectDefinition.Definition.InteractionCanBeAborted;
			}
			return false;
		}

		public bool AbortObjectInteraction(bool _simulate = false)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(CanAbortObjectInteraction()))
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction);
				if (payload.EntityId.Entity.Type == EntityTypes.EType.Pigeonry)
				{
					PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
					return true;
				}
				if (AssassinCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.InteractableObject)
				{
					if (!_simulate)
					{
						AnimationTaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base);
					}
					return true;
				}
			}
			return false;
		}

		public bool AbortInteractions(bool _simulate = false)
		{
			if (PlayerMain.CurrentStateType == EStateType.PlayerMain_ObjectInteraction)
			{
				if (CanAbortObjectInteraction())
				{
					return AbortObjectInteraction(_simulate);
				}
			}
			else if (PlayerMain.CurrentStateType == EStateType.PlayerMain_NpcInteraction)
			{
				return false;
			}
			return false;
		}

		public void Update(EntityId _targetId)
		{
			ISelectable selectable = m_SelectionProvider.Selection;
			if (selectable != null && !selectable.IsValid)
			{
				selectable = null;
			}
			Character character = selectable as Character;
			if (character != null && character.GetIsDead())
			{
				selectable = null;
			}
			InteractableObject interactableObject = selectable as InteractableObject;
			ISelectionIconCarrier selectionIconCarrier = selectable as ISelectionIconCarrier;
			Assets.Scripts.Mission.Definitions.SkillData.IApproachSkillDefinition activeApproachSkill = AssassinCharacter.Skills.ActiveApproachSkill;
			if (activeApproachSkill != null)
			{
				if (!activeApproachSkill.KeepSelectionAfterApproaching)
				{
					selectable = null;
				}
			}
			else if (AssassinCharacter.GetIsInHaystack())
			{
				selectable = (selectionIconCarrier = AssassinCharacter.GetActiveKillFromHideout().SelectedEnemy);
				if (selectionIconCarrier != null)
				{
					selectionIconCarrier.SelectionIcon.SelectionIconMaterial = (AssassinCharacter.KillFromHaystack.Executable ? IconMaterial.InteractAssassinate : IconMaterial.Invalid);
				}
			}
			else if (AssassinCharacter.GetIsInRooftopGarden())
			{
				selectable = (selectionIconCarrier = AssassinCharacter.GetActiveKillFromHideout().SelectedEnemy);
				if (selectionIconCarrier != null)
				{
					selectionIconCarrier.SelectionIcon.SelectionIconMaterial = (AssassinCharacter.KillFromRooftopGarden.Executable ? IconMaterial.InteractAssassinate : IconMaterial.Invalid);
				}
			}
			else if (AssassinCharacter.GetIsOnBench())
			{
				selectable = (selectionIconCarrier = AssassinCharacter.GetActiveKillFromHideout().SelectedEnemy);
				if (selectionIconCarrier != null)
				{
					selectionIconCarrier.SelectionIcon.SelectionIconMaterial = (AssassinCharacter.KillFromBench.Executable ? IconMaterial.InteractAssassinate : IconMaterial.Invalid);
				}
			}
			else if (AssassinCharacter.GetIsRoaming() || AssassinCharacter.GetIsHidingInPatrol() || AssassinCharacter.GetIsBeingChased())
			{
				if (character != null)
				{
					ESkillAvailabilityFlags _availabilityFlags;
					EInteractionReason eInteractionReason = AssassinApproachingTarget.EvaluateInteractionReason(_targetId, AssassinApproachingTarget.Reason, out _availabilityFlags);
					if (CanApproachNpc(_targetId, eInteractionReason))
					{
						if (selectionIconCarrier != null)
						{
							IconMaterial iconMaterial = IconMaterial.Invalid;
							EInteractionReason eInteractionReason2 = eInteractionReason;
							if (eInteractionReason2 == EInteractionReason.LetCourtesansSeduce)
							{
								iconMaterial = IconMaterial.InteractSeduce;
							}
							if (iconMaterial == IconMaterial.Invalid && AssassinCharacter.GetIsMovementAllowed())
							{
								Assets.Scripts.Mission.Definitions.SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance.GetBuffedApproachSkillDefinition(eInteractionReason, AssassinCharacter);
								if (buffedApproachSkillDefinition != null)
								{
									iconMaterial = buffedApproachSkillDefinition.IconMaterial;
								}
								else
								{
									eInteractionReason2 = eInteractionReason;
									if (eInteractionReason2 == EInteractionReason.Combat)
									{
										iconMaterial = IconMaterial.InteractAttack;
									}
								}
							}
							selectionIconCarrier.SelectionIcon.SelectionIconMaterial = iconMaterial;
						}
					}
					else
					{
						selectable = null;
					}
				}
				else if (interactableObject != null)
				{
					ESkillAvailabilityFlags _availabilityFlags2;
					EInteractionReason reason = AssassinApproachingTarget.EvaluateInteractionReason(_targetId, AssassinApproachingTarget.Reason, out _availabilityFlags2);
					if ((!(_targetId.Entity is HideoutObject) || !CanInteractWithInteractableObject(_targetId)) && !CanApproachInteractableObject(_targetId, reason))
					{
						selectable = null;
					}
				}
			}
			else if (!AssassinCharacter.GetIsInCombat() && !AssassinCharacter.GetIsInParcour())
			{
			}
			m_SelectionProvider.Selection = selectable;
		}

		public bool EnterMissionDeliver(EntityId _id)
		{
			return EnterMissionDeliver(_id, AnimationTaskType.MissionDeliverInstigator);
		}

		public bool EnterCodex(Codex _codex, InteractionLocators.Locator _locator)
		{
			AnimationTaskManager.CreateAnimationTaskCodex(_codex, _locator);
			PlayerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnim, EStateType.PlayerMain_ObjectInteraction, EStateType.PlayerMain_ObjectInteraction, AnimationTaskType.Codex);
			PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction);
			payload.EntityId = _codex.Id;
			return true;
		}

		public bool EnterParcour(OffMeshLinkEntity _offMeshLink, InteractionLocators.Locator _locator)
		{
			bool flag = AssassinCharacter.EnterParcour(new UseLinkEvent(_offMeshLink));
			if (flag)
			{
				RegisterConspiciousAction(EInteractionReason.OffMeshLink);
			}
			return flag;
		}

		public bool ApproachPatrol(Assets.Scripts.Mission.Logic.Entities.Patrol _patrol, EInteractionReason _reason)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(CanApproachPatrol(_patrol.Id, _reason)))
			{
				AssassinApproachingTarget.SetTarget(_patrol.Id, _reason);
				return true;
			}
			return false;
		}

		public bool TryApproachPatrol(Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			EInteractionReason reason = AssassinApproachingTarget.EvaluateInteractionReason(_patrol.Id);
			return CanApproachPatrol(_patrol.Id, reason) && ApproachPatrol(_patrol, reason);
		}

		public bool ApproachNpc(EntityId _npcId, EInteractionReason _reason)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(CanApproachNpc(_npcId, _reason)))
			{
				return false;
			}
			return AssassinApproachingTarget.SetTarget(_npcId, _reason);
		}

		public bool ApproachInteractableObject(InteractableObject _object, EInteractionReason _reason)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(CanApproachInteractableObject(_object.Id, _reason)))
			{
				InteractionLocators.Locator closestReachableLocator = GetClosestReachableLocator(_object);
				if (AssassinApproachingTarget.SetTarget(_object.Id, _reason, closestReachableLocator))
				{
					return true;
				}
			}
			return false;
		}

		public bool ApplyMerchantEffect(EntityId _npcId)
		{
			if (AssassinCharacter.RefillHealth() > 0f)
			{
				InteractionCharacter entity = _npcId.GetEntity<InteractionCharacter>();
				_npcId.GetEntity<InteractionCharacter>().EntityStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
				entity.Patrol.EntityStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
				LogicMain.MissionResult.IncreaseDoctorHealings();
				return true;
			}
			return false;
		}

		public bool CanApplyMerchantEffect(EntityId _npcId)
		{
			return CombatHelper.ComputeHealthToHeal(AssassinCharacter) > 0f;
		}

		private bool StartStealthKill(Func<EntityId, bool> _onKill, EntityId _targetId)
		{
			if (_onKill(_targetId))
			{
				RegisterConspiciousAction(EInteractionReason.AssassinationStealth);
				return true;
			}
			return false;
		}

		private void RegisterConspiciousAction(EInteractionReason _reason)
		{
			AssassinCharacter.RegisterInteraction(_reason);
		}

		private InteractionLocators.Locator GetClosestReachableLocator(EntityId _id)
		{
			InteractionLocators.Locator result = null;
			if (_id.Entity is InteractableObject)
			{
				result = GetClosestReachableLocator(_id.Entity.Cast<InteractableObject>());
			}
			return result;
		}

		private InteractionLocators.Locator GetClosestReachableLocator(InteractableObject _object)
		{
			InteractionLocators.Locator _locator = null;
			NavMeshAgent navMeshAgent = AssassinCharacter.UnityComponentCache.NavMeshAgent;
			NavMeshPath _path;
			_object.InteractionLocators.ChooseClosestLocator(navMeshAgent.transform.position, AssassinCharacter.GetAutoPassableNavMeshLayerMask(), out _locator, out _path);
			return _locator;
		}

		private bool EnterMissionDeliver(EntityId _id, AnimationTaskType _animTaskType)
		{
			if (PlayerMain.CurrentStateType != EStateType.CombatCharacterMain_Roaming)
			{
				return false;
			}
			if (PlayerMain.ScheduledStateType != 0)
			{
				return false;
			}
			InteractionCharacter entity = _id.GetEntity<InteractionCharacter>();
			CharacterInteraction characterInteraction = entity.InteractionCharacterStateMachines.CharacterInteraction;
			if (characterInteraction.CurrentStateType == EStateType.CharacterInteractionMain_Busy)
			{
				return false;
			}
			if (_animTaskType == AnimationTaskType.MissionDeliverInstigator)
			{
				InteractionCharacter entity2 = _id.GetEntity<InteractionCharacter>();
				entity2.CharacterAi.IsActive = false;
				entity2.CancelMovement(false);
				entity2.InteractionCharacterStateMachines.InteractionCharacterMain.ScheduleStateChange(EStateType.CharacterInteractionMain_Busy);
				AnimationPlotSequence animationPlotSequence = CreateMissionDeliveryAnimationPlotSequence(entity2);
				PlayerMain.EnterCallbackDelayState(EStateType.PlayerMain_NpcInteraction, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, animationPlotSequence);
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_NpcInteraction);
				payload.EntityId = _id;
				payload.Plot = animationPlotSequence;
				characterInteraction.Entity.InteractionCharacterStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
				characterInteraction.Entity.Patrol.EntityStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
				return true;
			}
			return false;
		}

		private AnimationPlotSequence CreateMissionDeliveryAnimationPlotSequence(NpcCharacter _character)
		{
			AnimationPlotSequence animationPlotSequence = new AnimationPlotSequence();
			_character.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle();
			animationPlotSequence.AddState(new AnimationPlotStateWaitForMovementStopped(new BBList<AnimationController>
			{
				PlayerMain.Entity.UnityComponentCache.AnimationController,
				_character.UnityComponentCache.AnimationController
			}));
			AnimationTaskPlayerTurning.Parameters parameters = new AnimationTaskPlayerTurning.Parameters();
			parameters.TargetProvider = _character;
			parameters.AnimationController = PlayerMain.Entity.UnityComponentCache.AnimationController;
			AnimationTaskNpcTurning.Parameters parameters2 = new AnimationTaskNpcTurning.Parameters();
			parameters2.TargetProvider = PlayerMain.Entity;
			parameters2.AnimationController = _character.UnityComponentCache.AnimationController;
			parameters2.TurnAccuracy = AnimationTaskNpcTurning.TurnAccuracy.High;
			animationPlotSequence.AddState(new AnimationPlotStateWaitForAllEntries(new BBList<AnimationPlotEntryBase>
			{
				new AnimationPlotEntry<AnimationTaskPlayerTurning>(parameters),
				new AnimationPlotEntry<AnimationTaskNpcTurning>(parameters2)
			}));
			AnimationTaskMissionDeliverInstigator.Parameters parameters3 = new AnimationTaskMissionDeliverInstigator.Parameters();
			parameters3.AnimationController = PlayerMain.Entity.UnityComponentCache.AnimationController;
			parameters3.DeliverTargetPosition = _character.Position;
			animationPlotSequence.AddState(new AnimationPlotStateWaitForAnimationEvent(new AnimationPlotEntry<AnimationTaskMissionDeliverInstigator>(parameters3), AnimatorEventType.ReactionStart));
			AnimationTaskMissionDeliverTarget.Parameters parameters4 = new AnimationTaskMissionDeliverTarget.Parameters();
			parameters4.AnimationController = _character.UnityComponentCache.AnimationController;
			animationPlotSequence.AddState(new AnimationPlotStateWaitForAllEntries(new BBList<AnimationPlotEntryBase>
			{
				new AnimationPlotEntry<AnimationTaskMissionDeliverTarget>(parameters4)
			}));
			return animationPlotSequence;
		}

		private bool EnterMissionObjectInteraction(EntityId _id, AnimationTaskType _animTaskType)
		{
			if (_id.Type == EntityTypes.EType.Pigeonry)
			{
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction);
				payload.EntityId = _id;
				PlayerMain.ScheduleStateChange(EStateType.PlayerMain_ObjectInteraction);
			}
			else
			{
				InteractableObject entity = _id.GetEntity<InteractableObject>();
				if (entity.InteractableObjectStateMachines.ObjectInteraction.RequiresLockPick)
				{
					int num = 0;
					UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
					for (int i = 0; i < profile.Inventory.elements.Length; i++)
					{
						InventoryElement inventoryElement = profile.Inventory.elements[i];
						if (inventoryElement.customLootData != null && inventoryElement.customLootData.customLootType == ECustomLootTemplateType.LockPick)
						{
							num += inventoryElement.stackSize;
						}
					}
					int consumedLockPicks = LogicMain.MissionResult.ConsumedLockPicks;
					if (num - consumedLockPicks < EntitlementExtensions.GetHeroicRankMapping(Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank).requiredLockPicks)
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(8393), true);
						AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
						return true;
					}
				}
				NavMeshAgent navMeshAgent = AssassinCharacter.UnityComponentCache.NavMeshAgent;
				AnimationTaskManager taskManager = PlayerMain.Entity.UnityComponentCache.AnimationController.TaskManager;
				InteractionLocators.Locator _locator;
				NavMeshPath _path;
				if (!entity.InteractionLocators.ChooseClosestLocator(navMeshAgent.transform.position, AssassinCharacter.GetAutoPassableNavMeshLayerMask(), out _locator, out _path))
				{
					return false;
				}
				if (_animTaskType == AnimationTaskType.InteractableObject)
				{
					taskManager.CreateAnimationTaskInteractableObjectLocation(_path, _id, _locator, 1);
				}
				PlayerMain.EnterCallbackDelayState(EStateType.PlayerMain_ObjectInteraction, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, _animTaskType);
				PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload payload2 = PlayerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_ObjectInteraction);
				payload2.EntityId = _id;
			}
			return true;
		}

		private void EnterLetCourtesansSeduce(EnemyCharacter _enemy)
		{
			AssassinCharacter.EnterRoaming();
			Assets.Scripts.Mission.Logic.Entities.Patrol hidingPatrol = AssassinCharacter.GetHidingPatrol();
			PatrolSeduce patrolSeduce = hidingPatrol.PatrolStateMachines.PatrolSeduce;
			PatrolSeduce.ApproachingState.Payload payload = patrolSeduce.GetPayload<PatrolSeduce.ApproachingState.Payload>(EStateType.PatrolSeduce_Approaching);
			payload.Init(hidingPatrol.Id, _enemy.Id);
			patrolSeduce.ScheduleStateChange(EStateType.PatrolSeduce_Approaching);
			RegisterConspiciousAction(EInteractionReason.LetCourtesansSeduce);
		}

		private bool AssassinCanInteractPeaceful(GameEntity _target)
		{
			return !IsInteractionPrevented(_target) && !AssassinCharacter.GetIsExposedAndSeen() && (AssassinCharacter.GetIsRoaming() || AssassinCharacter.GetIsInParcour() || AssassinCharacter.GetIsHidingInPatrol());
		}

		private bool AssassinCanInteractDefault(GameEntity _target)
		{
			return !IsInteractionPrevented(_target) && (AssassinCharacter.GetIsRoaming() || AssassinCharacter.GetIsInParcour() || AssassinCharacter.GetIsHidingInPatrol() || AssassinCharacter.GetIsBeingChased());
		}

		private bool IsInteractionPrevented(GameEntity _target)
		{
			if (_target is EnemyCharacter)
			{
				return false;
			}
			if (UbiCore.Utils.Singleton<SelectionManager>.Instance.Selection == _target)
			{
				PreventMovementExceptionTypes preventMovementExceptionTypes = (PreventMovementExceptionTypes)AssassinCharacter.EntityStats.Get_int(StatTypes.EStatType.Player_MovementPreventedException);
				EStateType currentStateType = AssassinCharacter.PlayerMainCarrier.PlayerMain.CurrentStateType;
				if (currentStateType == EStateType.PlayerMain_HidingInPatrol)
				{
					return preventMovementExceptionTypes != PreventMovementExceptionTypes.PreventMovementException_Courtesans;
				}
			}
			return !AssassinCharacter.GetIsMovementAllowed();
		}
	}
}
