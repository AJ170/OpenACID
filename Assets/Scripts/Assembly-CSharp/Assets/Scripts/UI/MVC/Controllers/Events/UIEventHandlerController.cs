using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Events
{
	public class UIEventHandlerController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIWalletHandler m_wallet;

		public void OnEventButtonClicked()
		{
			TrackingManager.OpenEventProgressionScreen("EventButton");
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			EntitlementManager entitlementManager = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager;
			bool showEventButton = entitlementManager.HasEntitlement(StaticEntitlementIds.FeatureAvailableWeeklyEventsGUID) || entitlementManager.HasEntitlement(StaticEntitlementIds.DailyQuestsEntitlementGUID);
			m_wallet.ShowEventButton = showEventButton;
		}
	}
}
