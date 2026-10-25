using UnityEngine;

public class CameraStateJumpDown : CameraStateOrbit
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.JumpDown;
		}
	}

	protected override void CalculateReference()
	{
		m_referenceTargetPosition = AcierCameraController.Animator.GetBoneTransform(HumanBodyBones.Spine).position;
		m_referenceDampedPosition = Damping.Damp(m_referenceDampedPosition, m_referenceTargetPosition, m_orbitValues.referenceDamping, 1f);
		if (CameraStateBase.Debugging)
		{
			DrawReferenceDebugging();
		}
	}
}
