using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	public class CTADefinitionRunningOutOfInventory : CTADefinitionBase
	{
		public float UsageThresholdFirst { get; protected set; }

		public float UsageThresholdNormal { get; protected set; }

		public CTADefinitionRunningOutOfInventory(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			UsageThresholdFirst = _s.SerializeFloat("UsageThresholdFirst", UsageThresholdFirst);
			UsageThresholdNormal = _s.SerializeFloat("UsageThresholdNormal", UsageThresholdNormal);
		}

		protected override EResult CheckTrigger()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			Globals globals = Singleton<StateManager>.Instance.TemplateManager.Globals;
			if (!profileManager.CanExpandInventory)
			{
				return EResult.Proceed;
			}
			int num = ((profileManager.InventorySize != globals.initialInventorySize) ? ((int)UsageThresholdNormal) : ((int)UsageThresholdFirst));
			if (profileManager.InventoryFreeSpace < num)
			{
				return EResult.Fullfilled;
			}
			return EResult.Proceed;
		}

		public override void RedirectToScreen()
		{
			base.RedirectToScreen();
			UIBuyHelper.OpenBuyInventoryPopup();
		}
	}
}
