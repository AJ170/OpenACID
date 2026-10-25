using Assets.Scripts.Mission.Camera;
using UbiCore.Messenger;
using UnityEngine;
using UnityEngine.AI;

public class CameraStateVR : CameraStateRaycast
{
	public bool firstPersonMode;

	public Vector3 thirdPersonPosition = new Vector3(0f, 2f, -3f);

	private Vector2 maxDragDelta;

	private Vector3 relativePosition;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.VR;
		}
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		Transform transform = AcierCameraController.NavMeshAgent.transform;
		SetRelativePosition(transform.forward);
		m_outputPosition = transform.position + relativePosition;
		m_outputRotation = transform.rotation;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		Transform transform = AcierCameraController.NavMeshAgent.transform;
		DragEvent cameraDragEvent = AcierCameraController.Events.cameraDragEvent;
		if (AcierInputHandler.Instance.Gamepad.CameraRealignDown)
		{
			firstPersonMode = !firstPersonMode;
			SetRelativePosition(transform.forward);
		}
		if (cameraDragEvent != null)
		{
			if (maxDragDelta.sqrMagnitude < cameraDragEvent.Delta.sqrMagnitude)
			{
				maxDragDelta = cameraDragEvent.Delta;
			}
		}
		else if (maxDragDelta != Vector2.zero)
		{
			Vector3 forward = base.transform.TransformVector(maxDragDelta.x, 0f, maxDragDelta.y);
			forward.Normalize();
			if (!firstPersonMode)
			{
				SetRelativePosition(forward);
			}
			m_outputRotation = Quaternion.LookRotation(forward);
			maxDragDelta = Vector2.zero;
		}
		m_outputPosition = transform.position + relativePosition;
	}

	public override void EndState()
	{
	}

	protected override Vector3 CalculateRaycastStart()
	{
		Vector3 position = AcierCameraController.NavMeshAgent.transform.position;
		position.y += m_raycastValues.eyeHeight;
		return position;
	}

	private void SetRelativePosition(Vector3 direction)
	{
		relativePosition = ((!firstPersonMode) ? (direction * thirdPersonPosition.z + Vector3.up * thirdPersonPosition.y) : (Vector3.up * m_raycastValues.eyeHeight));
	}
}
