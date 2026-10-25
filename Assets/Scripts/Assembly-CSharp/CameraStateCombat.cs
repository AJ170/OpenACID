using System.Collections.Generic;
using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic.Combat;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateCombat : CameraStateOrbit
{
	internal sealed class OrientationSortObject
	{
		public Quaternion Orientation { get; private set; }

		public int OccludedParticipantCount { get; private set; }

		public float Angle { get; private set; }

		public OrientationSortObject(Quaternion _orientation, int _occludedParticipantCount, float _angle)
		{
			Orientation = _orientation;
			OccludedParticipantCount = _occludedParticipantCount;
			Angle = _angle;
		}
	}

	[SerializeField]
	public CombatValues m_combatValues = new CombatValues();

	[SerializeField]
	public CameraAnimationCurve m_alignmentSpeed;

	private Vector3 m_combatTargetPosition;

	private Vector3 m_combatTargetDelta;

	private readonly List<Quaternion> m_orientationCandidates;

	private OrientationSortObject[] m_orientations;

	private Quaternion m_orientation;

	private int m_orientationOccludedParticipants;

	private float m_orientationLockTime = float.MinValue;

	protected bool m_wasRotating;

	private bool m_autoAlign;

	public override bool SpherecastAvoidance
	{
		get
		{
			return true;
		}
	}

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Combat;
		}
	}

	public CameraStateCombat()
	{
		m_orientationCandidates = new List<Quaternion>
		{
			Quaternion.Euler(0f, 0f, 0f),
			Quaternion.Euler(0f, 45f, 0f),
			Quaternion.Euler(0f, 90f, 0f),
			Quaternion.Euler(0f, 135f, 0f),
			Quaternion.Euler(0f, 180f, 0f),
			Quaternion.Euler(0f, 225f, 0f),
			Quaternion.Euler(0f, 270f, 0f),
			Quaternion.Euler(0f, 315f, 0f)
		};
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateCombat cameraStateCombat = (CameraStateCombat)_src;
		m_combatValues.CopyFrom(cameraStateCombat.m_combatValues);
		m_alignmentSpeed = cameraStateCombat.m_alignmentSpeed;
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		m_orientation = m_rotation;
		Vector3 position = AcierCameraController.NavMeshAgent.transform.position;
		m_combatTargetPosition = position;
		m_orientationLockTime = float.MinValue;
	}

	protected override void CalculateReference()
	{
		CombatController instance = CombatController.Instance;
		if (instance.PlayerParticipant != null)
		{
			CombatParticipant nextTarget = instance.PlayerParticipant.NextTarget;
			if (nextTarget != null && nextTarget.GameEntity.IsValid)
			{
				Vector3 vector = nextTarget.Position - instance.PlayerParticipant.Position;
				Vector3 normalized = vector.normalized;
				float magnitude = vector.magnitude;
				m_combatTargetPosition = instance.PlayerParticipant.Position + normalized * Mathf.Min(magnitude, m_combatValues.maxTargetDistance);
			}
		}
		m_combatTargetDelta = m_combatTargetPosition - m_agentPosition;
		m_referenceTargetPosition = m_agentPosition + m_combatTargetDelta * 0.5f;
		float num = Vector3.Distance(m_referenceDampedPosition, m_referenceTargetPosition);
		float num2 = m_combatValues.referenceMaxThreshold - m_combatValues.referenceMinThreshold;
		float speed = Mathf.Min(Mathf.Max(num - m_combatValues.referenceMinThreshold, 0f) / num2, 1f);
		m_referenceDampedPosition = Damping.Damp(m_referenceDampedPosition, m_referenceTargetPosition, m_orbitValues.referenceDamping, speed);
		if (CameraStateBase.Debugging)
		{
			DrawReferenceDebugging();
		}
	}

	protected override void CalculateRotation()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.isRotating)
		{
			CalculateZoomRotationWhenUserRotates();
			m_wasRotating = true;
			m_autoAlign = false;
		}
		else if (m_wasRotating)
		{
			CalculateBestOrientationWhenUserReleases();
			m_wasRotating = false;
		}
		CalculateZoomRotationDamp();
		if (!events.isRotating)
		{
			CalculateAutoAlignment();
		}
	}

	private void CalculateBestOrientationWhenUserReleases()
	{
		Quaternion orientation = m_rotation * Quaternion.Euler(0f, m_zoomRotation.y, 0f);
		if (IsAnyParticipantOccluded(orientation))
		{
			m_orientations = CalculateSortedOrientations();
			OrientationSortObject orientationSortObject = m_orientations[0];
			m_orientation = orientationSortObject.Orientation;
			m_orientationOccludedParticipants = orientationSortObject.OccludedParticipantCount;
			m_orientationLockTime = m_combatValues.orientationLockTimeWhenReleaseRotation;
			m_autoAlign = true;
		}
		else
		{
			m_orientationLockTime = 0f;
		}
	}

	private void CalculateAutoAlignment()
	{
		CameraEvents events = AcierCameraController.Events;
		m_orientationLockTime -= UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		if (!events.isRotating && m_orientationLockTime <= 0f)
		{
			Quaternion quaternion = Quaternion.Euler(0f, m_zoomRotationTarget.y, 0f);
			Quaternion orientation = m_rotation * quaternion;
			int num = OccludedParticipantCount(orientation);
			if (num > m_orientationOccludedParticipants)
			{
				m_orientations = CalculateSortedOrientations();
				OrientationSortObject orientationSortObject = m_orientations[0];
				if (orientationSortObject.OccludedParticipantCount < num)
				{
					Quaternion quaternion2 = Quaternion.Inverse(quaternion);
					m_orientation = orientationSortObject.Orientation * quaternion2;
					m_orientationOccludedParticipants = orientationSortObject.OccludedParticipantCount;
					m_orientationLockTime = float.MinValue;
					m_autoAlign = true;
				}
			}
		}
		float time = Quaternion.Angle(m_rotation, m_orientation);
		float t = ((!m_autoAlign) ? 0f : (m_alignmentSpeed.curve.Evaluate(time) * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime));
		m_rotation = Quaternion.Lerp(m_rotation, m_orientation, t);
	}

	private OrientationSortObject[] CalculateSortedOrientations()
	{
		List<OrientationSortObject> list = new List<OrientationSortObject>();
		for (int i = 0; i < m_orientationCandidates.Count; i++)
		{
			Quaternion orientation = m_orientationCandidates[i];
			OrientationSortObject item = RateOrientation(orientation);
			list.Add(item);
		}
		list.Sort(CompareOrientations);
		return list.ToArray();
	}

	private int CompareOrientations(OrientationSortObject _left, OrientationSortObject _right)
	{
		int num = _left.OccludedParticipantCount.CompareTo(_right.OccludedParticipantCount);
		if (num != 0)
		{
			return num;
		}
		return _left.Angle.CompareTo(_right.Angle);
	}

	private bool IsAnyParticipantOccluded(Quaternion _orientation)
	{
		CombatController instance = CombatController.Instance;
		int num = instance.ParticipantCount();
		Vector3 vector = m_orbitValues.cameraOffset + CalculateCameraOffset();
		Vector3 vector2 = m_referenceDampedPosition + _orientation * vector;
		for (int i = 0; i < num; i++)
		{
			CombatParticipant participant = instance.GetParticipant(i);
			Vector3 vector3 = participant.Position + Vector3.up * 1f;
			Vector3 vector4 = vector2 - vector3;
			Vector3 normalized = vector4.normalized;
			float maxDistance = vector4.magnitude + m_combatValues.minDistanceFromObstacles;
			if (Physics.Raycast(vector3, normalized, maxDistance, AcierCameraController.Instance.LayerMask))
			{
				return true;
			}
		}
		return false;
	}

	private int OccludedParticipantCount(Quaternion _orientation)
	{
		int num = 0;
		CombatController instance = CombatController.Instance;
		int num2 = instance.ParticipantCount();
		Vector3 vector = m_orbitValues.cameraOffset + CalculateCameraOffset();
		Vector3 vector2 = m_referenceDampedPosition + _orientation * vector;
		for (int i = 0; i < num2; i++)
		{
			CombatParticipant participant = instance.GetParticipant(i);
			Vector3 vector3 = participant.Position + Vector3.up * 1f;
			Vector3 vector4 = vector2 - vector3;
			Vector3 normalized = vector4.normalized;
			float maxDistance = vector4.magnitude + m_combatValues.minDistanceFromObstacles;
			if (Physics.Raycast(vector3, normalized, maxDistance, AcierCameraController.Instance.LayerMask))
			{
				num++;
			}
		}
		return num;
	}

	private OrientationSortObject RateOrientation(Quaternion _orientation)
	{
		CombatController instance = CombatController.Instance;
		int num = instance.ParticipantCount();
		Vector3 vector = m_orbitValues.cameraOffset + CalculateCameraOffset();
		Vector3 vector2 = m_referenceDampedPosition + _orientation * vector;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			CombatParticipant participant = instance.GetParticipant(i);
			Vector3 vector3 = participant.Position + Vector3.up * 1f;
			Vector3 vector4 = vector2 - vector3;
			Vector3 normalized = vector4.normalized;
			float maxDistance = vector4.magnitude + m_combatValues.minDistanceFromObstacles;
			if (Physics.Raycast(vector3, normalized, maxDistance, AcierCameraController.Instance.LayerMask))
			{
				num2++;
			}
		}
		Quaternion a = m_rotation * Quaternion.Euler(0f, m_zoomRotationTarget.y, 0f);
		float angle = Quaternion.Angle(a, _orientation);
		return new OrientationSortObject(_orientation, num2, angle);
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}
}
