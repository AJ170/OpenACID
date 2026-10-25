using System.Collections;
using UnityEngine;

public class DeactivateAnchoringOnChildren : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(Delayed());
	}

	private IEnumerator Delayed()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Transform cachedTransform = base.transform;
		for (int i = 0; i < cachedTransform.childCount; i++)
		{
			UIWidget childWidget = cachedTransform.GetChild(i).GetComponent<UIWidget>();
			childWidget.leftAnchor = new UIRect.AnchorPoint(0f);
			childWidget.rightAnchor = new UIRect.AnchorPoint(0f);
			childWidget.topAnchor = new UIRect.AnchorPoint(0f);
			childWidget.bottomAnchor = new UIRect.AnchorPoint(0f);
		}
	}
}
