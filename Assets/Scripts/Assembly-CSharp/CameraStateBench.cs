using System.Collections.Generic;
using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateBench : CameraStateHidingSpot
{
	[SerializeField]
	public BenchValues m_benchValues = new BenchValues();

	private Bench m_cachedBench;

	public override bool Outlines
	{
		get
		{
			return false;
		}
	}

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
			return AcierCameraController.ECameraState.Bench;
		}
	}

	private Bench cachedBench
	{
		get
		{
			Bench current = UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.Current;
			if (current == null)
			{
				return m_cachedBench;
			}
			return m_cachedBench = current;
		}
	}

	public CameraStateBench()
	{
		m_orientationCandidates = new List<Quaternion>
		{
			Quaternion.Euler(0f, 0f, 0f),
			Quaternion.Euler(0f, 90f, 0f),
			Quaternion.Euler(0f, 180f, 0f)
		};
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateBench cameraStateBench = (CameraStateBench)_src;
		m_benchValues.CopyFrom(cameraStateBench.m_benchValues);
	}

	public override void EndState()
	{
		m_cachedBench = null;
	}

	public override Vector3 CalculateHidingSpotPosition()
	{
		return cachedBench.InteractionLocators.Center.transform.position;
	}

	public override Quaternion CalculateHidingSpotRotation()
	{
		return cachedBench.InteractionLocators.Center.transform.rotation;
	}

	public override Vector3 CalculateHidingSpotReferencePosition()
	{
		return CalculateHidingSpotPosition() + CalculateHidingSpotRotation() * m_benchValues.benchCenterOffset;
	}
}
