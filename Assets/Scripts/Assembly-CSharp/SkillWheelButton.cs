using AcierProtocol;
using UnityEngine;

public class SkillWheelButton : MonoBehaviour
{
	public int Index;

	public UIEventTrigger EventTrigger;

	public SkillWheelEntry entry;

	public UITexture SkillIcon;

	public bool locked;

	public bool empty;

	public UISprite Highlight;

	public TweenScale TweenScale;

	public TweenAlpha HighLightTween;
}
