using Assets.Scripts.Mission.Camera;
using UnityEngine;

public abstract class CameraStateRaycast : CameraStateBase
{
	[SerializeField]
	protected RaycastValues m_raycastValues;

	private Vector3 m_raycastPosition;

	protected Ray m_ray;

	protected RaycastHit m_hitInfo;

	public override Vector3 CurrentRaycastPosition
	{
		get
		{
			return m_raycastPosition;
		}
	}

	public virtual RaycastValues Values
	{
		get
		{
			return m_raycastValues;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateRaycast cameraStateRaycast = (CameraStateRaycast)_src;
		m_raycastValues = cameraStateRaycast.m_raycastValues;
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		if (m_raycastValues == null)
		{
			m_raycastValues = AcierCameraController.Instance.GlobalValues.m_raycastValues;
		}
	}

	protected abstract Vector3 CalculateRaycastStart();

	public override void UpdateState()
	{
		base.UpdateState();
		m_raycastPosition = CalculateRaycastStart();
	}
}
