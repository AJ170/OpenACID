using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class AssassinApproachingTarget : StateMachineBase<AssassinCharacter, AssassinApproachingTarget>
	{
		private EntityId m_TargetId = EntityId.InvalidId;

		private EInteractionReason m_Reason = EInteractionReason.Invalid;

		private InteractionLocators.Locator m_Locator;

		private EStateType m_PlayerMainStateOnSetTarget;

		private float m_TargetDistanceOnSetTarget = float.MaxValue;

		private readonly ISelectionProvider m_SelectionProvider;

		private readonly TargetApproacher m_TargetApproacher = new TargetApproacher
		{
			UpdateInterval = 0.5f
		};

		private Enum<SkillTypes> m_QueuedCombatSkillType;

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.AssassinApproachingTarget;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.AssassinApproachingTarget_Idle;
			}
		}

		public EntityId TargetId
		{
			get
			{
				return m_TargetId;
			}
		}

		public EInteractionReason Reason
		{
			get
			{
				return m_Reason;
			}
		}

		public InteractionLocators.Locator Locator
		{
			get
			{
				return m_Locator;
			}
		}

		public Enum<SkillTypes> QueuedCombatSkillType
		{
			get
			{
				if (m_QueuedCombatSkillType == Enum<SkillTypes>.Invalid)
				{
					SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<SkillData>.Instance.GetBuffedApproachSkillDefinition(Reason, Entity);
					if (buffedApproachSkillDefinition != null && (buffedApproachSkillDefinition.ActivationType & ESkillActivationType.Slots) != 0)
					{
						return buffedApproachSkillDefinition.Type;
					}
				}
				return m_QueuedCombatSkillType;
			}
			private set
			{
				m_QueuedCombatSkillType = value;
			}
		}

		public bool FreezeStartApproachingStatus { get; set; }

		public EStateType PlayerMainStateOnSetTarget
		{
			get
			{
				return m_PlayerMainStateOnSetTarget;
			}
		}

		public float TargetDistanceOnSetTarget
		{
			get
			{
				return m_TargetDistanceOnSetTarget;
			}
		}

		private EMovementSpeed MovementSpeed
		{
			get
			{
				EMovementSpeed eMovementSpeed = EMovementSpeed.Run;
				SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
				if (approachSkillDefinition != null)
				{
					eMovementSpeed = ((approachSkillDefinition.Speed == EMovementSpeed.Invalid_EMovementSpeed) ? Entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType.ToPlayerSpeed(Entity) : approachSkillDefinition.Speed);
				}
				else
				{
					switch (m_Reason)
					{
					case EInteractionReason.InteractWithNpc:
						eMovementSpeed = EMovementSpeed.WalkFast;
						break;
					case EInteractionReason.Combat:
					case EInteractionReason.InteractWithObject:
					case EInteractionReason.HidingInPatrol:
					case EInteractionReason.OffMeshLink:
					case EInteractionReason.FreeCapturedNpcs:
						eMovementSpeed = Entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType.ToPlayerSpeed(Entity);
						break;
					}
				}
				if (eMovementSpeed == EMovementSpeed.Walk)
				{
					eMovementSpeed = EMovementSpeed.WalkFast;
				}
				return eMovementSpeed;
			}
		}

		private SkillData.IApproachSkillDefinition ApproachSkillDefinition
		{
			get
			{
				return UbiCore.Utils.Singleton<SkillData>.Instance.GetBuffedApproachSkillDefinition(m_Reason, Entity);
			}
		}

		private bool RetainPathOnCancel
		{
			get
			{
				bool result = false;
				SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
				if (approachSkillDefinition != null)
				{
					result = approachSkillDefinition.RetainPathAfterApproaching;
				}
				return result;
			}
		}

		private bool StopAfterApproaching
		{
			get
			{
				SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
				if (approachSkillDefinition != null)
				{
					return approachSkillDefinition.StopAfterApproaching;
				}
				return true;
			}
		}

		private EStartIdleAnimAfterTaskPolicy StartIdleAnimAfterTaskPolicy
		{
			get
			{
				return (!StopAfterApproaching) ? EStartIdleAnimAfterTaskPolicy.No : EStartIdleAnimAfterTaskPolicy.DontCare;
			}
		}

		private TargetApproacher.EModeFlags ApproachingModeFlags
		{
			get
			{
				TargetApproacher.EModeFlags eModeFlags = TargetApproacher.EModeFlags.InDistance;
				switch (Reason)
				{
				case EInteractionReason.Combat:
					eModeFlags |= TargetApproacher.EModeFlags.InWalkableDistance;
					break;
				case EInteractionReason.InteractWithObject:
					switch (TargetId.Type)
					{
					case EntityTypes.EType.Haystack:
						if (m_Locator == null)
						{
							eModeFlags |= TargetApproacher.EModeFlags.AllowObstacleInPlainSight;
						}
						break;
					case EntityTypes.EType.Codex:
						eModeFlags |= TargetApproacher.EModeFlags.AllowObstacleInPlainSight;
						break;
					}
					break;
				default:
				{
					SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
					if (approachSkillDefinition != null)
					{
						eModeFlags = approachSkillDefinition.ApproachingModeFlags;
					}
					break;
				}
				}
				return eModeFlags;
			}
		}

		private float InteractionRangeSqr
		{
			get
			{
				SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
				float num;
				if (approachSkillDefinition != null)
				{
					num = approachSkillDefinition.StartDistance;
				}
				else
				{
					switch (m_Reason)
					{
					case EInteractionReason.Combat:
						num = 4f;
						break;
					case EInteractionReason.FreeCapturedNpcs:
						num = 1f;
						break;
					case EInteractionReason.InteractWithNpc:
						num = 2f;
						break;
					case EInteractionReason.HidingInPatrol:
						num = 3f;
						break;
					case EInteractionReason.InteractWithObject:
						num = m_TargetId.GetEntity<InteractableObject>().InteractableObjectDefinition.Definition.InteractionDistance;
						break;
					case EInteractionReason.OffMeshLink:
						num = 1f;
						break;
					default:
						return float.MaxValue;
					}
				}
				return num * num;
			}
		}

		private float InteractionHeight
		{
			get
			{
				float result = 0f;
				SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
				if (approachSkillDefinition != null)
				{
					result = approachSkillDefinition.StartHeight;
				}
				return result;
			}
		}

		public AssassinApproachingTarget(AssassinCharacter _entity, ISelectionProvider _selectionProvider)
			: base(_entity)
		{
			m_SelectionProvider = _selectionProvider;
		}

		public static void InitAssassinApproachingTarget()
		{
			StateMachineBase<AssassinCharacter, AssassinApproachingTarget>.InitStateMachineBase();
			StateMachineBase<AssassinCharacter, AssassinApproachingTarget>.AddState<State<AssassinCharacter>>(EStateType.AssassinApproachingTarget_Idle, null, null, OnUpdateIdle);
			StateMachineBase<AssassinCharacter, AssassinApproachingTarget>.AddState<State<AssassinCharacter>>(EStateType.AssassinApproachingTarget_Approaching, OnEnterApproaching, null, OnUpdateApproaching);
			StateMachineBase<AssassinCharacter, AssassinApproachingTarget>.AddTransition(EStateType.AssassinApproachingTarget_Idle, EStateType.AssassinApproachingTarget_Approaching);
			StateMachineBase<AssassinCharacter, AssassinApproachingTarget>.AddTransition(EStateType.AssassinApproachingTarget_Approaching, EStateType.AssassinApproachingTarget_Idle);
		}

		public bool SetTarget(EntityId _targetId, EInteractionReason _reason, InteractionLocators.Locator _locator = null, bool _stopMoving = true)
		{
			QueuedCombatSkillType = Enum<SkillTypes>.Invalid;
			return SetTarget_Internal(_targetId, _reason, _locator, _stopMoving, Vector3.zero, false);
		}

		public bool SetTarget(EntityId _targetId, EInteractionReason _reason, Vector3 _targetPosition, bool _stopMoving = true)
		{
			QueuedCombatSkillType = Enum<SkillTypes>.Invalid;
			return SetTarget_Internal(_targetId, _reason, null, _stopMoving, _targetPosition, true);
		}

		public bool SetTarget(EntityId _targetId, EInteractionReason _reason, Enum<SkillTypes> _queuedCombatSkillType)
		{
			QueuedCombatSkillType = _queuedCombatSkillType;
			return SetTarget_Internal(_targetId, _reason, null, true, Vector3.zero, false);
		}

		private bool SetTarget_Internal(EntityId _targetId, EInteractionReason _reason, InteractionLocators.Locator _locator, bool _stopMoving, Vector3 _targetPosition, bool _useTargetPosition)
		{
			if (_targetId == m_TargetId && _reason == m_Reason)
			{
				return true;
			}
			ISelectable selectable = _targetId.TryGetEntity<ISelectable>();
			if (!FreezeStartApproachingStatus)
			{
				m_PlayerMainStateOnSetTarget = Entity.PlayerMainCarrier.PlayerMain.CurrentStateType;
				m_TargetDistanceOnSetTarget = float.MaxValue;
			}
			if (selectable == null)
			{
				m_TargetId = _targetId;
				m_Reason = EInteractionReason.Invalid;
				m_Locator = null;
				m_TargetApproacher.Reset();
				if (_stopMoving)
				{
					StopMoving(true);
				}
				IParticipantCarrier participantCarrier = Entity as IParticipantCarrier;
				if (participantCarrier != null && !participantCarrier.CombatParticipantProxy.IsInCombat)
				{
					Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Sheathed);
				}
				if (CurrentStateType != EStateType.AssassinApproachingTarget_Idle)
				{
					ChangeStateForced(EStateType.AssassinApproachingTarget_Idle);
				}
				return true;
			}
			m_TargetId = _targetId;
			m_Reason = _reason;
			m_Locator = _locator;
			if (_useTargetPosition)
			{
				m_TargetApproacher.SetData(Entity.Id, _targetPosition, MovementSpeed, InteractionRangeSqr);
			}
			else if (m_Locator == null)
			{
				m_TargetApproacher.SetData(Entity.Id, m_TargetId, MovementSpeed, InteractionRangeSqr);
			}
			else
			{
				m_TargetApproacher.SetData(Entity.Id, m_Locator, MovementSpeed, InteractionRangeSqr);
			}
			m_TargetApproacher.RetainPathOnCancel = RetainPathOnCancel;
			m_TargetApproacher.StartIdleAnimAfterTaskPolicy = StartIdleAnimAfterTaskPolicy;
			m_TargetApproacher.ApproachHeight = InteractionHeight;
			m_TargetApproacher.ModeFlags = ApproachingModeFlags;
			if (m_TargetApproacher.IsTargetReachable)
			{
				m_SelectionProvider.Selection = selectable;
				if (!FreezeStartApproachingStatus)
				{
					m_TargetDistanceOnSetTarget = Entity.Position.DistanceXZSquared(m_TargetApproacher.ActualTargetPos);
				}
				if (CurrentStateType == EStateType.AssassinApproachingTarget_Idle)
				{
					ChangeStateForced(EStateType.AssassinApproachingTarget_Approaching);
				}
				else
				{
					StartApproaching();
				}
				return true;
			}
			return SetTarget(EntityId.InvalidId, _reason);
		}

		public void Stop(bool _stopMoving = true)
		{
			SetTarget(EntityId.InvalidId, EInteractionReason.Invalid, null, _stopMoving);
		}

		public EInteractionReason EvaluateInteractionReason(EntityId _entityId, EInteractionReason _preferredReason, out ESkillAvailabilityFlags _availabilityFlags)
		{
			IGameEntity gameEntity = _entityId.TryGetEntity<IGameEntity>();
			Character character = gameEntity as Character;
			_availabilityFlags = ESkillAvailabilityFlags.Unknown;
			if (character != null)
			{
				if (character.GetIsDead())
				{
					return EInteractionReason.Invalid;
				}
				EntityIdSkillPayload entityIdSkillPayload = new EntityIdSkillPayload(_entityId);
				ESkillAvailabilityFlags _availabilityFlags2;
				if (Entity.CanTriggerSelectedSkill(entityIdSkillPayload, out _availabilityFlags2))
				{
					return EInteractionReason.TriggerSelectedSkill;
				}
				SkillData.IApproachSkillDefinition buffedApproachSkillDefinition = UbiCore.Utils.Singleton<SkillData>.Instance.GetBuffedApproachSkillDefinition(_preferredReason, Entity);
				if (_preferredReason == EInteractionReason.Invalid || buffedApproachSkillDefinition != null)
				{
					SkillData.IApproachSkillDefinition availableApproachSkill = Entity.Skills.GetAvailableApproachSkill(entityIdSkillPayload, buffedApproachSkillDefinition, true, out _availabilityFlags);
					if (buffedApproachSkillDefinition != null && (buffedApproachSkillDefinition.ActivationType & ESkillActivationType.Slots) != 0 && availableApproachSkill != buffedApproachSkillDefinition)
					{
						return EInteractionReason.Invalid;
					}
					if (availableApproachSkill != null)
					{
						return availableApproachSkill.InteractionReason;
					}
				}
				if (character is EnemyCharacter)
				{
					if (Entity.GetIsHidingInCourtesans())
					{
						if (character.GetIsThreateningOrNormal())
						{
							return EInteractionReason.LetCourtesansSeduce;
						}
						return EInteractionReason.Invalid;
					}
					if (!Entity.GetIsPerformingAssassination() && Entity is IParticipantCarrier)
					{
						return EInteractionReason.Combat;
					}
				}
				else if (character is CrowdCharacter)
				{
					CrowdCharacter crowdCharacter = (CrowdCharacter)character;
					Assets.Scripts.Mission.Logic.Entities.Patrol patrol = crowdCharacter.Patrol;
					if (patrol != null)
					{
						return EvaluateInteractionReason(patrol.Id, _preferredReason, out _availabilityFlags);
					}
				}
				else if (character is InteractionCharacter)
				{
					return EInteractionReason.InteractWithNpc;
				}
			}
			else if (gameEntity is Assets.Scripts.Mission.Logic.Entities.Patrol)
			{
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol2 = (Assets.Scripts.Mission.Logic.Entities.Patrol)gameEntity;
				if (patrol2.PatrolComponent.Formation.EntityAtHidingSpot != Entity.Id && !patrol2.IsOnSidetrack())
				{
					return patrol2.PatrolDefinition.Definition.InteractionReason;
				}
			}
			else if (gameEntity is InteractableObject)
			{
				return EInteractionReason.InteractWithObject;
			}
			return EInteractionReason.Invalid;
		}

		public EInteractionReason EvaluateInteractionReason(EntityId _entityId)
		{
			ESkillAvailabilityFlags _availabilityFlags;
			return EvaluateInteractionReason(_entityId, EInteractionReason.Invalid, out _availabilityFlags);
		}

		private void StartApproaching()
		{
			m_TargetApproacher.Approach();
			UpdateRunning();
		}

		private static void OnUpdateIdle(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> currentState)
		{
			AssassinApproachingTarget assassinApproachingTarget = (AssassinApproachingTarget)stateMachine;
			if (assassinApproachingTarget.m_TargetId != EntityId.InvalidId)
			{
				stateMachine.ScheduleStateChange(EStateType.AssassinApproachingTarget_Approaching);
			}
		}

		private static void OnEnterApproaching(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> prevState, IState<AssassinCharacter> currentState)
		{
			AssassinApproachingTarget assassinApproachingTarget = (AssassinApproachingTarget)stateMachine;
			assassinApproachingTarget.StartApproaching();
		}

		private static void OnUpdateApproaching(IStateMachine<AssassinCharacter> stateMachine, IPayload payload, IState<AssassinCharacter> currentState)
		{
			AssassinApproachingTarget assassinApproachingTarget = (AssassinApproachingTarget)stateMachine;
			bool flag = false;
			IPositionEntity positionEntity = null;
			if (assassinApproachingTarget.m_TargetId != EntityId.InvalidId)
			{
				positionEntity = assassinApproachingTarget.m_TargetId.GetEntity<IPositionEntity>();
				Character character = positionEntity as Character;
				flag = character == null || !character.GetIsDead();
			}
			if (positionEntity != null && !flag)
			{
				assassinApproachingTarget.SetTarget(EntityId.InvalidId, EInteractionReason.Invalid);
				positionEntity = null;
			}
			if (assassinApproachingTarget.m_TargetId == EntityId.InvalidId)
			{
				assassinApproachingTarget.Stop();
			}
			else if (assassinApproachingTarget.m_TargetApproacher.Status == TargetApproacher.EStatus.Running)
			{
				assassinApproachingTarget.UpdateRunning();
			}
			else if (assassinApproachingTarget.StopAfterApproaching)
			{
				assassinApproachingTarget.Stop();
			}
		}

		private void UpdateRunning()
		{
			if (m_TargetId == EntityId.InvalidId)
			{
				return;
			}
			ESkillAvailabilityFlags _availabilityFlags;
			m_Reason = EvaluateInteractionReason(m_TargetId, m_Reason, out _availabilityFlags);
			if (m_Reason == EInteractionReason.Invalid)
			{
				Stop();
				return;
			}
			m_TargetApproacher.SpeedType = MovementSpeed;
			m_TargetApproacher.ApproachDistanceSqr = InteractionRangeSqr;
			m_TargetApproacher.ApproachHeight = InteractionHeight;
			m_TargetApproacher.RetainPathOnCancel = RetainPathOnCancel;
			m_TargetApproacher.StartIdleAnimAfterTaskPolicy = StartIdleAnimAfterTaskPolicy;
			m_TargetApproacher.ModeFlags = ApproachingModeFlags;
			NpcCharacter npcCharacter = m_TargetId.TryGetEntity<NpcCharacter>();
			if (npcCharacter != null)
			{
				EnemyCharacter enemyCharacter = npcCharacter as EnemyCharacter;
				if (enemyCharacter != null)
				{
					UpdateRunning(enemyCharacter);
					return;
				}
				InteractionCharacter interactionCharacter = npcCharacter as InteractionCharacter;
				if (interactionCharacter != null)
				{
					UpdateRunning(interactionCharacter);
				}
				else
				{
					Stop();
				}
				return;
			}
			InteractableObject interactableObject = m_TargetId.TryGetEntity<InteractableObject>();
			if (interactableObject != null)
			{
				UpdateRunning(interactableObject);
				return;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = m_TargetId.TryGetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
			if (patrol != null)
			{
				UpdateRunning(patrol);
				return;
			}
			OffMeshLinkEntity offMeshLinkEntity = m_TargetId.TryGetEntity<OffMeshLinkEntity>();
			if (offMeshLinkEntity != null)
			{
				UpdateRunning(offMeshLinkEntity);
			}
			else
			{
				Stop();
			}
		}

		private void UpdateRunning(EnemyCharacter _target)
		{
			if (m_SelectionProvider.Selection == _target)
			{
				if (m_TargetApproacher.Update())
				{
					if (m_Reason == EInteractionReason.Combat && Entity.AssassinCharacterStateMachines.CharacterDrawMeleeWeapon.ChosenWeaponState != EWeaponState.Unsheathed)
					{
						bool flag = Entity.AssassinCharacterStateMachines.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_High;
						float magnitude = (_target.Position - Entity.Position).magnitude;
						float num = ((!flag) ? 7f : 10f);
						if (magnitude <= num)
						{
							Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
						}
					}
				}
				else if (m_TargetApproacher.Status == TargetApproacher.EStatus.Arrived)
				{
					SkillData.IApproachSkillDefinition approachSkillDefinition = ApproachSkillDefinition;
					if (approachSkillDefinition != null)
					{
						if (Entity.Skills.Trigger(approachSkillDefinition.Type, new EntityIdSkillPayload(_target.Id)))
						{
							FreezeStartApproachingStatus = true;
							if (ApproachSkillDefinition.StopAfterApproaching)
							{
								Stop();
							}
							FreezeStartApproachingStatus = false;
						}
						else
						{
							Stop();
						}
						return;
					}
					PlayerCharacter playerCharacter = Entity as PlayerCharacter;
					if (playerCharacter != null)
					{
						Entity.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
						StopMoving();
						if (PlayerHelper.EnterInitCombat(_target, false))
						{
							Stop(false);
						}
						else
						{
							Stop();
						}
					}
					else
					{
						Stop(false);
					}
				}
				else
				{
					Stop();
				}
			}
			else
			{
				Stop();
			}
		}

		private void UpdateRunning(InteractionCharacter _target)
		{
			if (m_SelectionProvider.Selection == _target)
			{
				if (m_TargetApproacher.Update())
				{
					return;
				}
				if (m_TargetApproacher.Status == TargetApproacher.EStatus.Arrived)
				{
					StopMoving(true);
					if (Entity.AssassinInteraction.CanInteractWithNpc(m_TargetId))
					{
						Entity.AssassinInteraction.EnterNpcInteraction(m_TargetId);
						Stop(false);
					}
					else
					{
						Stop();
					}
				}
				else
				{
					Stop();
				}
			}
			else
			{
				Stop();
			}
		}

		private void UpdateRunning(InteractableObject _target)
		{
			if (m_SelectionProvider.Selection == _target)
			{
				if (!m_TargetApproacher.Update() && m_TargetApproacher.Status == TargetApproacher.EStatus.Arrived)
				{
					StopMoving(true);
					EInteractionReason reason = m_Reason;
					if (reason == EInteractionReason.InteractWithObject)
					{
						Entity.AssassinInteraction.EnterObjectInteraction(m_TargetId, m_TargetApproacher.TargetLocator);
						Stop(false);
					}
				}
			}
			else
			{
				Stop();
			}
		}

		private void UpdateRunning(OffMeshLinkEntity _target)
		{
			if (m_TargetApproacher.Update())
			{
				return;
			}
			if (m_TargetApproacher.Status == TargetApproacher.EStatus.Arrived)
			{
				StopMoving();
				EInteractionReason reason = m_Reason;
				if (reason == EInteractionReason.OffMeshLink)
				{
					Entity.AssassinInteraction.EnterParcour(m_TargetId.GetEntity<OffMeshLinkEntity>(), m_TargetApproacher.TargetLocator);
					Stop(false);
				}
				else
				{
					Stop();
				}
			}
			else
			{
				Stop();
			}
		}

		private void UpdateRunning(Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			if (m_SelectionProvider.Selection == _patrol)
			{
				if (!m_TargetApproacher.Update())
				{
					if (m_TargetApproacher.Status == TargetApproacher.EStatus.Arrived)
					{
						Entity.AssassinInteraction.EnterPatrol(_patrol.Id, m_Reason);
						Stop();
					}
					else
					{
						Stop();
					}
				}
			}
			else
			{
				Stop();
			}
		}

		private void StopMoving(bool _cancelAnimation = false)
		{
			if (Entity.UnityComponentCache.NavMeshAgent.enabled)
			{
				Entity.UnityComponentCache.NavMeshAgent.Stop();
			}
			if (_cancelAnimation)
			{
				AnimationTaskBase currentTask = Entity.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
				if (currentTask == null)
				{
					Entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle();
				}
				else if (currentTask.IsMovementTask() || currentTask.IsParkourTask())
				{
					Entity.UnityComponentCache.AnimationController.TaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base, true);
				}
			}
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			FreezeStartApproachingStatus = false;
			m_TargetId = EntityId.InvalidId;
			m_Reason = EInteractionReason.Invalid;
			m_Locator = null;
			m_PlayerMainStateOnSetTarget = EStateType.Invalid;
			m_TargetDistanceOnSetTarget = float.MaxValue;
			m_TargetApproacher.Reset();
			m_QueuedCombatSkillType = Enum<SkillTypes>.Invalid;
		}
	}
}
