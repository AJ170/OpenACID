using System;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.ShopScreen.Advertisement;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIHappyHourAdvertisementDetails : MonoBehaviour, IShopAdBaseDetails
	{
		[SerializeField]
		private UIExtendedLabel m_timerLabel;

		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		private UIExtendedLabel m_descriptionText;

		private bool m_isActive;

		private float m_happyHourValue;

		private long m_happyHourEndTime;

		private DateTime m_happyHourEndTimeInDateTime;

		private EventShopAdvertisement m_shopEventData;

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
				base.gameObject.SetActive(m_isActive);
			}
		}

		public void UpdateDetails(EventShopAdvertisement _shopEventData, EBannerType _layoutType)
		{
			m_shopEventData = _shopEventData;
			UIItemUtil.UpdateWidgetColor(m_shopEventData.Template.DiscountColorGameId, m_valueLabel);
			UIItemUtil.UpdateWidgetColor(m_shopEventData.Template.RemainingTimeLabelColorGameId, m_timerLabel);
			UIItemUtil.UpdateWidgetColor(m_shopEventData.Template.SubTitleColorGameId, m_descriptionText);
			StringId stringId = m_shopEventData.Template.GetSubTitleLocaKey(_layoutType);
			if (!stringId.Valid)
			{
				stringId = 7693;
			}
			m_descriptionText.SetLocalizationKey(stringId);
			if (m_shopEventData.Template.ShopCategory.GetShopCategoryHappyHourData(out m_happyHourEndTime, out m_happyHourValue))
			{
				int num = (int)m_happyHourValue;
				m_valueLabel.LocalizedText = LocalizationManager.Instance.Get(7694, new StringMapper { 
				{
					"value",
					LocalizationManager.Instance.GetInteger(num)
				} });
			}
		}

		public void UpdateDynamicData()
		{
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			if (m_shopEventData.Template.ShopCategory.GetShopCategoryHappyHourData(out m_happyHourEndTime, out m_happyHourValue))
			{
				m_happyHourEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_happyHourEndTime);
				double totalSeconds = (m_happyHourEndTimeInDateTime - utcNow).TotalSeconds;
				m_timerLabel.SetLocalizationKey(2028, new StringMapper { 
				{
					"Shop_Discount_Time_Left",
					UIDuration.FromSeconds((long)totalSeconds)
				} });
			}
		}
	}
}
