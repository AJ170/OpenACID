public class CameraStateRoof : CameraStateRoaming
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Roof;
		}
	}

	public override AcierCameraController.ECameraState TransitionStateType
	{
		get
		{
			return AcierCameraController.ECameraState.Roaming;
		}
	}
}
