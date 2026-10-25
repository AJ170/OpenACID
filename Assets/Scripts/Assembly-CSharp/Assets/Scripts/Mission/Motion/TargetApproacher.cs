using System;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion
{
	public class TargetApproacher
	{
		public enum EStatus
		{
			Idle = 0,
			Running = 1,
			Failed = 2,
			Arrived = 3
		}

		[Flags]
		public enum EModeFlags
		{
			InDistance = 0,
			InWalkableDistance = 1,
			AtRoofEdge = 2,
			MinHeight = 4,
			AllowObstacleInPlainSight = 8,
			ThrowRaycast = 0x10
		}

		private const float ApproachLinkDistance = 1.5f;

		private const float ApproachLinkDistanceSqr = 2.25f;

		private const float kMaxOffMeshLinkInaccuracy = 0.1f;

		private EntityId m_sourceEntityId = EntityId.InvalidId;

		private EntityId m_targetEntityId = EntityId.InvalidId;

		private Vector3 m_targetPosition;

		private InteractionLocators.Locator m_targetLocator;

		private EStatus m_status;

		private bool m_wasInParcour;

		private OffMeshLinkEntity m_ApproachedLink;

		private Vector3 m_ApproachedLinkPos;

		private WeaponHandler.WeaponDesc m_rangedWeapon;

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private NavMeshPath m_pathToTarget__lazy;

		private NavMeshPath m_pathToTarget
		{
			get
			{
				return m_pathToTarget__lazy ?? (m_pathToTarget__lazy = new NavMeshPath());
			}
		}

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private NavMeshPath m_pathToLink__lazy;

		private NavMeshPath m_pathToLink
		{
			get
			{
				return m_pathToLink__lazy ?? (m_pathToLink__lazy = new NavMeshPath());
			}
		}

		private readonly SimpleGameTimer m_updateTimer = new SimpleGameTimer();

		private static readonly BBList<OffMeshLinkManager.OffMeshLinkOctreeEntry> s_offMeshLinkBuffer = new BBList<OffMeshLinkManager.OffMeshLinkOctreeEntry>(50);

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private static NavMeshPath s_navMeshPath__lazy;

		private static NavMeshPath s_navMeshPath
		{
			get
			{
				return s_navMeshPath__lazy ?? (s_navMeshPath__lazy = new NavMeshPath());
			}
		}

		private static NavMeshHit s_dummyHit;

		private static RaycastHit s_dummyRayHit;

		public EntityId SourceEntityId
		{
			get
			{
				return m_sourceEntityId;
			}
		}

		public EntityId TargetEntityId
		{
			get
			{
				return m_targetEntityId;
			}
		}

		public Vector3 TargetPosition
		{
			get
			{
				return m_targetPosition;
			}
		}

		public InteractionLocators.Locator TargetLocator
		{
			get
			{
				return m_targetLocator;
			}
		}

		public EModeFlags ModeFlags { get; set; }

		public float UpdateInterval { get; set; }

		public Vector3 ActualTargetPos
		{
			get
			{
				return (m_targetLocator != null) ? m_targetLocator.Position.transform.position : TryGetEntityApproachPosition();
			}
		}

		public EMovementSpeed SpeedType { get; set; }

		public float SpeedValue { get; set; }

		public float ApproachDistanceSqr { get; set; }

		public float AbortDistanceSqr { get; set; }

		public float ApproachHeight { get; set; }

		public EStartIdleAnimAfterTaskPolicy StartIdleAnimAfterTaskPolicy { get; set; }

		public bool RetainPathOnCancel { get; set; }

		public NavMeshPath PathToTarget
		{
			get
			{
				return m_pathToTarget;
			}
		}

		public EStatus Status
		{
			get
			{
				return m_status;
			}
		}

		public bool HasArrived
		{
			get
			{
				return Status == EStatus.Arrived;
			}
		}

		public bool IsTargetReachable
		{
			get
			{
				Character entity = m_sourceEntityId.GetEntity<Character>();
				return (entity.Position - ActualTargetPos).sqrMagnitude < AbortDistanceSqr && CalculatePath(entity, s_navMeshPath, ActualTargetPos, true, entity.GetAutoPassableNavMeshLayerMask()) && s_navMeshPath.status == NavMeshPathStatus.PathComplete;
			}
		}

		public TargetApproacher()
		{
			UpdateInterval = 0f;
			Reset();
		}

		public void SetData(EntityId _source, EntityId _target, EMovementSpeed _speed, float _distanceSqr, float _abortDistanceSqr = float.MaxValue)
		{
			Reset();
			m_sourceEntityId = _source;
			m_targetEntityId = _target;
			ApproachDistanceSqr = _distanceSqr;
			AbortDistanceSqr = _abortDistanceSqr;
			SpeedType = _speed;
			SpeedValue = 0f;
		}

		public void SetData(EntityId _source, Vector3 _target, EMovementSpeed _speed, float _distanceSqr)
		{
			Reset();
			m_sourceEntityId = _source;
			m_targetPosition = _target;
			m_targetEntityId = EntityId.InvalidId;
			ApproachDistanceSqr = _distanceSqr;
			AbortDistanceSqr = float.MaxValue;
			SpeedType = _speed;
			SpeedValue = 0f;
		}

		public void SetData(EntityId _source, InteractionLocators.Locator _target, EMovementSpeed _speed, float _distanceSqr)
		{
			Reset();
			m_sourceEntityId = _source;
			m_targetLocator = _target;
			ApproachDistanceSqr = _distanceSqr;
			AbortDistanceSqr = float.MaxValue;
			SpeedType = _speed;
			SpeedValue = 0f;
		}

		public void SetData(EntityId _source, Vector3 _target, float _speedValue, float _distanceSqr)
		{
			Reset();
			m_sourceEntityId = _source;
			m_targetPosition = _target;
			m_targetEntityId = EntityId.InvalidId;
			ApproachDistanceSqr = _distanceSqr;
			AbortDistanceSqr = float.MaxValue;
			SpeedType = EMovementSpeed.Invalid_EMovementSpeed;
			SpeedValue = _speedValue;
		}

		public void SetData(EntityId _source, Vector3 _target, float _speedValue, float _distanceSqr, float _abortDistance)
		{
			Reset();
			m_sourceEntityId = _source;
			m_targetPosition = _target;
			m_targetEntityId = EntityId.InvalidId;
			ApproachDistanceSqr = _distanceSqr;
			AbortDistanceSqr = _abortDistance;
			SpeedType = EMovementSpeed.Invalid_EMovementSpeed;
			SpeedValue = _speedValue;
		}

		public void SetTarget(EntityId _target)
		{
			m_targetPosition = Vector3.zero;
			m_targetLocator = null;
			m_targetEntityId = _target;
			m_wasInParcour = false;
			m_ApproachedLink = null;
			m_pathToTarget.ClearCorners();
			m_pathToLink.ClearCorners();
		}

		public void SetTarget(Vector3 _target)
		{
			m_targetPosition = _target;
			m_targetLocator = null;
			m_targetEntityId = EntityId.InvalidId;
			m_wasInParcour = false;
			m_ApproachedLink = null;
			m_pathToTarget.ClearCorners();
			m_pathToLink.ClearCorners();
		}

		public void SetTarget(InteractionLocators.Locator _target)
		{
			m_targetPosition = Vector3.zero;
			m_targetLocator = _target;
			m_targetEntityId = EntityId.InvalidId;
			m_wasInParcour = false;
			m_ApproachedLink = null;
			m_pathToTarget.ClearCorners();
			m_pathToLink.ClearCorners();
		}

		public void Approach()
		{
			m_pathToTarget.ClearCorners();
			m_pathToLink.ClearCorners();
			m_wasInParcour = false;
			m_ApproachedLink = null;
			if (IsTargetReachable)
			{
				StartTimer();
				m_status = EStatus.Running;
				Character entity = m_sourceEntityId.GetEntity<Character>();
				ICharacterDrawRangedWeaponCarrier characterDrawRangedWeaponCarrier = entity.StateMachines as ICharacterDrawRangedWeaponCarrier;
				if (characterDrawRangedWeaponCarrier == null || characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon == null)
				{
					return;
				}
				CharacterDrawWeaponRanged characterDrawRangedWeapon = characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon;
				WeaponHandler weaponHandler = characterDrawRangedWeapon.GetWeaponHandler();
				if (weaponHandler != null)
				{
					m_rangedWeapon = weaponHandler.CurrentWeaponDesc;
					if (m_rangedWeapon != null && characterDrawRangedWeapon.ChosenWeaponState == EWeaponState.Unsheathed)
					{
						entity.ChooseWeaponState(characterDrawRangedWeapon.WeaponCatgory, EWeaponState.Sheathed);
					}
				}
			}
			else
			{
				m_status = EStatus.Failed;
			}
		}

		public bool Update(bool _forceRecomputePath = false)
		{
			if (m_status != EStatus.Running)
			{
				return false;
			}
			if (m_sourceEntityId.GetEntity<Character>().GetDeadOrAboutToDie())
			{
				Reset();
				return false;
			}
			m_status = ApproachTarget(m_pathToTarget.corners.Length == 0 || !m_updateTimer.IsRunning || m_updateTimer.IsFinished || _forceRecomputePath);
			if (m_status == EStatus.Running)
			{
				if (m_updateTimer.IsFinished)
				{
					StartTimer();
				}
			}
			else
			{
				m_updateTimer.Reset();
			}
			return m_status == EStatus.Running;
		}

		public void Reset()
		{
			ModeFlags = EModeFlags.InDistance;
			m_sourceEntityId = EntityId.InvalidId;
			m_targetEntityId = EntityId.InvalidId;
			m_targetPosition = Vector3.zero;
			m_targetLocator = null;
			ApproachDistanceSqr = float.MaxValue;
			AbortDistanceSqr = float.MaxValue;
			ApproachHeight = 0f;
			StartIdleAnimAfterTaskPolicy = EStartIdleAnimAfterTaskPolicy.DontCare;
			m_updateTimer.Reset();
			m_status = EStatus.Idle;
			SpeedType = EMovementSpeed.Run;
			SpeedValue = -1f;
			m_pathToTarget.ClearCorners();
			m_pathToLink.ClearCorners();
			m_wasInParcour = false;
			m_ApproachedLink = null;
			m_rangedWeapon = null;
		}

		public bool CheckIsRunning()
		{
			if (Status != EStatus.Running)
			{
				return false;
			}
			return !CheckArrived(ActualTargetPos, ApproachDistanceSqr, ModeFlags);
		}

		private EStatus ApproachTarget(bool _forceRepathing)
		{
			if (CheckArrived(ActualTargetPos, ApproachDistanceSqr, ModeFlags))
			{
				return EStatus.Arrived;
			}
			EStatus eStatus = EStatus.Failed;
			Character entity = m_sourceEntityId.GetEntity<Character>();
			if (entity.IsAttached)
			{
				AnimationController animationController = entity.UnityComponentCache.AnimationController;
				IStateMachine mainStateMachine = entity.GetMainStateMachine();
				bool flag = animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) is IParkourAnimation || (mainStateMachine != null && mainStateMachine.CurrentStateType == EStateType.CombatCharacterMain_Parcour);
				if (!flag && m_rangedWeapon != null && m_rangedWeapon.UnsheathedAnimStateMachine != null)
				{
					AnimatorStateMachineStatic.StateMachine currentStateMachine = animationController.StateMachine.GetCurrentStateMachine();
					flag = currentStateMachine != null && currentStateMachine.CompareNameHash(m_rangedWeapon.UnsheathedAnimStateMachine);
				}
				if (!flag)
				{
					bool flag2 = _forceRepathing || m_wasInParcour;
					if (flag2)
					{
						m_ApproachedLink = null;
					}
					if (CalculatePath(m_pathToTarget, ActualTargetPos, flag2))
					{
						Vector3 linkPosition = Vector3.zero;
						GameObject gameObject = null;
						gameObject = FindLink(out linkPosition);
						if (gameObject != null || m_ApproachedLink != null)
						{
							EnemyCharacter enemyCharacter = entity as EnemyCharacter;
							if (enemyCharacter == null || !enemyCharacter.GetIsRangedUnsheathedOrInAnim())
							{
								m_ApproachedLink = ((!(gameObject == null)) ? UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.GetEntity(gameObject) : m_ApproachedLink);
								if (gameObject != null)
								{
									m_ApproachedLinkPos = linkPosition;
								}
								AnimationController animationController2 = animationController;
								EStatus eStatus2 = FollowPath(m_ApproachedLinkPos, 2.25f, true, ModeFlags & ~EModeFlags.InWalkableDistance, EStartIdleAnimAfterTaskPolicy.No, true, NavMeshLayers.WalkableLayerMaskPreset, m_pathToLink);
								if (eStatus2 == EStatus.Arrived)
								{
									m_pathToLink.ClearCorners();
									switch (m_ApproachedLink.LinkType)
									{
									case eLinkType.Climb:
										animationController2.TaskManager.CreateAnimationTaskClimb(m_ApproachedLink);
										break;
									case eLinkType.JumpDown:
										animationController2.TaskManager.CreateAnimationTaskJumpDown(m_ApproachedLink);
										break;
									case eLinkType.Balance:
										animationController2.TaskManager.CreateAnimationTaskBalance(m_ApproachedLink);
										break;
									case eLinkType.JumpHorizontal:
										animationController2.TaskManager.CreateAnimationTaskJumpHorizontal(m_ApproachedLink);
										break;
									case eLinkType.RunPassOver:
										animationController2.TaskManager.CreateAnimationTaskRunPassOver(m_ApproachedLink);
										break;
									case eLinkType.RunPassOverDown:
										animationController2.TaskManager.CreateAnimationTaskRunPassOverDown(m_ApproachedLink);
										break;
									case eLinkType.ParkourSequence:
										animationController2.TaskManager.CreateAnimationTaskParkourSequence(m_ApproachedLink);
										break;
									}
									m_wasInParcour = true;
									IEnemyMainCarrier enemyMainCarrier = entity.StateMachines as IEnemyMainCarrier;
									if (enemyMainCarrier != null)
									{
										EnterParcour(enemyMainCarrier.EnemyMain, animationController2);
									}
									else
									{
										IPlayerMainCarrier playerMainCarrier = entity.StateMachines as IPlayerMainCarrier;
										if (playerMainCarrier != null)
										{
											PlayerMain playerMain = playerMainCarrier.PlayerMain;
											PlayerMain.ParcourState.ParcourPayload payload = playerMain.GetPayload<PlayerMain.ParcourState.ParcourPayload>(EStateType.CombatCharacterMain_Parcour);
											payload.ParcourDestination = m_ApproachedLink.EndPosition;
											EnterParcour(playerMain, animationController2);
										}
									}
									m_ApproachedLink = null;
								}
							}
							eStatus = EStatus.Running;
						}
						else
						{
							eStatus = FollowPath(ActualTargetPos, ApproachDistanceSqr, _forceRepathing, ModeFlags, StartIdleAnimAfterTaskPolicy, RetainPathOnCancel);
							m_wasInParcour = false;
						}
					}
					else
					{
						eStatus = EStatus.Failed;
					}
				}
				else
				{
					eStatus = EStatus.Running;
				}
			}
			else
			{
				eStatus = FollowPath(ActualTargetPos, ApproachDistanceSqr, _forceRepathing, ModeFlags, StartIdleAnimAfterTaskPolicy, RetainPathOnCancel);
				m_wasInParcour = false;
			}
			return eStatus;
		}

		private GameObject FindLink(out Vector3 linkPosition)
		{
			GameObject gameObject = null;
			linkPosition = Vector3.zero;
			s_offMeshLinkBuffer.Clear();
			int num = 0;
			while (gameObject == null && num < m_pathToTarget.corners.Length)
			{
				UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.FindOffMeshLinks(m_pathToTarget.corners[num], 0.1f, s_offMeshLinkBuffer);
				if (s_offMeshLinkBuffer.Count > 0)
				{
					int num2 = 0;
					while (gameObject == null && num2 < s_offMeshLinkBuffer.Count)
					{
						OffMeshLink offMeshLink = s_offMeshLinkBuffer[num2].OffMeshLink;
						Vector3 position = offMeshLink.startTransform.position;
						OffMeshLinkManager offMeshLinkManager = UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager;
						if (LinkPosClose(position, m_pathToTarget.corners[num]))
						{
							if (num < m_pathToTarget.corners.Length - 1 && LinkPosClose(offMeshLink.endTransform.position, m_pathToTarget.corners[num + 1]) && !offMeshLinkManager.IsOffMeshLinkBlocked(offMeshLink))
							{
								gameObject = s_offMeshLinkBuffer[num2].Link;
								linkPosition = position;
							}
						}
						else
						{
							position = offMeshLink.endTransform.position;
							if (offMeshLink.biDirectional && LinkPosClose(position, m_pathToTarget.corners[num]) && num < m_pathToTarget.corners.Length - 1 && LinkPosClose(offMeshLink.startTransform.position, m_pathToTarget.corners[num + 1]) && !offMeshLinkManager.IsOffMeshLinkBlocked(offMeshLink, true))
							{
								gameObject = s_offMeshLinkBuffer[num2].Link;
								linkPosition = position;
							}
						}
						num2++;
					}
				}
				num++;
			}
			return gameObject;
		}

		private static bool LinkPosClose(Vector3 _a, Vector3 _b)
		{
			return (_a - _b).sqrMagnitude < 0.010000001f;
		}

		private EMovementSpeed ChooseSpeedFromProfile()
		{
			if (SpeedType == EMovementSpeed.Invalid_EMovementSpeed)
			{
				IPlayerProfileCarrierEntity playerProfileCarrierEntity = SourceEntityId.TryGetEntity<IPlayerProfileCarrierEntity>();
				if (Assets.Scripts.Engine.Helpers.Verify(playerProfileCarrierEntity != null))
				{
					return playerProfileCarrierEntity.PlayerProfileCarrier.PlayerProfile.CurrentStateType.ToPlayerSpeed(playerProfileCarrierEntity as IPlayerStatsCarrier);
				}
			}
			return SpeedType;
		}

		private bool CheckArrived(Vector3 _targetPos, float _approachDistanceSqr, EModeFlags _modeFlags)
		{
			Character entity = SourceEntityId.GetEntity<Character>();
			Vector3 vector = entity.Position - _targetPos;
			float num = ((_approachDistanceSqr != float.MaxValue) ? _approachDistanceSqr : 0.5f);
			bool flag = false;
			if (entity.IsAttached)
			{
				Character entity2 = m_sourceEntityId.GetEntity<Character>();
				AnimationController animationController = entity2.UnityComponentCache.AnimationController;
				flag = !(animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) is IParkourAnimation);
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				flag = vector.sqrMagnitude <= num;
			}
			if (flag && (_modeFlags & EModeFlags.InWalkableDistance) == EModeFlags.InWalkableDistance)
			{
				flag = CalculatePath(s_navMeshPath, ActualTargetPos, true, NavMeshLayers.WalkableLayerMaskPreset) && s_navMeshPath.status == NavMeshPathStatus.PathComplete && (double)s_navMeshPath.CalculateLength() <= Math.Sqrt(num);
			}
			if (flag && (_modeFlags & EModeFlags.AtRoofEdge) == EModeFlags.AtRoofEdge)
			{
				flag = ParcourHelper.IsAtRoofEdge(entity);
			}
			if (flag && (_modeFlags & EModeFlags.MinHeight) == EModeFlags.MinHeight)
			{
				flag = vector.y >= ApproachHeight;
			}
			if (flag && (_modeFlags & EModeFlags.AllowObstacleInPlainSight) == 0)
			{
				flag = !NavMesh.Raycast(entity.Position, _targetPos, out s_dummyHit, NavMeshLayers.WalkableLayerMaskPreset);
			}
			if (flag && (_modeFlags & EModeFlags.ThrowRaycast) == EModeFlags.ThrowRaycast)
			{
				Vector3 vector2 = _targetPos + new Vector3(0f, 1.15f, 0f);
				Vector3 vector3 = ((!entity.IsAttached) ? (entity.Position + new Vector3(0f, 1.15f, 0f)) : entity.UnityComponentCache.Animator.GetBoneTransform(HumanBodyBones.Head).position);
				Vector3 vector4 = new Vector3(vector2.x - vector3.x, 0f, vector2.z - vector3.z);
				float magnitude = vector4.magnitude;
				Vector3 vector5 = vector3;
				if (magnitude > 0.01f)
				{
					vector4 /= magnitude;
					magnitude = 1f;
					vector4 *= magnitude;
					vector5 = vector3 + vector4;
					flag = !Physics.Raycast(vector3, vector4, out s_dummyRayHit, magnitude, Layers.StaticCollidersLayersMaskPreset);
				}
				if (flag)
				{
					vector4 = vector2 - vector5;
					magnitude = vector4.magnitude;
					if (magnitude > 0.01f)
					{
						flag = !Physics.Raycast(vector5, vector4 / magnitude, out s_dummyRayHit, magnitude, Layers.StaticCollidersLayersMaskPreset);
					}
				}
			}
			return flag;
		}

		private EStatus FollowPath(Vector3 _targetPos, float _approachDistanceSqr, bool _forceRepathing, EModeFlags _modeFlags, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, bool _retainPathOnCancel)
		{
			Character entity = SourceEntityId.GetEntity<Character>();
			return FollowPath(_targetPos, _approachDistanceSqr, _forceRepathing, _modeFlags, _startIdleWhenFinishedPolicy, _retainPathOnCancel, entity.GetAutoPassableNavMeshLayerMask(), m_pathToTarget);
		}

		private EStatus FollowPath(Vector3 _targetPos, float _approachDistanceSqr, bool _forceRepathing, EModeFlags _modeFlags, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, bool _retainPathOnCancel, int _layerMask, NavMeshPath _path)
		{
			if (CheckArrived(_targetPos, _approachDistanceSqr, _modeFlags))
			{
				return EStatus.Arrived;
			}
			bool flag = CalculatePath(_path, _targetPos, _forceRepathing, _layerMask) && _path.status == NavMeshPathStatus.PathComplete;
			if (flag)
			{
				Character entity = SourceEntityId.GetEntity<Character>();
				if (SpeedValue > 0f)
				{
					entity.StartFollowPath(SpeedValue, _path, _retainPathOnCancel, _startIdleWhenFinishedPolicy, _approachDistanceSqr, _layerMask);
				}
				else
				{
					EMovementSpeed speed = ChooseSpeedFromProfile();
					entity.StartFollowPath(speed, _path, _retainPathOnCancel, _startIdleWhenFinishedPolicy, _approachDistanceSqr, _layerMask);
				}
			}
			return flag ? EStatus.Running : EStatus.Failed;
		}

		private static bool CalculatePath(Character _character, NavMeshPath _path, Vector3 _targetPos, bool _recompute, int _layerMask)
		{
			if (_recompute)
			{
				if (!NavigationHelper.CascadingSamplePosition(_targetPos, out s_dummyHit, _layerMask))
				{
					return false;
				}
				_targetPos = s_dummyHit.position;
				if (NavMesh.CalculatePath(_character.Position, _targetPos, _layerMask, _path))
				{
					return true;
				}
				if (!NavigationHelper.CascadingSamplePosition(_character.Position, out s_dummyHit, _layerMask))
				{
					return false;
				}
				return NavMesh.CalculatePath(s_dummyHit.position, _targetPos, _layerMask, _path);
			}
			return _path.status == NavMeshPathStatus.PathComplete;
		}

		private bool CalculatePath(NavMeshPath _path, Vector3 _targetPos, bool _recompute)
		{
			Character entity = m_sourceEntityId.GetEntity<Character>();
			return CalculatePath(entity, _path, _targetPos, _recompute, entity.GetAutoPassableNavMeshLayerMask());
		}

		private bool CalculatePath(NavMeshPath _path, Vector3 _targetPos, bool _recompute, int _layerMask)
		{
			Character entity = m_sourceEntityId.GetEntity<Character>();
			return CalculatePath(entity, _path, _targetPos, _recompute, _layerMask);
		}

		private void StartTimer()
		{
			if (UpdateInterval > 0f)
			{
				m_updateTimer.Start(UpdateInterval);
			}
			else
			{
				m_updateTimer.Reset();
			}
		}

		private static void EnterParcour<TEntity, TStateMachine>(StateMachineBase<TEntity, TStateMachine> _mainStatemachine, AnimationController _animController) where TEntity : IGameEntity, IStateMachinesCarrier where TStateMachine : StateMachineBase<TEntity, TStateMachine>
		{
			if (_mainStatemachine.CurrentStateType == EStateType.PlayerMain_HidingInPatrol)
			{
				_mainStatemachine.EnterCallbackDelayState(EStateType.CombatCharacterMain_Parcour, EStateType.CombatCharacterMain_Roaming, EStateType.CombatCharacterMain_Roaming, _animController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base), AnimationTaskManager.ELayer.Base, true);
			}
			else
			{
				_mainStatemachine.EnterCallbackDelayState(EStateType.CombatCharacterMain_Parcour, _mainStatemachine.CurrentStateType, _mainStatemachine.CurrentStateType, _animController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base), AnimationTaskManager.ELayer.Base, true);
			}
		}

		private Vector3 TryGetEntityApproachPosition()
		{
			if (m_targetEntityId.IsValid)
			{
				IPositionEntity positionEntity = m_targetEntityId.TryGetEntity<IPositionEntity>();
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = positionEntity as Assets.Scripts.Mission.Logic.Entities.Patrol;
				Vector3 result = positionEntity.Position;
				if (patrol != null)
				{
					result = patrol.PatrolComponent.Formation.CalcCurrentMemberCenter();
				}
				return result;
			}
			return m_targetPosition;
		}
	}
}
