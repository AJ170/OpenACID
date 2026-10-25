using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.ShopBundlePreview
{
	public class UIUnlockMissionBundlePreview : UIBaseBundlePreview
	{
		[SerializeField]
		[CheckAssigned]
		private UITexture m_previewBackground;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_waitingForServerContainer;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_waitForServerBox;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_unlockRewardTitle;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_unlockRewardVBox;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_unlockRewardLine1;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_unlockRewardLine2;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_unlockExtraRewardTitle;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_unlockExtraRewardVBox;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_unlockExtraRewardLine1;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_unlockExtraRewardLine2;

		private BBList<ShopBundle> m_shopBundleItems;

		private CampaignDefinition m_campaignDefinition;

		private ProductPricing m_productPricing;

		private ShopBundle m_selectedItem;

		public void OpenPreviewByEvent(CampaignCatalog.Campaign _campaign)
		{
			if (_campaign == null)
			{
				OnClosePopup();
				return;
			}
			m_selectedItem = null;
			m_productPricing = null;
			m_campaignDefinition = _campaign.GetCampaignDefinition();
			ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
			m_shopBundleItems = shopTable.GetShopBundleByEntitlementsPayout(_campaign.reqEntitlements);
			if (m_shopBundleItems.Count > 0)
			{
				OpenPreview();
				return;
			}
			OnClosePopup();
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(7760));
		}

		protected override void OnUnityEnable()
		{
			base.OnUnityEnable();
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			profileManager.onDirectPurchasedVerifyed = (Action<string>)Delegate.Combine(profileManager.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
			UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
			directPurchasesShopHelper.OnPricingListUpdated = (Action<ProductPricings>)Delegate.Combine(directPurchasesShopHelper.OnPricingListUpdated, new Action<ProductPricings>(OnPricingListUpdated));
		}

		protected override void OnUnityDisable()
		{
			base.OnUnityDisable();
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper.Clear();
				UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
				directPurchasesShopHelper.OnPricingListUpdated = (Action<ProductPricings>)Delegate.Remove(directPurchasesShopHelper.OnPricingListUpdated, new Action<ProductPricings>(OnPricingListUpdated));
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			profileManager.onDirectPurchasedVerifyed = (Action<string>)Delegate.Remove(profileManager.onDirectPurchasedVerifyed, new Action<string>(OnDirectPurchasedVerifyed));
			m_shopBundleItems = null;
		}

		protected override void OnActionButton()
		{
			base.OnActionButton();
			if (m_shopBundleItems == null || m_shopBundleItems.Count == 0)
			{
				return;
			}
			if (m_selectedItem != null && (!m_selectedItem.IsDirectPurchasesBundle() || m_productPricing != null))
			{
				ShopBundle selectedItem = m_selectedItem;
				if (!selectedItem.IsDirectPurchasesBundle())
				{
					OnClosePopup();
				}
				UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
				buyShopItemHelper.BuyItem(selectedItem, selectedItem.GetFinalDiscountedResourceCost());
			}
			else
			{
				UpdatePriceInformation();
			}
		}

		protected override void OnStartShopItemPurchased(bool _wasSentToInventory)
		{
			base.OnStartShopItemPurchased(_wasSentToInventory);
			if (m_shopBundleItems != null && m_shopBundleItems.Count != 0)
			{
				OnClosePopup();
			}
		}

		protected override void UpdatePreview()
		{
			base.UpdatePreview();
			UIItemUtil.UpdateUITexture(m_campaignDefinition.BackgroundGameId, m_previewBackground);
			m_headline.SetLocalizationKey(8364);
			m_background.color = UIDataMapper.UITextColors.CampaignUnlockBackgroundColor;
			m_unlockRewardTitle.color = UIDataMapper.UITextColors.CampaignUnlockTitleColor;
			m_unlockRewardLine1.SetLocalizationKey(m_campaignDefinition.UnlockRewardLocaLine1.Key);
			m_unlockRewardLine2.SetLocalizationKey(m_campaignDefinition.UnlockRewardLocaLine2.Key);
			m_unlockExtraRewardTitle.color = UIDataMapper.UITextColors.CampaignUnlockExtraTitleColor;
			m_unlockExtraRewardLine1.SetLocalizationKey(m_campaignDefinition.UnlockExtraRewardLocaLine1.Key);
			m_unlockExtraRewardLine2.SetLocalizationKey(m_campaignDefinition.UnlockExtraRewardLocaLine2.Key);
			m_unlockExtraRewardVBox.RepositionNow = true;
			m_unlockRewardVBox.RepositionNow = true;
			m_description.gameObject.SetActive(false);
			m_categoryDescription.gameObject.SetActive(false);
			m_availableEndTimeInDateTime = null;
			if (m_selectedItem == null)
			{
				m_selectedItem = GetFirstNoneDirectPurchasesBundle();
			}
			UpdatePriceInformation();
		}

		private ShopBundle GetFirstNoneDirectPurchasesBundle()
		{
			for (int i = 0; i < m_shopBundleItems.Count; i++)
			{
				ShopBundle shopBundle = m_shopBundleItems[i];
				if (!shopBundle.IsDirectPurchasesBundle() && shopBundle.IsEnabled())
				{
					return shopBundle;
				}
			}
			return null;
		}

		private void UpdatePriceInformation(ProductPricings _productPricings = null)
		{
			m_oldPriceContainer.gameObject.SetActive(false);
			m_resourceContainer.gameObject.SetActive(false);
			m_waitingForServerContainer.SetActive(false);
			m_actionButton.isEnabled = true;
			m_actionButton.TranslationKey = 1782;
			m_discountContainer.gameObject.SetActive(true);
			m_discountValue.text = LocalizationManager.Instance.Get(8369);
			bool flag = m_selectedItem != null && !m_selectedItem.IsDirectPurchasesBundle();
			if (!flag)
			{
				UpdateDirectPurchasesShopItemToDisplay(_productPricings);
			}
			if (m_selectedItem == null)
			{
				m_waitingForServerContainer.SetActive(true);
				m_waitForServerBox.RepositionNow = true;
				m_actionButton.isEnabled = false;
			}
			else
			{
				if (flag)
				{
					UpdateResourceCost();
				}
				else
				{
					UpdateDirectPurchaseItem();
				}
				m_availableEndTimeInDateTime = null;
				if (m_availableEndTimeValue != long.MaxValue)
				{
					DateTime dateTime = ServerTimeManager.ToUtcTime(m_availableEndTimeValue);
					DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
					double totalSeconds = (dateTime - utcNow).TotalSeconds;
					if (totalSeconds <= 0.0)
					{
						m_availableEndTimeValue = 0L;
					}
					if (m_availableEndTimeValue > 0)
					{
						m_availableEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_availableEndTimeValue);
					}
				}
				Update();
			}
			OnAlignmentBoxReposition();
		}

		private void UpdateDirectPurchaseItem()
		{
			m_availableEndTimeValue = long.MaxValue;
			m_prePriceText.gameObject.SetActive(false);
			m_oldPriceContainer.gameObject.SetActive(false);
			m_categoryDescription.gameObject.SetActive(false);
			m_newPrice.text = m_productPricing.productPrice;
			m_newPriceCurrencyIcon.gameObject.SetActive(false);
			m_alignmentBoxNewPrice.gameObject.SetActive(true);
			m_resourceContainer.gameObject.SetActive(true);
			m_alignmentBoxNewPrice.RepositionNow = true;
			m_resourceCostBox.RepositionNow = true;
		}

		private void UpdateResourceCost()
		{
			int num = 0;
			Resource resource = null;
			int _discountValue = 0;
			bool discountData = m_selectedItem.GetDiscountData(out m_availableEndTimeValue, out _discountValue);
			Resource firstResourceCost = m_selectedItem.GetFirstResourceCost();
			if (firstResourceCost != null && discountData)
			{
				num = (int)firstResourceCost.amount;
			}
			resource = m_selectedItem.GetFinalDiscountedResourceCost();
			m_categoryDescription.gameObject.SetActive(false);
			string preText = LocalizationManager.Instance.Get(7755);
			m_prePriceText.gameObject.SetActive(num > 0);
			UpdateCurrency(num, resource, preText);
			m_resourceCostBox.RepositionNow = true;
			m_resourceContainer.gameObject.SetActive(true);
			m_discountContainer.gameObject.SetActive(true);
		}

		private void UpdateDirectPurchasesShopItemToDisplay(ProductPricings _productPricings)
		{
			UIDirectPurchasesShopHelper directPurchasesShopHelper = UbiCore.Utils.Singleton<UiMain>.Instance.DirectPurchasesShopHelper;
			if (directPurchasesShopHelper.IsWaitingForNetworkCall)
			{
				return;
			}
			if (_productPricings == null)
			{
				directPurchasesShopHelper.UpdatePricingList(false);
				return;
			}
			for (int i = 0; i < m_shopBundleItems.Count; i++)
			{
				ShopBundle shopBundle = m_shopBundleItems[i];
				for (int j = 0; j < _productPricings.prices.Length; j++)
				{
					ProductPricing productPricing = _productPricings.prices[j];
					if (string.Compare(productPricing.productId, shopBundle.houstonId, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						m_selectedItem = shopBundle;
						m_productPricing = productPricing;
					}
				}
			}
		}

		private void OnDirectPurchasedVerifyed(string _productId)
		{
			if (m_selectedItem.houstonId == _productId)
			{
				OnClosePopup();
			}
		}

		private void OnPricingListUpdated(ProductPricings _productPricings)
		{
			if (_productPricings != null)
			{
				UpdatePriceInformation(_productPricings);
			}
			else
			{
				m_actionButton.TranslationKey = 1199;
			}
		}
	}
}
