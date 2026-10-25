using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.StateMachines;
using UbiCore.Utils;
using UnityEngine;

public abstract class CameraStateAutoRotation : CameraStateOrbit
{
	[SerializeField]
	protected CameraAnimationCurve m_alignmentSpeedLowProfile;

	[SerializeField]
	protected CameraAnimationCurve m_alignmentSpeedHighProfile;

	[SerializeField]
	protected CameraAnimationCurve m_alignmentSpeedChase;

	[SerializeField]
	protected float m_realignMaxPitchStart = 45f;

	[SerializeField]
	protected float m_realignMaxPitchEnd = 60f;

	private static AnimationCurve m_defaultAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateAutoRotation cameraStateAutoRotation = (CameraStateAutoRotation)_src;
		m_alignmentSpeedLowProfile = cameraStateAutoRotation.m_alignmentSpeedLowProfile;
		m_alignmentSpeedHighProfile = cameraStateAutoRotation.m_alignmentSpeedHighProfile;
		m_alignmentSpeedChase = cameraStateAutoRotation.m_alignmentSpeedChase;
	}

	protected override void Realign()
	{
		if (m_agentFacing.sqrMagnitude > 1E-05f)
		{
			m_rotation = Quaternion.LookRotation(m_agentFacing);
		}
		m_cameraDampedPosition = m_outputPosition;
		Vector3 vector = CalculateLookAtOffset();
		Quaternion quaternion = m_rotation * Quaternion.Euler(0f, m_zoomRotation.y, 0f);
		m_lookAtDampedPosition += quaternion * vector;
		m_zoomRotation = (m_zoomRotationTarget = Vector2.zero);
	}

	protected override void CalculateRotation()
	{
		CameraEvents events = AcierCameraController.Events;
		if (events.isRotating)
		{
			CalculateZoomRotationWhenUserRotates();
		}
		CalculateZoomRotationDamp();
		if (!events.isRotating)
		{
			CalculateAutoAlignment();
		}
	}

	public AnimationCurve GetBestAlignmentSpeedCurve()
	{
		switch (PlayerHelper.PlayerProfile.CurrentStateType)
		{
		case EStateType.PlayerProfile_Low:
			return (!(m_alignmentSpeedLowProfile != null)) ? m_defaultAnimationCurve : m_alignmentSpeedLowProfile.curve;
		case EStateType.PlayerProfile_High:
			return (!(m_alignmentSpeedHighProfile != null)) ? m_defaultAnimationCurve : m_alignmentSpeedHighProfile.curve;
		case EStateType.PlayerProfile_Chase:
			return (!(m_alignmentSpeedChase != null)) ? m_defaultAnimationCurve : m_alignmentSpeedChase.curve;
		default:
			return m_defaultAnimationCurve;
		}
	}

	protected virtual void CalculateAutoAlignment()
	{
		CalculateAutoAlignment(GetBestAlignmentSpeedCurve());
	}

	protected void CalculateAutoAlignment(AnimationCurve _curve)
	{
		Vector3 vector = CalculateRealignDirection();
		float time = Vector3.Angle(m_cameraForward, vector);
		float num = _curve.Evaluate(time) * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime * AcierCameraController.RealignSpeedMultiplier;
		float num2 = 90f - Vector3.Angle(Vector3.down, base.transform.forward);
		float num3 = 1f - Mathf.Clamp01((num2 - m_realignMaxPitchStart) / (m_realignMaxPitchEnd - m_realignMaxPitchStart));
		if (vector.sqrMagnitude > 1E-05f)
		{
			Quaternion rotation = Quaternion.Euler(0f, m_zoomRotation.y, 0f);
			if (vector.sqrMagnitude > 1E-05f)
			{
				Quaternion b = Quaternion.LookRotation(vector) * Quaternion.Inverse(rotation);
				m_rotation = Quaternion.Slerp(m_rotation, b, Mathf.Clamp01(num * num3));
			}
		}
	}

	protected virtual Vector3 CalculateRealignDirection()
	{
		return m_agentFacing;
	}

	public Vector3 FlatDirection(Quaternion _rotation)
	{
		Vector3 vector = _rotation * Vector3.forward;
		vector.y = 0f;
		return vector.normalized;
	}
}
