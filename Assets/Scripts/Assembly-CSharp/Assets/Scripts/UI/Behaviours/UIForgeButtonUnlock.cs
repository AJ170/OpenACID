using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	internal class UIForgeButtonUnlock : MonoBehaviour
	{
		[CheckAssigned]
		[SerializeField]
		private UIButtonProxy m_button;

		[SerializeField]
		[CheckAssigned]
		private UIEventTrigger m_trigger;

		private bool ShouldEnable
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FusionEntitlementGUID);
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			m_button.isEnabled = ShouldEnable;
			EventDelegate.Add(m_button.onClick, OnTabClick);
			EventDelegate.Add(m_trigger.onClick, OnTabClick);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnTabClick);
			EventDelegate.Remove(m_trigger.onClick, OnTabClick);
		}

		private void OnTabClick()
		{
			if (m_button.isEnabled)
			{
				UIInventoryScreen.StartingState = UIInventoryScreenData.EScreenState.Forge;
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.InventoryScreen, new UITweenHandle());
			}
			else
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(7346));
			}
		}
	}
}
