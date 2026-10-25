using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopItemDetails : UIShopBaseItem
	{
		private const string c_normalBackgroundSpriteName = "aUI_fra_goods_normal";

		private const string c_heroicBackgroundlaySpriteName = "aUI_fra_goods_heroic";

		private static readonly Color32 s_normalTitleColor = new Color32(126, 150, 163, byte.MaxValue);

		private static readonly Color32 s_heroicTitleColor = new Color32(38, 49, 64, byte.MaxValue);

		private static readonly Color32 s_normalInfoBackgroundColor = new Color32(0, 0, 0, 128);

		private static readonly Color32 s_heroicInfoBackgroundColor = new Color32(76, 87, 111, 128);

		[SerializeField]
		private BBTargetedTween m_toggleItemInfoTween;

		[SerializeField]
		private BBTargetedTween m_pressedTween;

		[SerializeField]
		private UISprite m_backgrond;

		[SerializeField]
		private UISprite m_fadeBackgrond;

		[SerializeField]
		private UIWidget m_infoOpenIndicator;

		[SerializeField]
		private UIWidget m_infoCloseIndicator;

		[SerializeField]
		private UISprite m_infoButtonBackgrond;

		[SerializeField]
		private UIExtendedLabel m_itemName;

		[SerializeField]
		private UISprite m_titleBackgorund;

		[SerializeField]
		private GameObject m_infoPopupArrowTarget;

		[SerializeField]
		private UIShopItemMainDetails m_itemMainDetails;

		[SerializeField]
		private UIShopItemInfoDetails m_itemInfoDetails;

		public SimpleDelegate OnDisplayChanged;

		private bool m_hasClosingItemTweenStarted;

		private bool m_reverseToggleTweenOnNextUpdate;

		private EShopItemViewType m_shopItemViewTypeShown;

		private EShopItemViewType m_nextShopItemViewType;

		private ShopItemNode m_itemNodeData;

		private UIShopItemScroller m_shopItemScrollerParent;

		private bool m_updateDataNow;

		private int m_discountIntValue;

		private long m_discountEndTime;

		private DateTime m_discountEndTimeInDateTime;

		private float m_happyHourValue;

		private long m_happyHourEndTime;

		private DateTime m_happyHourEndTimeInDateTime;

		public GameObject BuyButton
		{
			get
			{
				return m_itemInfoDetails.BuyButton;
			}
		}

		public string ShopItemId
		{
			get
			{
				return m_itemNodeData.ShopBundle.id;
			}
		}

		public ShopBundle ShopBundle
		{
			get
			{
				return m_itemNodeData.ShopBundle;
			}
		}

		public override EShopNodeType ShopDataNodeType
		{
			get
			{
				return EShopNodeType.Item;
			}
		}

		public UIShopItemScroller ShopItemsScrollParent
		{
			set
			{
				m_shopItemScrollerParent = value;
			}
		}

		public EShopItemViewType OnOpenViewTypeInLateUpdate { get; set; }

		public GameObject InfoPopupArrowTarget
		{
			get
			{
				return m_infoPopupArrowTarget ?? base.gameObject;
			}
		}

		public EShopItemViewType NextShopItemViewType
		{
			get
			{
				return m_nextShopItemViewType;
			}
			set
			{
				if (m_nextShopItemViewType == value)
				{
					return;
				}
				m_nextShopItemViewType = value;
				if (OnDisplayChanged != null)
				{
					OnDisplayChanged.Trigger(m_nextShopItemViewType);
				}
				if (m_nextShopItemViewType != ShopItemViewTypeShown && !m_hasClosingItemTweenStarted)
				{
					float duration = 0.15f;
					if (m_nextShopItemViewType == EShopItemViewType.ItemInfo)
					{
						duration = 0.1f;
						UpdateDataByViewType(EShopItemViewType.ItemInfo);
					}
					m_toggleItemInfoTween.duration = duration;
					if (!m_reverseToggleTweenOnNextUpdate)
					{
						m_hasClosingItemTweenStarted = true;
						EventDelegate.Add(m_toggleItemInfoTween.onFinished, OnTweenFinished, true);
						m_toggleItemInfoTween.StartTweenPlayForward();
					}
				}
			}
		}

		private EShopItemViewType ShopItemViewTypeShown
		{
			get
			{
				return m_shopItemViewTypeShown;
			}
			set
			{
				SetShopTypeVisibility(value, true);
				m_shopItemViewTypeShown = value;
			}
		}

		private bool IsAlreadyOwned { get; set; }

		private bool IsUnlocked { get; set; }

		private bool IsHeroicShop { get; set; }

		public UIShopItemDetails()
		{
			OnDisplayChanged = new SimpleDelegate(this, new Type[1] { typeof(EShopItemViewType) });
		}

		public override void UpdateData(ShopBaseNode _baseNode)
		{
			base.UpdateData(_baseNode);
			m_updateDataNow = true;
		}

		public override void ResetItemContainer()
		{
			NextShopItemViewType = EShopItemViewType.ItemMain;
		}

		public void OnTapBuyItem()
		{
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			buyShopItemHelper.BuyItem(m_itemNodeData.ShopBundle, m_itemNodeData.ShopBundle.GetFinalDiscountedResourceCost());
		}

		public void StartItemPurchasedEffect(string _bundleId, bool _wasSentToInventory, bool _isBuyingFromShop)
		{
			if (m_itemNodeData == null || (_bundleId != ShopItemId && _bundleId != m_itemNodeData.ShopBundle.houstonId))
			{
				return;
			}
			int _rank;
			if (m_itemNodeData.ShopBundle.category == EShopCategory.Shopcat_Heroic && m_itemNodeData.ShopBundle.TryGetHeroicRank(out _rank))
			{
				StartHeroicRankUnlockAnimation(_rank);
				return;
			}
			m_updateDataNow = true;
			ResetItemContainer();
			if (!_wasSentToInventory)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(8012));
			}
			if (_isBuyingFromShop && m_itemNodeData.ShopBundle.bundleValue == 0 && m_itemNodeData.ShopBundle.IsShownInShop(UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive))
			{
				EventDelegate.Callback resourceMovedFinished = null;
				EventDelegate.Callback animationFinished = null;
				if (m_itemNodeData.ShopBundle.HasWalletPayoutResource())
				{
					resourceMovedFinished = OnAnimationFinished;
				}
				else
				{
					animationFinished = OnAnimationFinished;
				}
				UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
				recivedItemMgr.StartReciveItemAnimation(m_itemNodeData.ShopBundle, m_itemMainDetails.ItemTextureObject, null, animationFinished, true, resourceMovedFinished, _wasSentToInventory);
			}
		}

		private void StartHeroicRankUnlockAnimation(int _rank)
		{
			UIHeroicRankUnlock heroicRankUnlockOverlay = m_shopItemScrollerParent.HeroicRankUnlockOverlay;
			heroicRankUnlockOverlay.Init(_rank, m_itemNodeData.ShopBundle);
			heroicRankUnlockOverlay.gameObject.SetActive(true);
		}

		private void OnShopItemPurchased(params object[] _params)
		{
			string bundleId = (string)_params[0];
			bool wasSentToInventory = (bool)_params[1];
			bool isBuyingFromShop = (bool)_params[2];
			StartItemPurchasedEffect(bundleId, wasSentToInventory, isBuyingFromShop);
			if (m_shopItemScrollerParent != null)
			{
				m_shopItemScrollerParent.OnForceUpdateLayout();
			}
			if (m_itemNodeData != null && m_itemNodeData.ParentNode.Category == EShopCategory.Shopcat_Heroic)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_unlock_buymissionunlock);
			}
		}

		private void OnAnimationFinished()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim);
		}

		protected override void OnAwake()
		{
			base.OnAwake();
		}

		protected override void OnDestroyItem()
		{
			m_itemNodeData = null;
			OnDisplayChanged = null;
			m_shopItemScrollerParent = null;
		}

		protected override void OnItemPress(bool _pressed)
		{
			if (_pressed)
			{
				m_pressedTween.StartTweenPlayForward();
			}
			else
			{
				m_pressedTween.SetTweenToBeginning();
			}
		}

		protected override void OnItemDragStart()
		{
			if (base.IsSelected)
			{
				m_pressedTween.StartTweenPlayReverse();
			}
		}

		protected override void OnItemClicked()
		{
			switch (ShopItemViewTypeShown)
			{
			case EShopItemViewType.ItemMain:
				NextShopItemViewType = EShopItemViewType.ItemInfo;
				EventDelegate.Execute(OnSelected);
				break;
			case EShopItemViewType.ItemInfo:
				NextShopItemViewType = EShopItemViewType.ItemMain;
				EventDelegate.Execute(OnSelected);
				break;
			}
		}

		private void OnEnable()
		{
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
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
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null)
			{
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				if (buyShopItemHelper != null)
				{
					buyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
				}
			}
		}

		private void OnTweenFinished()
		{
			m_reverseToggleTweenOnNextUpdate = true;
		}

		private void Update()
		{
			if (base.CurrentShopBaseNode != null)
			{
				bool flag = UpdateDiscountData();
				bool flag2 = UpdateHappyHourData();
				UpdateDiscountTimer();
				UpdateHappyHourTimer();
				if (flag)
				{
					UpdateDataByViewType(ShopItemViewTypeShown);
				}
				if (flag2)
				{
					UpdateDataByViewType(ShopItemViewTypeShown);
				}
			}
		}

		private void LateUpdate()
		{
			UpdateData();
			if (m_reverseToggleTweenOnNextUpdate)
			{
				m_reverseToggleTweenOnNextUpdate = false;
				ShopItemViewTypeShown = NextShopItemViewType;
				m_toggleItemInfoTween.StartTweenPlayReverse();
				m_hasClosingItemTweenStarted = false;
			}
		}

		private void UpdateData()
		{
			if (m_updateDataNow && base.CurrentShopBaseNode != null)
			{
				m_itemNodeData = (ShopItemNode)base.CurrentShopBaseNode;
				IsHeroicShop = m_itemNodeData.ShopBundle.IsHeroicBundle();
				IsUnlocked = m_itemNodeData.ShopBundle.IsUnlocked();
				IsAlreadyOwned = m_itemNodeData.ShopBundle.IsAlreadyOwned();
				m_discountEndTime = long.MaxValue;
				m_happyHourEndTime = long.MaxValue;
				m_discountIntValue = 0;
				m_happyHourValue = 0f;
				m_hasClosingItemTweenStarted = false;
				m_reverseToggleTweenOnNextUpdate = false;
				if (UIShopDetails.s_flipOnFocusId == m_itemNodeData.ShopBundle.id)
				{
					OnOpenViewTypeInLateUpdate = EShopItemViewType.ItemInfo;
					UIShopDetails.s_flipOnFocusId = string.Empty;
				}
				UpdateBasicLayout();
				UpdateDiscountData();
				UpdateHappyHourData();
				UpdateDiscountTimer();
				UpdateHappyHourTimer();
				EventDelegate.Remove(m_toggleItemInfoTween.onFinished, OnTweenFinished);
				ShopItemViewTypeShown = OnOpenViewTypeInLateUpdate;
				m_nextShopItemViewType = OnOpenViewTypeInLateUpdate;
				if (OnOpenViewTypeInLateUpdate == EShopItemViewType.ItemInfo)
				{
					UpdateDataByViewType(EShopItemViewType.ItemMain);
					UIShopBaseItem.Current = this;
					EventDelegate.Execute(OnSelected);
					UIShopBaseItem.Current = null;
				}
				UpdateDataByViewType(ShopItemViewTypeShown);
				m_toggleItemInfoTween.SetTweenToBeginning();
				m_itemName.text = m_itemNodeData.ShopBundle.LocaName();
				OnOpenViewTypeInLateUpdate = EShopItemViewType.ItemMain;
			}
			m_updateDataNow = false;
		}

		private void UpdateBasicLayout()
		{
			Color color = s_normalTitleColor;
			Color color2 = s_normalInfoBackgroundColor;
			string spriteName = "aUI_fra_goods_normal";
			if (IsHeroicShop)
			{
				color = s_heroicTitleColor;
				color2 = s_heroicInfoBackgroundColor;
				spriteName = "aUI_fra_goods_heroic";
			}
			else if (ShopBundle != null)
			{
				GameID previewImageGameID = ShopBundle.GetPreviewImageGameID();
				GameDB gameDB = Main.Instance.GameDB;
				ItemDef item = gameDB.GetItem<ItemDef>(previewImageGameID);
				if (item != null && item.UseDarkPriceBackground)
				{
					color2.a = 204f;
				}
			}
			m_infoButtonBackgrond.color = color2;
			m_titleBackgorund.color = color;
			m_backgrond.spriteName = spriteName;
			m_fadeBackgrond.gameObject.SetActive(IsHeroicShop);
		}

		private void UpdateDataByViewType(EShopItemViewType _viewType)
		{
			switch (_viewType)
			{
			case EShopItemViewType.ItemMain:
				m_itemMainDetails.UpdateData(m_itemNodeData);
				break;
			case EShopItemViewType.ItemInfo:
				m_itemInfoDetails.UpdateData(m_itemNodeData);
				break;
			}
		}

		private bool UpdateDiscountData()
		{
			if (m_itemNodeData == null)
			{
				return false;
			}
			long discountEndTime = m_discountEndTime;
			int discountIntValue = m_discountIntValue;
			bool discountData = m_itemNodeData.ShopBundle.GetDiscountData(out m_discountEndTime, out m_discountIntValue);
			bool flag = discountEndTime != m_discountEndTime || Math.Abs(discountIntValue - m_discountIntValue) > 0;
			if (discountData && flag)
			{
				m_discountEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_discountEndTime);
			}
			return flag;
		}

		private bool UpdateHappyHourData()
		{
			if (m_itemNodeData == null)
			{
				return false;
			}
			long happyHourEndTime = m_happyHourEndTime;
			float happyHourValue = m_happyHourValue;
			bool flag = m_itemNodeData.ShopBundle.category.GetShopCategoryHappyHourData(out m_happyHourEndTime, out m_happyHourValue) && (happyHourEndTime != m_happyHourEndTime || Math.Abs(happyHourValue - m_happyHourValue) > 0f);
			if (flag)
			{
				m_happyHourEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_happyHourEndTime);
			}
			return flag;
		}

		private void UpdateDiscountTimer()
		{
			double discountTimeLeftLeft = 0.0;
			if (m_discountIntValue > 0 && m_itemNodeData.ShopBundle.HasResourceCost())
			{
				DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
				discountTimeLeftLeft = (m_discountEndTimeInDateTime - utcNow).TotalSeconds;
			}
			m_itemMainDetails.DiscountTimeLeftLeft = discountTimeLeftLeft;
		}

		private void UpdateHappyHourTimer()
		{
			double happyHourTimeLeft = 0.0;
			if (m_happyHourValue > 0f && !m_itemNodeData.ShopBundle.HasResourceCost())
			{
				DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
				happyHourTimeLeft = (m_happyHourEndTimeInDateTime - utcNow).TotalSeconds;
			}
			m_itemMainDetails.HappyHourTimeLeft = happyHourTimeLeft;
		}

		private void UpdateCurrency(UISprite _sprite, UILabel _label, GameObject _currencyContainer)
		{
			bool active = false;
			if (!IsAlreadyOwned)
			{
				if (m_itemNodeData.DataType == EShopDataType.DirectPurchasItem)
				{
					active = true;
					UpdateHardCurrencyCost(_sprite, _label);
				}
				else if (m_itemNodeData.ShopBundle.HasResourceCost())
				{
					active = true;
					UpdateCurrencyCost(_sprite, _label);
				}
			}
			_currencyContainer.SetActive(active);
		}

		private void UpdateCurrencyCost(UISprite _sprite, UILabel _label)
		{
			Resource finalDiscountedResourceCost = m_itemNodeData.ShopBundle.GetFinalDiscountedResourceCost();
			string spriteName = string.Empty;
			CurrencyDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)finalDiscountedResourceCost.type, out value))
			{
				spriteName = value.SmallIconName;
			}
			_sprite.gameObject.SetActive(true);
			_sprite.spriteName = spriteName;
			_label.text = LocalizationManager.Instance.GetInteger(finalDiscountedResourceCost.amount);
		}

		private void UpdateHardCurrencyCost(UISprite _sprite, UILabel _label)
		{
			ShopDirectPurchasItemNode shopDirectPurchasItemNode = m_itemNodeData as ShopDirectPurchasItemNode;
			if (shopDirectPurchasItemNode != null)
			{
				_sprite.gameObject.SetActive(false);
				_label.text = shopDirectPurchasItemNode.ItemCurrencyText;
			}
		}

		private void SetShopTypeVisibility(EShopItemViewType _shopItemViewType, bool _visible)
		{
			bool active = false;
			bool active2 = false;
			switch (_shopItemViewType)
			{
			case EShopItemViewType.ItemMain:
				active = _visible;
				break;
			case EShopItemViewType.ItemInfo:
				active2 = _visible;
				break;
			}
			m_itemMainDetails.gameObject.SetActive(active);
			m_infoOpenIndicator.gameObject.SetActive(active);
			m_itemInfoDetails.gameObject.SetActive(active2);
			m_infoCloseIndicator.gameObject.SetActive(active2);
		}
	}
}
