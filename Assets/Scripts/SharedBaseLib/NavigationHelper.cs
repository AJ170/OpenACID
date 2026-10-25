using UnityEngine;
using UnityEngine.AI;

public class NavigationHelper
{
	public static Vector3 CalcVectorToMoveDestination(NavMeshAgent _navMeshAgent)
	{
		return _navMeshAgent.pathEndPosition - _navMeshAgent.gameObject.transform.position;
	}

	public static bool GetPlaneIntersection(Vector2 _screenPos, Vector3 _planeOrigin, out Vector3 _hitPoint)
	{
		Plane plane = new Plane(Vector3.up, _planeOrigin);
		Ray ray = Camera.main.ScreenPointToRay(_screenPos);
		float enter;
		if (plane.Raycast(ray, out enter))
		{
			_hitPoint = ray.origin + ray.direction * enter;
			return true;
		}
		_hitPoint = Vector3.zero;
		return false;
	}

	public static bool SampleAndCalculatePath(NavMeshAgent _agent, Vector3 targetPosition, float maxSampleDistance, ref NavMeshPath _path)
	{
		NavMeshHit hit;
		if (NavMesh.SamplePosition(targetPosition, out hit, maxSampleDistance, _agent.areaMask))
		{
			_path = new NavMeshPath();
			return _agent.CalculatePath(hit.position, _path);
		}
		return false;
	}

	public static bool SampleAndCalculatePath(int _walkableMak, Vector3 startPosition, Vector3 targetPosition, float maxSampleDistance, ref NavMeshPath _path)
	{
		NavMeshHit hit;
		NavMeshHit hit2;
		if (NavMesh.SamplePosition(startPosition, out hit, maxSampleDistance, _walkableMak) && NavMesh.SamplePosition(targetPosition, out hit2, maxSampleDistance, _walkableMak))
		{
			_path = new NavMeshPath();
			return NavMesh.CalculatePath(hit.position, hit2.position, _walkableMak, _path);
		}
		return false;
	}

	public static bool CascadingSamplePosition(Vector3 _position, out NavMeshHit _hit, int _walkableMask)
	{
		if (NavMesh.SamplePosition(_position, out _hit, 0.5f, _walkableMask))
		{
			return true;
		}
		if (NavMesh.SamplePosition(_position, out _hit, 5f, _walkableMask))
		{
			return true;
		}
		if (NavMesh.SamplePosition(_position, out _hit, 20f, _walkableMask))
		{
			return true;
		}
		return false;
	}

	public static bool SafeRaycast(Vector3 _startPosition, Vector3 _targetPosition, out NavMeshHit _hit, int _passableMask)
	{
		bool num = NavMesh.Raycast(_startPosition, _targetPosition, out _hit, _passableMask);
		float num2 = 2f;
		float num3 = 0.2f;
		if (num && Mathf.Abs(_hit.position.y) < num3 && Mathf.Abs(_startPosition.y - _hit.position.y) > num2)
		{
			Vector3 position = _hit.position;
			position.y = _startPosition.y;
			NavMeshHit _hit2;
			if (CascadingSamplePosition(position, out _hit2, _passableMask))
			{
				position = _hit2.position;
			}
			_hit.position = position;
			_hit.distance = (position - _startPosition).magnitude;
		}
		return num;
	}
}
