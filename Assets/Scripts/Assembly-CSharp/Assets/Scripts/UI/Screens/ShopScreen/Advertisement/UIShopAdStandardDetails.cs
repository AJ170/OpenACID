using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen.Advertisement
{
	public class UIShopAdStandardDetails : MonoBehaviour, IShopAdBaseDetails
	{
		private static readonly Vector2 s_advertisement1X1OriginalSize = new Vector2(308f, 308f);

		private static readonly Vector2 s_advertisement1X1ScaledSize = new Vector2(530f, 530f);

		private static readonly Vector3 s_advertisement1X1OriginalPos = new Vector3(-99f, -17f);

		private static readonly Vector3 s_advertisement1X1ScaledPos = Vector3.zero;

		private static readonly Vector2 s_advertisement1X2OriginalSize = new Vector2(512f, 512f);

		private static readonly Vector2 s_advertisement1X2ScaledSize = new Vector2(768f, 768f);

		private static readonly Vector3 s_advertisement1X2OriginalPos = Vector3.zero;

		private static readonly Vector3 s_advertisement1X2ScaledPos = new Vector3(-40f, -20f);

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_title;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_subTitle;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_adTexture;

		[CheckAssigned]
		[SerializeField]
		private UIEventBannerTexture m_maskedAdTexture;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_discountContainer;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_discountTopText;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_discountTimer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_discountValue;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_oldCostLine;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_oldCost;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_oldCostContainer;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_costIcon;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_newCost;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_newCostLabelIconBox;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_mainCostAlignmentBox;

		private bool m_isActive;

		private bool m_eventAdded;

		private EBannerType m_layoutType;

		private ShopBundle m_shopBundle;

		private EventShopAdvertisement m_shopEventData;

		private long m_discountEndTime;

		private int m_discountIntValue;

		private DateTime m_discountEndTimeInDateTime;

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
			UIItemUtil.UpdateWidgetColor(template.SubTitleColorGameId, m_subTitle);
			UIItemUtil.UpdateWidgetColor(template.DiscountColorGameId, m_discountValue);
			UIItemUtil.UpdateWidgetColor(template.RemainingColorGameId, m_discountTopText);
			UIItemUtil.UpdateWidgetColor(template.RemainingTimeLabelColorGameId, m_discountTimer);
			UIItemUtil.UpdateWidgetColor(template.OldPriceColorGameId, m_oldCost);
			UIItemUtil.UpdateWidgetColor(template.NewPriceColorGameId, m_newCost);
			m_shopBundle = template.GetShopProductShopBundle();
			StringMapper variables = ((m_shopBundle == null) ? null : m_shopBundle.GetShopBundleTitleStringMapper());
			m_title.SetLocalizationKey(template.GetTitleLocaKey(m_layoutType), variables);
			m_subTitle.SetLocalizationKey(template.GetSubTitleLocaKey(m_layoutType), variables);
			bool flag = false;
			bool flag2 = m_shopBundle != null;
			ShopBundle shopBundle = ((!flag2) ? null : m_shopBundle);
			Color widgetColor = UIItemUtil.GetWidgetColor(template.GlowLayerColorGameId);
			UpdateAdTexture(shopBundle, widgetColor);
			if (flag2)
			{
				bool discountData = m_shopBundle.GetDiscountData(out m_discountEndTime, out m_discountIntValue);
				Resource firstResourceCost = shopBundle.GetFirstResourceCost();
				flag = firstResourceCost == null;
				if (!flag)
				{
					if (discountData)
					{
						m_discountEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_discountEndTime);
						m_oldCost.LocalizedText = LocalizationManager.Instance.GetInteger(firstResourceCost.amount);
						int displayDiscountvalue = m_shopBundle.GetDisplayDiscountvalue();
						m_discountValue.LocalizedText = LocalizationManager.Instance.Get(1785, new StringMapper { 
						{
							"value",
							LocalizationManager.Instance.GetInteger(displayDiscountvalue)
						} });
						UpdateLine();
					}
					m_oldCostContainer.gameObject.SetActive(discountData);
					m_discountValue.gameObject.SetActive(discountData);
					string spriteName = string.Empty;
					CurrencyDescription value = null;
					if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)firstResourceCost.type, out value))
					{
						spriteName = value.SmallIconName;
					}
					Resource finalDiscountedResourceCost = m_shopBundle.GetFinalDiscountedResourceCost();
					m_newCost.LocalizedText = LocalizationManager.Instance.GetInteger(finalDiscountedResourceCost.amount);
					m_costIcon.spriteName = spriteName;
					m_costIcon.gameObject.SetActive(true);
				}
			}
			else
			{
				bool shopCategoryDiscountData = template.ShopCategory.GetShopCategoryDiscountData(out m_discountEndTime, out m_discountIntValue);
				m_oldCostContainer.gameObject.SetActive(false);
				m_discountValue.gameObject.SetActive(false);
				m_costIcon.gameObject.SetActive(false);
				m_newCost.text = string.Empty;
				if (shopCategoryDiscountData)
				{
					m_discountEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_discountEndTime);
					m_newCost.LocalizedText = LocalizationManager.Instance.Get(1785, new StringMapper { 
					{
						"value",
						LocalizationManager.Instance.GetInteger(m_discountIntValue)
					} });
				}
			}
			bool flag3 = true;
			if (flag || (m_discountEndTime == long.MaxValue && m_discountIntValue == 0))
			{
				m_oldCostContainer.gameObject.SetActive(false);
				m_discountValue.gameObject.SetActive(false);
				m_discountEndTime = m_shopEventData.Event.endTimeUTC;
				m_discountEndTimeInDateTime = ServerTimeManager.ToUtcTime(m_discountEndTime);
				flag3 = !flag && flag2;
			}
			m_newCostLabelIconBox.RepositionNow = true;
			m_mainCostAlignmentBox.GetComponent<UIWidget>().alpha = (flag3 ? 1 : 0);
			m_mainCostAlignmentBox.RepositionNow = flag3;
		}

		public void UpdateDynamicData()
		{
			if (HasDiscountDataChanged())
			{
				UpdateDetails(m_shopEventData, m_layoutType);
			}
			double num = 0.0;
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			num = (m_discountEndTimeInDateTime - utcNow).TotalSeconds;
			if (num > 0.0)
			{
				m_discountTimer.SetLocalizationKey(2028, new StringMapper { 
				{
					"Shop_Discount_Time_Left",
					UIDuration.FromSeconds((long)num)
				} });
			}
			m_discountContainer.gameObject.SetActive(num > 0.0);
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			if (m_eventAdded && m_adTexture != null)
			{
				m_adTexture.MaterialUpdated -= OnTextureMaterialUpdated;
			}
			m_eventAdded = false;
		}

		private bool IsBigBanner()
		{
			return m_layoutType == EBannerType.Advertisement1X2 || m_layoutType == EBannerType.EventBanner1X2;
		}

		private void UpdateAdTexture(ShopBundle _bundle, Color _qualityColor)
		{
			GameID previewImage = m_shopEventData.Template.IconOverrideGameId;
			if (!previewImage.Valid())
			{
				previewImage = _bundle.GetPreviewImageGameID();
			}
			if (!m_eventAdded)
			{
				m_eventAdded = true;
				m_adTexture.MaterialUpdated += OnTextureMaterialUpdated;
			}
			m_maskedAdTexture.gameObject.SetActive(false);
			m_adTexture.gameObject.SetActive(false);
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
					else
					{
						m_adTexture.gameObject.SetActive(true);
						UIItemUtil.ItemUVCoordType uvCoordType = ((!IsBigBanner()) ? UIItemUtil.ItemUVCoordType.EBanner1x1 : UIItemUtil.ItemUVCoordType.ENone);
						UIItemUtil.UpdateShopAdItem(m_shopEventData.Template.IconOverrideGameId, _bundle, _qualityColor, true, m_adTexture, uvCoordType, 0f);
					}
				}
			});
		}

		[ContextMenu("UpdateLine")]
		private void UpdateLine()
		{
			float z = Mathf.Atan2(46f, m_oldCost.width) * 180f / (float)Math.PI;
			m_oldCostLine.width = (int)Mathf.Sqrt(m_oldCost.width * m_oldCost.width + 2116);
			m_oldCostLine.transform.localRotation = Quaternion.Euler(0f, 0f, z);
			m_oldCostLine.transform.localPosition = new Vector3(-m_oldCost.width / 2, 0f);
		}

		private bool HasDiscountDataChanged()
		{
			bool flag = false;
			long num = m_discountEndTime;
			int discountIntValue = m_discountIntValue;
			flag = ((m_shopBundle == null) ? m_shopEventData.Template.ShopCategory.GetShopCategoryDiscountData(out m_discountEndTime, out m_discountIntValue) : m_shopBundle.GetDiscountData(out m_discountEndTime, out m_discountIntValue));
			bool result = (flag && num != m_discountEndTime) || Math.Abs(discountIntValue - m_discountIntValue) > 0;
			if (m_discountEndTime == long.MaxValue && !flag)
			{
				num = m_shopEventData.Event.endTimeUTC;
			}
			if (!flag)
			{
				result = num != m_shopEventData.Event.endTimeUTC;
			}
			return result;
		}

		private void OnTextureMaterialUpdated()
		{
			if (IsBigBanner())
			{
				if (m_shopBundle != null && !m_shopEventData.Template.IconOverrideGameId.Valid() && m_shopBundle.IsSoloOutFit())
				{
					UIPortraitTexture adTexture = m_adTexture;
					Vector2 vector = s_advertisement1X2ScaledSize;
					adTexture.width = (int)vector.x;
					UIPortraitTexture adTexture2 = m_adTexture;
					Vector2 vector2 = s_advertisement1X2ScaledSize;
					adTexture2.height = (int)vector2.y;
					m_adTexture.transform.localPosition = s_advertisement1X2ScaledPos;
				}
				else
				{
					UIPortraitTexture adTexture3 = m_adTexture;
					Vector2 vector3 = s_advertisement1X2OriginalSize;
					adTexture3.width = (int)vector3.x;
					UIPortraitTexture adTexture4 = m_adTexture;
					Vector2 vector4 = s_advertisement1X2OriginalSize;
					adTexture4.height = (int)vector4.y;
					m_adTexture.transform.localPosition = s_advertisement1X2OriginalPos;
				}
			}
			else if (m_adTexture.HasCustomUVCoords)
			{
				UIPortraitTexture adTexture5 = m_adTexture;
				Vector2 vector5 = s_advertisement1X1ScaledSize;
				adTexture5.width = (int)vector5.x;
				UIPortraitTexture adTexture6 = m_adTexture;
				Vector2 vector6 = s_advertisement1X1ScaledSize;
				adTexture6.height = (int)vector6.y;
				m_adTexture.transform.localPosition = s_advertisement1X1ScaledPos;
			}
			else
			{
				UIPortraitTexture adTexture7 = m_adTexture;
				Vector2 vector7 = s_advertisement1X1OriginalSize;
				adTexture7.width = (int)vector7.x;
				UIPortraitTexture adTexture8 = m_adTexture;
				Vector2 vector8 = s_advertisement1X1OriginalSize;
				adTexture8.height = (int)vector8.y;
				m_adTexture.transform.localPosition = s_advertisement1X1OriginalPos;
			}
		}
	}
}
