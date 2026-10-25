using UbiCore.Utils;
using UnityEngine;

public class CameraStateRoaming_Florent : CameraStateBase_Florent
{
	public bool EnableLookAt = true;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Roaming;
		}
	}

	public CameraStateRoaming_Florent()
	{
		LeadDistance = 1.75f;
		LeadDamping = 0.5f;
		PositionDamping = 0.1f;
		PositionOffset = new Vector3(0f, 2f, -3.5f);
		PositionOffsetForward = new Vector3(0f, 12f, -6f);
		PositionOffsetBack = new Vector3(0f, 2f, -1f);
		LookAtDamping = 0.1f;
		LookAtOffset = new Vector3(0f, 0.5f, 1f);
		LookAtOffsetForward = new Vector3(0f, 6f, 0f);
		LookAtOffsetBack = new Vector3(0f, 2f, 1f);
		ZoomDamping = 0.1f;
		ZoomDragDistance = 0.25f;
		RotateDragDistance = 0.45f;
		AutoRealign = true;
		MaxAngleToRealign = 120f;
		RealignDamping = 0.2f;
		MinRealignSpeed = 0.1f;
		MaxRealignSpeed = 0.6f;
		Fov = 60f;
		FovForward = 70f;
		FovBack = 70f;
	}

	protected override void RetargetCharacterLookAt()
	{
		if (EnableLookAt)
		{
			NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
			m_characterLookAtWeight = Mathf.Lerp(m_characterLookAtWeight, (!navMeshAgent.hasPath) ? 1f : 0f, GetDampingFactor(UbiCore.Utils.Singleton<Timer>.Instance.deltaTime, 1f, 0.3f));
			Vector3 vector = CurrentRotation * Vector3.forward;
			vector.y = 0f;
			vector.Normalize();
			Animator animator = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.Animator;
			animator.SetLookAtWeight(m_characterLookAtWeight, 0.3f, 0.9f, 1f, 1f);
			animator.SetLookAtPosition(navMeshAgent.transform.position + Vector3.up * 1.8f + vector * 20f - Vector3.up * Mathf.Clamp01(m_zoomAmount) * 20f);
		}
	}
}
