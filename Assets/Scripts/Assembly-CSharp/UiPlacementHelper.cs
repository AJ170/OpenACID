using UnityEngine;

public class UiPlacementHelper : MonoBehaviour
{
	public Vector3 startOffset;

	public Vector3 startScale;

	private void Start()
	{
		base.transform.localPosition = startOffset;
		base.transform.localScale = startScale;
	}
}
