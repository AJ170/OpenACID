using UnityEngine;

public class rendererFront : MonoBehaviour
{
	public TrailRenderer trailRenderer;

	private void Start()
	{
		if ((bool)trailRenderer)
		{
			trailRenderer.GetComponent<Renderer>().sortingLayerName = "Foreground";
			trailRenderer.GetComponent<Renderer>().sortingOrder = 2;
		}
	}
}
