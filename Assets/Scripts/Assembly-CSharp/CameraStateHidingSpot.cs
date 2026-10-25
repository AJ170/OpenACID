using System.Collections.Generic;
using Assets.Scripts.Mission.Camera;
using UnityEngine;

public abstract class CameraStateHidingSpot : CameraStateOrbit
{
	internal sealed class OrientationSortObject
	{
		public Quaternion Orientation { get; private set; }

		public float Angle { get; private set; }

		public bool Hit { get; private set; }

		public OrientationSortObject(Quaternion _orientation, float _angle, bool _hit)
		{
			Orientation = _orientation;
			Angle = _angle;
			Hit = _hit;
		}
	}

	protected List<Quaternion> m_orientationCandidates;

	private OrientationSortObject[] m_orientations;

	private Quaternion m_orientation;

	public abstract Vector3 CalculateHidingSpotPosition();

	public abstract Quaternion CalculateHidingSpotRotation();

	public virtual Vector3 CalculateHidingSpotReferencePosition()
	{
		return CalculateHidingSpotPosition();
	}

	protected override void CalculateReference()
	{
		m_referenceDampedPosition = (m_referenceTargetPosition = CalculateHidingSpotReferencePosition());
		if (CameraStateBase.Debugging)
		{
			DrawReferenceDebugging();
		}
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		CalculateReference();
		CameraEvents events = AcierCameraController.Events;
		if (!events.isRotating)
		{
			CalculateBestOrientation();
			m_rotation = m_orientation;
		}
	}

	private void CalculateBestOrientation()
	{
		m_orientations = CalculateSortedOrientations();
		OrientationSortObject orientationSortObject = m_orientations[0];
		m_orientation = orientationSortObject.Orientation;
	}

	private OrientationSortObject[] CalculateSortedOrientations()
	{
		Quaternion quaternion = CalculateHidingSpotRotation();
		List<OrientationSortObject> list = new List<OrientationSortObject>();
		for (int i = 0; i < m_orientationCandidates.Count; i++)
		{
			Quaternion quaternion2 = m_orientationCandidates[i];
			OrientationSortObject item = RateOrientation(quaternion2 * quaternion);
			list.Add(item);
		}
		list.Sort(CompareOrientations);
		return list.ToArray();
	}

	private int CompareOrientations(OrientationSortObject _left, OrientationSortObject _right)
	{
		int num = _left.Hit.CompareTo(_right.Hit);
		if (num != 0)
		{
			return num;
		}
		return _left.Angle.CompareTo(_right.Angle);
	}

	private OrientationSortObject RateOrientation(Quaternion _orientation)
	{
		Quaternion a = m_rotation * Quaternion.Euler(0f, m_zoomRotationTarget.y, 0f);
		Vector3 vector = m_referenceDampedPosition + _orientation * m_orbitValues.cameraOffset;
		Vector3 referenceDampedPosition = m_referenceDampedPosition;
		referenceDampedPosition.y += m_raycastValues.eyeHeight;
		Vector3 vector2 = vector - referenceDampedPosition;
		Vector3 normalized = vector2.normalized;
		float maxDistance = vector2.magnitude + 1f;
		RaycastHit hitInfo;
		bool hit = Physics.Raycast(referenceDampedPosition, normalized, out hitInfo, maxDistance, AcierCameraController.Instance.LayerMask);
		float angle = Quaternion.Angle(a, _orientation);
		return new OrientationSortObject(_orientation, angle, hit);
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (CameraStateBase.Debugging)
		{
			DrawDebugOrientations(m_orientations);
		}
	}

	private void DrawDebugOrientations(OrientationSortObject[] _orientations)
	{
		if (_orientations != null)
		{
			foreach (OrientationSortObject orientationSortObject in _orientations)
			{
				Vector3 vector = m_referenceDampedPosition + orientationSortObject.Orientation * m_orbitValues.cameraOffset;
				Vector3 referenceDampedPosition = m_referenceDampedPosition;
				referenceDampedPosition.y += m_raycastValues.eyeHeight;
				Color color = ((!orientationSortObject.Hit) ? Color.green : Color.red);
			}
		}
	}
}
