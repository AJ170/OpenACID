using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Animations.ValueObservers;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ForgePopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIForgeHandler : UIScreenChild, IInitialized<UIInventoryScreenData>
	{
		private const string k_resetForgeClip = "ResetForge";

		private const string k_forgeOverExposureClip = "ForgeOverExposure";

		private const string k_startForgeClip = "StartForge";

		private const string k_UpdateForgeClip = "ForgeUpdateTween";

		private const string k_forgeItemHide = "ForgeItemHide";

		private const string k_forgeItemShow = "ForgeItemShow";

		private const string k_itemForgeResult = "ItemForgeResult";

		private const string k_forgeCompleated = "ForgeResult";

		private const string k_reForgeCompleated = "ReForgeResult";

		private static readonly StringId s_levelId = new StringId(6801);

		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_closeButton;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_forgeButton;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_leveLabel;

		[SerializeField]
		[CheckAssigned]
		private UIForgeItemHandler[] m_forges;

		[SerializeField]
		[CheckAssigned]
		private UIInfoPopup m_detailsPopup;

		[CheckAssigned]
		[SerializeField]
		private Transform m_popupCenter;

		[SerializeField]
		[CheckAssigned]
		private UIInventoryBlocker m_blocker;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAnimation m_itemTween;

		[CheckAssigned]
		[SerializeField]
		private UIBoolObserver m_triggerDetailsObserver;

		[SerializeField]
		[CheckAssigned]
		private UIBoolObserver m_blockBackgroundObserver;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_armsTweenAlpha;

		[SerializeField]
		[CheckAssigned]
		private UIBoolObserver m_closeDetailsObserver;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_reforgeEffectObj;

		private bool m_helpDisplayed = true;

		private UIInventoryScreenData m_data;

		private UIButtonLabelHandler m_buttonLabelHandler;

		private UIForgeItemHandler m_currentItem;

		private UIInventoryBlocker.BlockerHandle m_fullBlocker;

		private InventoryElement m_forgeResult;

		public List<EventDelegate> OnOpenForgeItemPopup;

		public UIForgeItemHandler CurrentItem
		{
			get
			{
				return m_currentItem;
			}
		}

		private static bool IsForgeTutorialActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Forge);
			}
		}

		public List<UIPopupResourcesContent.ResourceConfiguration> ForgeCostConfiguration
		{
			get
			{
				if (IsForgeTutorialActive)
				{
					return CreateZeroCostConfiguration();
				}
				return (m_data != null) ? m_data.ForgeCostConfiguration : null;
			}
		}

		public List<UIPopupResourcesContent.ResourceConfiguration> ReRollCostConfigurations
		{
			get
			{
				if (IsForgeTutorialActive)
				{
					return CreateZeroCostConfiguration();
				}
				return (m_data != null) ? m_data.ReRollCostConfigurations : null;
			}
		}

		private bool IsForgeFull
		{
			get
			{
				return IsForgeTutorialActive || m_data.IsForgeFull;
			}
		}

		public event Action<UIForgeItemHandler> OnSelectedItemsChange;

		public static bool IsForgeUnlocked()
		{
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.FusionEntitlementGUID);
		}

		public static bool CanForge()
		{
			return IsForgeUnlocked() && Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.elements.Count((InventoryElement _x) => _x.CanStartForge()) > 0;
		}

		public static bool CanForge(int _minNumber, Quality.EQuality _quality)
		{
			return Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.elements.Count((InventoryElement _x) => _x.CanBeForged() && _x.quality == _quality) >= _minNumber;
		}

		protected override void OnScreenInit()
		{
			m_buttonLabelHandler = m_forgeButton.GetComponent<UIButtonLabelHandler>();
			m_fullBlocker = m_blocker.GetBlocker(UIInventoryBlocker.EBlocker.Full);
			m_blocker.gameObject.SetActive(true);
			m_animation.SampleAnimation(0f, string.Empty);
			m_helpDisplayed = true;
		}

		protected override void OnScreenDestroy()
		{
			m_buttonLabelHandler = null;
			m_fullBlocker = null;
		}

		public void Initialize(UIInventoryScreenData _data)
		{
			m_data = _data;
			m_data.OnStateChanged += OnStateChanged;
			m_data.OnForgeChange += OnForgeDataChange;
			m_data.OnForgeCleared += OnForgeCleared;
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].Initialize(m_data);
			}
			UpdateScreenState(m_data.State);
			UpdateFilter();
		}

		public void Clear()
		{
			if (m_data != null)
			{
				m_data.OnStateChanged -= OnStateChanged;
				m_data.OnForgeChange -= OnForgeDataChange;
				m_data.OnForgeCleared -= OnForgeCleared;
				m_data.OnForgeShowGridPreview -= OnShowGridPreview;
				m_data.OnForgeHideGridPreview -= OnHideGridPreview;
				m_data.ClearForge(false);
			}
			m_reforgeEffectObj.gameObject.SetActive(false);
			m_data = null;
			ItemInfoPopupHelper.OnAddToForgeCallback.Remove(OnAddItemToForge);
			BlueprintInfoPopupHelper.OnAddToForgeCallback.Remove(OnAddItemToForge);
			ItemInfoPopupHelper.OnRemoveFromForgeCallback.Remove(OnRemoveItemFromForge);
			BlueprintInfoPopupHelper.OnRemoveFromForgeCallback.Remove(OnRemoveItemFromForge);
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].Clear();
			}
			m_currentItem = null;
			m_forgeResult = null;
			DisableForgeItems();
		}

		public void TutorialAddlInventoryElement(InventoryElement _inventoryElement)
		{
			m_data.AddToForge(_inventoryElement);
			StartCoroutine(co_UpdateItems());
		}

		public void RemoveSelection()
		{
			if (m_currentItem != null)
			{
				m_currentItem.Selected = false;
			}
			m_currentItem = null;
		}

		private void OnForgeDataChange()
		{
			UpdateForgeButton();
			UpdateScreen();
		}

		private void OnForgeCleared()
		{
			UpdateFilter();
		}

		public void ForceCloseForge()
		{
			if (m_data.State == UIInventoryScreenData.EScreenState.Forge)
			{
				OnCloseButtonClick();
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_closeButton.onClick, OnCloseButtonClick);
			EventDelegate.Add(m_forgeButton.onClick, OnForgeClicked);
			m_detailsPopup.OnPopupClosedCallback += OnDetailsClosed;
			ItemInfoPopupHelper.OnClameItemCallback.Add(OnItemClaimed);
			ItemInfoPopupHelper.OnReRollCallback.Add(OnReRoll);
			m_triggerDetailsObserver.OnActive += OnTriggerDetails;
			m_blockBackgroundObserver.OnActive += OnBlockBackground;
			m_closeDetailsObserver.OnActive += OnCloseDetails;
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].OnClick += OnItemClicked;
			}
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_closeButton.onClick, OnCloseButtonClick);
			EventDelegate.Remove(m_forgeButton.onClick, OnForgeClicked);
			m_detailsPopup.OnPopupClosedCallback -= OnDetailsClosed;
			ItemInfoPopupHelper.OnClameItemCallback.Remove(OnItemClaimed);
			ItemInfoPopupHelper.OnReRollCallback.Remove(OnReRoll);
			m_triggerDetailsObserver.OnActive -= OnTriggerDetails;
			m_blockBackgroundObserver.OnActive -= OnBlockBackground;
			m_closeDetailsObserver.OnActive -= OnCloseDetails;
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].OnClick -= OnItemClicked;
				m_forges[i].gameObject.SetActive(false);
			}
		}

		private void OnItemClicked(UIForgeItemHandler _forgeItemHandler)
		{
			if (_forgeItemHandler.Empty)
			{
				return;
			}
			if (m_currentItem != null && m_currentItem == _forgeItemHandler && _forgeItemHandler.Selected)
			{
				RemoveSelection();
				m_detailsPopup.Close();
			}
			else
			{
				if (m_currentItem != null)
				{
					m_currentItem.Selected = false;
				}
				if (!_forgeItemHandler.Empty)
				{
					m_currentItem = _forgeItemHandler;
					m_currentItem.Selected = true;
					if (_forgeItemHandler.Element.IsBlueprint())
					{
						Transform indicatorTarget = _forgeItemHandler.transform;
						BlueprintInfoPopupHelper.ShowBlueprintInfoPopup(m_detailsPopup, _forgeItemHandler.Element, new Button[1]
						{
							new Button
							{
								Active = true,
								Mode = Button.ButtonMode.RemoveFromForge,
								OnClick = delegate(bool _b)
								{
									if (_b)
									{
										m_data.RemoveFromForge(_forgeItemHandler.Element);
									}
								}
							}
						}, true, indicatorTarget, string.Empty);
					}
					else
					{
						Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
						InventoryElement equipedEquivalent = currentAssassin.GetEquipedEquivalent(_forgeItemHandler.Element);
						ItemInfoPopupHelper.ShowInfoPopup(m_detailsPopup, currentAssassin, ItemInfoPopupHelper.EMode.RemoveForge, _forgeItemHandler.Element, equipedEquivalent, _forgeItemHandler.transform, null, true, 0, string.Empty);
					}
					m_detailsPopup.MovePopupToTarget(_forgeItemHandler.gameObject);
				}
			}
			Action<UIForgeItemHandler> onSelectedItemsChange = this.OnSelectedItemsChange;
			if (onSelectedItemsChange != null)
			{
				onSelectedItemsChange(m_currentItem);
			}
		}

		private void OnDetailsClosed()
		{
			if (m_currentItem != null)
			{
				m_currentItem.Selected = false;
			}
		}

		private void OnCloseButtonClick()
		{
			m_data.State = UIInventoryScreenData.EScreenState.Inventory;
			m_data.ClearForge(false);
			RemoveSelection();
		}

		private void OnStateChanged(UIInventoryScreenData.StateChangeData _stateChangeData)
		{
			UpdateScreenState(_stateChangeData.NewState);
		}

		private void OnAddItemToForge(params object[] _params)
		{
			InventoryElement inventoryElement = _params[1] as InventoryElement;
			if (inventoryElement != null && m_data.CanBeAddedToForge(inventoryElement))
			{
				bool isForgeEmpty = m_data.IsForgeEmpty;
				m_data.AddToForge(inventoryElement);
				m_detailsPopup.Close();
				StartCoroutine(co_UpdateItems());
				if (isForgeEmpty)
				{
					UpdateFilter();
				}
			}
		}

		public void OnShowGridPreview()
		{
			if (m_data.IsForgeEmpty)
			{
				StartCoroutine(co_ShowForge());
			}
		}

		public void OnHideGridPreview()
		{
			if (!m_helpDisplayed && m_data.IsForgeEmpty)
			{
				StartCoroutine(co_ShowHelp());
			}
		}

		private void OnRemoveItemFromForge(params object[] _params)
		{
			InventoryElement inventoryElement = _params[1] as InventoryElement;
			if (inventoryElement != null)
			{
				m_data.RemoveFromForge(inventoryElement);
			}
			m_detailsPopup.Close();
			if (!m_data.IsForgeValid)
			{
				m_data.ClearForge();
			}
			IEnumerator routine;
			if (m_data.IsForgeEmpty)
			{
				IEnumerator enumerator = co_ShowHelp();
				routine = enumerator;
			}
			else
			{
				routine = co_UpdateItems();
			}
			StartCoroutine(routine);
		}

		private void OnForgeClicked()
		{
			m_detailsPopup.Close();
			ForgePopUpContent.ForgePopUpData.EMode mode = ((!IsForgeFull) ? ForgePopUpContent.ForgePopUpData.EMode.ForgeOverspend : ForgePopUpContent.ForgePopUpData.EMode.ForgeFull);
			string description = LocalizationManager.Instance.Get((!IsForgeFull) ? ForgePopUpContent.ForgeDescriptionOverSpend : ForgePopUpContent.ForgeDescriptionFull);
			GameID gUID = ((!m_data.IsOutcomeKnown) ? UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_unkownOutcomeGUID : UbiCore.Utils.Singleton<UIDataMapper>.Instance.ForgeDescriptions[(int)m_data.ForgeOutcomeType].GUID);
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			List<UIPopupResourcesContent.ResourceConfiguration> forgeCostConfiguration = ForgeCostConfiguration;
			for (int i = 0; i < forgeCostConfiguration.Count; i++)
			{
				UIPopupResourcesContent.ResourceConfiguration resCost = forgeCostConfiguration[i];
				PopUpButtonConfiguration item = UIBuyHelper.CreateResourceCostButton(resCost, 3948, delegate
				{
					OnForgeOkPressed(resCost.Resource);
				}, IsForgeTutorialActive);
				list.Add(item);
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Buttons = list.ToArray();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.ShowCloseButton = !IsForgeTutorialActive;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Data = new ForgePopUpContent.ForgePopUpData
			{
				Description = description,
				Element = null,
				Mode = mode,
				Quality = m_data.ForgeOutcomeQuality,
				GUID = gUID,
				Cost = forgeCostConfiguration
			};
			popUpConfiguration.CloseButtonAudioControllerId = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel;
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			EventDelegate.Execute(OnOpenForgeItemPopup);
		}

		private void OnForgeOkPressed(Resource _resource)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_forge);
			if (IsForgeTutorialActive)
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				DoForgeItems(_resource);
			}
			else
			{
				UIBuyHelper.DetermineAllowPurches(_resource, delegate
				{
					DoForgeItems(_resource);
				}, null);
			}
		}

		private void DoForgeItems(Resource _resource)
		{
			m_data.IsForging = true;
			if (IsForgeTutorialActive)
			{
				List<string> tutorialForgeItems = new List<string>();
				for (int i = 0; i < m_data.ForgedIds.Count - 1; i++)
				{
					string item = m_data.ForgedIds[i];
					tutorialForgeItems.Add(item);
				}
				AsyncCallback callback = OnTutorialForgeFinished;
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.ForgeDuringTutorial(tutorialForgeItems, _resource, m_data.ForgeOutcomeLevel, _asyncCallback);
				}, null, callback);
			}
			else
			{
				AsyncCallback callback = OnForgeFinished;
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.Forge(m_data.ForgedIds, _resource, m_data.ForgeOutcomeLevel, _asyncCallback);
				}, null, callback);
			}
		}

		private void OnTutorialForgeFinished(IAsyncResult _ar)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (_ar.IsError())
			{
				m_data.IsForging = false;
				return;
			}
			bool flag = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(ETutorialCategory.TutCatMs25Forge);
			AsyncDataResult<ForgeDuringTutorialResponse> asyncDataResult = (AsyncDataResult<ForgeDuringTutorialResponse>)_ar;
			ForgeDuringTutorialResponse forgeDuringTutorialResponse = asyncDataResult.Result();
			UpdateForgeResult(forgeDuringTutorialResponse.rolledElement, false);
		}

		private void OnForgeFinished(IAsyncResult _ar)
		{
			if (_ar.IsError())
			{
				m_data.IsForging = false;
				return;
			}
			AsyncDataResult<ForgeResponse> asyncDataResult = (AsyncDataResult<ForgeResponse>)_ar;
			ForgeResponse forgeResponse = asyncDataResult.Result();
			UpdateForgeResult(forgeResponse.rolledElement, false);
		}

		private void OnItemClaimed(params object[] _params)
		{
			m_fullBlocker.Off(true);
			m_data.IsForging = false;
			m_data.ClearForge();
			ResetFilter();
			StartCoroutine(co_ShowHelp());
		}

		private void OnReRoll(params object[] _params)
		{
			m_fullBlocker.Off(true);
			InventoryElement element = _params[1] as InventoryElement;
			if (element == null)
			{
				return;
			}
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			if (!IsForgeTutorialActive)
			{
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(new StringId(1781)),
					SoundIntent = EButtonIntent.Cancel,
					OnButtonPressed = delegate
					{
						m_fullBlocker.On(true, false);
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				});
			}
			list.Add(UIBuyHelper.CreateResourceCostButton(ReRollCostConfigurations[0], 3948, delegate
			{
				OnReRollOkPressed(element);
			}));
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Buttons = list.ToArray();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.ShowCloseButton = !IsForgeTutorialActive;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				m_fullBlocker.On(true, false);
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Data = new ForgePopUpContent.ForgePopUpData
			{
				Description = LocalizationManager.Instance.Get(ForgePopUpContent.RerollDescription, new StringMapper { 
				{
					"name_of_the_item",
					LocalizationManager.Instance.Get(element.langKey.neutral)
				} }),
				Element = element,
				GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_unkownOutcomeGUID,
				Mode = ForgePopUpContent.ForgePopUpData.EMode.ReRoll,
				Quality = element.quality,
				Cost = ReRollCostConfigurations
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		private void OnReRollOkPressed(InventoryElement _element)
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_forge);
			if (IsForgeTutorialActive)
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				DoReRoll();
			}
			else
			{
				UIBuyHelper.DetermineAllowPurches(m_data.ReRollCost, DoReRoll, null);
			}
		}

		private void DoReRoll()
		{
			m_forgeResult = null;
			if (IsForgeTutorialActive)
			{
				AsyncCallback callback = OnTutorialReRollFinished;
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.ForgeRerollDuringTutorial(m_data.ReRollCost, _asyncCallback);
				}, null, callback);
			}
			else
			{
				AsyncCallback callback = OnReRollFinished;
				UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncCallback)
				{
					Singleton<StateManager>.Instance.ProfileManager.ForgeReroll(m_data.ReRollCost, _asyncCallback);
				}, null, callback);
			}
		}

		private void OnTutorialReRollFinished(IAsyncResult _ar)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (!_ar.IsError())
			{
				AsyncDataResult<ForgeRerollDuringTutorialResponse> asyncDataResult = (AsyncDataResult<ForgeRerollDuringTutorialResponse>)_ar;
				ForgeRerollDuringTutorialResponse forgeRerollDuringTutorialResponse = asyncDataResult.Result();
				UpdateForgeResult(forgeRerollDuringTutorialResponse.rolledElement, true);
			}
		}

		private void OnReRollFinished(IAsyncResult _ar)
		{
			if (!_ar.IsError())
			{
				AsyncDataResult<ForgeRerollResponse> asyncDataResult = (AsyncDataResult<ForgeRerollResponse>)_ar;
				ForgeRerollResponse forgeRerollResponse = asyncDataResult.Result();
				UpdateForgeResult(forgeRerollResponse.rolledElement, true);
			}
		}

		private void UpdateForgeResult(InventoryElement _element, bool _reroll)
		{
			m_forgeResult = _element;
			StartCoroutine(co_ForgeEnded(_reroll));
		}

		private void OnCloseDetails()
		{
			m_detailsPopup.Close();
		}

		private void UpdateScreenState(UIInventoryScreenData.EScreenState _state)
		{
			switch (_state)
			{
			case UIInventoryScreenData.EScreenState.Inventory:
			case UIInventoryScreenData.EScreenState.Details:
				DisableForgeItems();
				ItemInfoPopupHelper.OnAddToForgeCallback.Remove(OnAddItemToForge);
				BlueprintInfoPopupHelper.OnAddToForgeCallback.Remove(OnAddItemToForge);
				m_data.OnForgeShowGridPreview -= OnShowGridPreview;
				m_data.OnForgeHideGridPreview -= OnHideGridPreview;
				ItemInfoPopupHelper.OnRemoveFromForgeCallback.Remove(OnRemoveItemFromForge);
				BlueprintInfoPopupHelper.OnRemoveFromForgeCallback.Remove(OnRemoveItemFromForge);
				break;
			case UIInventoryScreenData.EScreenState.Forge:
			{
				ItemInfoPopupHelper.OnAddToForgeCallback.Add(OnAddItemToForge);
				BlueprintInfoPopupHelper.OnAddToForgeCallback.Add(OnAddItemToForge);
				m_data.OnForgeShowGridPreview += OnShowGridPreview;
				m_data.OnForgeHideGridPreview += OnHideGridPreview;
				ItemInfoPopupHelper.OnRemoveFromForgeCallback.Add(OnRemoveItemFromForge);
				BlueprintInfoPopupHelper.OnRemoveFromForgeCallback.Add(OnRemoveItemFromForge);
				for (int i = 0; i < m_forges.Length; i++)
				{
					m_forges[i].gameObject.SetActive(true);
					m_forges[i].PrepareVisibility();
				}
				m_animation.SampleAnimation(0f, "StartForge");
				m_helpDisplayed = true;
				UpdateForgeButton();
				break;
			}
			}
		}

		private void UpdateForgeButton()
		{
			m_buttonLabelHandler.ShowResource = false;
			m_forgeButton.isEnabled = !m_data.IsForgeEmpty && m_data.ForgePossible;
		}

		private void UpdateScreen()
		{
			m_leveLabel.gameObject.SetActive(!m_data.IsForgeEmpty);
			if (!m_data.IsForgeEmpty)
			{
				m_leveLabel.text = LocalizationManager.Instance.Get(s_levelId, new StringMapper { { "forge_item_lvl", m_data.ForgeOutcomeLevel } });
			}
		}

		private void UpdateFilter()
		{
			if (m_data.ForgeSize == 0)
			{
				m_data.CurrentFilter = (InventoryItemBase _element) => _element.Element.CanBeForged();
			}
			else if (m_data.IsOutcomeKnown)
			{
				m_data.CurrentFilter = (InventoryItemBase _element) => _element.Element.CanBeForged() && (_element.Element.quality == m_data.ForgedQuality || _element.Element.IsBlueprint());
			}
			else
			{
				if (!m_data.IsForgeValid)
				{
					return;
				}
				m_data.CurrentFilter = (InventoryItemBase _element) => _element is BuyBluePrintSlot || (_element.Element != null && ((_element.Element.CanBeForged() && _element.Element.quality == m_data.ForgedQuality && m_data.VisibleInventory.Exists((InventoryItemBase _x) => _x.Element != null && _x.Element.id.Equals(_element.Element.id))) || _element.Element.IsBlueprint()));
			}
		}

		private void ResetFilter()
		{
			m_data.CurrentFilter = ItemFilters.FilterForgeItems;
		}

		private void EnableForgeItems()
		{
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].gameObject.SetActive(true);
			}
		}

		private void DisableForgeItems()
		{
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].gameObject.SetActive(false);
			}
		}

		private void OnBlockBackground()
		{
			m_fullBlocker.ShowFade(true, true);
		}

		private void OnTriggerDetails()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			InventoryElement equipedEquivalent = currentAssassin.GetEquipedEquivalent(m_forgeResult);
			m_detailsPopup.transform.position = m_popupCenter.position;
			ItemInfoPopupHelper.ShowInfoPopup(m_detailsPopup, currentAssassin, ItemInfoPopupHelper.EMode.ReRoll | ItemInfoPopupHelper.EMode.ForgeClaim, m_forgeResult, equipedEquivalent, null, null, false, 0, string.Empty, ReRollCostConfigurations[0].Resource.amount <= 0);
		}

		private IEnumerator co_ShowForge()
		{
			m_fullBlocker.On(false, false);
			UpdateForgeButton();
			UpdateScreen();
			string currentClip = ((!m_helpDisplayed) ? "ForgeUpdateTween" : "StartForge");
			m_animation.SampleAnimation(0f, currentClip);
			EnableForgeItems();
			List<UnityEngine.Animation> anims = new List<UnityEngine.Animation>(m_forges.Length);
			for (int j = 0; j < m_forges.Length; j++)
			{
				m_forges[j].PrepareVisibility();
				anims.Add(m_forges[j].GetComponent<UnityEngine.Animation>());
			}
			m_armsTweenAlpha.from = 0f;
			m_armsTweenAlpha.to = 0.3f;
			m_itemTween.SetAnimationList(anims.ToArray(), "ForgeItemShow");
			m_itemTween.IgnoreParent = true;
			m_itemTween.enabled = false;
			m_itemTween.StartAnimation();
			m_animation.Play(currentClip);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_animation.IsPlaying(currentClip))
			{
				yield return wait;
			}
			for (int i = 0; i < m_forges.Length; i++)
			{
				m_forges[i].UpdateVisibility();
			}
			yield return StartCoroutine(co_UpdateItems());
			m_helpDisplayed = false;
			m_fullBlocker.Off(false);
		}

		private IEnumerator co_ShowHelp()
		{
			m_fullBlocker.On(false, false);
			m_helpDisplayed = true;
			List<UnityEngine.Animation> anims = new List<UnityEngine.Animation>(m_forges.Length);
			for (int i = 0; i < m_forges.Length; i++)
			{
				if (m_forges[i].ItemCurrentlyVisible)
				{
					anims.Add(m_forges[i].GetComponent<UnityEngine.Animation>());
				}
			}
			m_itemTween.SetAnimationList(anims.ToArray(), "ForgeItemHide");
			m_itemTween.IgnoreParent = true;
			m_itemTween.enabled = true;
			m_itemTween.StartAnimation();
			m_animation.Play("ResetForge");
			m_helpDisplayed = true;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_animation.IsPlaying("ResetForge"))
			{
				yield return wait;
			}
			DisableForgeItems();
			m_fullBlocker.Off(false);
		}

		private IEnumerator co_UpdateItems()
		{
			m_fullBlocker.On(false, false);
			List<UnityEngine.Animation> anims = new List<UnityEngine.Animation>(m_forges.Length);
			for (int i = 0; i < m_forges.Length; i++)
			{
				if (m_forges[i].Changed)
				{
					anims.Add(m_forges[i].GetComponent<UnityEngine.Animation>());
				}
			}
			m_itemTween.SetAnimationList(anims.ToArray(), "ForgeOverExposure");
			m_itemTween.IgnoreParent = true;
			m_itemTween.enabled = true;
			m_itemTween.StartAnimation();
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_itemTween.IsAnimating)
			{
				yield return wait;
			}
			m_fullBlocker.Off(false);
		}

		private IEnumerator co_ForgeEnded(bool _reRoll)
		{
			string clip = ((!_reRoll) ? "ForgeResult" : "ReForgeResult");
			float tmpDelay = m_itemTween.Delay;
			m_fullBlocker.On(_reRoll, false);
			if (_reRoll)
			{
				m_itemTween.SetAnimationList(new UnityEngine.Animation[0], string.Empty);
			}
			else
			{
				List<UnityEngine.Animation> anims = new List<UnityEngine.Animation>(m_forges.Length);
				for (int i = 0; i < m_forges.Length; i++)
				{
					if (m_forges[i].Visible)
					{
						anims.Add(m_forges[i].GetComponent<UnityEngine.Animation>());
					}
				}
				anims.Sort((UnityEngine.Animation _x, UnityEngine.Animation _y) => _x.transform.position.x.CompareTo(_y.transform.position.x));
				m_itemTween.SetAnimationList(anims.ToArray(), "ItemForgeResult");
				m_itemTween.Delay = 0.05f;
				m_itemTween.IgnoreParent = true;
				m_itemTween.enabled = false;
			}
			m_animation.Play(clip);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_animation.IsPlaying(clip))
			{
				yield return wait;
			}
			m_itemTween.Delay = tmpDelay;
		}

		private static List<UIPopupResourcesContent.ResourceConfiguration> CreateZeroCostConfiguration()
		{
			List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
			list.Add(new UIPopupResourcesContent.ResourceConfiguration
			{
				HasEnoughResources = true,
				Resource = new Resource
				{
					amount = 0L,
					type = EResourceType.SoftCurrency
				}
			});
			return list;
		}
	}
}
