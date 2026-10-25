using Assets.Scripts.Mission.Motion;
using UbiCore.Debug;
using UnityEngine;
using UnityEngine.AI;

public class RoofEdgeDetection : MonoBehaviour
{
	public static float IdleDelay = 0.3f;

	public static float TriggerDelay = 0.1f;

	public static float RepeatDelay = 1f;

	public static float MinHeight = 3f;

	public static float MaxEdgeDistance = 2f;

	public static float MaxAngle = 60f;

	public static float MaxCameraAngleToEnter = 110f;

	[SerializeField]
	private float m_triggerMinTime;

	[SerializeField]
	private float m_lastIdleTime;

	[SerializeField]
	private Vector3 m_lastDirection;

	public Vector3 RoofEdgeNormal { get; private set; }

	public float RoofEdgeHeight { get; private set; }

	public bool AtRoofEdge { get; private set; }

	public bool WasAtRoofEdge { get; private set; }

	public void Reset()
	{
		RoofEdgeNormal = Vector3.zero;
		RoofEdgeHeight = 0f;
		AtRoofEdge = false;
		WasAtRoofEdge = false;
		m_lastDirection = Vector3.zero;
	}

	public void UpdateRoofEdgeDetection(Vector3 _direction)
	{
		WasAtRoofEdge = AtRoofEdge;
		Profiling.Start("Check roof edge mode", float.MaxValue);
		Vector3 _roofNormal = RoofEdgeNormal;
		float _roofHeight = RoofEdgeHeight;
		bool flag = AtRoofEdge;
		bool flag2 = _direction.magnitude < 0.5f && !LogicMain.PlayerEntity.UnityComponentCache.NavMeshAgent.hasPath;
		if (!flag2 && LogicMain.CurrentTime > m_lastIdleTime + TriggerDelay)
		{
			m_triggerMinTime = LogicMain.CurrentTime + TriggerDelay;
		}
		if (!flag2)
		{
			m_lastDirection = _direction;
		}
		if (flag2)
		{
			m_lastIdleTime = LogicMain.CurrentTime;
		}
		if (m_lastDirection.magnitude > 0.5f)
		{
			_direction = m_lastDirection;
			flag = ParcourHelper.IsAtRoofEdge(LogicMain.PlayerEntity, _direction, out _roofNormal, out _roofHeight, MaxEdgeDistance) && Vector3.Angle(_direction, -_roofNormal) < MaxAngle && _roofHeight > MinHeight;
		}
		ParcourPrediction parcourPrediction = AcierInputHandler.Instance.ParcourPrediction;
		if (!flag && parcourPrediction.HasInteraction && parcourPrediction.DistanceToStart < MaxEdgeDistance && parcourPrediction.EndPosition.y - parcourPrediction.StartPosition.y > MinHeight)
		{
			Vector3 vector = parcourPrediction.StartPosition - LogicMain.PlayerEntity.Position;
			vector.y = 0f;
			_direction = vector.normalized;
			float magnitude = vector.magnitude;
			flag = ParcourHelper.IsAtRoofEdge(LogicMain.PlayerEntity, _direction, out _roofNormal, out _roofHeight, magnitude) && Vector3.Angle(_direction, -_roofNormal) < MaxAngle && _roofHeight > MinHeight;
		}
		if (!flag)
		{
			_direction = LogicMain.PlayerEntity.Rotation * Vector3.forward;
			if (ParcourHelper.IsAtRoofEdge(LogicMain.PlayerEntity, _direction, out _roofNormal, out _roofHeight, MaxEdgeDistance) && Vector3.Angle(_direction, -_roofNormal) < MaxAngle && _roofHeight > MinHeight)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (!WasAtRoofEdge)
			{
				Vector3 forward = AcierCameraController.Instance.transform.forward;
				forward.y = 0f;
				float num = Vector3.Angle(forward.normalized, -_roofNormal);
				if (LogicMain.CurrentTime < m_triggerMinTime || num > MaxCameraAngleToEnter)
				{
					AtRoofEdge = false;
				}
				else
				{
					AtRoofEdge = true;
					RoofEdgeNormal = _roofNormal;
					RoofEdgeHeight = _roofHeight;
				}
			}
			else
			{
				AtRoofEdge = true;
				RoofEdgeNormal = _roofNormal;
				RoofEdgeHeight = _roofHeight;
			}
		}
		else
		{
			AtRoofEdge = false;
		}
		if (AtRoofEdge)
		{
			m_lastDirection = Vector3.zero;
		}
		if (!AtRoofEdge && WasAtRoofEdge)
		{
			m_triggerMinTime = LogicMain.CurrentTime + RepeatDelay;
		}
		Profiling.Stop();
	}
}
