using System;
using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Helpers.Shop;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.ShopBundlePreview
{
	public class UIBannerBundlePreview : UIBaseBundlePreview
	{
		private static StringId s_singleItemHeaderId = 7979;

		private static StringId s_categoryHeaderId = 7980;

		private static StringId s_categoryBodyId = 7981;

		[SerializeField]
		[CheckAssigned]
		private UITexture m_bannerBackground;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_bundleTexture;

		private long m_endTime;

		private EventShopBannerTemplate m_bannerTemplate;

		private ShopItemNode m_shopItemNode;

		private ShopBundle m_shopBundle;

		private int m_discountIntValue;

		public void OpenPreviewByEvent(EventShopAdvertisement _event)
		{
			m_endTime = _event.Event.endTimeUTC;
			m_bannerTemplate = _event.Template;
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			m_shopItemNode = shopDetails.CategoryRootNode.GetShopItemById(m_bannerTemplate.ShopProductId);
			m_shopBundle = m_bannerTemplate.GetShopProductShopBundle();
			OpenPreviewIfPossible();
		}

		public void OpenPreviewByEvent(EventShopBannerTemplate _bannerTemplate)
		{
			m_endTime = -1L;
			m_bannerTemplate = _bannerTemplate;
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			m_shopItemNode = shopDetails.CategoryRootNode.GetShopItemById(m_bannerTemplate.ShopProductId);
			m_shopBundle = m_bannerTemplate.GetShopProductShopBundle();
			OpenPreviewIfPossible();
		}

		public void OnBannerClicked()
		{
			if (m_shopItemNode == null)
			{
				return;
			}
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.ShopScreen)
			{
				if (!string.IsNullOrEmpty(m_bannerTemplate.ShopProductId))
				{
					UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
					shopDetails.OpenAndSetFocusOnItem(m_shopItemNode, true);
				}
				else
				{
					UIShopDetails.OpenAndSetFocusOnItem(m_bannerTemplate.ShopCategory);
				}
			}
			OnClosePopup();
		}

		private void OpenPreviewIfPossible()
		{
			if (m_shopBundle != null && !m_shopBundle.IsUnlocked())
			{
				OnClosePopup();
			}
			else
			{
				OpenPreview();
			}
		}

		protected override void OnUnityDisable()
		{
			base.OnUnityDisable();
			m_bannerTemplate = null;
			m_shopItemNode = null;
			m_shopBundle = null;
		}

		protected override void OnActionButton()
		{
			base.OnActionButton();
			if (m_bannerTemplate != null)
			{
				CampaignCatalog.Campaign _campaign = null;
				if (m_shopBundle != null && m_shopBundle.IsAnUnlockCampaignBundle(ref _campaign) && m_shopBundle.IsDirectPurchasesBundle())
				{
					OnClosePopup();
					Singleton<UIScreenManager>.Instance.SceneData.UnlockMissionBundlePreview.OpenPreviewByEvent(_campaign);
				}
				else if (!string.IsNullOrEmpty(m_bannerTemplate.ShopProductId))
				{
					UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
					buyShopItemHelper.OnGotoShopEnded = (Action)Delegate.Combine(buyShopItemHelper.OnGotoShopEnded, new Action(OnGotoShopEnded));
					buyShopItemHelper.BuyItem(m_shopBundle, m_shopBundle.GetFinalDiscountedResourceCost(), false);
				}
				else
				{
					UIShopDetails.OpenAndSetFocusOnItem(m_bannerTemplate.ShopCategory);
					OnClosePopup();
				}
			}
		}

		private void OnGotoShopEnded()
		{
			UIBuyShopItemHelper buyShopItemHelper = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper;
			if (buyShopItemHelper != null)
			{
				buyShopItemHelper.OnGotoShopEnded = (Action)Delegate.Remove(buyShopItemHelper.OnGotoShopEnded, new Action(OnGotoShopEnded));
				OnClosePopup();
			}
		}

		protected override void OnStartShopItemPurchased(bool _wasSentToInventory)
		{
			base.OnStartShopItemPurchased(_wasSentToInventory);
			if (m_shopBundle != null && !m_shopBundle.IsDirectPurchasesBundle())
			{
				base.OnStartShopItemPurchased(_wasSentToInventory);
				EventDelegate.Callback resourceMovedFinished = null;
				EventDelegate.Callback animationFinished = null;
				if (m_shopBundle.HasWalletPayoutResource())
				{
					resourceMovedFinished = base.OnAnimationFinished;
				}
				else
				{
					animationFinished = base.OnAnimationFinished;
				}
				UIRecivedItemManager recivedItemMgr = Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr;
				recivedItemMgr.StartReciveItemAnimation(m_shopBundle, m_bundleTexture.gameObject, null, animationFinished, true, resourceMovedFinished, _wasSentToInventory);
				OnClosePopup();
			}
		}

		protected override void UpdatePreview()
		{
			base.UpdatePreview();
			bool flag = m_shopBundle != null;
			StringMapper stringMapper = ((!flag) ? null : m_shopBundle.GetShopBundleTitleStringMapper());
			UIItemUtil.UpdateWidgetColor(m_bannerTemplate.CTATitleColorColorGameId, m_headline);
			UIItemUtil.UpdateWidgetColor(m_bannerTemplate.CTASubTitleColorGameId, m_description);
			UIItemUtil.UpdateWidgetColor(m_bannerTemplate.CTASubTitleColorGameId, m_categoryDescription);
			UIItemUtil.UpdateWidgetColor(m_bannerTemplate.CTABackgroundColorGameId, m_background);
			UIItemUtil.UpdateWidgetColor(m_bannerTemplate.CTATitleBackgroundColorGameId, m_headerBackground);
			UpdateHeaderAndActionButton();
			UpdateBannerTexture();
			if (m_bannerTemplate.IsUsingTintableBackground())
			{
				m_background.color = m_bannerBackground.color;
				m_headerBackground.color = m_bannerBackground.color;
			}
			if (flag && m_shopBundle.IsDirectPurchasesBundle())
			{
				UpdateDirectPurchaseItem(stringMapper);
			}
			else
			{
				UpdateResourceCost(flag, stringMapper);
			}
			if (m_availableEndTimeValue == long.MaxValue && m_discountIntValue == 0)
			{
				m_availableEndTimeValue = m_endTime;
			}
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
			else
			{
				m_availableEndTimeInDateTime = null;
			}
			Update();
			OnAlignmentBoxReposition();
		}

		private void UpdateHeaderAndActionButton()
		{
			bool flag = m_shopBundle != null;
			StringId key = s_singleItemHeaderId;
			int translationKey = 1782;
			if (m_shopBundle != null && m_shopBundle.HasCampaignUnlock())
			{
				key = 10495;
				if (m_shopBundle.IsDirectPurchasesBundle())
				{
					translationKey = 8376;
				}
			}
			else if (!flag)
			{
				key = s_categoryHeaderId;
				translationKey = 670;
			}
			m_headline.text = LocalizationManager.Instance.StripNGUIAndNewLineFormat(LocalizationManager.Instance.Get(key));
			m_actionButton.TranslationKey = translationKey;
			UIButtonLabelHandler component = m_actionButton.Button.GetComponent<UIButtonLabelHandler>();
			if (!flag && component != null)
			{
				component.SetIcon(Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus), "aUI_icon_shop");
			}
			m_actionButton.ShowIcon = !flag;
			m_actionButton.isEnabled = true;
		}

		private void UpdateDirectPurchaseItem(StringMapper _stringMapper)
		{
			m_availableEndTimeValue = long.MaxValue;
			m_discountIntValue = 0;
			m_prePriceText.gameObject.SetActive(false);
			m_oldPriceContainer.gameObject.SetActive(false);
			m_categoryDescription.gameObject.SetActive(false);
			m_discountContainer.gameObject.SetActive(false);
			ShopDirectPurchasItemNode shopDirectPurchasItemNode = (ShopDirectPurchasItemNode)m_shopItemNode;
			if (shopDirectPurchasItemNode != null)
			{
				m_description.gameObject.SetActive(true);
				m_description.text = LocalizationManager.Instance.StripNGUIAndNewLineFormat(m_shopBundle.LocaName(_stringMapper), _stringMapper);
				m_newPrice.text = shopDirectPurchasItemNode.ItemCurrencyText;
				m_newPriceCurrencyIcon.gameObject.SetActive(false);
				m_alignmentBoxNewPrice.gameObject.SetActive(true);
				m_resourceContainer.gameObject.SetActive(true);
				m_alignmentBoxNewPrice.RepositionNow = true;
				m_resourceCostBox.RepositionNow = true;
			}
			else
			{
				m_categoryDescription.text = LocalizationManager.Instance.StripNGUIAndNewLineFormat(m_shopBundle.LocaName(_stringMapper), _stringMapper);
				m_alignmentBoxNewPrice.gameObject.SetActive(false);
				m_description.gameObject.SetActive(false);
				m_resourceContainer.gameObject.SetActive(true);
				m_resourceCostBox.RepositionNow = true;
				m_categoryDescription.gameObject.SetActive(true);
			}
		}

		private void UpdateResourceCost(bool _isShopBundleItem, StringMapper _stringMapper)
		{
			m_prePriceText.gameObject.SetActive(true);
			m_prePriceText.LocalizationKey = 7755;
			int oldPrice = 0;
			Resource newPrice = null;
			string empty = string.Empty;
			if (_isShopBundleItem)
			{
				bool discountData = m_shopBundle.GetDiscountData(out m_availableEndTimeValue, out m_discountIntValue);
				Resource firstResourceCost = m_shopBundle.GetFirstResourceCost();
				if (firstResourceCost != null && discountData)
				{
					oldPrice = (int)firstResourceCost.amount;
				}
				newPrice = m_shopBundle.GetFinalDiscountedResourceCost();
				m_description.text = LocalizationManager.Instance.StripNGUIAndNewLineFormat(m_shopBundle.LocaName(_stringMapper), _stringMapper);
				m_description.gameObject.SetActive(true);
				m_categoryDescription.gameObject.SetActive(false);
			}
			else
			{
				if (m_bannerTemplate.ShopCategory.GetShopCategoryDiscountData(out m_availableEndTimeValue, out m_discountIntValue))
				{
					m_discountContainer.gameObject.SetActive(true);
					m_discountValue.LocalizedText = LocalizationManager.Instance.Get(1785, new StringMapper { 
					{
						"value",
						LocalizationManager.Instance.GetInteger(m_discountIntValue)
					} });
				}
				ShopCategoryDescription value = null;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ShopCategoryDescriptions.TryGetValue((int)m_bannerTemplate.ShopCategory, out value))
				{
					m_categoryDescription.text = LocalizationManager.Instance.StripNGUIAndNewLineFormat(s_categoryBodyId, new StringMapper { 
					{
						"Category_Name",
						LocalizationManager.Instance.Get(value.CategoryNameId)
					} });
				}
				m_categoryDescription.gameObject.SetActive(true);
				m_description.gameObject.SetActive(false);
			}
			UpdateCurrency(oldPrice, newPrice, empty);
			if (!_isShopBundleItem)
			{
				m_resourceCostBox.RepositionNow = true;
				m_resourceContainer.gameObject.SetActive(true);
			}
		}

		private void UpdateBannerTexture()
		{
			ShopBundle shopBundle = m_shopBundle;
			Color widgetColor = UIItemUtil.GetWidgetColor(m_bannerTemplate.GlowLayerColorGameId);
			if (m_bannerTemplate.CTABackgroundGameId.Valid())
			{
				UIItemUtil.UpdateUITexture(m_bannerTemplate.CTABackgroundGameId, m_bannerBackground);
				Color32 _qualityColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				if (m_bannerTemplate.IsUsingTintableBackground())
				{
					_qualityColor = widgetColor;
					if (m_shopBundle != null)
					{
						UIUtil.UpdateShopItemBannerQuality(m_shopBundle, ref _qualityColor);
					}
				}
				m_bannerBackground.color = _qualityColor;
			}
			bool flag = !m_bannerTemplate.IsHappyHourBanner() && (m_shopBundle == null || !m_shopBundle.HasCampaignUnlock());
			if (flag)
			{
				UIItemUtil.UpdateShopAdItem(m_bannerTemplate.IconOverrideGameId, shopBundle, widgetColor, true, m_bundleTexture, UIItemUtil.ItemUVCoordType.ENone, 0f);
			}
			m_bundleTexture.gameObject.SetActive(flag);
		}
	}
}
