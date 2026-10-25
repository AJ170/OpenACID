using UnityEngine;

public class ChangeRenderQueue : MonoBehaviour
{
	public int depth;

	[TooltipAttribute("Assign -1 if you don't want to change the sortingOrder.")]
	public int sortingOrder = -1;

	public Material Mat;

	private void Start()
	{
		Renderer component = GetComponent<Renderer>();
		if (component == null && (Mat == null || sortingOrder >= 0))
		{
			Debug.LogWarning("No Renderer assigned to this object.");
			return;
		}
		if (Mat != null)
		{
			Mat.renderQueue = depth;
		}
		else if (component != null)
		{
			component.material.renderQueue = depth;
		}
		if (sortingOrder >= 0 && component != null)
		{
			component.sortingOrder = sortingOrder;
		}
	}
}
