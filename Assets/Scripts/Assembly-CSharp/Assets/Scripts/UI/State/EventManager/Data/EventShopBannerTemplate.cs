using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventShopBannerTemplate
	{
		private const string c_titleLocaParamName_1X1 = "titleLineId_1x1";

		private const string c_titleLocaParamName_1X2 = "titleLineId_1x2";

		private const string c_subTitleLocaParamName_1X1 = "subTitleLineId_1x1";

		private const string c_subTitleLocaParamName_1X2 = "subTitleLineId_1x2";

		private const string c_backgroundGuidParamName_1X1 = "background_1x1";

		private const string c_bigBannerParamName = "bigBanner";

		private const string c_iconOverrideGuidParamName = "iconOverride";

		private const string c_shopProductParamName = "shopProduct";

		private const string c_shopCategoryParamName = "shopCategory";

		private const string c_advertisementBannerTypeParamName = "advertisementBannerType";

		private const string c_showInShopParamName = "showInShop";

		private const string c_showInNewsfeedParamName = "showInNewsfeed";

		private const string c_titleColorParamName = "titleColor";

		private const string c_subTitleColorParamName = "subtitleColor";

		private const string c_secondSubTitleColorParamName = "secondSubtitleColor";

		private const string c_discountColorParamName = "discountColor";

		private const string c_remainingTimeLabelColorParamName = "remainingTimeLabelColor";

		private const string c_remainingColorParamName = "remainingColor";

		private const string c_oldPriceColorParamName = "oldPriceColor";

		private const string c_newPriceColorParamName = "newPriceColor";

		private const string c_glowLayerColorParamName = "glowLayerColor";

		private const string c_ctaTitleColorParamName = "cta_title_color";

		private const string c_ctaSubtitleColorParamName = "cta_subtitle_color";

		private const string c_ctaTitleBackgroundColorParamName = "cta_title_background_color";

		private const string c_ctaBackgroundColorParamName = "cta_background_color";

		private const string c_ctaHighlightWithShopCta = "highlightWithShopCta";

		private StringId m_title1X1LocaKey;

		private StringId m_title1X2LocaKey;

		private StringId m_subTitle1X1LocaKey;

		private StringId m_subTitle1X2LocaKey;

		private GameID m_background1X1GameId;

		private GameID m_background1X2GameId;

		public GameID IconOverrideGameId { get; private set; }

		public bool IsBigBannerPrefered { get; private set; }

		public string ShopProductId { get; private set; }

		public EShopCategory ShopCategory { get; private set; }

		public EAdvertisementBannerType AdvertisementBannerLayoutType { get; private set; }

		public bool ShowInShop { get; private set; }

		public bool ShowInNewsFeed { get; private set; }

		public bool HighlightWithShopCta { get; set; }

		public GameID TitleColorGameId { get; private set; }

		public GameID SubTitleColorGameId { get; private set; }

		public GameID SecondSubtitleColorGameId { get; private set; }

		public GameID DiscountColorGameId { get; private set; }

		public GameID RemainingTimeLabelColorGameId { get; private set; }

		public GameID RemainingColorGameId { get; private set; }

		public GameID OldPriceColorGameId { get; private set; }

		public GameID NewPriceColorGameId { get; private set; }

		public GameID GlowLayerColorGameId { get; private set; }

		public GameID CTABackgroundGameId { get; private set; }

		public GameID CTATitleColorColorGameId { get; private set; }

		public GameID CTASubTitleColorGameId { get; private set; }

		public GameID CTATitleBackgroundColorGameId { get; private set; }

		public GameID CTABackgroundColorGameId { get; private set; }

		public StringId GetTitleLocaKey(EBannerType _type)
		{
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
			case EBannerType.EventBanner1X2:
				return m_title1X2LocaKey;
			default:
				return m_title1X1LocaKey;
			}
		}

		public StringId GetSubTitleLocaKey(EBannerType _type)
		{
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
			case EBannerType.EventBanner1X2:
				return m_subTitle1X2LocaKey;
			default:
				return m_subTitle1X1LocaKey;
			}
		}

		public GameID GetBackgroundGameId(EBannerType _type)
		{
			switch (_type)
			{
			case EBannerType.Advertisement1X2:
			case EBannerType.EventBanner1X2:
				return m_background1X2GameId;
			default:
				return m_background1X1GameId;
			}
		}

		public bool IsHappyHourBannerAndAllowedToBeShown()
		{
			if (IsHappyHourBanner())
			{
				return ShopBundleExtension.IsPayoutIncreaseActForCategory(ShopCategory);
			}
			return true;
		}

		public bool IsHappyHourBanner()
		{
			return !HasValidShopProductId() && (AdvertisementBannerLayoutType == EAdvertisementBannerType.HappyHours || ShopCategory == EShopCategory.Shopcat_HcPackage);
		}

		public bool IsAffectedItemShownInShop()
		{
			if (HasValidShopProductId())
			{
				ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
				ShopBundle shopBundle = shopTable.bundles.Find((ShopBundle _entry) => _entry.id == ShopProductId);
				if (shopBundle != null)
				{
					return shopBundle.HasCampaignUnlock() || shopBundle.IsShownInShop();
				}
				return false;
			}
			return ShopCategory.IsCategoryShownInShop();
		}

		public ShopBundle GetShopProductShopBundle()
		{
			if (HasValidShopProductId())
			{
				ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
				return shopTable.bundles.Find((ShopBundle _entry) => _entry != null && _entry.id == ShopProductId);
			}
			return null;
		}

		public bool IsUsingTintableBackground()
		{
			return UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.TintableBackground1X1GameId == m_background1X1GameId;
		}

		public bool IsShopItemUnlocked()
		{
			ShopBundle shopProductShopBundle = GetShopProductShopBundle();
			if (shopProductShopBundle == null || shopProductShopBundle.IsUnlocked())
			{
				return true;
			}
			return false;
		}

		public void ProcessEvent(Event.Property[] _properties)
		{
			ResetData();
			for (int i = 0; i < _properties.Length; i++)
			{
				ParsePropertieDate(_properties[i]);
			}
			ValidateData();
		}

		public void ProcessEvent(EventTemplate.PropertyDefinition[] _properties)
		{
			ResetData();
			foreach (EventTemplate.PropertyDefinition propertyDefinition in _properties)
			{
				ParsePropertieDate(new Event.Property
				{
					name = propertyDefinition.name,
					value = propertyDefinition.defaultValue
				});
			}
			ValidateData();
		}

		public bool HasValidShopProductId()
		{
			return IsIdValid(ShopProductId);
		}

		private bool IsIdValid(string _guidID)
		{
			return !string.IsNullOrEmpty(_guidID) && Guid.Empty.ToString() != _guidID;
		}

		private void ResetData()
		{
			m_title1X1LocaKey = StringId.Invalid;
			m_title1X2LocaKey = StringId.Invalid;
			m_subTitle1X1LocaKey = StringId.Invalid;
			m_subTitle1X2LocaKey = StringId.Invalid;
			m_background1X1GameId = GameID.Invalid;
			m_background1X2GameId = GameID.Invalid;
			CTABackgroundGameId = GameID.Invalid;
			IsBigBannerPrefered = false;
			IconOverrideGameId = GameID.Invalid;
			ShopProductId = string.Empty;
			ShopCategory = EShopCategory.Shopcat_HcPackage;
			AdvertisementBannerLayoutType = EAdvertisementBannerType.Standard;
			ShowInShop = false;
			ShowInNewsFeed = false;
			HighlightWithShopCta = false;
			TitleColorGameId = GameID.Invalid;
			SubTitleColorGameId = GameID.Invalid;
			SecondSubtitleColorGameId = GameID.Invalid;
			DiscountColorGameId = GameID.Invalid;
			RemainingTimeLabelColorGameId = GameID.Invalid;
			RemainingColorGameId = GameID.Invalid;
			OldPriceColorGameId = GameID.Invalid;
			NewPriceColorGameId = GameID.Invalid;
			GlowLayerColorGameId = GameID.Invalid;
			CTATitleColorColorGameId = GameID.Invalid;
			CTASubTitleColorGameId = GameID.Invalid;
			CTATitleBackgroundColorGameId = GameID.Invalid;
			CTABackgroundColorGameId = GameID.Invalid;
		}

		private void ParsePropertieDate(Event.Property _propertie)
		{
			if (string.Compare(_propertie.name, "titleLineId_1x1", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				m_title1X1LocaKey = _propertie.GetIntValue();
			}
			if (string.Compare(_propertie.name, "titleLineId_1x2", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				m_title1X2LocaKey = _propertie.GetIntValue();
			}
			else if (string.Compare(_propertie.name, "subTitleLineId_1x1", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				m_subTitle1X1LocaKey = _propertie.GetIntValue();
			}
			else if (string.Compare(_propertie.name, "subTitleLineId_1x2", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				m_subTitle1X2LocaKey = _propertie.GetIntValue();
			}
			else if (string.Compare(_propertie.name, "background_1x1", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				m_background1X1GameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "bigBanner", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				IsBigBannerPrefered = _propertie.GetBoolValue();
			}
			else if (string.Compare(_propertie.name, "iconOverride", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				IconOverrideGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "shopProduct", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				ShopProductId = _propertie.GetStringValue();
			}
			else if (string.Compare(_propertie.name, "shopCategory", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				ShopCategory = _propertie.ParseEnum<EShopCategory>();
			}
			else if (string.Compare(_propertie.name, "advertisementBannerType", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				AdvertisementBannerLayoutType = _propertie.ParseEnum<EAdvertisementBannerType>();
			}
			else if (string.Compare(_propertie.name, "showInShop", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				ShowInShop = _propertie.GetBoolValue();
			}
			else if (string.Compare(_propertie.name, "showInNewsfeed", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				ShowInNewsFeed = _propertie.GetBoolValue();
			}
			else if (string.Compare(_propertie.name, "titleColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				TitleColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "subtitleColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				SubTitleColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "secondSubtitleColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				SecondSubtitleColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "discountColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				DiscountColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "remainingTimeLabelColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				RemainingTimeLabelColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "remainingColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				RemainingColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "oldPriceColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				OldPriceColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "newPriceColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				NewPriceColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "glowLayerColor", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				GlowLayerColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "cta_background_color", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				CTABackgroundColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "cta_title_background_color", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				CTATitleBackgroundColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "cta_subtitle_color", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				CTASubTitleColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "cta_title_color", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				CTATitleColorColorGameId = _propertie.GetGameIdValue();
			}
			else if (string.Compare(_propertie.name, "highlightWithShopCta", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				HighlightWithShopCta = _propertie.GetBoolValue();
			}
		}

		private void ValidateData()
		{
			ValidateItem();
			ValidateBackground();
			ValidateLinkedBackgroundsAndColors();
		}

		private void ValidateItem()
		{
			ShopCategoryDescription value;
			if (!IconOverrideGameId.Valid() && !HasValidShopProductId() && UbiCore.Utils.Singleton<UIDataMapper>.Instance.ShopCategoryDescriptions.TryGetValue((int)ShopCategory, out value))
			{
				IconOverrideGameId = value.ItemGameId;
			}
		}

		private void ValidateBackground()
		{
			GameDB gameDB = Main.Instance.GameDB;
			if (m_background1X1GameId.Valid())
			{
				return;
			}
			GameID gameID = GameID.Invalid;
			if (!IconOverrideGameId.Valid())
			{
				ShopBundle shopProductShopBundle = GetShopProductShopBundle();
				if (shopProductShopBundle != null)
				{
					gameID = shopProductShopBundle.GetPreviewImageGameID();
				}
			}
			if (gameID.Valid())
			{
				ItemDef item = gameDB.GetItem<ItemDef>(gameID);
				if (item != null)
				{
					m_background1X1GameId = item.BannerBackground1x1;
				}
			}
		}

		private bool HasCampaignUnlock()
		{
			ShopBundle shopProductShopBundle = GetShopProductShopBundle();
			if (shopProductShopBundle != null)
			{
				return shopProductShopBundle.HasCampaignUnlock();
			}
			return false;
		}

		private void ValidateLinkedBackgroundsAndColors()
		{
			string _backgroundPrefix = string.Empty;
			string _backgroundSufix = string.Empty;
			GetBackgroundAndSufixFromBackground(m_background1X1GameId, ref _backgroundPrefix, ref _backgroundSufix);
			if (!string.IsNullOrEmpty(_backgroundPrefix) && !string.IsNullOrEmpty(_backgroundSufix))
			{
				Dictionary<string, GameID> advertiesmentBackgroundItems = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.AdvertiesmentBackgroundItems;
				string key = _backgroundPrefix + "1x2_" + _backgroundSufix;
				GameID value = GameID.Invalid;
				if (advertiesmentBackgroundItems.TryGetValue(key, out value))
				{
					m_background1X2GameId = value;
				}
				string key2 = _backgroundPrefix + "2x1_" + _backgroundSufix;
				if (HasCampaignUnlock())
				{
					key2 = _backgroundPrefix + "2x1_forli";
				}
				GameID value2 = GameID.Invalid;
				if (advertiesmentBackgroundItems.TryGetValue(key2, out value2))
				{
					CTABackgroundGameId = value2;
				}
				Dictionary<string, Dictionary<string, GameID>> advertiesmentBackgroundColorTables = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.AdvertiesmentBackgroundColorTables;
				string key3 = _backgroundPrefix + _backgroundSufix + "_colors";
				Dictionary<string, GameID> value3 = null;
				if (advertiesmentBackgroundColorTables.TryGetValue(key3, out value3))
				{
					TitleColorGameId = UpdateColor(TitleColorGameId, "title_color", value3);
					SubTitleColorGameId = UpdateColor(SubTitleColorGameId, "subtitle_color", value3);
					SecondSubtitleColorGameId = UpdateColor(SecondSubtitleColorGameId, "second_subtitle_color", value3);
					DiscountColorGameId = UpdateColor(DiscountColorGameId, "discount_color", value3);
					RemainingTimeLabelColorGameId = UpdateColor(RemainingTimeLabelColorGameId, "remaining_time_label_color", value3);
					RemainingColorGameId = UpdateColor(RemainingColorGameId, "remaining_time_color", value3);
					OldPriceColorGameId = UpdateColor(OldPriceColorGameId, "old_price_color", value3);
					NewPriceColorGameId = UpdateColor(NewPriceColorGameId, "new_price_color", value3);
					GlowLayerColorGameId = UpdateColor(GlowLayerColorGameId, "glow_layer_color", value3);
					CTATitleColorColorGameId = UpdateColor(CTATitleColorColorGameId, "cta_title_color", value3);
					CTASubTitleColorGameId = UpdateColor(CTASubTitleColorGameId, "cta_subtitle_color", value3);
					CTATitleBackgroundColorGameId = UpdateColor(CTATitleBackgroundColorGameId, "cta_title_background_color", value3);
					CTABackgroundColorGameId = UpdateColor(CTABackgroundColorGameId, "cta_background_color", value3);
				}
			}
		}

		private void GetBackgroundAndSufixFromBackground(GameID _itemDef, ref string _backgroundPrefix, ref string _backgroundSufix)
		{
			_backgroundPrefix = string.Empty;
			_backgroundSufix = string.Empty;
			GameDB gameDB = Main.Instance.GameDB;
			ItemDef item = gameDB.GetItem<ItemDef>(_itemDef);
			if (item != null)
			{
				_backgroundPrefix = "ad_background_";
				int length = _backgroundPrefix.Length;
				_backgroundSufix = item.Name.Substring(length + 4);
			}
		}

		private GameID UpdateColor(GameID _target, string _targetLookupName, Dictionary<string, GameID> _colors)
		{
			GameID value = GameID.Invalid;
			if (!_target.Valid() && _colors.TryGetValue(_targetLookupName, out value))
			{
				_target = value;
			}
			return _target;
		}
	}
}
