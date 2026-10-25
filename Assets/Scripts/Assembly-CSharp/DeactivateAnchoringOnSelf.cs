using System.Collections;
using UnityEngine;

public class DeactivateAnchoringOnSelf : MonoBehaviour
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
		if (base.transform.GetComponent<UIWidget>() != null)
		{
			UIWidget widget = base.transform.GetComponent<UIWidget>();
			widget.leftAnchor = new UIRect.AnchorPoint(0f);
			widget.rightAnchor = new UIRect.AnchorPoint(0f);
			widget.topAnchor = new UIRect.AnchorPoint(0f);
			widget.bottomAnchor = new UIRect.AnchorPoint(0f);
		}
		if (base.transform.GetComponent<UIPanel>() != null)
		{
			UIPanel panel = base.transform.GetComponent<UIPanel>();
			panel.leftAnchor = new UIRect.AnchorPoint(0f);
			panel.rightAnchor = new UIRect.AnchorPoint(0f);
			panel.topAnchor = new UIRect.AnchorPoint(0f);
			panel.bottomAnchor = new UIRect.AnchorPoint(0f);
		}
		if (base.transform.GetComponent<UISprite>() != null)
		{
			UISprite sprite = base.transform.GetComponent<UISprite>();
			sprite.leftAnchor = new UIRect.AnchorPoint(0f);
			sprite.rightAnchor = new UIRect.AnchorPoint(0f);
			sprite.topAnchor = new UIRect.AnchorPoint(0f);
			sprite.bottomAnchor = new UIRect.AnchorPoint(0f);
		}
	}
}
