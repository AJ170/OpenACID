using UnityEngine;

public class CameraStateClimb_Florent : CameraStateBase_Florent
{
	public bool EnableLookAt = true;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Climb;
		}
	}

	public CameraStateClimb_Florent()
	{
		LeadDistance = 0f;
		LeadDamping = 0.5f;
		PositionDamping = 0.1f;
		PositionOffset = new Vector3(0f, 4f, -5f);
		PositionOffsetForward = new Vector3(0f, 8f, -5f);
		PositionOffsetBack = new Vector3(0f, 4f, -6f);
		LookAtDamping = 0.1f;
		LookAtOffset = new Vector3(0f, 1f, 0f);
		LookAtOffsetForward = new Vector3(0f, 0f, 4f);
		LookAtOffsetBack = new Vector3(0f, -4f, 0f);
		ZoomDamping = 0.1f;
		ZoomDragDistance = 0.25f;
		RotateDragDistance = 0.45f;
		AutoRealign = true;
		MaxAngleToRealign = 180f;
		RealignDamping = 0.2f;
		MinRealignSpeed = 0.3f;
		MaxRealignSpeed = 0.6f;
		Fov = 60f;
		FovForward = 70f;
		FovBack = 70f;
	}

	protected override float GetAutoRealignFactor()
	{
		return 1f;
	}
}
