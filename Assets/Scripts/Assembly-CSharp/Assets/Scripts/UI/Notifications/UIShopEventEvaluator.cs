using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.ProfileManager;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Notifications
{
	public class UIShopEventEvaluator : MonoBehaviour
	{
		[SerializeField]
		private UISprite m_shopHeaderFlagIcon;

		private UIButtonHeader m_shopMenuButton;

		private bool m_isShopButtonSelected;

		private bool m_hasShopEventActive;

		private void Awake()
		{
			UIButtonHeaderManager buttonHeaderManager = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager;
			if (buttonHeaderManager != null)
			{
				m_shopMenuButton = buttonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.Shop);
			}
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.OnChanged += new SimpleDelegate.CallbackFunction(OnEventManagerChanged);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate += new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenUpdate);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenPopped, OnScreenUpdate);
			m_hasShopEventActive = HasShopEvent();
			m_shopHeaderFlagIcon.alpha = 0f;
			m_shopHeaderFlagIcon.gameObject.SetActive(true);
			OnScreenUpdate();
		}

		private void OnDestroy()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate -= new SimpleDelegate.CallbackFunction(OnProfileUpdate);
			Singleton<StateManager>.Instance.EventManager.OnChanged -= new SimpleDelegate.CallbackFunction(OnEventManagerChanged);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenUpdate);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnScreenUpdate);
		}

		private void OnProfileUpdate(object[] _params)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			EntitlementInventory entitlementInventory = profileManager.Profile.entitlementInventory;
			EntitlementInventory entitlementInventory2 = ((profileManager.PreviousProfileData == null) ? null : profileManager.PreviousProfileData.EntitlementInventory);
			if (entitlementInventory2 == null || entitlementInventory.entries.Length != entitlementInventory2.entries.Length)
			{
				m_hasShopEventActive = HasShopEvent();
				UpdateIconVisibility();
			}
		}

		private void OnEventManagerChanged(object[] _params)
		{
			m_hasShopEventActive = HasShopEvent();
			UpdateIconVisibility();
		}

		private void OnScreenUpdate()
		{
			m_isShopButtonSelected = m_shopMenuButton.ScreenID == Singleton<UIScreenManager>.Instance.CurrentScreenId;
			UpdateIconVisibility();
		}

		private void UpdateIconVisibility()
		{
			bool flag = m_shopMenuButton.IsUnlocked && !m_isShopButtonSelected && m_hasShopEventActive;
			m_shopHeaderFlagIcon.alpha = ((!flag) ? 0f : 1f);
		}

		private bool HasShopEvent()
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventShopAdvertisementsManager eventShopAdvertisementsManager = (EventShopAdvertisementsManager)eventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner);
			if (eventShopAdvertisementsManager.HasActiveShopAdvertisements())
			{
				return true;
			}
			EventManagerBase eventManger = eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage);
			if (eventManger.HasValidEvent())
			{
				return true;
			}
			eventManger = eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage);
			if (eventManger.HasValidEvent())
			{
				return true;
			}
			eventManger = eventManager.GetEventManger(EEventType.Event_Shop_Category_Payout_Increase);
			if (eventManger.HasValidEvent())
			{
				return true;
			}
			return false;
		}
	}
}
