using UnityEngine;

public class TextureOffset : MonoBehaviour
{
	[SerializeField]
	private bool uDirection;

	[SerializeField]
	private bool vDirection;

	[SerializeField]
	private float uSpeed;

	[SerializeField]
	private float vSpeed;

	private float uProgress;

	private float vProgress;

	private void Update()
	{
		if (uDirection)
		{
			uProgress += uSpeed * Time.deltaTime;
		}
		if (vDirection)
		{
			vProgress += vSpeed * Time.deltaTime;
		}
		base.gameObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(uProgress, vProgress));
	}
}
