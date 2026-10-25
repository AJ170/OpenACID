using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Banner
{
	public class UIBannerShopAdvertisementController : UIBannerBaseController
	{
		[SerializeField]
		private UIShopAdvertisementDetails m_advertisementDetails;

		public override bool IsAllowedToBeTapped
		{
			get
			{
				return m_advertisementDetails.IsAllowedToBeTapped;
			}
			set
			{
				m_advertisementDetails.IsAllowedToBeTapped = value;
			}
		}

		public override EBannerType LayoutType
		{
			get
			{
				return m_advertisementDetails.LayoutType;
			}
			set
			{
				m_advertisementDetails.LayoutType = value;
			}
		}

		public override void UpdateBannerData(EventBase _event)
		{
			IsAllowedToBeTapped = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.ShopTabEntitlementGUID);
			m_advertisementDetails.UpdateAdvertisementData((EventShopAdvertisement)_event, LayoutType);
		}

		public override void InstantShowItem()
		{
			base.InstantShowItem();
			m_advertisementDetails.InstantShowItem();
		}
	}
}
