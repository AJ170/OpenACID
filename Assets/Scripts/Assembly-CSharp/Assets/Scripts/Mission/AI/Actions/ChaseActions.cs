using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class ChaseActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData
		{
			public readonly BBList<GameEntity> m_hidingSpotsInRange = new BBList<GameEntity>();

			public readonly SimpleGameTimer m_inspectionSearchTimer = new SimpleGameTimer();

			public readonly AutoAnimationRotation m_autoRotation;

			public float m_lookAroundEndAngle;

			public InspectionTarget m_currentInspectionTarget = new InspectionTarget();

			public float m_inspectionStartTime = float.MaxValue;

			public bool m_clothAnimationStarted;

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
				m_autoRotation = new AutoAnimationRotation(_npcRuntimeData.NpcCharacter)
				{
					FireAndForget = false
				};
			}
		}

		public class InspectionTarget
		{
			public NpcCharacter m_inspector;

			public GameEntity m_spotEntity;

			public Vector3 m_approachPos;

			public float m_successDistanceSqr;

			public InteractionLocators.Locator m_locator;

			public float m_pokeCorrectionAngle;

			public bool IsInitialized
			{
				get
				{
					return m_spotEntity != null && m_inspector != null;
				}
			}

			public bool IsInitializedAndRegistered
			{
				get
				{
					EntityId _inspector;
					return IsInitialized && UbiCore.Utils.Singleton<HotspotService>.Instance.SpotInspectionManager.GetSpotInspector(m_spotEntity.Id, out _inspector) && _inspector == m_inspector.Id;
				}
			}

			public bool IsPokingLocation
			{
				get
				{
					return GetIsPokingLocation(m_spotEntity);
				}
			}

			public bool MayMove
			{
				get
				{
					Assets.Scripts.Mission.Logic.Entities.Patrol patrol = m_spotEntity as Assets.Scripts.Mission.Logic.Entities.Patrol;
					return patrol != null && patrol.PatrolComponent.CurrentRoute != null && patrol.PatrolComponent.CurrentRoute.Count > 1;
				}
			}

			public bool RouseForced
			{
				get
				{
					return true;
				}
			}

			public void Initialize(NpcCharacter _inspector, GameEntity _spot, Vector3 _spotPosition, float _successDistanceSqr)
			{
				m_inspector = _inspector;
				m_spotEntity = _spot;
				m_approachPos = _spotPosition;
				m_successDistanceSqr = _successDistanceSqr;
			}

			public void Reset()
			{
				if (IsInitialized)
				{
					DeregisterSpotInspector();
				}
				m_spotEntity = null;
			}

			public void RegisterSpotInspection()
			{
				if (IsInitialized)
				{
					UbiCore.Utils.Singleton<HotspotService>.Instance.SpotInspectionManager.RegisterSpotInspector(m_spotEntity.Id, m_inspector.Id);
				}
			}

			public void DeregisterSpotInspector()
			{
				if (IsInitialized)
				{
					UbiCore.Utils.Singleton<HotspotService>.Instance.SpotInspectionManager.DeregisterSpotInspector(m_spotEntity.Id, m_inspector.Id);
				}
			}

			public static bool GetIsPokingLocation(GameEntity _spot)
			{
				return _spot is Haystack || _spot is RooftopGarden;
			}
		}

		private const float DetachedTurnSpeed = 1f;

		private const float InspectionPokingReduceDistFactor = 0.24f;

		private const float InspectionPokingHaystackAngleCorrection = 70f;

		private const float InspectionPokingRoofTopGardenAngleCorrection = -10f;

		private const float PokeRouseTime = 3f;

		private const float PokeClothMovingTime = 2f;

		private const float InspectDurationWithoutRouse = 3f;

		private const EHidingSpotFlags GroundHidingSpotPreset = EHidingSpotFlags.Haystack | EHidingSpotFlags.Bench | EHidingSpotFlags.Crowd | EHidingSpotFlags.StalkingZone;

		private const EHidingSpotFlags RoofHidingSpotPreset = EHidingSpotFlags.RooftopGarden;

		private readonly int[] InspectionTaskTypes = new int[2] { 34, 56 };

		private float InspectionStartTime
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_inspectionStartTime;
			}
			set
			{
				base.NpcRuntimeData.Chase.m_inspectionStartTime = value;
			}
		}

		private float LookAroundEndAngle
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_lookAroundEndAngle;
			}
		}

		private SimpleGameTimer InspectionSearchTimer
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_inspectionSearchTimer;
			}
		}

		private InspectionTarget CurrentInspectionTarget
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_currentInspectionTarget;
			}
		}

		private BBList<GameEntity> HidingSpotsInRange
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_hidingSpotsInRange;
			}
		}

		private AutoAnimationRotation AutoRotation
		{
			get
			{
				return base.NpcRuntimeData.Chase.m_autoRotation;
			}
		}

		private TargetApproacher TargetApproacher
		{
			get
			{
				return base.NpcRuntimeData.Chase.TargetApproacher;
			}
		}

		private NavMeshPath DummyPath
		{
			get
			{
				return base.NpcRuntimeData.Chase.DummyPath;
			}
		}

		public ChaseActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool ConditionHasHidingSpotsCollected()
		{
			return HidingSpotsInRange.Count > 0;
		}

		public bool ConditionInspectionAtRouseTime()
		{
			if (CurrentInspectionTarget.IsPokingLocation)
			{
				return LogicMain.CurrentTime > InspectionStartTime + 3f;
			}
			if (!CurrentInspectionTarget.RouseForced)
			{
				return LogicMain.CurrentTime > InspectionStartTime + 3f;
			}
			return true;
		}

		public bool ConditionInspectionRouseForced()
		{
			return CurrentInspectionTarget.RouseForced;
		}

		public void AttachApproachTarget()
		{
			ActionApproachTarget(BehaviorReturnCode.Running);
		}

		public BehaviorReturnCode ActionApproachTarget(BehaviorReturnCode _lastReturnCode)
		{
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Failure;
			CharacterAwareness.AlarmedState.AlarmedPayload alarmedPayload = AiHelper.GetAlarmedPayload(base.NpcCharacter);
			if (!alarmedPayload.LastKnownIsSet)
			{
				return BehaviorReturnCode.Failure;
			}
			Vector3 lastKnownPosition = alarmedPayload.LastKnownPosition;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(lastKnownPosition, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				lastKnownPosition = _hit.position;
				float speedValue = EMovementSpeed.Sprint.GetDefaultSpeedValue() * LogicMain.GameDefinition.EnemyChaseSpeedFactor;
				TargetApproacher.SetData(base.NpcCharacter.Id, lastKnownPosition, speedValue, CalcChasingSuccessDistSqr());
				behaviorReturnCode = TargetApproacher.ApproachCurrentTargetAi(lastKnownPosition);
			}
			EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
			if (enemyCharacter != null)
			{
				if (behaviorReturnCode == BehaviorReturnCode.Running)
				{
					if (_lastReturnCode != BehaviorReturnCode.Running)
					{
						EnemyHelper.EnterChasing(enemyCharacter);
					}
					AiHelper.RestartAlarmedActivity(enemyCharacter);
				}
				else if (_lastReturnCode == BehaviorReturnCode.Running)
				{
					EnemyHelper.EnterRoaming(enemyCharacter);
				}
			}
			return behaviorReturnCode;
		}

		private float CalcChasingSuccessDistSqr()
		{
			return (!base.Knowledge.TargetKnowledge.TargetAsPlayer.GetIsInParcour()) ? 1f : 16f;
		}

		public void CancelApproachTarget()
		{
			TargetApproacher.Reset();
			if (base.NpcCharacter.IsAttached)
			{
				base.NpcCharacter.CancelMovement(true, false);
			}
			EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
			if (enemyCharacter != null)
			{
				EnemyMain enemyMain = enemyCharacter.EnemyCharacterStateMachines.EnemyMain;
				if (enemyMain.CurrentStateType == EStateType.EnemyMain_Chasing && enemyMain.ScheduledStateType == EStateType.Invalid)
				{
					EnemyHelper.EnterRoaming(enemyCharacter);
				}
			}
		}

		public BehaviorReturnCode ActionWait(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionWait(base.NpcCharacter, _lastReturnCode);
		}

		public BehaviorReturnCode ActionTurnInLastKnownRotationY(BehaviorReturnCode _lastReturnCode)
		{
			CharacterAwareness.AlarmedState.AlarmedPayload alarmedPayload = AiHelper.GetAlarmedPayload(base.NpcCharacter);
			base.NpcCharacter.Rotation = Quaternion.Euler(0f, alarmedPayload.LastKnownRotation, 0f);
			return BehaviorReturnCode.Success;
		}

		public void AttachLookAroundLastKnownRotationY()
		{
			if (AutoRotation.IsRunning)
			{
				AutoRotation.Stop();
			}
			base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskNpcTurning(LookAroundEndAngle);
		}

		public BehaviorReturnCode ActionLookLeftOfLastKnownRotationY(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, OnEnterLookLeftOfLastKnownDirection, OnUpdateLookAround, false);
		}

		public BehaviorReturnCode ActionLookRightOfLastKnownRotationY(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, OnEnterLookRightOfLastKnownDirection, OnUpdateLookAround, false);
		}

		public void CancelLookAroundLastKnownRotationY()
		{
			if (AutoRotation.IsRunning)
			{
				AutoRotation.Stop();
			}
		}

		public BehaviorReturnCode ActionCombat(BehaviorReturnCode _lastReturnCode)
		{
			Profiling.Start("ActionCombat", float.MaxValue);
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Failure;
			if (!base.NpcCharacter.IsAttached)
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (base.NpcCharacter.GetIsInParcourOrScheduled())
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (LogicMain.PlayerEntity.GetIsInParcourOrScheduled())
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (LogicMain.PlayerEntity.GetIsHiding())
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (!base.Knowledge.TargetKnowledge.TargetAttackable())
			{
				behaviorReturnCode = BehaviorReturnCode.Running;
			}
			else if (base.Knowledge.TargetKnowledge.TargetDead())
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (!NavMesh.CalculatePath(base.NpcCharacter.Position, base.Knowledge.TargetKnowledge.TargetEntity.Position, NavMeshLayers.WalkableLayerMaskPreset, DummyPath) || DummyPath.status != 0)
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else if (DummyPath.CalculateLength() > 7f)
			{
				behaviorReturnCode = BehaviorReturnCode.Failure;
			}
			else
			{
				EnemyCharacter enemy = base.NpcCharacter.Cast<EnemyCharacter>();
				Profiling.Start("ActionCombat:EnterInitCombat", float.MaxValue);
				behaviorReturnCode = ((!PlayerHelper.EnterInitCombat(enemy, true)) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success);
				Profiling.Stop();
			}
			if (behaviorReturnCode == BehaviorReturnCode.Running)
			{
				IdleType idleType = IdleType.HighProfile;
				for (AnimatorStateMachineStatic.StateMachine stateMachine = base.NpcCharacter.UnityComponentCache.AnimationController.StateMachine.GetCurrentStateMachine(); stateMachine != null; stateMachine = stateMachine.ParentStateMachine)
				{
					if (stateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_NpcIdleTypes))
					{
						idleType = IdleType.Neutral;
						break;
					}
				}
				AiHelper.ActionWait(base.NpcCharacter, idleType, _lastReturnCode);
				base.NpcCharacter.CancelMovement(false);
				if (idleType == IdleType.HighProfile)
				{
					base.NpcCharacter.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
				}
			}
			Profiling.Stop();
			return behaviorReturnCode;
		}

		public BehaviorReturnCode ActionUnsheathMelee(BehaviorReturnCode _behaviorReturnCode)
		{
			ICharacterDrawMeleeWeaponCarrier characterDrawMeleeWeaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawMeleeWeaponCarrier;
			AiHelper.UnsheathWeapon(base.NpcCharacter, characterDrawMeleeWeaponCarrier.CharacterDrawMeleeWeapon, EWeaponCategory.Melee, true);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionSheathMelee(BehaviorReturnCode _behaviorReturnCode)
		{
			ICharacterDrawMeleeWeaponCarrier characterDrawMeleeWeaponCarrier = base.NpcCharacter.StateMachines as ICharacterDrawMeleeWeaponCarrier;
			CharacterDrawWeaponMelee characterDrawMeleeWeapon = characterDrawMeleeWeaponCarrier.CharacterDrawMeleeWeapon;
			AiHelper.SheathWeapon(base.NpcCharacter, characterDrawMeleeWeaponCarrier.CharacterDrawMeleeWeapon, EWeaponCategory.Melee, true);
			return (characterDrawMeleeWeapon.CurrentStateType != EStateType.CharacterDrawWeapon_Sheathed) ? BehaviorReturnCode.Running : BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionClearHidingSpots(BehaviorReturnCode _lastReturnCode)
		{
			HidingSpotsInRange.Clear();
			CurrentInspectionTarget.Reset();
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionCollectHidingSpots(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			float hidingSpotInspectionDistance = npcCharacter.NpcDefinition.Definition.HidingSpotInspectionDistance;
			HidingSpotsInRange.Clear();
			EHidingSpotFlags eHidingSpotFlags = DetermineSpotFlags(npcCharacter);
			bool flag = (eHidingSpotFlags & EHidingSpotFlags.Haystack) != 0 && CollectHidingObjectInRadius<Haystack>(hidingSpotInspectionDistance, EntityTypes.EType.Haystack);
			flag |= (eHidingSpotFlags & EHidingSpotFlags.RooftopGarden) != 0 && CollectHidingObjectInRadius<RooftopGarden>(hidingSpotInspectionDistance, EntityTypes.EType.RooftopGarden);
			flag |= (eHidingSpotFlags & EHidingSpotFlags.Bench) != 0 && CollectHidingObjectInRadius<Bench>(hidingSpotInspectionDistance, EntityTypes.EType.Bench);
			flag |= (eHidingSpotFlags & EHidingSpotFlags.Crowd) != 0 && CollectHidingObjectInRadius<Assets.Scripts.Mission.Logic.Entities.Patrol>(hidingSpotInspectionDistance, EntityTypes.EType.Patrol, PatrolIsCover);
			flag |= (eHidingSpotFlags & EHidingSpotFlags.StalkingZone) != 0 && CollectHidingObjectInRadius<StalkingZone>(hidingSpotInspectionDistance, EntityTypes.EType.StalkingZone);
			HidingSpotsInRange.Sort(SortByDistance);
			return (!flag) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionChooseHidingSpot(BehaviorReturnCode _lastReturnCode)
		{
			int count = HidingSpotsInRange.Count;
			if (count == 0)
			{
				return BehaviorReturnCode.Failure;
			}
			CurrentInspectionTarget.Reset();
			BBList<GameEntity> bBList = new BBList<GameEntity>();
			for (int i = 0; i < count; i++)
			{
				GameEntity gameEntity = HidingSpotsInRange[i];
				SpotInspectionManager spotInspectionManager = UbiCore.Utils.Singleton<HotspotService>.Instance.SpotInspectionManager;
				NpcCharacter npcCharacter = base.NpcCharacter;
				EntityId _inspector;
				if (spotInspectionManager.GetSpotInspector(gameEntity.Id, out _inspector))
				{
					if (_inspector != npcCharacter.Id)
					{
						continue;
					}
					spotInspectionManager.DeregisterSpotInspector(gameEntity.Id, npcCharacter.Id);
				}
				InteractableObject interactableObject = gameEntity as InteractableObject;
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = gameEntity as Assets.Scripts.Mission.Logic.Entities.Patrol;
				StalkingZone stalkingZone = gameEntity as StalkingZone;
				Vector3 position = npcCharacter.Position;
				if (interactableObject != null)
				{
					InteractionLocators.Locator _locator = null;
					NavMeshPath _path;
					if (interactableObject.InteractionLocators.ChooseClosestLocator(position, NavMeshLayers.FullParcourMaskPreset, out _locator, out _path))
					{
						float successDistanceSqr = 16f;
						Vector3 position2 = _locator.Position.transform.position;
						float pokeCorrectionAngle = 0f;
						if (InspectionTarget.GetIsPokingLocation(gameEntity))
						{
							successDistanceSqr = 0.48999998f;
							Vector3 vector = gameEntity.Position - position2;
							position2 += vector * 0.24f;
							if (gameEntity is Haystack)
							{
								pokeCorrectionAngle = 70f;
							}
							else if (gameEntity is RooftopGarden)
							{
								pokeCorrectionAngle = -10f;
							}
						}
						CurrentInspectionTarget.Initialize(npcCharacter, gameEntity, position2, successDistanceSqr);
						CurrentInspectionTarget.m_locator = _locator;
						CurrentInspectionTarget.m_pokeCorrectionAngle = pokeCorrectionAngle;
						break;
					}
					bBList.Add(gameEntity);
				}
				else if (patrol != null)
				{
					if (!patrol.IsOnSidetrack() && patrol.PatrolComponent.Formation.MemberCount > 0 && NavMesh.CalculatePath(position, gameEntity.Position, NavMeshLayers.WalkableLayerMaskPreset, DummyPath))
					{
						CurrentInspectionTarget.Initialize(npcCharacter, gameEntity, gameEntity.Position, 16f);
						break;
					}
					bBList.Add(gameEntity);
				}
				else if (stalkingZone != null)
				{
					Vector3 spotPosition = stalkingZone.StalkingZoneComponent.ChooseClosestColliderPositionInStalkingZone(position);
					CurrentInspectionTarget.Initialize(npcCharacter, gameEntity, spotPosition, 16f);
					break;
				}
			}
			int count2 = bBList.Count;
			for (int j = 0; j < count2; j++)
			{
				GameEntity item = bBList[j];
				HidingSpotsInRange.Remove(item);
			}
			if (CurrentInspectionTarget.IsInitialized)
			{
				CurrentInspectionTarget.RegisterSpotInspection();
				return BehaviorReturnCode.Success;
			}
			return BehaviorReturnCode.Failure;
		}

		public BehaviorReturnCode ActionMoveToHidingSpot(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running && CurrentInspectionTarget.IsInitialized)
			{
				TargetApproacher.Reset();
				NpcCharacter npcCharacter = base.NpcCharacter;
				if (CurrentInspectionTarget.MayMove)
				{
					TargetApproacher.SetData(npcCharacter.Id, CurrentInspectionTarget.m_spotEntity.Id, EMovementSpeed.WalkFast, CurrentInspectionTarget.m_successDistanceSqr);
				}
				else
				{
					TargetApproacher.SetData(npcCharacter.Id, CurrentInspectionTarget.m_approachPos, EMovementSpeed.WalkFast, CurrentInspectionTarget.m_successDistanceSqr);
				}
			}
			TargetApproacher.ModeFlags |= TargetApproacher.EModeFlags.AllowObstacleInPlainSight;
			BehaviorReturnCode behaviorReturnCode = ((TargetApproacher.SourceEntityId == EntityId.InvalidId) ? BehaviorReturnCode.Failure : TargetApproacher.ApproachTargetAi());
			if (behaviorReturnCode == BehaviorReturnCode.Running)
			{
				RestartAlarmedActivity();
			}
			else
			{
				TargetApproacher.Reset();
			}
			if (LogicMain.IsInTurnUpdate)
			{
				LogicMain.MissionResult.IncreaseTracedBySeekersTurnCount();
			}
			return behaviorReturnCode;
		}

		public BehaviorReturnCode ActionInspectHidingSpot(BehaviorReturnCode _lastReturnCode)
		{
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Failure;
			if (CurrentInspectionTarget.IsInitialized)
			{
				if (base.NpcCharacter.IsAttached)
				{
					if (CurrentInspectionTarget.IsPokingLocation)
					{
						behaviorReturnCode = AiHelper.ActionWaitForAnim(_lastReturnCode, base.NpcCharacter, OnEnterPokeIntoHidingSpot, InspectionTaskTypes, false);
						if (!base.NpcRuntimeData.Chase.m_clothAnimationStarted && LogicMain.CurrentTime > InspectionStartTime + 2f)
						{
							base.NpcRuntimeData.Chase.m_clothAnimationStarted = true;
							RooftopGarden rooftopGarden = CurrentInspectionTarget.m_spotEntity as RooftopGarden;
							if (rooftopGarden != null)
							{
								rooftopGarden.RooftopGardenClothes.JumpIn(CurrentInspectionTarget.m_locator);
							}
						}
					}
					else if (!CurrentInspectionTarget.RouseForced)
					{
						if (_lastReturnCode != BehaviorReturnCode.Running)
						{
							base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdle(IdleType.Searching);
							InspectionSearchTimer.Start(3f);
						}
						behaviorReturnCode = ((!InspectionSearchTimer.IsRunning) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running);
					}
					else
					{
						behaviorReturnCode = BehaviorReturnCode.Success;
					}
				}
				else
				{
					behaviorReturnCode = BehaviorReturnCode.Success;
				}
			}
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				base.NpcCharacter.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
				InspectionStartTime = LogicMain.CurrentTime;
			}
			if (behaviorReturnCode == BehaviorReturnCode.Running)
			{
				RestartAlarmedActivity();
			}
			else
			{
				CancelInspectHidingSpot();
			}
			return behaviorReturnCode;
		}

		public void CancelInspectHidingSpot()
		{
			InspectionStartTime = float.MaxValue;
			base.NpcRuntimeData.Chase.m_clothAnimationStarted = false;
			base.NpcRuntimeData.Chase.m_hidingSpotsInRange.Remove(CurrentInspectionTarget.m_spotEntity);
			CurrentInspectionTarget.Reset();
		}

		private bool OnEnterLookLeftOfLastKnownDirection()
		{
			return OnEnterLookAroundOfLastKnownDirection(0f - LogicMain.GameDefinition.EnemySearchSweepAngle);
		}

		private bool OnEnterLookRightOfLastKnownDirection()
		{
			return OnEnterLookAroundOfLastKnownDirection(LogicMain.GameDefinition.EnemySearchSweepAngle);
		}

		private bool OnEnterLookAroundOfLastKnownDirection(float _angle)
		{
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			CharacterAwareness.AlarmedState.AlarmedPayload alarmedPayload = AiHelper.GetAlarmedPayload(base.NpcCharacter);
			base.NpcRuntimeData.Chase.m_lookAroundEndAngle = alarmedPayload.LastKnownRotation + _angle;
			if (base.NpcCharacter.IsAttached)
			{
				return animationController.TaskManager.CreateAnimationTaskNpcTurning(base.NpcRuntimeData.Chase.m_lookAroundEndAngle);
			}
			base.NpcRuntimeData.Chase.m_autoRotation.From = base.NpcCharacter.Rotation;
			base.NpcRuntimeData.Chase.m_autoRotation.To = Quaternion.Euler(0f, base.NpcRuntimeData.Chase.m_lookAroundEndAngle, 0f);
			base.NpcRuntimeData.Chase.m_autoRotation.Speed = 1f;
			base.NpcRuntimeData.Chase.m_autoRotation.Start();
			return true;
		}

		private bool OnUpdateLookAround()
		{
			bool flag = ((!base.NpcCharacter.IsAttached) ? base.NpcRuntimeData.Chase.m_autoRotation.IsRunning : (base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.NpcTurning));
			if (flag)
			{
				RestartAlarmedActivity();
			}
			return flag;
		}

		private bool OnEnterPokeIntoHidingSpot()
		{
			return base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskPokeIntoHaystack(CurrentInspectionTarget.m_spotEntity.Position, CurrentInspectionTarget.m_pokeCorrectionAngle);
		}

		private EHidingSpotFlags DetermineSpotFlags(NpcCharacter _npcCharacter)
		{
			EHidingSpotFlags hidingSpotInspectionMask = _npcCharacter.NpcDefinition.Definition.HidingSpotInspectionMask;
			hidingSpotInspectionMask = (EHidingSpotFlags)((int)hidingSpotInspectionMask & ~LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_GlitchHidingSpots));
			if (hidingSpotInspectionMask == EHidingSpotFlags.None)
			{
				return EHidingSpotFlags.None;
			}
			NavMeshHit hit;
			if (NavMesh.SamplePosition(_npcCharacter.Position, out hit, 1f, -1))
			{
				if ((hit.mask & NavMeshLayers.GroundSurfaceLayerMask) != 0)
				{
					hidingSpotInspectionMask &= EHidingSpotFlags.Haystack | EHidingSpotFlags.Bench | EHidingSpotFlags.Crowd | EHidingSpotFlags.StalkingZone;
				}
				else if ((hit.mask & NavMeshLayers.RoofSurfaceLayerMask) != 0)
				{
					hidingSpotInspectionMask &= EHidingSpotFlags.RooftopGarden;
				}
			}
			return hidingSpotInspectionMask;
		}

		private bool CollectHidingObjectInRadius<TSpotType>(float _range, EntityTypes.EType _type, Predicate<TSpotType> _predicate = null) where TSpotType : GameEntity
		{
			bool result = false;
			Vector3 position = base.NpcCharacter.Position;
			IEnumerable enumerable = ((_type != EntityTypes.EType.StalkingZone) ? LogicMain.EntityManager.CollectEntitiesInRadius<TSpotType>(position, _range) : UbiCore.Utils.Singleton<HotspotService>.Instance.StalkingZoneManager.CollectStalkingZonesInRadius(position, _range));
			foreach (TSpotType item in enumerable)
			{
				if (_predicate == null || _predicate(item))
				{
					base.NpcRuntimeData.Chase.m_hidingSpotsInRange.Add(item);
					result = true;
				}
			}
			return result;
		}

		private void RestartAlarmedActivity()
		{
			EnemyCharacter enemy = base.NpcCharacter as EnemyCharacter;
			AiHelper.RestartAlarmedActivity(enemy);
		}

		private int SortByDistance(GameEntity _x, GameEntity _y)
		{
			Vector3 position = base.NpcCharacter.Position;
			float num = _x.Position.DistanceXZSquared(position);
			float num2 = _y.Position.DistanceXZSquared(position);
			return (int)(num - num2);
		}

		private bool PatrolIsCover(Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			return _patrol.PatrolDefinition.Definition.InteractionReason == EInteractionReason.HidingInPatrol;
		}
	}
}
