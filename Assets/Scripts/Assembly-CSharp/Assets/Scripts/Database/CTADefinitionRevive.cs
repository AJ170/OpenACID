using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.Screens.ShopScreen;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	public class CTADefinitionRevive : CTADefinitionBase
	{
		public CTADefinitionRevive(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			if (instance.CTACounterGet(LocalStorage.CTACounterReviveNotPossible) <= 0)
			{
				return EResult.Proceed;
			}
			instance.CTACounterSet(LocalStorage.CTACounterReviveNotPossible, 0);
			return EResult.Fullfilled;
		}

		public override void RedirectToScreen()
		{
			base.RedirectToScreen();
			UIShopDetails.OpenAndSetFocusOnItem(EShopCategory.Shopcat_HcPackage);
		}
	}
}
