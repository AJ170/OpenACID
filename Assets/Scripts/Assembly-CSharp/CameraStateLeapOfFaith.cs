using UnityEngine;

public class CameraStateLeapOfFaith : CameraStateOrbit
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.LeapOfFaith;
		}
	}

	public override bool Outlines
	{
		get
		{
			return false;
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
