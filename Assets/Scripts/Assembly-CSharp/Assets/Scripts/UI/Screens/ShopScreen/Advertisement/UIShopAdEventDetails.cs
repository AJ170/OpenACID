using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen.Advertisement
{
	public class UIShopAdEventDetails : MonoBehaviour, IShopAdBaseDetails
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_title;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private UIExtendedLabel m_subTitle;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_bottomText;

		[CheckAssigned]
		[SerializeField]
		private UIEventBannerTexture m_maskedAdTexture;

		private EBannerType m_layoutType;

		private ShopItemNode m_itemNodeData;

		private EventShopAdvertisement m_shopEventData;

		private bool m_isActive;

		public bool IsActive
		{
			get
			{
				return m_isActive;
			}
			set
			{
				m_isActive = value;
				base.gameObject.SetActive(m_isActive);
			}
		}

		public void UpdateDetails(EventShopAdvertisement _shopEventData, EBannerType _layoutType)
		{
			m_shopEventData = _shopEventData;
			m_layoutType = _layoutType;
			EventShopBannerTemplate template = m_shopEventData.Template;
			UIItemUtil.UpdateWidgetColor(template.TitleColorGameId, m_title);
			if (m_subTitle != null)
			{
				UIItemUtil.UpdateWidgetColor(template.SubTitleColorGameId, m_subTitle);
			}
			UIItemUtil.UpdateWidgetColor(template.SecondSubtitleColorGameId, m_bottomText);
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			m_itemNodeData = shopDetails.CategoryRootNode.GetShopItemById(template.ShopProductId);
			StringMapper variables = ((m_itemNodeData == null) ? null : m_itemNodeData.ShopBundle.GetShopBundleTitleStringMapper());
			m_title.SetLocalizationKey(template.GetTitleLocaKey(_layoutType), variables);
			if (m_subTitle != null)
			{
				m_subTitle.SetLocalizationKey(template.GetSubTitleLocaKey(_layoutType), variables);
			}
			m_bottomText.SetLocalizationKey(5878);
			ShopBundle bundle = ((m_itemNodeData == null) ? null : m_itemNodeData.ShopBundle);
			UpdateAdTexture(bundle);
		}

		public void UpdateDynamicData()
		{
		}

		private void OnAwake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private bool IsBigBanner()
		{
			return m_layoutType == EBannerType.Advertisement1X2 || m_layoutType == EBannerType.EventBanner1X2;
		}

		private void UpdateAdTexture(ShopBundle _bundle)
		{
			GameID previewImage = m_shopEventData.Template.IconOverrideGameId;
			if (!previewImage.Valid())
			{
				previewImage = _bundle.GetPreviewImageGameID();
			}
			m_maskedAdTexture.gameObject.SetActive(false);
			DBEntry def = Main.Instance.GameDB.GetItem<DBEntry>(previewImage);
			if (def == null)
			{
				return;
			}
			def.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success)
				{
					ItemDef itemDef = def as ItemDef;
					if (itemDef != null && itemDef.BannerUseAlphaMask)
					{
						m_maskedAdTexture.gameObject.SetActive(true);
						if (IsBigBanner())
						{
							m_maskedAdTexture.SetTextureOffset(Vector2.zero);
						}
						else
						{
							m_maskedAdTexture.SetTextureOffset(itemDef.AlphaMaskOffset);
						}
						UIItemUtil.UpdateUITexture(previewImage, m_maskedAdTexture);
					}
				}
			});
		}
	}
}
