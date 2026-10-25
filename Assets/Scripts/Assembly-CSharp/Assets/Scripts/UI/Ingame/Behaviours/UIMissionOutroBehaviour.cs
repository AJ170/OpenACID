using Assets.Scripts.UI.Helpers;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIMissionOutroBehaviour : UIBaseBehaviour
	{
		[SerializeField]
		private UITexture m_logo;

		[SerializeField]
		private GameObject m_mainContainer;

		public UITexture Logo
		{
			get
			{
				return m_logo;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MissionOutro;
			}
		}

		public override void SetVisibility(bool _visible)
		{
			base.gameObject.SetActive(_visible);
		}

		public void Update()
		{
			UIRect component = Logo.GetComponent<UIRect>();
			component.alpha = component.CalculateFinalAlpha(0);
		}

		public void Init()
		{
			Logo.mainTexture = UIImageLocalizationHelper.GetLogoForCurrentLanguage();
			base.gameObject.SetActive(false);
			m_mainContainer.SetActive(true);
		}
	}
}
