using Assets.Scripts.Mission.Camera;
using UnityEngine;

public abstract class CameraStateBase : MonoBehaviour, ICameraState
{
	[SerializeField]
	protected FieldOfViewValues m_fieldOfViewValues;

	protected Vector3 m_outputPosition;

	protected Quaternion m_outputRotation;

	protected float m_outputFieldOfView;

	public static bool Debugging;

	public Vector3 CurrentPosition
	{
		get
		{
			return m_outputPosition;
		}
	}

	public virtual Vector3 CurrentRaycastPosition
	{
		get
		{
			return m_outputPosition;
		}
	}

	public Quaternion CurrentRotation
	{
		get
		{
			return m_outputRotation;
		}
	}

	public float CurrentFieldOfView
	{
		get
		{
			return m_outputFieldOfView;
		}
	}

	public virtual bool Outlines
	{
		get
		{
			return true;
		}
	}

	public virtual bool DrawAvatar
	{
		get
		{
			return true;
		}
	}

	public virtual bool SpherecastAvoidance
	{
		get
		{
			return false;
		}
	}

	public virtual AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.None;
		}
	}

	public virtual AcierCameraController.ECameraState TransitionStateType
	{
		get
		{
			return StateType;
		}
	}

	public virtual void CopyFrom(ICameraState _src)
	{
		CameraStateBase cameraStateBase = (CameraStateBase)_src;
		m_fieldOfViewValues = cameraStateBase.m_fieldOfViewValues;
	}

	public virtual void StartState(ICameraState _previous)
	{
		if (m_fieldOfViewValues == null)
		{
			m_fieldOfViewValues = AcierCameraController.Instance.GlobalValues.m_fieldOfViewValues;
		}
	}

	public virtual void UpdateState()
	{
		m_outputFieldOfView = m_fieldOfViewValues.fieldOfView;
	}

	public abstract void EndState();
}
