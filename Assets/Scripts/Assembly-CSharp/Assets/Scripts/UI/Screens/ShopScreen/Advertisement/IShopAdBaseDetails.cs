using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Screens.ShopScreen.Advertisement
{
	public interface IShopAdBaseDetails
	{
		void UpdateDetails(EventShopAdvertisement _shopEventData, EBannerType _layoutType);

		void UpdateDynamicData();
	}
}
