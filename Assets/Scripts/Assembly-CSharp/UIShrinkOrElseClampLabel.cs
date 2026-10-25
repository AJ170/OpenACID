using System;
using UnityEngine;

public class UIShrinkOrElseClampLabel : MonoBehaviour
{
	[SerializeField]
	private UIExtendedLabel label;

	private void Start()
	{
		label.overflowMethod = UILabel.Overflow.DynamicScale;
		OnDimentionChange();
		UIExtendedLabel uIExtendedLabel = label;
		uIExtendedLabel.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(uIExtendedLabel.onChange, new UIWidget.OnDimensionsChanged(OnDimentionChange));
	}

	private void OnDimentionChange()
	{
		label.MaxBoxHeight = label.height;
		label.MaxBoxWidth = label.width;
	}

	private void OnDestroy()
	{
		UIExtendedLabel uIExtendedLabel = label;
		uIExtendedLabel.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(uIExtendedLabel.onChange, new UIWidget.OnDimensionsChanged(OnDimentionChange));
	}
}
