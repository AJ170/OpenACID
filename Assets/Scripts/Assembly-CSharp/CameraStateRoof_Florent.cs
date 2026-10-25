using UnityEngine;

public class CameraStateRoof_Florent : CameraStateBase_Florent
{
	public bool EnableLookAt = true;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Roof;
		}
	}

	public CameraStateRoof_Florent()
	{
		LeadDistance = 0.8f;
		LeadDamping = 0.8f;
		PositionDamping = 0.1f;
		PositionOffset = new Vector3(0f, 5f, -4f);
		PositionOffsetForward = new Vector3(0f, 6f, 1f);
		PositionOffsetBack = new Vector3(0f, 2f, -1f);
		LookAtDamping = 0.1f;
		LookAtOffset = new Vector3(0f, 1f, 1f);
		LookAtOffsetForward = new Vector3(0f, 0f, 2f);
		LookAtOffsetBack = new Vector3(0f, 2f, 1f);
		ZoomDamping = 0.1f;
		ZoomDragDistance = 0.25f;
		RotateDragDistance = 0.45f;
		AutoRealign = true;
		MaxAngleToRealign = 120f;
		RealignDamping = 0.3f;
		MinRealignSpeed = 0.1f;
		MaxRealignSpeed = 0.4f;
		Fov = 60f;
		FovForward = 70f;
		FovBack = 70f;
	}
}
