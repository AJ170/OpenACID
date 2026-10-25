using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

public class GenericOptionsTab : MonoBehaviour
{
	public UIExtendedLabel m_name;

	public UISprite m_Icon;

	public GameObject m_selectCollider;

	public UIEventTrigger m_tabTrigger;

	[SerializeField]
	private BoxCollider m_tabActivatorCollider;

	[SerializeField]
	private UIButtonProxy m_button;

	public UIScreen m_correspondingScreen;

	public TweenScale m_ScaleTween;

	public UISprite m_HighLight;

	public UIWidget m_deactivatedOverlay;

	public EScreenID m_changeScreenID = EScreenID.Invalid;

	public UIButtonProxy Button
	{
		get
		{
			return m_button;
		}
	}

	public void GetActivated()
	{
		m_selectCollider.SetActive(true);
		m_tabActivatorCollider.enabled = false;
		m_ScaleTween.from = Vector3.one;
		m_ScaleTween.to = new Vector3(1.1f, 1.1f, 1f);
		m_ScaleTween.ResetToBeginning();
		m_ScaleTween.PlayForward();
		m_HighLight.alpha = 1f;
		m_deactivatedOverlay.enabled = false;
	}

	public void GetDeactivated()
	{
		m_selectCollider.SetActive(false);
		m_tabActivatorCollider.enabled = true;
		m_ScaleTween.from = new Vector3(1.1f, 1.1f, 1f);
		m_ScaleTween.to = Vector3.one;
		m_ScaleTween.ResetToBeginning();
		m_ScaleTween.PlayForward();
		m_HighLight.alpha = 0f;
		m_deactivatedOverlay.enabled = true;
	}

	public void ActivateCorrespondingMenu()
	{
		if (m_correspondingScreen != null)
		{
			Singleton<UIScreenManager>.Instance.PopScreen();
			if (m_changeScreenID == EScreenID.Invalid)
			{
				Singleton<UIScreenManager>.Instance.PushScreen(m_correspondingScreen, null);
			}
			else
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(m_changeScreenID, new UITweenHandle(null, null));
			}
		}
	}

	public void UpdateData(UIAtlas _atlas, string _spriteName, string _name)
	{
		m_Icon.atlas = _atlas;
		m_Icon.spriteName = _spriteName;
		m_name.text = _name;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		UISpriteData atlasSprite = m_Icon.GetAtlasSprite();
		if (atlasSprite != null)
		{
			num = atlasSprite.height;
			num2 = atlasSprite.width;
			num3 = atlasSprite.paddingLeft;
			num4 = atlasSprite.paddingRight;
		}
		m_Icon.MakePixelPerfect();
	}

	public void SetAnchoring(UIPanel _scrollView)
	{
		UIWidget component = GetComponent<UIWidget>();
		component.leftAnchor = _scrollView.leftAnchor;
		component.rightAnchor = _scrollView.rightAnchor;
	}
}
