using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraAnimator : MonoBehaviour
{
	public Camera TargetCamera;

	private static int ms_idAspect;

	private static int ms_idFarClipPlane;

	private static int ms_idNearClipPlane;

	private static int ms_idFieldOfView;

	static CameraAnimator()
	{
		ms_idAspect = Animator.StringToHash("CameraAspect");
		ms_idFarClipPlane = Animator.StringToHash("CameraFarClipPlane");
		ms_idNearClipPlane = Animator.StringToHash("CameraNearClipPlane");
		ms_idFieldOfView = Animator.StringToHash("CameraFieldOfView");
	}

	private void Update()
	{
		if (!(TargetCamera != null))
		{
			return;
		}
		Animator component = GetComponent<Animator>();
		if (component != null)
		{
			if (component.IsParameterControlledByCurve(ms_idAspect))
			{
				TargetCamera.aspect = component.GetFloat(ms_idAspect);
			}
			if (component.IsParameterControlledByCurve(ms_idFarClipPlane))
			{
				TargetCamera.farClipPlane = component.GetFloat(ms_idFarClipPlane);
			}
			if (component.IsParameterControlledByCurve(ms_idNearClipPlane))
			{
				TargetCamera.nearClipPlane = component.GetFloat(ms_idNearClipPlane);
			}
			if (component.IsParameterControlledByCurve(ms_idFieldOfView))
			{
				TargetCamera.fieldOfView = component.GetFloat(ms_idFieldOfView);
			}
		}
	}
}
