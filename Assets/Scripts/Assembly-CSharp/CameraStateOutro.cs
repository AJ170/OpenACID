using Assets.Scripts.Mission.Camera;
using UnityEngine;

public class CameraStateOutro : CameraStateBase
{
	private Animation m_animation;

	private BarrierManager m_cachedBarrierMgr;

	public override bool DrawAvatar
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
			return AcierCameraController.ECameraState.Outro;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		m_animation = Outro.Instance.gameObject.GetComponent<Animation>();
		m_animation.Sample();
		m_outputPosition = m_animation.transform.position;
		m_outputRotation = m_animation.transform.rotation;
		m_outputFieldOfView = Outro.Instance.TargetCameraFov;
		m_cachedBarrierMgr = Object.FindObjectOfType<BarrierManager>();
		if (m_cachedBarrierMgr != null)
		{
			m_cachedBarrierMgr.SetVisible(false);
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		m_outputPosition = m_animation.transform.position;
		m_outputRotation = m_animation.transform.rotation;
		m_outputFieldOfView = Outro.Instance.TargetCameraFov;
	}

	public override void EndState()
	{
		if (m_cachedBarrierMgr != null)
		{
			m_cachedBarrierMgr.SetVisible(true);
		}
	}
}
