using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Motion.Hotspots;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion
{
	public static class ParcourHelper
	{
		public enum EEnvironmentInteraction
		{
			Invalid = 0,
			Hotspot = 1,
			CivilianCover = 2,
			Link = 3,
			JumpForward = 4,
			JumpDown = 5,
			Rebound = 6,
			JumpHorizontalToWall = 7
		}

		public static class Acrobatics
		{
			public static float EdgeDistance = 1f;

			public static float EdgeAngle = 40f;

			public static float MaxLinkHeightDifference = 0.75f;

			public static bool IgnoreJumpDownLinks = true;

			public static float HideoutDirectTriggerDistanceInside = -0.5f;

			public static float HideoutDirectTriggerDistanceOutside = 2f;

			public static float HideoutDirectTriggerExtraOffset = 0.2f;

			public static float HideoutDirectTriggerMaxDeltaY = 1f;

			public static float LinkOffsetFromEdge = 1f;

			public static float LinkSearchRadius = 2f;

			public static float HotspotSearchRadius = 1f;

			public static float ClearRaycastOffset = 0.2f;

			public static float MinClearDistance = 2f;

			public static float MaxWallDistance = 1.2f;

			public static float ObstacleHeight = 0.4f;

			public static float WallHeight = 3f;

			public static float JumpNavMeshLookUpRadius = 0.25f;

			public static float JumpEdgeDistance = 0.5f;

			public static float JumpMaxLength = 9f;

			public static float JumpMaxFallHeightNoDamage = 12.4f;

			public static float JumpMaxFallHeightDamaged = 20f;

			public static float JumpSampleOffset = 0.25f;

			public static float JumpDownFallAngle = 70f;

			public static float JumpForwardFallAngle = 45f;

			public static float JumpForwardStraightLength = 6f;

			public static float JumpDownMinExtraWalkingDistance = 1f;

			public static float JumpDownMinExtraWalkingMultiplier = 1f;

			public static float JumpForwardMinExtraWalkingDistance = 4f;

			public static float JumpForwardMinExtraWalkingMultiplier = 1.3f;

			public static float JumpForwardMinHeightDelta = 2.5f;

			public static bool EnableJumpHorizontalToWall;

			public static float JumpHorizontalToWallStraightLength = 8f;

			public static float JumpHorizontalToWallMinStraightLength = 3f;

			public static float JumpHorizontalToWallMaxHeight = 6f;

			public static float JumpHorizontalToWallMaxLength = 12f;

			public static float JumpHorizontalToWallMaxAngle = 30f;

			public static float JumpHorizontalToWallEdgeLookupRadius = 1.5f;

			public static float JumpHorizontalToWallMaxHeightDifference = 1.5f;

			public static float JumpHorizontalToWallObstacleHeight = 1.5f;

			public static float JumpHorizontalToWallMinGapDepth = 3f;

			public static float JumpHorizontalToWallRaycastOffsetZ = 0.5f;

			public static float JumpHorizontalToWallRaycastOffsetY = 0.1f;

			public static float JumpHorizontalToWallRaycastOffsetX = 0.25f;
		}

		public static class CivilianCover
		{
			public const float SearchRadius = 1.1f;

			public const float MaxCurrentToPatrolDistSqr = 2.25f;
		}

		public static bool Debugging = false;

		public static float RelativeDirectionThreshold = 0.15f;

		public static readonly int s_physicalLayerMask = Layers.StaticCollidersLayersMaskPreset;

		public static EdgeObject JumpHorizontalToWallLastEdge = null;

		public static float ClearDistanceForward(Vector3 _position, Vector3 _forward, float maxDistance = 2f, int mask = -1)
		{
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(_position, _position + _forward * maxDistance, out _hit, mask))
			{
				return Mathf.Min(_hit.distance, maxDistance);
			}
			return maxDistance;
		}

		public static bool IsLeavingMissionArea(Vector3 _position, Vector3 _forward)
		{
			RaycastHit _hit;
			return CheckForAnimusWalls(_position, _forward, 1f, 0.5f, out _hit);
		}

		public static bool CheckForAnimusWalls(Vector3 _position, Vector3 _forward, float _backOffset, float _upOffset, out RaycastHit _hit)
		{
			Vector3 normalized = _forward.normalized;
			float maxDistance = _forward.magnitude + _backOffset;
			Vector3 origin = _position + Vector3.up * _upOffset - normalized;
			if (!Physics.Raycast(origin, normalized, out _hit, maxDistance, Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			if (Debugging)
			{
			}
			if (Vector3.Dot(_hit.normal, _hit.collider.transform.forward) > 0f)
			{
				return true;
			}
			return false;
		}

		public static eDirection ComputeRelativeDirection(Vector3 _targetDirection, Vector3 _referenceDirection)
		{
			_referenceDirection.y = 0f;
			_referenceDirection.Normalize();
			_targetDirection.y = 0f;
			_targetDirection.Normalize();
			float num = Vector3.Dot(_targetDirection, _referenceDirection);
			if (num > RelativeDirectionThreshold)
			{
				return eDirection.Up;
			}
			if (num < 0f - RelativeDirectionThreshold)
			{
				return eDirection.Down;
			}
			return eDirection.Invalid;
		}

		public static bool IsAtRoofEdge(Character _character)
		{
			Vector3 _roofNormal;
			float _roofHeight;
			return IsAtRoofEdge(_character, _character.Rotation * Vector3.forward, out _roofNormal, out _roofHeight);
		}

		public static bool IsAtRoofEdge(Character _character, Vector3 _direction, out Vector3 _roofNormal, out float _roofHeight, float _distance = 2f, float _minDepth = 1f, float _maxBalconyHeight = 1f)
		{
			float num = 0.3f;
			Vector3 vector = _character.Position - _direction * num;
			Vector3 vector2 = _character.Position + _direction * (_distance + num);
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(vector, vector2, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				Vector3 position = _hit.position;
				float num2 = 100f;
				vector += Vector3.up * _maxBalconyHeight;
				Vector3 vector3 = position + Vector3.up * _maxBalconyHeight;
				Vector3 vector4 = vector2 + Vector3.down * num2;
				Vector3 vector5 = vector4;
				vector5.y = vector3.y;
				_roofNormal = _hit.normal;
				if (!Physics.Raycast(vector, (vector3 - vector).normalized, (vector - vector3).magnitude, Layers.StaticCollidersLayersMaskPreset) && !Physics.Raycast(vector3, (vector5 - vector3).normalized, (vector5 - vector3).magnitude, Layers.StaticCollidersLayersMaskPreset))
				{
					RaycastHit hitInfo;
					bool flag = Physics.Raycast(vector5, (vector4 - vector5).normalized, out hitInfo, (vector4 - vector5).magnitude, Layers.StaticCollidersLayersMaskPreset);
					bool flag2 = flag && NavMesh.SamplePosition(hitInfo.point, out _hit, 1f, -1);
					_roofHeight = ((!flag) ? num2 : (hitInfo.distance - _maxBalconyHeight));
					return flag && flag2 && _roofHeight > _minDepth;
				}
			}
			_roofNormal = Vector3.zero;
			_roofHeight = 0f;
			return false;
		}

		public static bool CanLeapOfFaith(Character _character, Haystack _haystack, out Vector3 _edgePosition)
		{
			Vector3 position = _haystack.InteractionLocators.Center.transform.position;
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(_character.Position, position, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				Vector3 position2 = _hit.position;
				Vector3 v = position - position2;
				float num = 0f - v.y;
				float num2 = v.LengthXZ();
				float num3 = Mathf.Lerp(10f, 20f, Mathf.Clamp01((num - 5f) / 45f));
				if (num > 2f && num2 < num3)
				{
					Vector3 vector = position2 + Vector3.up * Acrobatics.ObstacleHeight;
					Vector3 vector2 = position + Vector3.up * 2f;
					Vector3 vector3 = Vector3.Lerp(vector, vector2, 0.6f);
					vector3.y = vector.y;
					if (!Physics.Raycast(vector, (vector3 - vector).normalized, (vector3 - vector).magnitude, Layers.StaticCollidersLayersMaskPreset) && !Physics.Raycast(vector3, (vector2 - vector3).normalized, (vector2 - vector3).magnitude, Layers.StaticCollidersLayersMaskPreset))
					{
						_edgePosition = position2;
						return true;
					}
				}
			}
			_edgePosition = _character.Position;
			return false;
		}

		public static bool IsJumpPathClear(Vector3 _currentPosition, Vector3 _targetPosition)
		{
			Vector3 vector = _currentPosition + Acrobatics.ObstacleHeight * Vector3.up;
			Vector3 vector2 = _targetPosition - _currentPosition;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			Vector3 normalized = vector2.normalized;
			Vector3 vector3 = vector + normalized * magnitude;
			Vector3 vector4 = _targetPosition + Vector3.up * 0.2f;
			if (Physics.Raycast(vector, (vector3 - vector).normalized, (vector3 - vector).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			if (Physics.Raycast(vector3, (vector4 - vector3).normalized, (vector4 - vector3).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			return true;
		}

		public static bool CanJumpToPoint(Vector3 _currentPosition, Vector3 _targetPosition, bool _checkPath = true)
		{
			Vector3 vector = _currentPosition + Acrobatics.ObstacleHeight * Vector3.up;
			Vector3 vector2 = _targetPosition - _currentPosition;
			float num = 0f - vector2.y;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			if (num < 0f || num > Acrobatics.JumpMaxFallHeightDamaged || magnitude > Acrobatics.JumpMaxLength)
			{
				return false;
			}
			float num2 = Acrobatics.JumpForwardStraightLength + num / Mathf.Atan(Acrobatics.JumpForwardFallAngle * ((float)Math.PI / 180f));
			if (magnitude > num2)
			{
				return false;
			}
			NavMeshHit _hit;
			if (!NavigationHelper.SafeRaycast(_currentPosition, _targetPosition, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return false;
			}
			Vector3 normalized = vector2.normalized;
			Vector3 origin = vector + normalized * magnitude;
			RaycastHit hitInfo;
			if (!Physics.Raycast(origin, Vector3.down, out hitInfo, num + Acrobatics.ObstacleHeight + 0.5f, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			if (!NavMesh.SamplePosition(hitInfo.point, out _hit, Acrobatics.JumpNavMeshLookUpRadius, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return false;
			}
			Vector3 position = _hit.position;
			if ((position - _targetPosition).magnitude > Acrobatics.JumpNavMeshLookUpRadius)
			{
				return false;
			}
			Vector3 vector3 = _hit.position + Vector3.up * 0.2f;
			Vector3 down = Vector3.down;
			float num3 = vector.y - position.y;
			Vector3 vector4 = position + down * num3 / down.y;
			if (Physics.Raycast(vector, (vector4 - vector).normalized, out hitInfo, (vector4 - vector).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			if (Physics.Raycast(vector4, (vector3 - vector4).normalized, out hitInfo, (vector3 - vector4).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return false;
			}
			if (_checkPath)
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				if (NavMesh.CalculatePath(_currentPosition, position, NavMeshLayers.WalkableLayerMaskPreset, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete && navMeshPath.CalculateLength() < (position - _currentPosition).magnitude * Acrobatics.JumpDownMinExtraWalkingMultiplier + Acrobatics.JumpDownMinExtraWalkingDistance)
				{
					return false;
				}
			}
			return true;
		}

		public static EEnvironmentInteraction CheckCivilianCovers(PlayerCharacter _playerChar, NavMeshAgent _navMeshAgent, Vector3 _currentPosition, ref Vector3 _targetPosition, ref Entity _targetEntity)
		{
			if (_playerChar.GetIsApproachingTarget())
			{
				return EEnvironmentInteraction.Invalid;
			}
			if (_playerChar.GetIsApproachingPosition())
			{
				if (!_playerChar.GetIsApproachingTappedPosition())
				{
					return EEnvironmentInteraction.Invalid;
				}
			}
			else if (LogicMain.CurrentTurn - _playerChar.PlayerEventHandler.PositionArrivalTurn > 1)
			{
				return EEnvironmentInteraction.Invalid;
			}
			Vector3 position = ((!_navMeshAgent.hasPath) ? _currentPosition : _navMeshAgent.destination);
			Collider[] array = Physics.OverlapSphere(position, 1.1f, Layers.InteractorsLayerMask);
			foreach (Collider collider in array)
			{
				if (collider.tag == "Interactable" && CheckCivilianCover(_playerChar, _currentPosition, collider, ref _targetPosition, ref _targetEntity))
				{
					return EEnvironmentInteraction.CivilianCover;
				}
			}
			return EEnvironmentInteraction.Invalid;
		}

		private static bool CheckCivilianCover(PlayerCharacter _playerChar, Vector3 _currentPosition, Collider _target, ref Vector3 _targetPosition, ref Entity _targetEntity)
		{
			Transform parent = _target.transform.parent;
			if (parent == null)
			{
				return false;
			}
			EntityWrapperComponent component = parent.GetComponent<EntityWrapperComponent>();
			if (component == null || component.Entity == null)
			{
				return false;
			}
			NpcCharacter npcCharacter = component.Entity as NpcCharacter;
			if (npcCharacter == null)
			{
				return false;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = npcCharacter.Patrol;
			EPatrolType type = patrol.PatrolDefinition.Definition.Type;
			if (type == EPatrolType.CapturedNpcs)
			{
				return false;
			}
			if (!PlayerHelper.AssassinInteraction.CanApproachPatrol(patrol.Id, patrol.PatrolDefinition.Definition.InteractionReason))
			{
				return false;
			}
			if (LogicMain.CurrentTime - _playerChar.GetHidingInPatrolEndTime() < 1f)
			{
				return false;
			}
			Assets.Scripts.Mission.Logic.Components.Patrol patrolComponent = patrol.PatrolComponent;
			Vector3 vector = patrolComponent.Formation.CalcCurrentMemberCenter();
			if (_playerChar.GetIsApproachingTappedPosition() && _playerChar.GetApproachPosition().DistanceXZSquared(vector) >= 2.25f)
			{
				return false;
			}
			if (_currentPosition.DistanceXZSquared(vector) < 2.25f)
			{
				_targetEntity = patrol;
				_targetPosition = vector;
				return true;
			}
			return false;
		}

		public static EEnvironmentInteraction CheckAcrobatics(NavMeshAgent _navMeshAgent, Vector3 _position, Vector3 _direction, out eLinkType _linkType, out OffMeshLinkEntity _link, out Vector3 _targetPosition, out Vector3 _targetDirection, out GameObject _targetObject, float _preferredLength = -1f)
		{
			_linkType = eLinkType.None;
			_link = null;
			_targetPosition = _position;
			_targetDirection = _direction;
			_targetObject = null;
			Vector3 vector = _direction;
			vector.y = 0f;
			vector.Normalize();
			if (vector.sqrMagnitude <= 0.5f)
			{
				return EEnvironmentInteraction.Invalid;
			}
			Vector3 targetPosition = _position + vector * Acrobatics.EdgeDistance;
			Vector3 targetPosition2 = _position + Quaternion.Euler(0f, 45f, 0f) * vector * Acrobatics.EdgeDistance;
			Vector3 targetPosition3 = _position + Quaternion.Euler(0f, 45f, 0f) * vector * Acrobatics.EdgeDistance;
			NavMeshHit _hit;
			if (!NavigationHelper.SafeRaycast(_position, targetPosition, out _hit, _navMeshAgent.areaMask))
			{
				if (Debugging)
				{
				}
				return EEnvironmentInteraction.Invalid;
			}
			Vector3 vector2 = -_hit.normal;
			vector2.y = 0f;
			vector2.Normalize();
			_targetDirection = vector2;
			if (Debugging)
			{
			}
			if (Debugging)
			{
			}
			if (CheckHotspots(_navMeshAgent, _position, vector, ref _targetPosition, ref _targetObject))
			{
				return EEnvironmentInteraction.Hotspot;
			}
			Vector3 vector3 = _hit.position - vector * Acrobatics.LinkOffsetFromEdge;
			NavMeshHit _hit2;
			if (NavigationHelper.SafeRaycast(_position, vector3, out _hit2, _navMeshAgent.areaMask))
			{
				vector3 = _hit2.position;
			}
			_linkType = CheckLinks(vector3, vector, ref _link, ref _targetPosition);
			if (_linkType != 0)
			{
				return EEnvironmentInteraction.Link;
			}
			NavMeshHit _hit3;
			bool flag = NavigationHelper.SafeRaycast(_position, targetPosition2, out _hit3, _navMeshAgent.areaMask);
			NavMeshHit _hit4;
			bool flag2 = NavigationHelper.SafeRaycast(_position, targetPosition3, out _hit4, _navMeshAgent.areaMask);
			if (flag && flag2)
			{
				_targetDirection = _direction;
			}
			else if (!(Vector3.Angle(vector, vector2) <= Acrobatics.EdgeAngle))
			{
				return EEnvironmentInteraction.Invalid;
			}
			RaycastHit hitInfo;
			bool flag3 = !Physics.Raycast(_position + Vector3.up * Acrobatics.WallHeight - vector * Acrobatics.ClearRaycastOffset, vector, out hitInfo, Acrobatics.MinClearDistance + Acrobatics.ClearRaycastOffset, s_physicalLayerMask);
			if (Debugging)
			{
			}
			RaycastHit hitInfo2;
			bool flag4 = !Physics.Raycast(_position + Vector3.up * Acrobatics.ObstacleHeight - vector * Acrobatics.ClearRaycastOffset, vector, out hitInfo2, Acrobatics.MinClearDistance + Acrobatics.ClearRaycastOffset, s_physicalLayerMask);
			if (Debugging)
			{
			}
			if (flag3 && flag4 && _hit.distance < Acrobatics.JumpEdgeDistance)
			{
				EEnvironmentInteraction eEnvironmentInteraction = EEnvironmentInteraction.Invalid;
				if (_preferredLength > 0f)
				{
					eEnvironmentInteraction = CheckPreferredJump(_position, _hit.position, vector, ref _targetPosition, Acrobatics.JumpMaxFallHeightDamaged, Acrobatics.JumpMaxLength, _preferredLength);
					if (eEnvironmentInteraction != 0)
					{
						return eEnvironmentInteraction;
					}
				}
				if (Acrobatics.EnableJumpHorizontalToWall)
				{
					Profiling.Start("Check for jump to wall", float.MaxValue);
					eEnvironmentInteraction = CheckJumpHorizontalToWall(_position, _hit.position, vector, ref _targetPosition, Acrobatics.JumpHorizontalToWallMaxHeight, Acrobatics.JumpHorizontalToWallMaxLength);
					Profiling.Stop();
					if (eEnvironmentInteraction != 0)
					{
						return eEnvironmentInteraction;
					}
				}
				Profiling.Start("Check for jumps (no fall damage)", float.MaxValue);
				eEnvironmentInteraction = CheckJumps(_position, _hit.position, vector, ref _targetPosition, Acrobatics.JumpMaxFallHeightNoDamage, Acrobatics.JumpMaxLength);
				Profiling.Stop();
				if (eEnvironmentInteraction != 0)
				{
					return eEnvironmentInteraction;
				}
				Profiling.Start("Check for jumps (including fall damage)", float.MaxValue);
				eEnvironmentInteraction = CheckJumps(_position, _hit.position, vector, ref _targetPosition, Acrobatics.JumpMaxFallHeightDamaged, Acrobatics.JumpMaxLength);
				Profiling.Stop();
				if (eEnvironmentInteraction != 0)
				{
					return eEnvironmentInteraction;
				}
			}
			if (PlayerHelper.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_Low)
			{
				return EEnvironmentInteraction.Invalid;
			}
			if (!flag3 && hitInfo.distance < Acrobatics.MaxWallDistance && !flag4 && hitInfo2.distance < Acrobatics.MaxWallDistance)
			{
				Vector3 normal = hitInfo2.normal;
				normal.y = 0f;
				normal.Normalize();
				if (Vector3.Dot(normal, vector) > 0f)
				{
					normal *= -1f;
				}
				_targetDirection = -normal;
				_targetPosition = hitInfo2.point;
				if (!(Vector3.Angle(vector, -normal) <= Acrobatics.EdgeAngle))
				{
					return EEnvironmentInteraction.Invalid;
				}
				return EEnvironmentInteraction.Rebound;
			}
			return EEnvironmentInteraction.Invalid;
		}

		private static EEnvironmentInteraction CheckJumpHorizontalToWall(Vector3 _currentPosition, Vector3 _edgePosition, Vector3 _normalizedDirection, ref Vector3 _targetPosition, float _maxHeight, float _maxLength)
		{
			float jumpSampleOffset = Acrobatics.JumpSampleOffset;
			Vector3 vector = _edgePosition + Acrobatics.JumpHorizontalToWallObstacleHeight * Vector3.up;
			float num = jumpSampleOffset + Acrobatics.JumpHorizontalToWallMinStraightLength;
			float num2 = Acrobatics.JumpHorizontalToWallStraightLength;
			_targetPosition = _currentPosition;
			float num3 = 0.25f;
			if (Debugging)
			{
			}
			RaycastHit hitInfo;
			if (Physics.Raycast(vector, _normalizedDirection, out hitInfo, num2, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				num2 = hitInfo.distance;
			}
			if (Debugging)
			{
			}
			NavMeshPath navMeshPath = new NavMeshPath();
			Vector3 vector2 = _currentPosition;
			bool flag = false;
			float num4 = float.MinValue;
			float y = _currentPosition.y;
			Vector3 vector3 = _currentPosition;
			for (; num < num2; num += jumpSampleOffset)
			{
				float x = Mathf.Lerp(Acrobatics.JumpDownFallAngle, Acrobatics.JumpForwardFallAngle, num / Acrobatics.JumpForwardStraightLength);
				Vector3 vector4 = Quaternion.LookRotation(_normalizedDirection) * Quaternion.Euler(x, 0f, 0f) * Vector3.forward;
				float num5 = _maxHeight / (0f - vector4.y);
				Vector3 vector5 = vector + _normalizedDirection * num;
				if (Debugging)
				{
				}
				if (!Physics.Raycast(vector5, vector4, out hitInfo, num5, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
				{
					Vector3 vector6 = vector5 + vector4 * num5;
					if (vector6.y < y)
					{
						y = vector6.y;
						vector3 = vector6;
					}
					continue;
				}
				if (hitInfo.point.y < y)
				{
					y = hitInfo.point.y;
					vector3 = hitInfo.point;
				}
				NavMeshHit hit;
				if (!NavMesh.SamplePosition(hitInfo.point, out hit, Acrobatics.JumpNavMeshLookUpRadius, NavMeshLayers.WalkableLayerMaskPreset))
				{
					continue;
				}
				Vector3 position = hit.position;
				float num6 = vector.y - position.y;
				Vector3 vector7 = position - vector;
				vector7.y = 0f;
				float magnitude = vector7.magnitude;
				if (num6 > _maxHeight || magnitude > _maxLength)
				{
					continue;
				}
				if (Debugging)
				{
				}
				if (!(position.y <= num4))
				{
					float num7 = (position - vector2).magnitude * Acrobatics.JumpForwardMinExtraWalkingMultiplier + Acrobatics.JumpForwardMinExtraWalkingDistance;
					if (!NavMesh.CalculatePath(vector2, position, NavMeshLayers.WalkableLayerMaskPreset, navMeshPath) || navMeshPath.status != 0 || !(navMeshPath.CalculateLength() < num7))
					{
						num4 = position.y;
						vector2 = position;
						flag = true;
					}
				}
			}
			if (!flag || Debugging)
			{
			}
			if (flag)
			{
				_targetPosition = vector2;
				List<EdgeObject> edges = EdgeDetector.GetEdges(_targetPosition, Acrobatics.JumpHorizontalToWallEdgeLookupRadius);
				EdgeObject edgeObject = null;
				Vector3 zero = Vector3.zero;
				float num8 = float.MaxValue;
				Vector3 mask = new Vector3(1f, 0f, 1f);
				float num9 = Mathf.Cos(Acrobatics.JumpHorizontalToWallMaxAngle * ((float)Math.PI / 180f));
				for (int i = 0; i < edges.Count; i++)
				{
					EdgeObject edgeObject2 = edges[i];
					if (Debugging)
					{
					}
					if (Debugging)
					{
					}
					if (Vector3.Dot(-edgeObject2.Normals[0], _normalizedDirection) < num9)
					{
						continue;
					}
					Vector3 vector8 = edgeObject2.ClosestPoint(_targetPosition);
					Vector3 vector9 = (vector8 - _targetPosition).Mask(mask);
					float num10 = vector9.magnitude / Vector3.Dot(vector9.normalized, -_normalizedDirection);
					Vector3 vector10 = _targetPosition - _normalizedDirection * num10;
					Vector3 vector11 = edgeObject2.ClosestPoint(vector10);
					if (Debugging)
					{
					}
					if (Mathf.Abs(vector11.y - _targetPosition.y) > Acrobatics.JumpHorizontalToWallMaxHeightDifference || Vector3.Dot((_targetPosition - vector11).Mask(mask).normalized, _normalizedDirection) < num9 || Vector3.Dot((vector11 - _currentPosition).Mask(mask).normalized, _normalizedDirection) < num9)
					{
						continue;
					}
					Vector3 vector12 = vector11 + edgeObject2.Normals[0] * Acrobatics.JumpHorizontalToWallRaycastOffsetZ;
					vector12.y = _targetPosition.y + Acrobatics.JumpHorizontalToWallRaycastOffsetY;
					if (Physics.Raycast(vector12, Vector3.down, out hitInfo, Acrobatics.JumpHorizontalToWallMinGapDepth + Acrobatics.JumpHorizontalToWallRaycastOffsetY, Layers.StaticCollidersLayersMaskPreset) && hitInfo.distance < Acrobatics.JumpHorizontalToWallMinGapDepth + Acrobatics.JumpHorizontalToWallRaycastOffsetY)
					{
						continue;
					}
					Vector3 vector13 = _targetPosition + Vector3.up * Acrobatics.JumpHorizontalToWallRaycastOffsetY;
					Vector3 vector14 = vector13 - vector12;
					Vector3 normalized = vector14.normalized;
					float magnitude2 = vector14.magnitude;
					if (Debugging)
					{
					}
					if (Physics.Raycast(vector12, normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset) || Physics.Raycast(vector13, -normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset))
					{
						continue;
					}
					Vector3 vector15 = (edgeObject2.End - edgeObject2.Start).normalized * Acrobatics.JumpHorizontalToWallRaycastOffsetX;
					if (!Physics.Raycast(vector12 + vector15, normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset) && !Physics.Raycast(vector13 + vector15, -normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset) && !Physics.Raycast(vector12 - vector15, normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset) && !Physics.Raycast(vector13 - vector15, -normalized, magnitude2, Layers.StaticCollidersLayersMaskPreset))
					{
						if (Debugging)
						{
						}
						Vector3 vector16 = _targetPosition + Vector3.up * Acrobatics.JumpHorizontalToWallObstacleHeight;
						if (Debugging)
						{
						}
						float magnitude3 = (vector11 - vector16).magnitude;
						if (edgeObject == null || magnitude3 < num8)
						{
							edgeObject = edgeObject2;
							zero = vector10;
							num8 = magnitude3;
						}
					}
				}
				if (edgeObject != null)
				{
					JumpHorizontalToWallLastEdge = edgeObject;
					if (Debugging)
					{
					}
					return EEnvironmentInteraction.JumpHorizontalToWall;
				}
			}
			_targetPosition = vector3;
			return EEnvironmentInteraction.Invalid;
		}

		private static EEnvironmentInteraction CheckPreferredJump(Vector3 _currentPosition, Vector3 _navMeshEdgePosition, Vector3 _normalizedDirection, ref Vector3 _targetPosition, float _maxHeight, float _maxLength, float _preferedLength)
		{
			Vector3 vector = _navMeshEdgePosition + Acrobatics.ObstacleHeight * Vector3.up;
			float num = Mathf.Clamp(_preferedLength, 0f, _maxLength);
			Vector3 origin = vector + _normalizedDirection * num;
			RaycastHit hitInfo;
			if (!Physics.Raycast(origin, Vector3.down, out hitInfo, _maxHeight + Acrobatics.ObstacleHeight, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return EEnvironmentInteraction.Invalid;
			}
			NavMeshHit hit;
			if (!NavMesh.SamplePosition(hitInfo.point, out hit, 0.2f, NavMeshLayers.WalkableLayerMaskPreset))
			{
				return EEnvironmentInteraction.Invalid;
			}
			Vector3 position = hit.position;
			Vector3 vector2 = hit.position + Vector3.up * 0.2f;
			Vector3 down = Vector3.down;
			float num2 = vector.y - position.y;
			Vector3 vector3 = position + down * num2 / down.y;
			Vector3 vector4 = position - vector;
			vector4.y = 0f;
			float magnitude = vector4.magnitude;
			float num3 = Acrobatics.JumpForwardStraightLength + num2 / Mathf.Atan(Acrobatics.JumpForwardFallAngle * ((float)Math.PI / 180f));
			if (magnitude > num3)
			{
				return EEnvironmentInteraction.Invalid;
			}
			if (Physics.Raycast(vector, (vector3 - vector).normalized, out hitInfo, (vector3 - vector).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return EEnvironmentInteraction.Invalid;
			}
			if (Physics.Raycast(vector3, (vector2 - vector3).normalized, out hitInfo, (vector2 - vector3).magnitude, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				return EEnvironmentInteraction.Invalid;
			}
			_targetPosition = position;
			return EEnvironmentInteraction.JumpDown;
		}

		private static EEnvironmentInteraction CheckJumps(Vector3 _currentPosition, Vector3 _navMeshEdgePosition, Vector3 _normalizedDirection, ref Vector3 _targetPosition, float _maxHeight, float _maxLength)
		{
			float jumpSampleOffset = Acrobatics.JumpSampleOffset;
			Vector3 vector = _navMeshEdgePosition + Acrobatics.ObstacleHeight * Vector3.up;
			float num = jumpSampleOffset;
			float num2 = Acrobatics.JumpForwardStraightLength;
			_targetPosition = _currentPosition;
			if (Debugging)
			{
			}
			RaycastHit hitInfo;
			if (Physics.Raycast(vector, _normalizedDirection, out hitInfo, num2, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
			{
				num2 = hitInfo.distance;
			}
			if (Debugging)
			{
			}
			Vector3 vector2 = Vector3.zero;
			bool flag = false;
			NavMeshPath navMeshPath = new NavMeshPath();
			Vector3 vector3 = Vector3.zero;
			bool flag2 = false;
			float num3 = _currentPosition.y;
			float num4 = float.MinValue;
			float y = _currentPosition.y;
			Vector3 vector4 = _currentPosition;
			for (; num < num2; num += jumpSampleOffset)
			{
				float x = Mathf.Lerp(Acrobatics.JumpDownFallAngle, Acrobatics.JumpForwardFallAngle, num / Acrobatics.JumpForwardStraightLength);
				Vector3 vector5 = Quaternion.LookRotation(_normalizedDirection) * Quaternion.Euler(x, 0f, 0f) * Vector3.forward;
				float num5 = _maxHeight / (0f - vector5.y);
				Vector3 vector6 = vector + _normalizedDirection * num;
				if (Debugging)
				{
				}
				if (!Physics.Raycast(vector6, vector5, out hitInfo, num5, s_physicalLayerMask | (int)Layers.MissionBarrierLayerMask))
				{
					Vector3 vector7 = vector6 + vector5 * num5;
					if (vector7.y < y)
					{
						y = vector7.y;
						vector4 = vector7;
					}
					continue;
				}
				if (hitInfo.point.y < y)
				{
					y = hitInfo.point.y;
					vector4 = hitInfo.point;
				}
				NavMeshHit hit;
				if (!NavMesh.SamplePosition(hitInfo.point, out hit, Acrobatics.JumpNavMeshLookUpRadius, NavMeshLayers.WalkableLayerMaskPreset))
				{
					continue;
				}
				Vector3 position = hit.position;
				float num6 = vector.y - position.y;
				Vector3 vector8 = position - vector;
				vector8.y = 0f;
				float magnitude = vector8.magnitude;
				if (num6 > _maxHeight || magnitude > _maxLength)
				{
					continue;
				}
				if (Debugging)
				{
				}
				if (!flag)
				{
					if (NavMesh.CalculatePath(_currentPosition, position, NavMeshLayers.WalkableLayerMaskPreset, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete && navMeshPath.CalculateLength() < (position - _currentPosition).magnitude * Acrobatics.JumpDownMinExtraWalkingMultiplier + Acrobatics.JumpDownMinExtraWalkingDistance)
					{
						break;
					}
					vector2 = position;
					vector3 = position;
					flag = true;
					if (PlayerHelper.PlayerProfile.CurrentStateType == EStateType.PlayerProfile_Low)
					{
						break;
					}
				}
				if (flag)
				{
					num3 = Mathf.Min(num3, position.y);
					if (position.y > num3 + Acrobatics.JumpForwardMinHeightDelta && position.y > num4 && (!NavMesh.CalculatePath(vector3, position, NavMeshLayers.WalkableLayerMaskPreset, navMeshPath) || navMeshPath.status != 0 || !(navMeshPath.CalculateLength() < (position - vector3).magnitude * Acrobatics.JumpForwardMinExtraWalkingMultiplier + Acrobatics.JumpForwardMinExtraWalkingDistance)))
					{
						num4 = position.y;
						vector3 = position;
						flag2 = true;
					}
					if (!flag2)
					{
						vector3 = position;
					}
				}
			}
			if (!flag2 || Debugging)
			{
			}
			if (!flag || Debugging)
			{
			}
			if (flag2)
			{
				_targetPosition = vector3;
				return EEnvironmentInteraction.JumpForward;
			}
			if (flag)
			{
				_targetPosition = vector2;
				return EEnvironmentInteraction.JumpDown;
			}
			_targetPosition = vector4;
			return EEnvironmentInteraction.Invalid;
		}

		private static bool CheckHotspots(NavMeshAgent _navMeshAgent, Vector3 _currentPosition, Vector3 _direction, ref Vector3 _targetPosition, ref GameObject _targetObject)
		{
			Collider[] array = Physics.OverlapSphere(_currentPosition, Acrobatics.HotspotSearchRadius, Layers.InteractorsLayerMask);
			if (array.Length > 0)
			{
				BBList<Collider> bBList = new BBList<Collider>(array);
				bBList.Sort((Collider o, Collider p) => (o.bounds.center - _currentPosition).sqrMagnitude.CompareTo((p.bounds.center - _currentPosition).sqrMagnitude));
				foreach (Collider item in bBList)
				{
					if (!(item.tag == "Interactable"))
					{
						continue;
					}
					Vector3 to = item.bounds.center - _currentPosition;
					to.y = 0f;
					if (Vector3.Angle(_direction, to) > 90f)
					{
						continue;
					}
					HotspotMeta component = item.transform.parent.gameObject.GetComponent<HotspotMeta>();
					if (component == null || !component.Active)
					{
						continue;
					}
					RaycastHit hitInfo;
					if (Physics.Raycast(_currentPosition + Vector3.up * Acrobatics.ObstacleHeight, to.normalized, out hitInfo, to.magnitude, Layers.StaticCollidersLayersMaskPreset))
					{
						Transform parent = hitInfo.collider.transform.parent;
						if (parent == null || (parent != item.transform.parent && parent.gameObject.layer != Layers.InteractorsLayer))
						{
							continue;
						}
					}
					_targetObject = item.gameObject;
					_targetPosition = item.bounds.center;
					return true;
				}
			}
			return false;
		}

		private static eLinkType CheckLinks(Vector3 _currentPosition, Vector3 _direction, ref OffMeshLinkEntity _link, ref Vector3 _targetPosition)
		{
			OffMeshLinkEntity _bestLink = null;
			float _minSqrDistance = Acrobatics.LinkSearchRadius * Acrobatics.LinkSearchRadius;
			BBList<OffMeshLinkManager.OffMeshLinkOctreeEntry> bBList = new BBList<OffMeshLinkManager.OffMeshLinkOctreeEntry>();
			UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.FindOffMeshLinks(_currentPosition, Acrobatics.LinkSearchRadius, bBList);
			if (Debugging)
			{
			}
			NavMeshHit _navHit = default(NavMeshHit);
			foreach (OffMeshLinkManager.OffMeshLinkOctreeEntry item in bBList)
			{
				OffMeshLinkEntity entity = UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.GetEntity(item.Link);
				if ((!Acrobatics.IgnoreJumpDownLinks || entity.LinkType != eLinkType.JumpDown) && entity.LinkType != eLinkType.JumpHorizontalToWall)
				{
					if (Debugging)
					{
					}
					CheckLinksCompare(_currentPosition, _direction, entity, true, ref _minSqrDistance, ref _bestLink, ref _targetPosition, ref _navHit);
					if (entity.OffMeshLink.biDirectional)
					{
						CheckLinksCompare(_currentPosition, _direction, entity, false, ref _minSqrDistance, ref _bestLink, ref _targetPosition, ref _navHit);
					}
				}
			}
			if (_bestLink != null)
			{
				_link = _bestLink;
				return _bestLink.LinkType;
			}
			return eLinkType.None;
		}

		private static bool CheckLinksCompare(Vector3 _currentPosition, Vector3 _currentDirection, OffMeshLinkEntity _link, bool _forward, ref float _minSqrDistance, ref OffMeshLinkEntity _bestLink, ref Vector3 _targetPosition, ref NavMeshHit _navHit)
		{
			Vector3 vector = ((!_forward) ? _link.EndPosition : _link.StartPosition);
			Vector3 vector2 = ((!_forward) ? _link.StartPosition : _link.EndPosition);
			Vector3 vector3 = ((!_forward) ? Vector3.back : Vector3.forward);
			Vector3 vector4 = vector - _currentPosition;
			float sqrMagnitude = vector4.sqrMagnitude;
			if (sqrMagnitude >= _minSqrDistance || Vector3.Angle(_currentDirection, _link.Rotation * vector3) > Acrobatics.EdgeAngle || Mathf.Abs(vector4.y) >= Acrobatics.MaxLinkHeightDifference || NavigationHelper.SafeRaycast(_currentPosition, vector, out _navHit, -1) || UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.IsOffMeshLinkBlocked(_link.OffMeshLink, !_forward))
			{
				return false;
			}
			if (sqrMagnitude >= (vector2 - _currentPosition).sqrMagnitude && !NavigationHelper.SafeRaycast(_currentPosition, vector2, out _navHit, -1))
			{
				return false;
			}
			_bestLink = _link;
			_minSqrDistance = sqrMagnitude;
			_targetPosition = vector2;
			if (Debugging)
			{
			}
			if (_link.LinkType == eLinkType.Climb && _link.ClimbRoute.ClimbRouteEntries.Count > 0)
			{
				ClimbRouteEntry climbRouteEntry = _link.ClimbRoute.ClimbRouteEntries[_link.ClimbRoute.ClimbRouteEntries.Count - 1];
				Vector3 p = _currentPosition;
				p.y = climbRouteEntry.Edge.MidPoint.y;
				_targetPosition = climbRouteEntry.Edge.ClosestPoint(p);
			}
			return true;
		}

		public static bool TryEnterHideout(AssassinCharacter _assassin, HideoutObject _entity, NavMeshAgent _agent)
		{
			if (_assassin.GetIsInParcourOrScheduled())
			{
				return false;
			}
			InteractionLocators.Locator _locator;
			NavMeshPath _path;
			if (!_entity.InteractionLocators.ChooseClosestLocator(_agent.transform.position, _assassin.GetAutoPassableNavMeshLayerMask(), out _locator, out _path))
			{
				return false;
			}
			if (CanUseLocatorDirectly(_assassin.Position, _locator))
			{
				return _assassin.EnterHideout(_entity, _locator);
			}
			return _assassin.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(_entity.Id, EInteractionReason.InteractWithObject, _locator, false);
		}

		public static bool CanUseLocatorDirectly(Vector3 _assassin, InteractionLocators.Locator _locator)
		{
			Vector3 position = _locator.Step.transform.position;
			if (Mathf.Abs(_assassin.y - position.y) > Acrobatics.HideoutDirectTriggerMaxDeltaY)
			{
				return false;
			}
			_assassin.y = position.y;
			float distanceToPoint = _locator.Plane.GetDistanceToPoint(_assassin);
			if (distanceToPoint < Acrobatics.HideoutDirectTriggerDistanceInside)
			{
				return false;
			}
			if (distanceToPoint > Acrobatics.HideoutDirectTriggerDistanceOutside)
			{
				return false;
			}
			Vector3 vector = _assassin - _locator.Plane.normal * distanceToPoint;
			if ((vector - _assassin).sqrMagnitude > Acrobatics.HideoutDirectTriggerDistanceOutside * Acrobatics.HideoutDirectTriggerDistanceOutside)
			{
				return false;
			}
			float magnitude = (vector - position).magnitude;
			float num = _locator.MaxOffset + Acrobatics.HideoutDirectTriggerExtraOffset;
			if (distanceToPoint < 0f)
			{
				return magnitude < num;
			}
			if (magnitude > num)
			{
				return magnitude - num < distanceToPoint;
			}
			return true;
		}
	}
}
