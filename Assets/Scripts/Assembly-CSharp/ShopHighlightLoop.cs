using System.Collections;
using Assets.Scripts.UI.Behaviours.Buttons.Headers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using UnityEngine;

public class ShopHighlightLoop : MonoBehaviour
{
	[SerializeField]
	private UIWidget m_uiWidget;

	[SerializeField]
	private UIButtonHeaderButton m_button;

	[SerializeField]
	private DynamicSwipe m_dynamicSwipe;

	[SerializeField]
	private bool Loop;

	[SerializeField]
	private float Delay;

	private void OnEnable()
	{
		if (Singleton<StateManager>.Instance.TemplateManager.ShowShopEffect)
		{
			Initialize();
			StartCoroutine(PlaySwipe());
		}
	}

	private void Initialize()
	{
		m_dynamicSwipe.InitializeDoubleSkew(true, m_uiWidget.width, m_uiWidget.height);
	}

	private IEnumerator PlaySwipe()
	{
		while (true)
		{
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.ShopScreen && m_button.IsEnabled && m_button.IsUnlocked)
			{
				m_dynamicSwipe.ActivateAnimation();
			}
			yield return new WaitForSeconds(Delay);
		}
	}
}
