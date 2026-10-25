using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.ShopBundlePreview
{
	public class UIItemBundlePreview : MonoBehaviour
	{
		private const int c_multiBundleScreenSize = 1160;

		private const int c_multiBundleResourceLineSize = 70;

		private const int c_singleBundleScreenSize = 1430;

		private const int c_singleBundleResourceLineSize = 120;

		private const int c_multilineItemStartCount = 4;

		private const string c_openAnimationName = "ItemBundleOpen";

		private const string c_closeAnimationName = "ItemBundleClose";

		private static Color32 s_defaultbackgroundColor = new Color32(68, 136, 162, byte.MaxValue);

		[SerializeField]
		[CheckAssigned]
		private UIInfoPopup m_infoPopup;

		[CheckAssigned]
		[SerializeField]
		private UnityEngine.Animation m_mainAnimation;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_mainContainer;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_alignmentBoxMain;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_background;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_headline;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_description;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_alignmentBoxRow1;

		[CheckAssigned]
		[SerializeField]
		private UIMissionRewardItemBehaviour[] m_itemsRow1;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_alignmentBoxRow2;

		[SerializeField]
		[CheckAssigned]
		private UIMissionRewardItemBehaviour[] m_itemsRow2;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_resourceContainer;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_oldPriceContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_oldPrice;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_oldPriceLine;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_priceAlignmentBox;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_alignmentBoxNewPrice;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_newPrice;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_prePriceText;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_newPriceCurrencyIcon;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_discountContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_discountValue;

		[SerializeField]
		[CheckAssigned]
		private UIButtonProxy m_buyButton;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_timerContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_timerText;

		private long m_availableEndTimeValue;

		private DateTime m_availableEndTimeInDateTime;

		private ShopBundle m_currentSelectedShopBundle;

		private UIMissionRewardItemBehaviour m_currentSelectedItem;

		private bool m_showBuyAnimation;

		private bool m_showBuyInventorySpace;

		private bool m_checkMailIsFull;

		private string m_overrideTitle;

		private string m_overrideDescription;

		private ShopBundle[] m_shopBundles;

		private int m_waitForAlignmentCounter;

		public List<EventDelegate> OnCloseButton = new List<EventDelegate>();

		public List<EventDelegate> OnAcceptedButton = new List<EventDelegate>();

		public bool IsMultiBundelPreview
		{
			get
			{
				return m_shopBundles.Length > 1;
			}
		}

		private bool IsForgeTutorialActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Forge);
			}
		}

		public event Action OnPreviewOpenCallback;

		public event Action OnPreviewCloseCallback;

		private void ResetValue()
		{
			m_showBuyInventorySpace = false;
			m_checkMailIsFull = true;
			m_showBuyAnimation = true;
			m_overrideTitle = string.Empty;
			m_overrideDescription = string.Empty;
			m_shopBundles = null;
		}

		public void OpenPreviewForShopItemSelection(ShopBundle[] _bundles, string _title, string _description, bool _showBuyAnimation, bool _showBuyInventorySpace, bool _checkMailIsFull)
		{
			ResetValue();
			m_checkMailIsFull = _checkMailIsFull;
			m_showBuyInventorySpace = _showBuyInventorySpace;
			m_showBuyAnimation = _showBuyAnimation;
			m_overrideTitle = _title;
			m_overrideDescription = _description;
			m_shopBundles = _bundles;
			UpdatePreview();
			Action onPreviewOpenCallback = this.OnPreviewOpenCallback;
			if (onPreviewOpenCallback != null)
			{
				onPreviewOpenCallback();
			}
			base.gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(OnStartAnimation("ItemBundleOpen"));
		}

		public void OpenPreviewByShopItem(ShopBundle _bundle, string _title = "", string _description = "")
		{
			ResetValue();
			Action onPreviewOpenCallback = this.OnPreviewOpenCallback;
			if (onPreviewOpenCallback != null)
			{
				onPreviewOpenCallback();
			}
			if (_bundle == null || _bundle.bundleValue == 0 || !_bundle.IsUnlocked())
			{
				OnClosePopup();
				return;
			}
			m_shopBundles = new ShopBundle[1] { _bundle };
			m_currentSelectedShopBundle = _bundle;
			m_overrideTitle = _title;
			m_overrideDescription = _description;
			UpdatePreview();
			base.gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(OnStartAnimation("ItemBundleOpen"));
		}

		public UIMissionRewardItemBehaviour GetRewardItemByBundleId(string _bundleId)
		{
			UIMissionRewardItemBehaviour rewardItemByBundleId = GetRewardItemByBundleId(_bundleId, m_itemsRow1);
			if (rewardItemByBundleId == null)
			{
				rewardItemByBundleId = GetRewardItemByBundleId(_bundleId, m_itemsRow2);
			}
			return rewardItemByBundleId;
		}

		private UIMissionRewardItemBehaviour GetRewardItemByBundleId(string _bundleId, UIMissionRewardItemBehaviour[] _items)
		{
			for (int i = 0; i < m_itemsRow1.Length; i++)
			{
				UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = _items[i];
				if (uIMissionRewardItemBehaviour.ShopBundle != null && uIMissionRewardItemBehaviour.ShopBundle.id.Equals(_bundleId))
				{
					return uIMissionRewardItemBehaviour;
				}
			}
			return null;
		}

		public void OnClosePopupUnity()
		{
			OnClosePopup();
		}

		public void OnClosePopup()
		{
			if (base.gameObject.activeSelf)
			{
				Action onPreviewCloseCallback = this.OnPreviewCloseCallback;
				if (onPreviewCloseCallback != null)
				{
					onPreviewCloseCallback();
				}
				m_infoPopup.Close();
				StopAllCoroutines();
				StartCoroutine(OnStartAnimation("ItemBundleClose", delegate
				{
					base.gameObject.SetActive(false);
				}));
				EventDelegate.Execute(OnCloseButton);
			}
		}

		public void OnBuyBundle()
		{
			if (IsForgeTutorialActive)
			{
				OnClosePopup();
			}
			else if (m_showBuyInventorySpace)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				int num = profileManager.InventoryFreeSpace - m_currentSelectedShopBundle.GetInventorySpaceNeededForLootTemplates();
				if (num < 0)
				{
					UIBuyHelper.ShowInventoryAndMailIsFullPopup(BuySelectedShopBundle, Math.Abs(num), m_checkMailIsFull);
				}
				else
				{
					BuySelectedShopBundle();
				}
			}
			else
			{
				BuySelectedShopBundle();
			}
		}

		private void Awake()
		{
			for (int i = 0; i < m_itemsRow1.Length; i++)
			{
				m_itemsRow1[i].OnClickCallback += OnRewardItemClicked;
			}
			for (int j = 0; j < m_itemsRow2.Length; j++)
			{
				m_itemsRow2[j].OnClickCallback += OnRewardItemClicked;
			}
			m_infoPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
		}

		private void OnEnable()
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged += new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null)
			{
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				if (buyShopItemHelper != null)
				{
					buyShopItemHelper.OnPurchasedEvent += new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
				}
			}
		}

		private void OnDisable()
		{
			m_overrideTitle = string.Empty;
			m_overrideDescription = string.Empty;
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.IsSelected = false;
			}
			m_shopBundles = null;
			m_currentSelectedItem = null;
			m_currentSelectedShopBundle = null;
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			if (eventManager != null)
			{
				eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
				eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage).OnChanged -= new SimpleDelegate.CallbackFunction(OnEventUpdatePopup);
			}
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null)
			{
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				if (buyShopItemHelper != null)
				{
					buyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
				}
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_itemsRow1.Length; i++)
			{
				if (m_itemsRow1[i] != null)
				{
					m_itemsRow1[i].OnClickCallback -= OnRewardItemClicked;
				}
			}
			for (int j = 0; j < m_itemsRow2.Length; j++)
			{
				if (m_itemsRow2[j] != null)
				{
					m_itemsRow2[j].OnClickCallback -= OnRewardItemClicked;
				}
			}
			m_infoPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
		}

		private void OnEventUpdatePopup(object[] _params)
		{
			UpdatePreview();
		}

		private void OnShopItemPurchased(object[] _params)
		{
			EventDelegate.Execute(OnAcceptedButton);
			if (m_showBuyAnimation)
			{
				bool flag = (bool)_params[1];
				if (!flag)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(8012));
				}
				if (IsMultiBundelPreview)
				{
					EventDelegate.Callback resourceMovedFinished = null;
					EventDelegate.Callback animationFinished = null;
					if (m_currentSelectedItem.ShopBundle.HasWalletPayoutResource())
					{
						resourceMovedFinished = OnAnimationFinished;
					}
					else
					{
						animationFinished = OnAnimationFinished;
					}
					UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
					recivedItemMgr.StartReciveItemAnimation(m_currentSelectedItem.ShopBundle, m_currentSelectedItem.gameObject, null, animationFinished, true, resourceMovedFinished, flag);
				}
				else
				{
					for (int i = 0; i < m_itemsRow1.Length; i++)
					{
						if (m_itemsRow1[i].gameObject.activeSelf)
						{
							StartClaimAnimation(m_itemsRow1[i].gameObject, m_itemsRow1[i].LootTemplate, flag);
						}
					}
					for (int j = 0; j < m_itemsRow2.Length; j++)
					{
						if (m_itemsRow2[j].gameObject.activeSelf)
						{
							StartClaimAnimation(m_itemsRow2[j].gameObject, m_itemsRow2[j].LootTemplate, flag);
						}
					}
				}
			}
			OnClosePopup();
		}

		private void StartClaimAnimation(GameObject _target, LootTemplate _template, bool _wasSentToInventory)
		{
			EventDelegate.Callback resourceMovedFinished = null;
			EventDelegate.Callback animationFinished = null;
			if (m_currentSelectedShopBundle.HasWalletPayoutResource())
			{
				resourceMovedFinished = OnAnimationFinished;
			}
			else
			{
				animationFinished = OnAnimationFinished;
			}
			UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
			recivedItemMgr.StartReciveItemAnimation(m_currentSelectedShopBundle, _template, _target, null, animationFinished, true, resourceMovedFinished, _wasSentToInventory);
		}

		private void OnAnimationFinished()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
		}

		private void OnBeforePopupCloseCallback()
		{
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.IsSelected = false;
			}
			m_currentSelectedItem = null;
		}

		private void OnRewardItemClicked(UIMissionRewardItemBehaviour _rewardItem)
		{
			if (IsMultiBundelPreview)
			{
				if (m_currentSelectedItem != null)
				{
					m_currentSelectedItem.IsSelected = false;
				}
				if (m_currentSelectedItem != _rewardItem && _rewardItem != null)
				{
					m_currentSelectedItem = _rewardItem;
					m_currentSelectedItem.IsSelected = true;
					m_currentSelectedShopBundle = _rewardItem.ShopBundle;
				}
				else
				{
					m_currentSelectedItem = null;
					m_currentSelectedShopBundle = null;
				}
				UpdateCurrency();
			}
			else
			{
				m_currentSelectedItem = m_infoPopup.UpdateInfoPopupByRewardItem(m_currentSelectedItem, _rewardItem, true);
				if (m_currentSelectedItem != null)
				{
					m_infoPopup.MovePopupToTarget(m_currentSelectedItem.gameObject);
				}
			}
		}

		private IEnumerator OnStartAnimation(string _name, Action _callback = null)
		{
			m_mainAnimation.SampleAnimation(0f, _name);
			m_mainAnimation.Play(_name);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_mainAnimation.isPlaying)
			{
				yield return wait;
			}
			if (_callback != null)
			{
				_callback();
			}
		}

		public void OnAlignmentBoxReposition()
		{
			m_waitForAlignmentCounter--;
			if (m_waitForAlignmentCounter <= 0)
			{
				m_alignmentBoxMain.RepositionNow = true;
			}
		}

		private void Update()
		{
			if (IsMultiBundelPreview)
			{
				m_buyButton.isEnabled = m_currentSelectedItem != null;
				return;
			}
			m_buyButton.isEnabled = true;
			double num = 0.0;
			if (m_availableEndTimeValue > 0)
			{
				DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
				num = (m_availableEndTimeInDateTime - utcNow).TotalSeconds;
				if (num <= 0.0)
				{
					UpdatePreview();
					return;
				}
			}
			if (num > 0.0)
			{
				m_timerText.text = UIDuration.FromSeconds((long)num).ToString();
			}
			m_timerContainer.gameObject.SetActive(num > 0.0);
		}

		private void UpdatePreview()
		{
			ResetInfoPoup();
			m_waitForAlignmentCounter = 0;
			m_headline.text = (string.IsNullOrEmpty(m_overrideTitle) ? m_currentSelectedShopBundle.LocaName().Replace("\n", " ") : m_overrideTitle);
			if (!string.IsNullOrEmpty(m_overrideDescription))
			{
				m_description.text = m_overrideDescription;
			}
			else if (m_currentSelectedShopBundle.shortDescLangKey != null)
			{
				m_description.text = m_currentSelectedShopBundle.LocaShortDescription();
			}
			else
			{
				m_description.text = m_currentSelectedShopBundle.LocaDescription();
			}
			UpdateCurrency();
			if (IsMultiBundelPreview)
			{
				int num = m_shopBundles.Length;
				int num2 = ((num > 4) ? (num / 2) : 0);
				int itemCount = num - num2;
				int _startIndex = 0;
				UpdateShopBundleResourceRow(ref _startIndex, itemCount, m_itemsRow1, m_alignmentBoxRow1);
				UpdateShopBundleResourceRow(ref _startIndex, num2, m_itemsRow2, m_alignmentBoxRow2);
				m_timerContainer.gameObject.SetActive(false);
			}
			else
			{
				int num3 = m_currentSelectedShopBundle.payout.lootTemplates.Length;
				int num4 = ((num3 > 4) ? (num3 / 2) : 0);
				int itemCount2 = num3 - num4;
				int _payoutIndex = 0;
				UpdatePayoutResourceRow(ref _payoutIndex, itemCount2, m_itemsRow1, m_alignmentBoxRow1);
				UpdatePayoutResourceRow(ref _payoutIndex, num4, m_itemsRow2, m_alignmentBoxRow2);
				long availableEndTimer = m_currentSelectedShopBundle.GetAvailableEndTimer();
				m_availableEndTimeValue = availableEndTimer;
				m_availableEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_availableEndTimeValue);
				Update();
			}
			FixLayoutScreeSize();
		}

		private void FixLayoutScreeSize()
		{
			int width = 1430;
			int height = 120;
			if (IsMultiBundelPreview)
			{
				width = 1160;
				height = 70;
			}
			m_mainContainer.width = width;
			m_resourceContainer.height = height;
		}

		private void UpdatePayoutResourceRow(ref int _payoutIndex, int _itemCount, UIMissionRewardItemBehaviour[] _items, UIAlignmentBox _alignmentBox)
		{
			_alignmentBox.gameObject.SetActive(_itemCount > 0);
			if (_itemCount <= 0)
			{
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = _items[i];
				if (i < _itemCount)
				{
					uIMissionRewardItemBehaviour.UpdateRewardByTemplate(m_currentSelectedShopBundle.payout.lootTemplates[_payoutIndex]);
					_payoutIndex++;
				}
				uIMissionRewardItemBehaviour.gameObject.SetActive(i < _itemCount);
			}
			m_waitForAlignmentCounter++;
			EventDelegate.Add(_alignmentBox.OnReposition, OnAlignmentBoxReposition, true);
			_alignmentBox.RepositionNow = true;
		}

		private void UpdateShopBundleResourceRow(ref int _startIndex, int _itemCount, UIMissionRewardItemBehaviour[] _items, UIAlignmentBox _alignmentBox)
		{
			_alignmentBox.gameObject.SetActive(_itemCount > 0);
			if (_itemCount <= 0)
			{
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				UIMissionRewardItemBehaviour uIMissionRewardItemBehaviour = _items[i];
				if (i < _itemCount)
				{
					uIMissionRewardItemBehaviour.UpdateRewardByShopBundle(m_shopBundles[_startIndex]);
					_startIndex++;
				}
				uIMissionRewardItemBehaviour.gameObject.SetActive(i < _itemCount);
			}
			m_waitForAlignmentCounter++;
			EventDelegate.Add(_alignmentBox.OnReposition, OnAlignmentBoxReposition, true);
			_alignmentBox.RepositionNow = true;
		}

		private void UpdateCurrency()
		{
			bool isMultiBundelPreview = IsMultiBundelPreview;
			bool flag = m_currentSelectedShopBundle != null && m_currentSelectedShopBundle.IsDirectPurchasesBundle();
			bool active = !isMultiBundelPreview && !flag;
			m_discountContainer.gameObject.SetActive(active);
			m_oldPriceContainer.gameObject.SetActive(active);
			m_prePriceText.gameObject.SetActive(active);
			m_newPriceCurrencyIcon.transform.parent.gameObject.SetActive(!flag);
			m_alignmentBoxNewPrice.gameObject.SetActive(m_currentSelectedShopBundle != null);
			if (m_currentSelectedShopBundle == null)
			{
				return;
			}
			if (flag)
			{
				UpdateDirectPriceInfo();
			}
			else
			{
				Resource resource2;
				if (IsForgeTutorialActive)
				{
					Resource resource = new Resource();
					resource.amount = 0L;
					resource.type = EResourceType.SoftCurrency;
					resource2 = resource;
				}
				else
				{
					resource2 = m_currentSelectedShopBundle.GetFinalDiscountedResourceCost();
				}
				Resource resource3 = resource2;
				UIUtil.SetupCurrencyWidgets(resource3, m_newPrice, m_newPriceCurrencyIcon, false, true);
				if (!isMultiBundelPreview)
				{
					m_prePriceText.LocalizationKey = 7755;
					UpdatePriceInfo(resource3);
				}
			}
			m_alignmentBoxNewPrice.RepositionNow = true;
			m_priceAlignmentBox.RepositionNow = true;
		}

		private void UpdatePriceInfo(Resource _finalCost)
		{
			int bundleValue = m_currentSelectedShopBundle.bundleValue;
			m_discountValue.text = LocalizationManager.Instance.Get(1785, new StringMapper { 
			{
				"value",
				Math.Abs(Math.Round((1f - (float)_finalCost.amount / (float)bundleValue) * 100f))
			} });
			m_oldPrice.text = LocalizationManager.Instance.GetInteger(bundleValue);
			m_oldPrice.color = UIDataMapper.UITextColors.DiscountOldPriceColorBright;
			Vector3[] localCorners = m_oldPrice.localCorners;
			Vector2 vector = localCorners[0] - localCorners[2];
			float z = Mathf.Atan2(vector.y, vector.x) * 180f / (float)Math.PI;
			m_oldPriceLine.width = (int)Mathf.Sqrt(m_oldPrice.width * m_oldPrice.width + m_oldPrice.height * m_oldPrice.height);
			m_oldPriceLine.transform.localRotation = Quaternion.Euler(0f, 0f, z);
			m_oldPriceLine.transform.localPosition = new Vector3(-m_oldPrice.width / 2, 0f);
			m_oldPriceLine.color = UIDataMapper.UITextColors.DiscountCrossOutLineColorBright;
		}

		private void UpdateDirectPriceInfo()
		{
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopItemNode shopItemById = shopDetails.CategoryRootNode.GetShopItemById(m_currentSelectedShopBundle.id);
			ShopDirectPurchasItemNode shopDirectPurchasItemNode = shopItemById as ShopDirectPurchasItemNode;
			if (shopDirectPurchasItemNode != null)
			{
				m_newPrice.ClearFormatFlags();
				m_newPrice.text = shopDirectPurchasItemNode.ItemCurrencyText;
			}
			m_discountValue.text = LocalizationManager.Instance.Get(8369);
		}

		private void BuySelectedShopBundle()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			int num = profileManager.InventoryFreeSpace - m_currentSelectedShopBundle.GetInventorySpaceNeededForLootTemplates();
			if (num < 0)
			{
				m_showBuyAnimation = true;
			}
			else if (m_currentSelectedShopBundle.HasWalletPayoutResource())
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			}
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			buyShopItemHelper.BuyItem(m_currentSelectedShopBundle, m_currentSelectedShopBundle.GetFinalDiscountedResourceCost());
		}

		private void ResetInfoPoup()
		{
			m_infoPopup.IsEnabled = false;
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.IsSelected = false;
			}
			m_currentSelectedItem = null;
			m_background.color = s_defaultbackgroundColor;
		}
	}
}
