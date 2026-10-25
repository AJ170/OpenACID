using UnityEngine;

public class SetTextureColorHelper : MonoBehaviour
{
	public Color color = default(Color);

	private void Update()
	{
		GetComponent<Renderer>().material.SetColor("_TintColor", color);
	}
}
