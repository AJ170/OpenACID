using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.Popup.Widgets
{
	public class UIPopupCurrencyWidget : MonoBehaviour
	{
		[SerializeField]
		private UISprite m_currencyIcon;

		[SerializeField]
		private UIExtendedLabel m_currencyLabel;

		[SerializeField]
		private UIWidget m_parentWidget;

		[SerializeField]
		private UISprite m_background;

		[SerializeField]
		private UIAlignmentBox m_centerBox;

		private Resource m_resource;

		public void Configure(Resource _resource, bool _hasEnoughResources)
		{
			m_resource = _resource;
			if (m_background != null)
			{
				Color color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 77);
				if (!_hasEnoughResources)
				{
					color = new Color32(byte.MaxValue, 0, 0, 77);
				}
				m_background.color = color;
			}
			UIUtil.SetupCurrencyWidgets(m_resource, m_currencyLabel, m_currencyIcon);
			if (m_centerBox != null)
			{
				m_centerBox.RepositionNow = true;
			}
			Show();
		}

		public void Show()
		{
			if (m_parentWidget != null)
			{
				m_parentWidget.alpha = 1f;
			}
		}

		private void Awake()
		{
			if (m_parentWidget == null)
			{
				m_parentWidget = GetComponent<UIWidget>();
			}
		}
	}
}
