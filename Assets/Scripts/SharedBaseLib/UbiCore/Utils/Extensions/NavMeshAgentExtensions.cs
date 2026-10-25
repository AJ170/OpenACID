using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Utils.Extensions
{
	public static class NavMeshAgentExtensions
	{
		public static void SnapToMesh(this NavMeshAgent _agent, float _maxHeight = float.PositiveInfinity, int _mask = -1)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(_agent.transform.position, out hit, _maxHeight, _mask))
			{
				_agent.transform.position = hit.position;
			}
		}

		public static float GetTurningAngle(this NavMeshAgent _agent, Vector3 _target)
		{
			if (_agent.pathStatus == NavMeshPathStatus.PathInvalid)
			{
				return 0f;
			}
			return VectorHelper.SignedAngle(_agent.transform.forward, _target - _agent.transform.position, _agent.transform.up);
		}

		public static TurnDirection GetTurnDirection(this NavMeshAgent _agent, Vector3 _target, out float _angle)
		{
			_angle = _agent.GetTurningAngle(_target);
			if (MathHelper.Approximately(_angle, 0f))
			{
				return TurnDirection.None;
			}
			if (_angle > 0f)
			{
				return TurnDirection.Left;
			}
			return TurnDirection.Right;
		}

		public static float DistanceToOffMeshLinkStart(this NavMeshAgent _agent)
		{
			float result = float.PositiveInfinity;
			if (_agent.currentOffMeshLinkData.offMeshLink != null)
			{
				result = Vector3.Distance(_agent.currentOffMeshLinkData.startPos, _agent.transform.position);
			}
			else if (_agent.nextOffMeshLinkData.offMeshLink != null)
			{
				result = Vector3.Distance(_agent.nextOffMeshLinkData.startPos, _agent.transform.position);
			}
			return result;
		}
	}
}
