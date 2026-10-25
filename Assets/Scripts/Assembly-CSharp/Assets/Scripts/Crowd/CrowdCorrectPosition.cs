using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	public class CrowdCorrectPosition : MonoBehaviour
	{
		[HideInInspector]
		public NavMeshAgent Agent;

		public float LastUpdateTime;

		public Vector3 LastUpdatePosition;

		public float NextUpdateTime;

		public float EstimatedSlopeVelocity;

		public void Reset(Vector3 _currentPosition)
		{
			NextUpdateTime = LogicMain.CurrentTime;
			LastUpdateTime = LogicMain.CurrentTime;
			LastUpdatePosition = _currentPosition;
		}

		public Vector3 CorrectPositionUpdate(Vector3 _lastPosition, Vector3 _targetPosition, float _dt)
		{
			if (CrowdSettings.CorrectPositionPeriod <= 0f || LogicMain.CurrentTime >= NextUpdateTime)
			{
				_dt = Mathf.Max(LogicMain.CurrentTime - LastUpdateTime, _dt);
				_targetPosition = CorrectPosition(_lastPosition, _targetPosition, _dt);
				EstimatedSlopeVelocity = (_targetPosition.y - LastUpdatePosition.y) / _dt;
				LastUpdatePosition = _targetPosition;
				LastUpdateTime = LogicMain.CurrentTime;
				NextUpdateTime = LastUpdateTime + CrowdSettings.CorrectPositionPeriod * Random.Range(0.8f, 1.2f);
			}
			else
			{
				_targetPosition += Vector3.up * EstimatedSlopeVelocity * _dt;
			}
			return _targetPosition;
		}

		private Vector3 CorrectPosition(Vector3 _currentPosition, Vector3 _targetPosition, float _dt)
		{
			if (_dt < float.Epsilon)
			{
				return _targetPosition;
			}
			Vector3 vector = _targetPosition - _currentPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			if (magnitude / _dt > 10f)
			{
				return _targetPosition;
			}
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(_targetPosition, out _hit, Agent.areaMask))
			{
				Vector3 vector2 = _hit.position - _targetPosition;
				vector2.y = 0f;
				_targetPosition = _hit.position;
				if (vector2.sqrMagnitude > 0.0001f)
				{
					float num = 0.3f;
					NavMeshHit _hit2;
					NavigationHelper.SafeRaycast(_currentPosition, _currentPosition + normalized * num, out _hit2, Agent.areaMask);
					if (_hit2.distance > 0.01f && _hit2.distance < num * 2f + 0.1f)
					{
						NavMeshHit _hit3;
						NavigationHelper.SafeRaycast(_hit2.position, _currentPosition, out _hit3, Agent.areaMask);
						if (_hit3.distance > 0.01f && _hit3.distance < num * 2f + 0.1f)
						{
							Vector3 vector3 = _hit2.position - _hit3.position;
							_targetPosition = _hit3.position + vector3.normalized * magnitude;
						}
					}
				}
			}
			return _targetPosition;
		}
	}
}
