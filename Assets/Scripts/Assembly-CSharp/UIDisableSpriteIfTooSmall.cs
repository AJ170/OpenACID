using System;
using UnityEngine;

public class UIDisableSpriteIfTooSmall : MonoBehaviour
{
	private UISprite spriteComponent;

	private Color startColor;

	private void Start()
	{
		spriteComponent = GetComponent<UISprite>();
		startColor = spriteComponent.color;
	}

	private void OnEnable()
	{
		UIWidget component = GetComponent<UIWidget>();
		component.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(component.onChange, new UIWidget.OnDimensionsChanged(UpdateSize));
	}

	private void OnDisable()
	{
		UIWidget component = GetComponent<UIWidget>();
		component.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(component.onChange, new UIWidget.OnDimensionsChanged(UpdateSize));
	}

	private void Update()
	{
	}

	private void UpdateSize()
	{
		if ((float)spriteComponent.height < 4f || (float)spriteComponent.width < 4f)
		{
			spriteComponent.color = new Color(0f, 0f, 0f, 0f);
		}
		else
		{
			spriteComponent.color = startColor;
		}
	}
}
