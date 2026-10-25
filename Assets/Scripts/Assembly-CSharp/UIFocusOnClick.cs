using Assets.Scripts.UI.Utils;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Focus Scroll View on Click")]
public class UIFocusOnClick : MonoBehaviour
{
	public UIFocusChildInScrollView FocusChildInScrollView { get; set; }

	public void OnClick()
	{
		if (FocusChildInScrollView == null)
		{
			FocusChildInScrollView = NGUITools.FindInParents<UIFocusChildInScrollView>(base.gameObject);
		}
		if (FocusChildInScrollView != null)
		{
			if (FocusChildInScrollView.enabled)
			{
				FocusChildInScrollView.FocusOn(base.transform);
			}
			return;
		}
		UIPanel uIPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		if (uIPanel != null && uIPanel.clipping != 0)
		{
			UIScrollView component = uIPanel.GetComponent<UIScrollView>();
			Vector3 pos = -uIPanel.cachedTransform.InverseTransformPoint(base.transform.position);
			if (!component.canMoveHorizontally)
			{
				pos.x = uIPanel.cachedTransform.localPosition.x;
			}
			if (!component.canMoveVertically)
			{
				pos.y = uIPanel.cachedTransform.localPosition.y;
			}
			SpringPanel.Begin(uIPanel.cachedGameObject, pos, 6f);
		}
	}
}
