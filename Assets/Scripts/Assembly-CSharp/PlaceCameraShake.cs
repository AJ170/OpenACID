using UnityEngine;

public class PlaceCameraShake : MonoBehaviour
{
	[SerializeField]
	private Vector2 animationRange;

	[SerializeField]
	private float animationTime;

	public void OnExplode()
	{
		AcierCameraController instance = AcierCameraController.Instance;
		if ((bool)instance)
		{
			float a = Vector3.Distance(Camera.main.transform.position, base.transform.position);
			a = Mathf.Max(a, 0.01f) / 100f;
			instance.StartAnimating(animationRange / a, animationTime);
		}
	}
}
