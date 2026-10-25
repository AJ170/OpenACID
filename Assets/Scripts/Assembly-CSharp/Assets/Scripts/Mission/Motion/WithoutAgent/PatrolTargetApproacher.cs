using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UbiCore;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.WithoutAgent
{
	public class PatrolTargetApproacher
	{
		public const float DefaultAdvantageToMembers = 2f;

		public const float MinDotForVerticalWarp = 0.9f;

		private const float SpeedFactorTargetApproach = 0.5f;

		private const float SpeedFactorMemberDeviation = 0.25f;

		private const float NoSpeedDeviationThreshold = 1.98f;

		private readonly NavMeshPath m_dummyPath = new NavMeshPath();

		public Assets.Scripts.Mission.Logic.Entities.Patrol PatrolEntity { get; private set; }

		public IFormation Formation
		{
			get
			{
				return PatrolEntity.PatrolComponent.Formation;
			}
		}

		public float Speed { get; protected set; }

		public float ReferenceSpeed { get; set; }

		public float MinMemberDeviationSqr { get; private set; }

		public PatrolTargetApproacher(Assets.Scripts.Mission.Logic.Entities.Patrol _patrolEntity)
		{
			PatrolEntity = _patrolEntity;
		}

		public void DoTurnUpdate(bool _speedBasedOnDistanceToWaypoint, bool _forcePathRecalculation, out bool _closeToTarget, out bool _hasAttachedMembers, ref float _deltaTime)
		{
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = PatrolEntity.PatrolComponent;
			Assets.Scripts.Mission.Logic.Components.Patrol.Waypoint currentWaypoint = patrolComponent.CurrentWaypoint;
			Vector3 position = currentWaypoint.Position;
			Vector3 _position = PatrolEntity.Position;
			float _rotationXZ = PatrolEntity.Rotation.eulerAngles.y;
			Vector3 vector = position - _position;
			bool _aMemberIsStuck = false;
			if (currentWaypoint.WarpTarget || Mathf.Abs(Vector3.Dot(Vector3.up, vector.normalized)) > 0.9f)
			{
				PatrolEntity.Position = position;
				_hasAttachedMembers = UpdateMembers(_forcePathRecalculation, out _aMemberIsStuck);
				_closeToTarget = true;
			}
			else
			{
				_hasAttachedMembers = UpdateMembers(_forcePathRecalculation, out _aMemberIsStuck);
				float magnitude = vector.magnitude;
				CalculateSpeed(magnitude, _hasAttachedMembers, _speedBasedOnDistanceToWaypoint, _aMemberIsStuck);
				_closeToTarget = Helper.InterpolatePosAndRotation(Speed, position, ref _position, ref _rotationXZ, ref _deltaTime);
				PatrolEntity.Position = _position;
				if (magnitude < 2f && !MathHelper.Approximately(currentWaypoint.IdleTime, 0f))
				{
					_rotationXZ = currentWaypoint.Rotation.eulerAngles.y;
				}
			}
			PatrolEntity.Rotation = Quaternion.Euler(0f, _rotationXZ, 0f);
		}

		private void UpdateFrameTarget(EntityId _entityId)
		{
			Vector3 _position;
			Formation.GetTargetWorldPosition(_entityId, out _position);
			((NpcCharacter)_entityId.Entity).NavAgentSurrogate.UpdateFrameTarget(_position, PatrolEntity.Rotation.eulerAngles.y, Speed);
		}

		public void PerformMemberAttach(EntityId _id)
		{
			MemberInfo _memberInfo;
			bool memberInfo = PatrolEntity.PatrolComponent.Formation.GetMemberInfo(_id, out _memberInfo);
			PatrolHelper.AttachPatrol(_id.TryGetEntity<NpcCharacter>(), m_dummyPath, _memberInfo.m_targetApproacher);
		}

		private bool UpdateMembers(bool _forcePathRecalculation, out bool _aMemberIsStuck)
		{
			MinMemberDeviationSqr = float.MaxValue;
			bool flag = false;
			_aMemberIsStuck = false;
			BBList<MemberInfo> entities = Formation.Entities;
			int count = entities.Count;
			for (int i = 0; i < count; i++)
			{
				MemberInfo memberInfo = entities[i];
				EntityId id = memberInfo.m_id;
				NpcCharacter npcCharacter = id.GameEntity as NpcCharacter;
				if (npcCharacter.IsAttached)
				{
					flag = true;
					Vector3 _position;
					Formation.GetTargetWorldPosition(id, out _position);
					float sqrMagnitude = (_position - npcCharacter.Position).sqrMagnitude;
					if (sqrMagnitude < MinMemberDeviationSqr)
					{
						MinMemberDeviationSqr = sqrMagnitude;
					}
					PatrolHelper.Patrol(npcCharacter, m_dummyPath, memberInfo.m_targetApproacher, _forcePathRecalculation, ref memberInfo.m_idling);
					bool flag2 = memberInfo.m_targetApproacher.Status == TargetApproacher.EStatus.Failed;
					_aMemberIsStuck |= flag2;
				}
				else
				{
					UpdateFrameTarget(id);
				}
			}
			if (!flag)
			{
				MinMemberDeviationSqr = 0f;
			}
			if (Formation.EntityAtHidingSpot != EntityId.InvalidId)
			{
				MemberInfo entityAtHidingSpotInfo = Formation.EntityAtHidingSpotInfo;
				AssassinCharacter player = entityAtHidingSpotInfo.m_id.Entity as AssassinCharacter;
				PatrolHelper.Patrol(player, PatrolEntity, m_dummyPath, entityAtHidingSpotInfo.m_targetApproacher, ref entityAtHidingSpotInfo.m_idling);
			}
			return flag;
		}

		private void CalculateSpeed(float _distMagnitude, bool _hasAttachedMembers, bool _speedBasedOnDistanceToWaypoint, bool _aMemberIsStuck)
		{
			if (_speedBasedOnDistanceToWaypoint)
			{
				CalculateSpeedByDeviation(_distMagnitude, 0.5f);
			}
			else if (_hasAttachedMembers && !_aMemberIsStuck)
			{
				CalculateSpeedByMembers();
			}
			else
			{
				Speed = ReferenceSpeed;
			}
		}

		private void CalculateSpeedByMembers()
		{
			float num = Mathf.Sqrt(MinMemberDeviationSqr);
			float num2 = 2f * (float)PatrolEntity.PatrolComponent.TurnUpdateIntervalTurns;
			float num3 = Mathf.Clamp(num2 - num, -2f, 2f);
			if (num3 <= -1.98f)
			{
				Speed = 0f;
			}
			else
			{
				CalculateSpeedByDeviation(num3, 0.25f);
			}
		}

		public void CalculateSpeedByDeviation(float _deviation, float _distanceSpeedFactor)
		{
			float num = Mathf.Exp(_distanceSpeedFactor * _deviation);
			if (num >= 0f)
			{
				Speed = ReferenceSpeed * num;
			}
		}
	}
}
