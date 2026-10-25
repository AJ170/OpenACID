using Assets.Scripts.Mission.Camera;
using UnityEngine;

public class CameraStateClimb : CameraStateAutoRotation
{
	[SerializeField]
	public ClimbValues m_climbValues = new ClimbValues();

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Climb;
		}
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateClimb cameraStateClimb = (CameraStateClimb)_src;
		m_climbValues.CopyFrom(cameraStateClimb.m_climbValues);
	}

	protected override void FetchAgentFacing()
	{
		AnimationTaskClimb animationTaskClimb = CameraStateHelper.CurrentAnimationTask as AnimationTaskClimb;
		if (animationTaskClimb != null)
		{
			m_agentFacing = FlatDirection(animationTaskClimb.OffMeshLinkEntity.Rotation);
		}
		if (!CameraStateBase.Debugging)
		{
		}
	}

	protected override void CalculateReference()
	{
		m_referenceTargetPosition = AcierCameraController.Animator.GetBoneTransform(HumanBodyBones.Spine).position;
		base.CalculateReference();
	}

	protected override Vector3 CalculateRealignDirection()
	{
		if (Vector3.Angle(m_cameraForward, m_agentFacing) > m_climbValues.maxUserYawAngle)
		{
			Vector3 vector = Quaternion.Euler(0f, m_climbValues.maxUserYawAngle, 0f) * m_agentFacing;
			Vector3 vector2 = Quaternion.Euler(0f, 0f - m_climbValues.maxUserYawAngle, 0f) * m_agentFacing;
			float num = Vector3.Angle(m_cameraForward, vector);
			float num2 = Vector3.Angle(m_cameraForward, vector2);
			return (!(num < num2)) ? vector2 : vector;
		}
		return m_cameraForward;
	}
}
