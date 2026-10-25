using AcierProtocol;
using UnityEngine;

public class SkillModel : MonoBehaviour
{
	public int Index;

	public UITexture SkillIcon;

	public UITexture SkillIconPressed;

	public UIEventTrigger EventTrigger;

	public BoxCollider Collider;

	public UISprite Background;

	public UISprite GoldenFrame;

	public UIExtendedLabel Level;

	[HideInInspector]
	public int TierID;

	[HideInInspector]
	public Assassin.EClassTypeId ClassTypeID;

	public TweenAlpha Highlight;

	public TweenPosition TweenPosition;

	public void ResetSkillModel()
	{
		ClassTypeID = Assassin.EClassTypeId.None;
		TierID = 0;
		SkillIcon.ClearTexture();
		Collider.enabled = false;
		Level.gameObject.SetActive(false);
	}
}
