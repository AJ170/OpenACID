using UnityEngine;

namespace UbiCore.CameraController
{
	public abstract class CameraController : MonoBehaviour
	{
		public abstract bool suppressesInteraction { get; }

		public static bool ToggleCameraController(bool state)
		{
			Camera main = Camera.main;
			bool flag = false;
			if (main != null)
			{
				CameraController component = main.GetComponent<CameraController>();
				if (component != null)
				{
					flag = flag || component.enabled;
					component.enabled = state;
				}
			}
			return flag;
		}
	}
}
