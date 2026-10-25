using UnityEngine;

public class CameraStateJumpDown_Florent : CameraStateBase_Florent
{
	public bool EnableLookAt = true;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.JumpDown;
		}
	}

	public CameraStateJumpDown_Florent()
	{
		LeadDistance = 0f;
		LeadDamping = 0.5f;
		PositionDamping = 0.5f;
		PositionOffset = new Vector3(0f, 6f, -1f);
		PositionOffsetForward = new Vector3(0f, 6f, -1f);
		PositionOffsetBack = new Vector3(0f, 6f, -1f);
		LookAtDamping = 0.1f;
		LookAtOffset = new Vector3(0f, -1f, 2f);
		LookAtOffsetForward = new Vector3(0f, -1f, 2f);
		LookAtOffsetBack = new Vector3(0f, -1f, 2f);
		ZoomDamping = 0.1f;
		ZoomDragDistance = 0.25f;
		RotateDragDistance = 0.45f;
		AutoRealign = false;
		MaxAngleToRealign = 180f;
		RealignDamping = 0.1f;
		MinRealignSpeed = 0.6f;
		MaxRealignSpeed = 1f;
		Fov = 60f;
		FovForward = 70f;
		FovBack = 70f;
	}

	protected override float GetAutoRealignFactor()
	{
		return 1f;
	}
}
