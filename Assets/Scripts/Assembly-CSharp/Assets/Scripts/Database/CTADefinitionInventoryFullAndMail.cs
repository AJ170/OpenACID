using System.Collections.Generic;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Screens.Popup.CallToActionPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	public class CTADefinitionInventoryFullAndMail : CTADefinitionBase
	{
		public int MaxInventorySlots { get; protected set; }

		public CTADefinitionInventoryFullAndMail(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			MaxInventorySlots = _s.SerializeInt("MaxInventorySlots", MaxInventorySlots);
		}

		protected override EResult CheckTrigger()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			if (profileManager.InventoryFreeSpace > 0)
			{
				return EResult.Proceed;
			}
			if (!profileManager.CanExpandInventory || profileManager.InventorySize > MaxInventorySlots)
			{
				return EResult.Proceed;
			}
			if (CallToActionPopup.LastMissionHadInventoryOverflow)
			{
				return EResult.Fullfilled;
			}
			List<MailHeader> headers = MailCache.Headers;
			for (int i = 0; i < headers.Count; i++)
			{
				if (headers[i].fullInventory && !headers[i].isRead)
				{
					return EResult.Fullfilled;
				}
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
