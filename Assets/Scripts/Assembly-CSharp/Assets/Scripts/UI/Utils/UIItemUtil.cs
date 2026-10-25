using System;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUI_Extensions.Model;
using Assets.Scripts.UI.Screens.Popup.ItemPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ThemeManager;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	internal class UIItemUtil
	{
		public enum ItemUVCoordType
		{
			ENone = 0,
			EBanner1x1 = 1,
			EShopItem = 2,
			ECtaShopItem = 3,
			ELeaderboard = 4
		}

		public static readonly Color s_defaultShopItemColor = new Color32(22, 75, 93, byte.MaxValue);

		public static ColorDef GetColorDefForColorGuid(GameID _colorGUID)
		{
			GameDB gameDB = Main.Instance.GameDB;
			return gameDB.GetItem<ColorDef>(_colorGUID);
		}

		public static ColorDef GetColorDefForQuality(Quality.EQuality _itemQuality)
		{
			return GetColorDefForColorGuid(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_itemQuality].ColorGUID);
		}

		public static ColorDef GetTextColorDefForQuality(Quality.EQuality _itemQuality)
		{
			return GetColorDefForColorGuid(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_itemQuality].ColorTextGUID);
		}

		public static Color GetItemQualityColor(Quality.EQuality _itemQuality)
		{
			ColorDef colorDefForQuality = GetColorDefForQuality(_itemQuality);
			if (colorDefForQuality != null)
			{
				return colorDefForQuality.Color0;
			}
			return Color.white;
		}

		public static Color GetItemTextQualityColor(Quality.EQuality _itemQuality)
		{
			ColorDef textColorDefForQuality = GetTextColorDefForQuality(_itemQuality);
			if (textColorDefForQuality != null)
			{
				return textColorDefForQuality.Color0;
			}
			return Color.white;
		}

		public static UINGUIFormattedText GetFormattedColoredText(Quality.EQuality _quality)
		{
			ColorDef colorDefForColorGuid = GetColorDefForColorGuid(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_quality].ColorGUID);
			string text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_quality].NameId);
			return new UINGUIFormattedText(text, new NGUIColoredText(colorDefForColorGuid.Color0));
		}

		public static ColorDef GetColorFromGameId(GameID _colorGameID)
		{
			GameDB gameDB = Main.Instance.GameDB;
			return gameDB.GetItem<ColorDef>(_colorGameID);
		}

		public static ColorDef GetColorFromGuid(string _colorGuid)
		{
			GameDB gameDB = Main.Instance.GameDB;
			GameID _gameId = GameID.Invalid;
			if (gameDB.TryFindGameIdFromGuid(_colorGuid, out _gameId))
			{
				return GetColorFromGameId(_gameId);
			}
			return null;
		}

		public static Color GetWidgetColor(string _colorGuid)
		{
			ColorDef colorFromGuid = GetColorFromGuid(_colorGuid);
			if (colorFromGuid != null)
			{
				return colorFromGuid.Color0;
			}
			return Color.white;
		}

		public static Color GetWidgetColor(GameID _colorGameID)
		{
			ColorDef colorFromGameId = GetColorFromGameId(_colorGameID);
			if (colorFromGameId != null)
			{
				return colorFromGameId.Color0;
			}
			return Color.white;
		}

		public static void UpdateWidgetColor(GameID _colorGameId, UIWidget _widget)
		{
			ColorDef colorFromGameId = GetColorFromGameId(_colorGameId);
			if (colorFromGameId != null)
			{
				_widget.color = colorFromGameId.Color0;
			}
		}

		public static void UpdateRankAvatarByLvl(int _avatarRank, UITexture _texture)
		{
			UIDataMapper instance = UbiCore.Utils.Singleton<UIDataMapper>.Instance;
			BBDictionary<int, UIPath> playerLevelSprite = instance.PlayerLevelSprite;
			_texture.ClearTexture();
			_texture.mainTexture = UnityEngine.Resources.Load(playerLevelSprite[_avatarRank], typeof(Texture2D)) as Texture2D;
		}

		public static void UpdateResourceItem(EResourceType _resourceType, Color _quality, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, bool _isShopItem = false, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, string _overrideColorDefGuid = "", float _desaturation = 0f)
		{
			ResourceDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)_resourceType, out value))
			{
				ColorDef colorDef = null;
				if (!string.IsNullOrEmpty(_overrideColorDefGuid))
				{
					GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_overrideColorDefGuid);
					colorDef = Main.SafeGameDb.GetItem<ColorDef>(gameID);
				}
				UpdateItemByColorDef(value.ItemGUID, _quality, _turnBackgroundOff, colorDef, _itemTexture, _isShopItem, _uvCoordType, false, _desaturation);
			}
		}

		public static void UpdateCustomItem(ECustomItemType _customItemType, Color _quality, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, float _desaturation = 0f)
		{
			CustomItemDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CustomItemDescriptions.TryGetValue((int)_customItemType, out value))
			{
				GameID gameID = value.ItemGUID;
				if (Singleton<StateManager>.Instance.ThemesManager.ActiveTheme == ThemesManager.EFlavorTheme.Pirate)
				{
					gameID = value.PirateThemeIconGUID;
				}
				UpdateItemByColorDef(gameID, _quality, _turnBackgroundOff, null, _itemTexture, false, ItemUVCoordType.ENone, false, _desaturation);
			}
		}

		public static void UpdateMysteryBoxElement(InventoryElement _element, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, bool _isShopItem = false, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, string _overrideColorDefGuid = "")
		{
			MysteryBoxDescription value = null;
			if (_element.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxItemType)
			{
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemMysteryBoxDescription.TryGetValue((int)_element.quality, out value);
			}
			else
			{
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceMysteryBoxDescription.TryGetValue((int)_element.quality, out value);
			}
			if (value != null)
			{
				ColorDef colorDef = null;
				if (!string.IsNullOrEmpty(_overrideColorDefGuid))
				{
					GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_overrideColorDefGuid);
					colorDef = Main.SafeGameDb.GetItem<ColorDef>(gameID);
				}
				UpdateItemByColorDef(_element.guid, _element.quality, _turnBackgroundOff, colorDef, _itemTexture, 0f);
			}
		}

		public static void UpdateResourceItem(EResourceType _resourceType, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, bool _isShopItem = false, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, string _overrideColorDefGuid = "", float _desaturation = 0f)
		{
			ResourceDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)_resourceType, out value))
			{
				ColorDef colorDef = null;
				if (!string.IsNullOrEmpty(_overrideColorDefGuid))
				{
					GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_overrideColorDefGuid);
					colorDef = Main.SafeGameDb.GetItem<ColorDef>(gameID);
				}
				UpdateItemByColorDef(value.ItemGUID, s_defaultShopItemColor, _turnBackgroundOff, colorDef, _itemTexture, _isShopItem, _uvCoordType, false, _desaturation);
			}
		}

		public static void UpdateModuleImage(string _moduleGuid, bool _turnBackgroundOff, UIPortraitTexture _itemTexture)
		{
			ModuleQualityMapping moduleQualityMapping = Singleton<StateManager>.Instance.TemplateManager.Globals.moduleQualityMapping;
			Quality.EQuality moduleDisplayItemQuality = moduleQualityMapping.GetModuleDisplayItemQuality(Quality.EQuality.Mundane);
			ColorDef colorDefForQuality = GetColorDefForQuality(moduleDisplayItemQuality);
			UpdateItemByColorDef(_moduleGuid, moduleDisplayItemQuality, _turnBackgroundOff, colorDefForQuality, _itemTexture, 0f);
		}

		public static void UpdateItem(InventoryElement _item, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, bool _useShopItem = false, string _overrideColorDefGuid = "", float _desaturation = 0f)
		{
			GameID _humanoidId;
			GameID _colorDefId;
			AvatarData.TryGetOutfitModification(_item, out _humanoidId, out _colorDefId);
			if (!string.IsNullOrEmpty(_overrideColorDefGuid))
			{
				_colorDefId = Main.SafeGameDb.FindGameIdFromGuid(_overrideColorDefGuid);
			}
			ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(_colorDefId);
			UpdateItem(_item, _turnBackgroundOff, _itemTexture, item, _useShopItem, _desaturation);
		}

		public static void UpdateItem(InventoryElement _item, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ColorDef _colorDef, bool _useShopItem = false, float _desaturation = 0f)
		{
			GameID _humanoidId;
			GameID _colorDefId;
			AvatarData.TryGetOutfitModification(_item, out _humanoidId, out _colorDefId);
			Quality.EQuality eQuality = _item.quality;
			if (_item.category == EInventoryElementCategory.InventoryModule)
			{
				ModuleQualityMapping moduleQualityMapping = Singleton<StateManager>.Instance.TemplateManager.Globals.moduleQualityMapping;
				eQuality = moduleQualityMapping.GetModuleDisplayItemQuality(eQuality);
			}
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_item.guid);
			ColorDef colorDef = GetColorDefForQuality(eQuality);
			if (_item.IsBlueprint())
			{
				colorDef = GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor);
			}
			else if (_item.IsCustomLoot())
			{
				colorDef = GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor);
			}
			UpdateItemByColorDef(gameID, colorDef.Color0, _turnBackgroundOff, _colorDef, _itemTexture, _useShopItem, ItemUVCoordType.ENone, false, _desaturation);
		}

		public static void UpdateItemByLootTemplate(LootTemplate _lootTemplate, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, float _desaturation = 0f)
		{
			if (_lootTemplate.itemTemplate != null)
			{
				GameID gameID = AvatarData.TryGetItemModification(_lootTemplate);
				GameID gameID2 = AvatarData.TryGetItemColorModification(_lootTemplate);
				ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(gameID2);
				ColorDef colorDefForQuality = GetColorDefForQuality(_lootTemplate.quality);
				float desaturation = _desaturation;
				UpdateItemByColorDef(gameID, colorDefForQuality.Color0, _turnBackgroundOff, item, _itemTexture, true, ItemUVCoordType.ENone, false, desaturation);
			}
			else if (_lootTemplate.IsBluePrint())
			{
				ColorDef colorDefForColorGuid = GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.BlueprintGlowColor);
				float desaturation = _desaturation;
				UpdateItemByColorDef(_lootTemplate.guid, colorDefForColorGuid.Color0, _turnBackgroundOff, null, _itemTexture, true, ItemUVCoordType.ENone, desaturation);
			}
			else if (_lootTemplate.IsCustomItemType(ECustomLootTemplateType.LockPick))
			{
				ColorDef colorDefForColorGuid2 = GetColorDefForColorGuid(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.LockpicksGlowColor);
				float desaturation = _desaturation;
				UpdateItemByColorDef(_lootTemplate.guid, colorDefForColorGuid2.Color0, _turnBackgroundOff, null, _itemTexture, true, ItemUVCoordType.ENone, desaturation);
			}
			else
			{
				if (_lootTemplate.IsCodexEntitlement())
				{
					return;
				}
				if (_lootTemplate.resourceTemplate != null)
				{
					UpdateResourceItem(_lootTemplate.resourceTemplate.maxResource.type, s_defaultShopItemColor, false, _itemTexture, false, ItemUVCoordType.ENone, string.Empty, 0f);
					return;
				}
				Quality.EQuality itemQuality = _lootTemplate.quality;
				if (_lootTemplate.moduleTemplate != null)
				{
					ModuleQualityMapping moduleQualityMapping = Singleton<StateManager>.Instance.TemplateManager.Globals.moduleQualityMapping;
					itemQuality = moduleQualityMapping.GetModuleDisplayItemQuality(_lootTemplate.quality);
				}
				ColorDef colorDefForQuality2 = GetColorDefForQuality(itemQuality);
				UpdateItemByColorDef(_lootTemplate.guid, itemQuality, _turnBackgroundOff, colorDefForQuality2, _itemTexture, 0f);
			}
		}

		public static void UpdateShopAdItem(GameID _overrideImagegameId, ShopBundle _shopBundle, Color _quality, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			if (_overrideImagegameId.Valid())
			{
				UpdateItemByColorDef(_overrideImagegameId, _quality, _turnBackgroundOff, null, _itemTexture, true, _uvCoordType, false, _desaturation);
			}
			else if (_shopBundle != null)
			{
				UpdateShopItem(_shopBundle, _quality, _turnBackgroundOff, _itemTexture, _uvCoordType, _desaturation);
			}
		}

		public static void UpdateShopAdItem(string _overrideImageGuid, ShopBundle _shopBundle, Color _quality, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			if (!string.IsNullOrEmpty(_overrideImageGuid))
			{
				UpdateItemByColorDef(_overrideImageGuid, _quality, _turnBackgroundOff, null, _itemTexture, true, _uvCoordType, _desaturation);
			}
			else if (_shopBundle != null)
			{
				UpdateShopItem(_shopBundle, _quality, _turnBackgroundOff, _itemTexture, _uvCoordType, _desaturation);
			}
		}

		public static void UpdateShopItemReward(ShopBundle _shopBundle, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			UpdateShopItem(_shopBundle.GetPreviewImageGameID(), _shopBundle.GetPreviewColorGameID(), _turnBackgroundOff, _itemTexture, _shopBundle.GetShopBundleRewardItemColor(), _uvCoordType, _desaturation);
		}

		public static void UpdateShopItem(ShopBundle _shopBundle, Color _quality, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			UpdateShopItem(_shopBundle.GetPreviewImageGameID(), _shopBundle.GetPreviewColorGameID(), _turnBackgroundOff, _itemTexture, _quality, _uvCoordType, _desaturation);
		}

		public static void UpdateShopItem(ItemPopupContent.PopupItemConfiguration _configuration, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			if (_configuration.Item == null)
			{
				GameID previewImageGameID = _configuration.ShopBundle.GetPreviewImageGameID();
				GameID previewColorGameID = _configuration.ShopBundle.GetPreviewColorGameID();
				UpdateShopItem(previewImageGameID, previewColorGameID, _turnBackgroundOff, _itemTexture, _configuration.ShopBundle.GetShopBundleItemColor(), _uvCoordType, _desaturation);
			}
			else
			{
				GameID _humanoidId;
				GameID _colorDefId;
				AvatarData.TryGetOutfitModification(_configuration.Item.itemData, _configuration.Item.guid, out _humanoidId, out _colorDefId);
				Color itemQualityColor = GetItemQualityColor(_configuration.Item.quality);
				HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(_humanoidId);
				ColorDef item2 = Main.SafeGameDb.GetItem<ColorDef>(_colorDefId);
				UpdateItemByColorDef((item != null) ? item.GameID : Main.SafeGameDb.FindGameIdFromGuid(_configuration.Item.guid), itemQualityColor, _turnBackgroundOff, item2, _itemTexture, true, _uvCoordType, false, _desaturation);
			}
		}

		public static void UpdateShopItem(GameID _itemGameId, GameID _colorDefGuid, bool _turnBackgroundOff, UIPortraitTexture _itemTexture, Color _quality, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(_colorDefGuid);
			UpdateItemByColorDef(_itemGameId, _quality, _turnBackgroundOff, item, _itemTexture, true, _uvCoordType, false, _desaturation);
		}

		public static void UpdateItemByColorDef(string _guid, Quality.EQuality _itemQuality, bool _turnBackgroundOff, ColorDef _colorDef, UIPortraitTexture _itemTexture, float _desaturation = 0f)
		{
			UIGUID uIGUID = new UIGUID(_guid);
			if (uIGUID.Valid)
			{
				ColorDef colorDefForQuality = GetColorDefForQuality(_itemQuality);
				UpdateItemByColorDef(_guid, colorDefForQuality.Color0, _turnBackgroundOff, _colorDef, _itemTexture, false, ItemUVCoordType.ENone, _desaturation);
			}
		}

		private static void UpdateItemByColorDef(string _guid, Color _itemQuality, bool _turnBackgroundOff, ColorDef _colorDef, UIPortraitTexture _itemTexture, bool _isShopItem = false, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, float _desaturation = 0f)
		{
			UpdateItemByColorDef(Main.SafeGameDb.FindGameIdFromGuid(_guid), _itemQuality, _turnBackgroundOff, _colorDef, _itemTexture, _isShopItem, _uvCoordType, false, _desaturation);
		}

		private static string GetTextureBackground(bool _isItemUnknown)
		{
			return (!_isItemUnknown) ? "UI/Texture/icon_background_03" : "UI/Texture/icon_bg_unknown_quality";
		}

		public static void UpdateItemByColorDef(GameID _gameID, Color _itemQuality, bool _turnBackgroundOff, ColorDef _colorDef, UIPortraitTexture _itemTexture, bool _isShopItem = false, ItemUVCoordType _uvCoordType = ItemUVCoordType.ENone, bool _isUnknownItem = false, float _desaturation = 0f)
		{
			GameDB gameDB = Main.Instance.GameDB;
			string bgTexturePath = GetTextureBackground(_isUnknownItem);
			DBEntry def = gameDB.GetItem<DBEntry>(_gameID);
			if (def == null)
			{
				return;
			}
			def.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success)
				{
					Texture foregroundTexture = null;
					Texture maskingTexture = null;
					Color color = Color.white;
					Color color2 = Color.white;
					Vector2 orgSize = Vector2.zero;
					Vector4 value = UIPortraitTexture.s_defaultUVCoord;
					Vector3 vector = _itemTexture.transform.localPosition;
					if (_colorDef != null)
					{
						color = _colorDef.Color0;
						color2 = _colorDef.Color1;
					}
					if (def is ItemDef)
					{
						ItemDef itemDef = (ItemDef)def;
						foregroundTexture = itemDef.ItemImage.GetObject<Texture>();
						maskingTexture = itemDef.ItemImageMask.GetObject<Texture>();
						float num = itemDef.ItemImageScaleFactor;
						switch (_uvCoordType)
						{
						case ItemUVCoordType.EBanner1x1:
							value = itemDef.BannerUVPosition;
							num = itemDef.BannerImageScaleFactor;
							break;
						case ItemUVCoordType.EShopItem:
							vector = itemDef.ItemOffset;
							value = itemDef.ItemUVPosition;
							num = itemDef.ItemImageScaleFactor;
							break;
						case ItemUVCoordType.ECtaShopItem:
							vector = itemDef.CtaOffset;
							value = itemDef.CtaUVPosition;
							num = itemDef.CtaImageScaleFactor;
							break;
						}
						if (_isShopItem)
						{
							orgSize = itemDef.ItemImageOrgSize * num;
						}
					}
					else if (def is HumanoidDef)
					{
						HumanoidDef humanoidDef = (HumanoidDef)def;
						if (_isShopItem)
						{
							foregroundTexture = humanoidDef.BodyItemShopImage.GetObject<Texture>();
							maskingTexture = humanoidDef.BodyItemShopImageMask.GetObject<Texture>();
							float num2 = 1f;
							switch (_uvCoordType)
							{
							case ItemUVCoordType.EBanner1x1:
								value = humanoidDef.BannerUVPosition;
								num2 = humanoidDef.BannerImageScaleFactor;
								break;
							case ItemUVCoordType.EShopItem:
								vector = humanoidDef.ShopItemOffset;
								value = humanoidDef.ShopItemUVPosition;
								num2 = humanoidDef.ShopImageScaleFactor;
								break;
							}
							orgSize = humanoidDef.BodyItemShopImageOrgSize * num2;
						}
						else
						{
							foregroundTexture = humanoidDef.BodyItemImage.GetObject<Texture>();
							maskingTexture = humanoidDef.BodyItemImageMask.GetObject<Texture>();
							orgSize = humanoidDef.BodyItemImageOrgSize;
						}
					}
					Color[] colors = new Color[3] { color, color2, _itemQuality };
					TextureModel s_textureModel = TextureModel.s_textureModel;
					s_textureModel.Clear();
					s_textureModel.Colors = colors;
					s_textureModel.MainTexture = UnityEngine.Resources.Load<Texture>(bgTexturePath);
					s_textureModel.MaskingTexture = maskingTexture;
					s_textureModel.ForegroundTexture = foregroundTexture;
					s_textureModel.HideBackground = _turnBackgroundOff;
					s_textureModel.Desaturation = _desaturation;
					s_textureModel.OrgSize = orgSize;
					s_textureModel.UvCoord = value;
					s_textureModel.OffsetPos = vector;
					_itemTexture.SetMaterialData(s_textureModel);
				}
			});
		}

		public static void UpdateUITexture(string _guid, UIBasicSprite _texture, Action<ItemDef> _onCompleted = null)
		{
			UIGUID uIGUID = new UIGUID(_guid);
			if (uIGUID.Valid)
			{
				GameID gameId = Main.SafeGameDb.FindGameIdFromGuid(uIGUID);
				UpdateUITexture(gameId, _texture, _onCompleted);
			}
		}

		public static void UpdateUITexture(GameID _gameId, UIBasicSprite _texture, Action<ItemDef> _onCompleted = null)
		{
			GameDB gameDB = Main.Instance.GameDB;
			DBEntry def = gameDB.GetItem<DBEntry>(_gameId);
			if (def == null)
			{
				return;
			}
			def.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success)
				{
					Texture mainTexture = null;
					ItemDef itemDef = def as ItemDef;
					if (itemDef != null)
					{
						mainTexture = itemDef.ItemImage.GetObject<Texture>();
					}
					_texture.mainTexture = mainTexture;
					Action<ItemDef> action = _onCompleted;
					if (action != null)
					{
						action(itemDef);
					}
				}
			});
		}
	}
}
