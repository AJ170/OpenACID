using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHelper
{
	private const float NpcTurnDirectionStep = 45f;

	private const float NpcTurnDirectionMax = 180f;

	private const float NpcTurnThreshold = 7f;

	private const float StayOnNavMeshNearThreshold = 0.03f;

	private static readonly int[] NpcTurnDirectionsCw = new int[5] { 2, 2, 1, 6, 0 };

	private static readonly int[] NpcTurnDirectionsCcw = new int[5] { 3, 3, 4, 7, 5 };

	public static Quaternion AlignTargetToTargetDirection(AnimationController.Direction _targetDirection, GameObject _instigator, GameObject _target)
	{
		Quaternion result = Quaternion.identity;
		switch (_targetDirection)
		{
		case AnimationController.Direction.Front:
		{
			Vector3 forward2 = _instigator.transform.position - _target.transform.position;
			forward2.y = 0f;
			result = Quaternion.LookRotation(forward2, Vector3.up);
			break;
		}
		case AnimationController.Direction.Right:
		{
			Vector3 vector2 = _target.transform.position - _instigator.transform.position;
			vector2.y = 0f;
			vector2 = Quaternion.Euler(0f, 90f, 0f) * vector2;
			result = Quaternion.LookRotation(vector2, Vector3.up);
			break;
		}
		case AnimationController.Direction.Back:
		{
			Vector3 forward = _target.transform.position - _instigator.transform.position;
			forward.y = 0f;
			forward.Normalize();
			result = Quaternion.LookRotation(forward, Vector3.up);
			break;
		}
		case AnimationController.Direction.Left:
		{
			Vector3 vector = _target.transform.position - _instigator.transform.position;
			vector.y = 0f;
			vector.Normalize();
			vector = Quaternion.Euler(0f, -90f, 0f) * vector;
			result = Quaternion.LookRotation(vector, Vector3.up);
			break;
		}
		}
		return result;
	}

	public static Quaternion AlignInstigatorToTarget(GameObject _instigator, GameObject _target)
	{
		Quaternion identity = Quaternion.identity;
		Vector3 forward = _target.transform.position - _instigator.transform.position;
		forward.y = 0f;
		return Quaternion.LookRotation(forward, Vector3.up);
	}

	public static AnimationController.Direction DetermineDirectionFrontOrBack(GameObject _instigator, GameObject _target)
	{
		Vector3 forward = _target.transform.forward;
		forward.y = 0f;
		Vector3 rhs = _target.transform.position - _instigator.transform.position;
		rhs.y = 0f;
		if (Vector3.Dot(forward, rhs) >= 0f)
		{
			return AnimationController.Direction.Back;
		}
		return AnimationController.Direction.Front;
	}

	public static AnimationController.Direction DetermineDirection(GameObject _instigator, GameObject _target)
	{
		Vector3 forward = _target.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Vector3 vector = _target.transform.position - _instigator.transform.position;
		vector.y = 0f;
		vector.Normalize();
		Vector3 vector2 = Quaternion.Euler(0f, -90f, 0f) * vector;
		vector2.y = 0f;
		vector2.Normalize();
		if (Vector3.Angle(forward, vector) <= 45f)
		{
			return AnimationController.Direction.Back;
		}
		if (Vector3.Angle(forward, vector2) <= 45f)
		{
			return AnimationController.Direction.Left;
		}
		if (Vector3.Angle(forward, -vector2) <= 45f)
		{
			return AnimationController.Direction.Right;
		}
		return AnimationController.Direction.Front;
	}

	public static AnimationController.Direction DetermineDirection6Ways(Vector3 _currentFacing, Vector3 _incomingDirection)
	{
		_currentFacing.y = 0f;
		_currentFacing.Normalize();
		_incomingDirection.y = 0f;
		_incomingDirection.Normalize();
		Vector3 to = Quaternion.Euler(0f, -90f, 0f) * _incomingDirection;
		to.y = 0f;
		to.Normalize();
		float num = Vector3.Angle(_currentFacing, -_incomingDirection);
		float num2 = Vector3.Angle(_currentFacing, to);
		if (num2 >= 90f)
		{
			return (num < 45f) ? AnimationController.Direction.FrontLeft : ((!(num < 135f)) ? AnimationController.Direction.BackLeft : AnimationController.Direction.Left);
		}
		return (num < 45f) ? AnimationController.Direction.FrontRight : ((!(num < 135f)) ? AnimationController.Direction.BackRight : AnimationController.Direction.Right);
	}

	public static float NpcTurnDirectionToAngle(NpcTurnDirection _turnDirection)
	{
		switch (_turnDirection)
		{
		case NpcTurnDirection.Cw45:
			return 45f;
		case NpcTurnDirection.Cw90:
			return 90f;
		case NpcTurnDirection.Cw135:
			return 135f;
		case NpcTurnDirection.Cw180:
			return 180f;
		case NpcTurnDirection.Ccw45:
			return -45f;
		case NpcTurnDirection.Ccw90:
			return -90f;
		case NpcTurnDirection.Ccw135:
			return -135f;
		case NpcTurnDirection.Ccw180:
			return -180f;
		case NpcTurnDirection.Invalid:
			return 0f;
		default:
			return 0f;
		}
	}

	public static NpcTurnDirection DetermineNpcTurnDirection(float _currentAngle, float _targetAngle)
	{
		float deltaAngle = Mathf.DeltaAngle(_currentAngle, _targetAngle);
		return DetermineNpcTurnDirection(deltaAngle);
	}

	public static NpcTurnDirection DetermineNpcTurnDirection(float _deltaAngle)
	{
		float num = Mathf.Abs(_deltaAngle);
		if (num < 7f)
		{
			return NpcTurnDirection.Invalid;
		}
		int[] array = ((!(_deltaAngle < 0f)) ? NpcTurnDirectionsCw : NpcTurnDirectionsCcw);
		float num2 = num / 180f;
		int num3 = (int)Mathf.Round(num2 * (float)(array.Length - 1));
		return (NpcTurnDirection)array[num3];
	}

	public static void AlignEntityToNavMeshPlane(EntityId _entityId, int _mask, float _factor)
	{
		AlignEntityToNavMeshPlaneWithTargetOrientation(_entityId, _mask, _factor, EntityId.InvalidId);
	}

	public static void AlignEntityToNavMeshPlaneWithTargetOrientation(EntityId _entityId, int _mask, float _factor, EntityId _targetId)
	{
		GameEntity gameEntity = _entityId.GameEntity;
		AnimationController animationController = gameEntity.UnityComponentCache.AnimationController;
		Vector3 nextPosition = animationController.NextPosition;
		NavMeshHit _hit;
		if (!NavigationHelper.CascadingSamplePosition(nextPosition, out _hit, _mask))
		{
			return;
		}
		Vector3 vector = Vector3.Lerp(nextPosition, _hit.position, LogicMain.FrameTime / (LogicMain.FrameTime + CombatController.Instance.StayOnNavMeshDamping));
		if (!MathHelper.Approximately(vector.DistanceSquared(nextPosition), 0f))
		{
			animationController.NextPosition = vector;
		}
		Character character = null;
		character = ((_targetId == EntityId.InvalidId) ? gameEntity.Cast<Character>() : _targetId.GameEntity.Cast<Character>());
		Vector3 _normal;
		if (character.SampleNormal(out _normal))
		{
			Quaternion quaternion = gameEntity.GameObject.AlignRotationTo(_normal);
			if (!animationController.NextRotation.Equals(quaternion))
			{
				animationController.NextRotation = Quaternion.Slerp(animationController.NextRotation, quaternion, _factor);
			}
		}
	}

	public static void StayOnNavMesh(EntityId _character)
	{
		Character character = _character.TryGetEntity<Character>();
		if (character != null)
		{
			StayOnNavMesh(character);
		}
	}

	public static void StayOnNavMesh(Character _character)
	{
		Vector3 nextPosition = _character.UnityComponentCache.AnimationController.NextPosition;
		NavMeshHit _hit;
		if (NavigationHelper.CascadingSamplePosition(nextPosition, out _hit, _character.UnityComponentCache.NavMeshAgent.areaMask))
		{
			_character.UnityComponentCache.AnimationController.NextPosition = ((!(_hit.distance < 0.03f)) ? Vector3.Lerp(nextPosition, _hit.position, LogicMain.FrameTime / (LogicMain.FrameTime + 0.1f)) : _hit.position);
		}
	}

	public static void AlignEntityToUpVector(EntityId _entityId, Vector3 _upVector, float _factor)
	{
		GameEntity gameEntity = _entityId.GameEntity;
		AnimationController animationController = gameEntity.UnityComponentCache.AnimationController;
		animationController.NextRotation = Quaternion.Slerp(animationController.NextRotation, gameEntity.GameObject.AlignRotationTo(_upVector), _factor);
	}

	public static bool GetMatchTargetData(AnimationController _animController, out float _matchStart, out float _matchEnd, int _layer, int _eventInstance = 0)
	{
		_matchStart = 0f;
		_matchEnd = 0f;
		float num = 0f;
		bool flag = false;
		AnimatorClipInfo[] currentAnimationClipState = _animController.DataController.GetCurrentAnimationClipState(_layer);
		for (int i = 0; i < currentAnimationClipState.Length; i++)
		{
			AnimatorClipInfo animatorClipInfo = currentAnimationClipState[i];
			List<AnimatorData.Event> eventsByMotion = _animController.DataController.GetEventsByMotion(animatorClipInfo.clip, AnimatorEventType.MatchStart);
			List<AnimatorData.Event> eventsByMotion2 = _animController.DataController.GetEventsByMotion(animatorClipInfo.clip, AnimatorEventType.MatchEnd);
			AnimatorData.Event @event = ((eventsByMotion.Count <= _eventInstance) ? null : eventsByMotion[_eventInstance]);
			AnimatorData.Event event2 = ((eventsByMotion2.Count <= _eventInstance) ? null : eventsByMotion2[_eventInstance]);
			if (@event != null && event2 != null && animatorClipInfo.weight > 0f)
			{
				_matchStart += @event.m_time * animatorClipInfo.weight;
				_matchEnd += event2.m_time * animatorClipInfo.weight;
				num += animatorClipInfo.weight;
				flag = true;
			}
		}
		if (flag)
		{
			_matchStart /= num;
			_matchEnd /= num;
		}
		return flag && !_animController.Animator.IsInTransition(_layer);
	}

	public static int ClampAnimatorNpcId(int _slot, int range = 3)
	{
		return 1 + _slot % range;
	}
}
