using Assets.Scripts.Mission.Camera;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateDead : CameraStateOrbit
{
	[SerializeField]
	// The original initialised this with `new DeadValues()`, a detached MonoBehaviour that
	// compares equal to null, so StartState always took the global values. Unity 2017 rejects
	// that construction during deserialisation; leaving it null keeps the same behaviour.
	public DeadValues m_DeadValues;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Dead;
		}
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		if (m_DeadValues == null)
		{
			m_DeadValues = AcierCameraController.Instance.GlobalValues.m_deadValues;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateDead cameraStateDead = (CameraStateDead)_src;
		m_DeadValues = cameraStateDead.m_DeadValues;
	}

	protected override void CalculateReference()
	{
		m_referenceTargetPosition = AcierCameraController.AgentDataFilter.Position;
		base.CalculateReference();
	}

	protected override void CalculateRotation()
	{
		float num = UbiCore.Utils.Singleton<Timer>.Instance.deltaTime / UbiCore.Utils.Singleton<Timer>.Instance.timeScale;
		m_rotation *= Quaternion.AngleAxis(num * m_DeadValues.m_rotationSpeed, Vector3.up);
		base.CalculateRotation();
	}
}
