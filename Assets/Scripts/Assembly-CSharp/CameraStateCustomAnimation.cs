using Assets.Scripts.Mission.Camera;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

public class CameraStateCustomAnimation : CameraStateBase
{
	private Animation m_animation;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.CustomAnimated;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		CameraCustomAnimationHelper cameraCustomAnimationHelper = Main.Instance.CameraCustomAnimationHelper;
		Transform transform = cameraCustomAnimationHelper.Animation.transform;
		if (transform != null && cameraCustomAnimationHelper.CurrentClip != null)
		{
			m_animation = cameraCustomAnimationHelper.Animation;
			m_animation.RemoveAnimationClip();
			if (m_animation != null)
			{
				m_animation.AddClip(cameraCustomAnimationHelper.CurrentClip, cameraCustomAnimationHelper.CurrentClip.name);
				m_animation.Play(cameraCustomAnimationHelper.CurrentClip.name);
				m_animation.Sample();
				m_outputFieldOfView = transform.localScale.x;
			}
			m_outputPosition = transform.position;
			m_outputRotation = transform.rotation;
		}
	}

	public override void EndState()
	{
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (m_animation != null)
		{
			m_outputPosition = m_animation.transform.position;
			m_outputRotation = m_animation.transform.rotation;
			float x = m_animation.transform.localScale.x;
			m_outputFieldOfView = ((!(x > 1f)) ? m_outputFieldOfView : x);
			if (!m_animation.isPlaying)
			{
				CameraCustomAnimationHelper.ShouldActivateCamera = false;
			}
		}
	}
}
