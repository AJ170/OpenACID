using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.InventorySpace;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinSelectionScreen
{
	public class AssassinCreationHandler : MonoBehaviour
	{
		public static List<EventDelegate> onAssassinSlotPurchased = new List<EventDelegate>();

		[SerializeField]
		private UIButton berserkButton;

		[SerializeField]
		private UIButton shadowBladeButton;

		[SerializeField]
		private UIButton thiefButton;

		[SerializeField]
		private UIButton tricksterButton;

		[SerializeField]
		private List<ClassChooseContainer> m_containerList;

		[SerializeField]
		private UIExtendedButton m_buySlotButton;

		[SerializeField]
		private UIButtonLabelHandler m_buySlotButtonLabelHandler;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			if (m_buySlotButton != null)
			{
				EventDelegate.Remove(m_buySlotButton.onClick, ShowBuyAssassinSlotPopup);
			}
		}

		private void Start()
		{
			if (m_buySlotButton != null)
			{
				EventDelegate.Add(m_buySlotButton.onClick, ShowBuyAssassinSlotPopup);
			}
		}

		private void ShowBuyAssassinSlotPopup()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			long numAlreadyPurchasedSlots = profile.GetCounterAmountByType(ECounterType.BoughtAssassinSlots);
			Resource[] cost2 = Singleton<StateManager>.Instance.TemplateManager.AssassinSlotsCostsMapping.GetCost((int)numAlreadyPurchasedSlots);
			Array.Sort(cost2, UIUtil.SortResourcesByType);
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>(cost2.Length);
			List<PopUpButtonConfiguration> list2 = new List<PopUpButtonConfiguration>();
			int num = ((cost2.Length != 1) ? 692 : 1780);
			foreach (Resource cost in cost2)
			{
				UIPopupResourcesContent.ResourceConfiguration resConfig = new UIPopupResourcesContent.ResourceConfiguration
				{
					HasEnoughResources = profile.HasEnoughResources(cost),
					Resource = cost
				};
				PopUpButtonConfiguration item = UIBuyHelper.CreateResourceCostButton(resConfig, num, delegate
				{
					UIBuyHelper.DetermineAllowPurches(resConfig.Resource, delegate
					{
						PurchaseAssassinSlot((int)numAlreadyPurchasedSlots, cost);
						AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_buy);
					}, null);
				});
				list2.Add(item);
				list.Add(resConfig);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(5212);
			popUpConfiguration.Buttons = list2.ToArray();
			popUpConfiguration.Data = new InventorySpacePopUpContent.InventoryPopUpData
			{
				DisplayType = InventorySpacePopUpContent.InventoryPopUpData.EDisplayType.AssassinSlot,
				Description = LocalizationManager.Instance.Get(5214),
				ResourcesCost = list,
				ResourceSymbolAdditive = false
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void PurchaseAssassinSlot(int _numAlreadyPurchasedSlots, Resource _resource)
		{
			SetBuySlotInterface(false);
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			AsyncCallback callback = OnPurchaseAssassinSlot;
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncRequest)
			{
				Singleton<StateManager>.Instance.ProfileManager.PurchaseAssassinSlot(_numAlreadyPurchasedSlots, _resource.type, _asyncRequest);
			}, null, callback);
		}

		private void OnPurchaseAssassinSlot(IAsyncResult _result)
		{
			if (!_result.IsError())
			{
				EventDelegate.Execute(onAssassinSlotPurchased);
			}
		}

		private void SetBuySlotInterface(bool _value)
		{
			for (int i = 0; i < m_containerList.Count; i++)
			{
				m_containerList[i].UpdateLockedStat();
			}
			if (_value)
			{
				m_buySlotButton.gameObject.SetActive(true);
				for (int j = 0; j < m_containerList.Count; j++)
				{
					if (!m_containerList[j].Active)
					{
						m_containerList[j].GetDeActivated(false);
					}
					else
					{
						m_containerList[j].GetDeActivated();
					}
					m_containerList[j].SetBlocked(true);
				}
				return;
			}
			m_buySlotButton.gameObject.SetActive(false);
			for (int k = 0; k < m_containerList.Count; k++)
			{
				if (!m_containerList[k].Active)
				{
					m_containerList[k].GetDeActivated(false);
				}
				else
				{
					m_containerList[k].GetDeActivated();
				}
				m_containerList[k].SetBlocked(false);
			}
		}

		public void EnableHandler(bool _enable, int _index)
		{
			for (int i = 0; i < m_containerList.Count; i++)
			{
				m_containerList[i].Init();
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			long freeAssassinSlots = profileManager.FreeAssassinSlots;
			if (_index > freeAssassinSlots + (profileManager.AssassinCount - 1))
			{
				UserProfile profile = profileManager.Profile;
				EResourceType firstAvailableResourceCostType = Singleton<StateManager>.Instance.TemplateManager.AssassinSlotsCostsMapping.GetFirstAvailableResourceCostType(profile);
				m_buySlotButtonLabelHandler.ResourceType = firstAvailableResourceCostType;
				SetBuySlotInterface(true);
				return;
			}
			SetBuySlotInterface(false);
			if (berserkButton != null)
			{
				berserkButton.isEnabled = _enable;
			}
			if (shadowBladeButton != null)
			{
				shadowBladeButton.isEnabled = _enable;
			}
			if (thiefButton != null)
			{
				thiefButton.isEnabled = _enable;
			}
			if (tricksterButton != null)
			{
				tricksterButton.isEnabled = _enable;
			}
		}

		public void HideHandler(bool _hide)
		{
			base.gameObject.SetActive(!_hide);
		}
	}
}
