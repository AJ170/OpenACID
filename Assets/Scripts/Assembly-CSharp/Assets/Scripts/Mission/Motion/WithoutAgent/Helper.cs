using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.WithoutAgent
{
	internal static class Helper
	{
		private const float RotationLerpFactor = 2f;

		private const float DeltaThreshold = 0.01f;

		public static bool InterpolatePosAndRotation(float _speed, Vector3 _target, ref Vector3 _position, ref float _rotationXZ, ref float _deltaTime)
		{
			float targetRotationXZ = _rotationXZ;
			Vector3 direction = _target - _position;
			float magnitude = direction.magnitude;
			if (magnitude > 0.01f)
			{
				targetRotationXZ = direction.AngleZX();
			}
			return InterpolatePosAndRotation(_speed, _target, magnitude, targetRotationXZ, ref _position, ref _rotationXZ, ref _deltaTime);
		}

		public static bool InterpolatePosAndRotation(float _speed, Vector3 _target, float _targetRotationXZ, ref Vector3 _position, ref float _rotationXZ, ref float _deltaTime)
		{
			return InterpolatePosAndRotation(_speed, _target, (_target - _position).magnitude, _targetRotationXZ, ref _position, ref _rotationXZ, ref _deltaTime);
		}

		public static void DemandNavAgentSurrogateFrameUpdate(EntityId _id, bool _demand)
		{
			INavAgentSurrogateCarrier navAgentSurrogateCarrier = _id.TryGetEntity<INavAgentSurrogateCarrier>();
			DemandNavAgentSurrogateFrameUpdate(navAgentSurrogateCarrier, _demand);
		}

		public static void DemandNavAgentSurrogateFrameUpdate(Entity _entity, bool _demand)
		{
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = _entity as Assets.Scripts.Mission.Logic.Entities.Patrol;
			if (patrol != null)
			{
				patrol.PatrolComponent.Formation.DemandMemberFrameUpdate(_demand);
				return;
			}
			INavAgentSurrogateCarrier navAgentSurrogateCarrier = _entity as INavAgentSurrogateCarrier;
			DemandNavAgentSurrogateFrameUpdate(navAgentSurrogateCarrier, _demand);
		}

		private static bool InterpolatePosAndRotation(float _speed, Vector3 _target, float _deltaMagnitude, float _targetRotationXZ, ref Vector3 _position, ref float _rotationXZ, ref float _deltaTime)
		{
			float a = _rotationXZ;
			_rotationXZ = _targetRotationXZ;
			if (_deltaMagnitude > 0.01f)
			{
				float num = _deltaTime;
				if (_deltaTime > MathHelper.Epsilon)
				{
					_rotationXZ = Mathf.LerpAngle(a, _targetRotationXZ, _deltaTime * 2f);
					float num2 = _speed * _deltaTime;
					float t = num2 / _deltaMagnitude;
					_position = Vector3.Lerp(_position, _target, t);
					num = Mathf.Clamp((num2 - _deltaMagnitude) / _speed, 0f, _deltaTime);
				}
				_deltaTime = ((!(num > MathHelper.Epsilon)) ? 0f : num);
				return false;
			}
			return true;
		}

		private static void DemandNavAgentSurrogateFrameUpdate(INavAgentSurrogateCarrier _navAgentSurrogateCarrier, bool _demand)
		{
			if (_navAgentSurrogateCarrier != null)
			{
				_navAgentSurrogateCarrier.NavAgentSurrogate.DemandFrameUpdate(_demand);
			}
		}
	}
}
