using UnityEngine;

public class GenericToggleHighlighter : MonoBehaviour
{
	public Color BackgroundOffColor = new Color(0.5019608f, 0.5019608f, 0.5019608f);

	public Color BackgroundOnColor = new Color(1f, 1f, 1f);

	public Color ThumbOffColor = new Color(0.5019608f, 0.5019608f, 0.5019608f);

	public Color ThumbOnColor = new Color(33f / 85f, 62f / 85f, 1f);

	public int ThumbLabelOn = 5282;

	public int ThumbLabelOff = 5283;

	public UISlider Slider;

	public UIWidget SliderBackground;

	public UIWidget SliderThumb;

	public UIExtendedLabel SliderLabel;

	public void OnValueChange()
	{
		bool flag = Slider.value >= 0.5f;
		SliderBackground.color = ((!flag) ? BackgroundOffColor : BackgroundOnColor);
		SliderThumb.color = ((!flag) ? ThumbOffColor : ThumbOnColor);
		if (SliderLabel != null)
		{
			SliderLabel.SetLocalizationKey((!flag) ? ThumbLabelOff : ThumbLabelOn);
		}
	}
}
