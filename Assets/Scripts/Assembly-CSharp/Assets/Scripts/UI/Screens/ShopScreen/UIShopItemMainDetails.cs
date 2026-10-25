using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Shop;
using Assets.Scripts.UI.MVC.Models.Shop;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopItemMainDetails : MonoBehaviour
	{
		private static readonly Color32 s_normalPriceBarColor = new Color32(0, 0, 0, 65);

		private static readonly Color32 s_heroicPriceBarColor = new Color32(0, 0, 0, 128);

		[SerializeField]
		[CheckAssigned]
		private UISprite m_itemOverlaySprite;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private UISprite m_heroicOverlaySprite;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_itemTexture;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_bottomBar;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_bottomBarSprite;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_currencyContainer;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_currencySprite;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_currencyValue;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_numOfItemCurrencyIcon;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_numOfItems;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_numOfItemsBackground;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_numOfItemsBox;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_discountContainer;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_discountTimer;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_discountValue;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_discountLine;

		[CheckAssigned]
		[SerializeField]
		private UILabel m_oldCost;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_checkboxContainer;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_checkboxMarker;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_lockedLabel;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_lockedBackground;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_lockedContainer;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_lockIcon;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UIWidget m_shortInfoContainer;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UIExtendedLabel m_shortInfoText;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private GameObject m_happyHourBanner;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private UIExtendedLabel m_happyHourValueLabel;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private UILabel m_oldHappyHourCost;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UISprite m_happyHourBackground;

		[CheckAssigned(NullAllowed = true)]
		[SerializeField]
		private UISprite m_happyHourResourceIcon;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UISprite m_happyHourDiscountLine;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UIShopProductLabelController m_shopProductLabelController;

		private double m_discountTimeLeft;

		private double m_happyHourTimeLeft;

		private readonly UIShopProductLabelModel m_shopProductLabelModel = new UIShopProductLabelModel();

		public double DiscountTimeLeftLeft
		{
			get
			{
				return m_discountTimeLeft;
			}
			set
			{
				m_discountTimeLeft = value;
				m_discountTimer.SetLocalizationKey(2028, new StringMapper { 
				{
					"Shop_Discount_Time_Left",
					UIDuration.FromSeconds((long)m_discountTimeLeft)
				} });
				m_discountContainer.gameObject.SetActive(m_discountTimeLeft > 0.0);
				if (m_lockIcon != null)
				{
					m_lockIcon.bottomAnchor.absolute = ((!(m_discountTimeLeft > 0.0)) ? (-141) : (-57));
					m_lockIcon.topAnchor.absolute = ((!(m_discountTimeLeft > 0.0)) ? 63 : 147);
				}
			}
		}

		public double HappyHourTimeLeft
		{
			get
			{
				return m_happyHourTimeLeft;
			}
			set
			{
				m_happyHourTimeLeft = value;
				if (m_happyHourBanner != null)
				{
					m_happyHourBanner.SetActive(m_happyHourTimeLeft > 0.0);
				}
				UpdateShopProductLabelVisibility();
			}
		}

		public GameObject ItemTextureObject
		{
			get
			{
				return m_itemTexture.gameObject;
			}
		}

		private ShopItemNode ItemNodeData { get; set; }

		private ShopBundle ShopBundle { get; set; }

		private bool IsAlreadyOwned { get; set; }

		private bool IsUnlocked { get; set; }

		private bool IsHeroicShop { get; set; }

		public void UpdateData(ShopItemNode _itemNodeData)
		{
			if (_itemNodeData == null)
			{
				return;
			}
			bool updateTexture = ItemNodeData == null || ItemNodeData.ShopBundle.id != _itemNodeData.ShopBundle.id;
			ItemNodeData = _itemNodeData;
			ShopBundle = _itemNodeData.ShopBundle;
			IsHeroicShop = ShopBundle.IsHeroicBundle();
			IsUnlocked = ShopBundle.IsUnlocked();
			IsAlreadyOwned = ShopBundle.IsAlreadyOwned();
			UpdateShopProductLabel();
			if (m_lockedContainer != null)
			{
				m_lockedContainer.gameObject.SetActive(!IsUnlocked);
			}
			UpdateBasicLayout();
			if (!IsUnlocked)
			{
				string[] reqEntitlementGuids = ShopBundle.reqEntitlementGuids;
				if (reqEntitlementGuids.Length > 0)
				{
					EntitlementManager entitlementManager = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager;
					StringBuilder stringBuilder = new StringBuilder();
					LocalizationManager instance = LocalizationManager.Instance;
					for (int i = 0; i < reqEntitlementGuids.Length; i++)
					{
						if (!entitlementManager.HasEntitlement(reqEntitlementGuids[i]))
						{
							EntitlementDefinition entitlementByGUID = EntitlementExtensions.GetEntitlementByGUID(reqEntitlementGuids[i]);
							stringBuilder.AppendLine(instance.Get(6824, new StringMapper { 
							{
								"name",
								new UINGUIFormattedText(instance.Get(entitlementByGUID.NameLocaKey.Key))
							} }));
							break;
						}
					}
					m_lockedLabel.text = stringBuilder.ToString();
				}
			}
			UpdateItemData(updateTexture);
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			ShopBundle = null;
			ItemNodeData = null;
		}

		private void UpdateShopProductLabel()
		{
			if (!(m_shopProductLabelController == null) && ShopBundle != null)
			{
				bool flag = m_happyHourTimeLeft > 0.0;
				ShopProductLabelDescription value = null;
				Dictionary<int, ShopProductLabelDescription> shopProductLabelDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ShopProductLabelDescriptions;
				if (!flag && shopProductLabelDescriptions.TryGetValue((int)ShopBundle.shopProductLabel, out value))
				{
					m_shopProductLabelModel.LocaKey = value.LocaKey;
					m_shopProductLabelModel.Icon = value.Icon;
					m_shopProductLabelModel.Background = ((!IsHeroicShop) ? value.Background : value.HeroicBackground);
					m_shopProductLabelController.UpdateByModel(m_shopProductLabelModel);
				}
				UpdateShopProductLabelVisibility();
			}
		}

		private void UpdateShopProductLabelVisibility()
		{
			if (!(m_shopProductLabelController == null) && ShopBundle != null)
			{
				if (!(m_happyHourTimeLeft > 0.0) && ShopBundle.shopProductLabel != 0)
				{
					m_shopProductLabelController.Show();
				}
				else
				{
					m_shopProductLabelController.Hide();
				}
			}
		}

		private void UpdateBasicLayout()
		{
			Color32 color = UIDataMapper.UITextColors.DiscountCrossOutLineColorBright;
			Color32 color2 = UIDataMapper.UITextColors.DiscountOldPriceColorBright;
			Color32 color3 = ((!IsHeroicShop) ? s_normalPriceBarColor : s_heroicPriceBarColor);
			GameID previewImageGameID = ShopBundle.GetPreviewImageGameID();
			GameDB gameDB = Main.Instance.GameDB;
			ItemDef item = gameDB.GetItem<ItemDef>(previewImageGameID);
			if (item != null)
			{
				if (item.UseDarkOldPriceColor)
				{
					color = UIDataMapper.UITextColors.DiscountCrossOutLineColorDark;
					color2 = UIDataMapper.UITextColors.DiscountOldPriceColorDark;
				}
				if (item.UseDarkPriceBackground)
				{
					color3.a = 204;
				}
			}
			m_discountLine.color = color;
			m_oldCost.color = color2;
			m_bottomBarSprite.color = color3;
		}

		private void UpdateItemData(bool _updateTexture)
		{
			Color32 _qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
			Color32 _textQualityColor = UIDataMapper.UITextColors.ShopMundaneOverideColor;
			UIUtil.UpdateShopItemQuality(ShopBundle, ref _qualityColor, ref _textQualityColor, m_itemOverlaySprite);
			if (m_heroicOverlaySprite != null)
			{
				m_heroicOverlaySprite.gameObject.SetActive(IsHeroicShop && !ShopBundle.IsSoloOutFit() && IsUnlocked);
			}
			if (!IsUnlocked)
			{
				m_itemOverlaySprite.color = Color.white;
			}
			if (_updateTexture)
			{
				m_itemTexture.IsResizeWidgetToOrgSize = true;
				m_itemTexture.transform.localPosition = Vector3.zero;
				Main.Instance.StartCoroutine(UpdateTexture(_qualityColor));
			}
			else
			{
				m_itemTexture.Desaturation = ((!IsUnlocked) ? 1 : 0);
			}
			if (m_shortInfoContainer != null)
			{
				bool flag = ShopBundle.shortDescLangKey != null && StringId.ValidStringId(ShopBundle.shortDescLangKey.neutral);
				m_shortInfoContainer.gameObject.SetActive(flag);
				if (flag)
				{
					m_shortInfoText.SetLocalizationKey(ShopBundle.shortDescLangKey.neutral);
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = ShopBundle.HasHeroicRankPayout();
			Resource firstResourcePayout = ShopBundle.GetFirstResourcePayout();
			if (!flag4 && ShopBundle.bundleValue == 0 && firstResourcePayout != null && firstResourcePayout.amount > 1)
			{
				m_numOfItems.SetLocalizationKey(1784, new StringMapper { { "Items_Current", firstResourcePayout.amount } });
				flag2 = true;
				CurrencyDescription value = null;
				flag3 = UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)firstResourcePayout.type, out value);
				if (flag3)
				{
					m_numOfItemCurrencyIcon.spriteName = value.SmallIconName;
				}
			}
			else if (ShopBundle.payout.lootTemplates.GetFirstModuleTemplate() != null && ShopBundle.category == EShopCategory.Shopcat_Modules)
			{
				flag2 = true;
				m_numOfItems.UseLocalizedText = false;
				m_numOfItems.text = new UIShopModuleQuality(ShopBundle.payout.lootTemplates.GetFirstModuleTemplate(), true).ToString();
			}
			else if (!flag4 && ShopBundle.bundleValue == 0)
			{
				int num = 0;
				for (int i = 0; i < ShopBundle.payout.lootTemplates.Length; i++)
				{
					LootTemplate lootTemplate = ShopBundle.payout.lootTemplates[i];
					if (lootTemplate.mysteryBoxTemplate != null)
					{
						num += (int)lootTemplate.mysteryBoxTemplate.maxAmount;
					}
					else if (lootTemplate.blueprintTemplate != null)
					{
						num += (int)lootTemplate.blueprintTemplate.maxAmount;
					}
					else if (lootTemplate.customTemplate != null)
					{
						num += (int)lootTemplate.customTemplate.maxAmount;
					}
				}
				if (num > 1)
				{
					flag2 = true;
					m_numOfItems.SetLocalizationKey(1784, new StringMapper { { "Items_Current", num } });
				}
			}
			m_numOfItemCurrencyIcon.gameObject.SetActive(flag3);
			m_numOfItemsBackground.gameObject.SetActive(flag2);
			m_numOfItemsBox.gameObject.SetActive(flag2);
			m_numOfItemsBox.RepositionNow = flag2;
			if (DiscountTimeLeftLeft > 0.0 && ShopBundle.HasResourceCost())
			{
				Resource firstResourceCost = ShopBundle.GetFirstResourceCost();
				m_oldCost.text = LocalizationManager.Instance.GetInteger(firstResourceCost.amount);
				int displayDiscountvalue = ShopBundle.GetDisplayDiscountvalue();
				m_discountValue.LocalizedText = LocalizationManager.Instance.Get(1785, new StringMapper { 
				{
					"value",
					LocalizationManager.Instance.GetInteger(displayDiscountvalue)
				} });
				Vector3[] localCorners = m_oldCost.localCorners;
				Vector2 vector = localCorners[0] - localCorners[2];
				float z = Mathf.Atan2(vector.y, vector.x) * 180f / (float)Math.PI;
				m_discountLine.width = (int)Mathf.Sqrt(m_oldCost.width * m_oldCost.width + m_oldCost.height * m_oldCost.height);
				m_discountLine.transform.localRotation = Quaternion.Euler(0f, 0f, z);
				m_discountLine.transform.localPosition = new Vector3(-m_oldCost.width / 2, 0f);
			}
			if (HappyHourTimeLeft > 0.0 && !ShopBundle.HasResourceCost())
			{
				EResourceType eResourceType = ((firstResourcePayout == null) ? EResourceType.SoftCurrency : firstResourcePayout.type);
				long resourceAmountByType = ShopBundle.payout.lootTemplates.GetResourceAmountByType(eResourceType);
				m_oldHappyHourCost.text = LocalizationManager.Instance.GetInteger(resourceAmountByType);
				m_happyHourValueLabel.text = LocalizationManager.Instance.GetInteger(ShopBundle.GetFinalHappyHourPayout());
				CurrencyDescription value2 = null;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)eResourceType, out value2))
				{
					m_happyHourResourceIcon.spriteName = value2.SmallIconName;
				}
				m_happyHourBackground.spriteName = ((!IsHeroicShop) ? "aUI_fra_label_happy_hour" : "aUI_fra_label_happy_hour_heroic");
				Vector3[] localCorners2 = m_oldHappyHourCost.localCorners;
				Vector2 vector2 = localCorners2[0] - localCorners2[2];
				float z2 = Mathf.Atan2(vector2.y, vector2.x) * 180f / (float)Math.PI;
				m_happyHourDiscountLine.width = (int)Mathf.Sqrt(m_oldHappyHourCost.width * m_oldHappyHourCost.width + m_oldHappyHourCost.height * m_oldHappyHourCost.height);
				m_happyHourDiscountLine.transform.localRotation = Quaternion.Euler(0f, 0f, z2);
				m_happyHourDiscountLine.transform.localPosition = new Vector3(-m_oldHappyHourCost.width / 2, 0f);
			}
			UpdateCurrency();
			m_checkboxContainer.gameObject.SetActive(IsAlreadyOwned);
			bool flag5 = IsAlreadyOwned || m_currencyContainer.gameObject.activeSelf;
			m_lockedBackground.bottomAnchor.absolute = ((!flag5) ? (-m_bottomBar.height) : 0);
			m_bottomBar.gameObject.SetActive(flag5);
		}

		private IEnumerator UpdateTexture(Color32 _qualityColor)
		{
			yield return new WaitForEndOfFrame();
			UIItemUtil.UpdateShopItem(ShopBundle, _qualityColor, true, m_itemTexture, UIItemUtil.ItemUVCoordType.EShopItem, (!IsUnlocked) ? 1 : 0);
		}

		private void UpdateCurrency()
		{
			bool active = false;
			if (!IsAlreadyOwned)
			{
				if (ItemNodeData.DataType == EShopDataType.DirectPurchasItem)
				{
					active = true;
					ShopDirectPurchasItemNode shopDirectPurchasItemNode = ItemNodeData as ShopDirectPurchasItemNode;
					if (shopDirectPurchasItemNode != null)
					{
						m_currencySprite.gameObject.SetActive(false);
						m_currencyValue.ClearFormatFlags();
						m_currencyValue.text = shopDirectPurchasItemNode.ItemCurrencyText;
					}
				}
				else
				{
					active = true;
					Resource finalDiscountedResourceCost = ShopBundle.GetFinalDiscountedResourceCost();
					UIUtil.SetupCurrencyWidgets(finalDiscountedResourceCost, m_currencyValue, m_currencySprite);
				}
				m_currencyValue.leftAnchor.Set(m_currencyContainer.transform, 0f, 0f);
				m_currencyValue.rightAnchor.Set(m_currencyContainer.transform, 1f, (!m_currencySprite.gameObject.activeSelf) ? (-10) : (-75));
			}
			m_currencyContainer.gameObject.SetActive(active);
		}
	}
}
