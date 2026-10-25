using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager.Popup.Widgets;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup
{
	public class UIPopupResourcesContent : MonoBehaviour
	{
		public class ResourceConfiguration
		{
			public Resource Resource;

			public bool HasEnoughResources;
		}

		[SerializeField]
		private UIWidget m_currencyContainer;

		[SerializeField]
		private UIPopupCurrencyWidget m_leftCurrencyWidgets;

		[SerializeField]
		private UIPopupCurrencyWidget m_rigthCurrencyWidgets;

		[SerializeField]
		private UIExtendedLabel m_currencyOptionLabel;

		[SerializeField]
		private UIWidget m_singelCurrencyContainer;

		[SerializeField]
		private UIPopupCurrencyWidget m_singleCurrencyWidgets;

		private void Awake()
		{
		}

		public void ConfigurateItemsCost(List<ResourceConfiguration> _resourceCost, bool _additive)
		{
			bool flag = _resourceCost != null && _resourceCost.Count > 0;
			bool flag2 = false;
			if (flag)
			{
				flag2 = _resourceCost.Count == 1;
				if (flag2)
				{
					ResourceConfiguration resourceConfiguration = _resourceCost[0];
					m_singleCurrencyWidgets.Configure(resourceConfiguration.Resource, resourceConfiguration.HasEnoughResources);
				}
				else
				{
					m_currencyOptionLabel.text = LocalizationManager.Instance.Get(5385);
					m_currencyOptionLabel.gameObject.SetActive(_additive);
					if (_resourceCost.Count > 2)
					{
					}
					ResourceConfiguration resourceConfiguration2 = _resourceCost[0];
					m_leftCurrencyWidgets.Configure(resourceConfiguration2.Resource, resourceConfiguration2.HasEnoughResources);
					resourceConfiguration2 = _resourceCost[1];
					m_rigthCurrencyWidgets.Configure(resourceConfiguration2.Resource, resourceConfiguration2.HasEnoughResources);
				}
			}
			m_singelCurrencyContainer.gameObject.SetActive(flag && flag2);
			m_currencyContainer.gameObject.SetActive(flag && !flag2);
		}
	}
}
