using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.NGUI_Extensions.Model;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	internal class UIPortraitUtil
	{
		public static void UpdateInMissionPortrait(EntityId _entityId, UIPortraitTexture _itemTexture)
		{
			IReadonlyHumanoidDef humanoidDef = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetHumanoidDef(_entityId);
			if (humanoidDef == null)
			{
				return;
			}
			GameID gameID = GameID.Invalid;
			if (Main.Instance.IsNetworkEnabled && LogicMain.PlayerEntity.Id == _entityId)
			{
				Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				if (currentAssassin != null)
				{
					gameID = currentAssassin.GetAssassinHeadAccessoryGameID();
				}
			}
			IReadonlyCharacterAppearance characterAppearance = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetCharacterAppearance(_entityId);
			Texture2D mainTexture = null;
			HeadDef headDefinition = characterAppearance.HeadDefinition;
			if (headDefinition != null && characterAppearance.HeadMaterials != null)
			{
				mainTexture = characterAppearance.HeadMaterials.Portrait.GetObject<Texture2D>();
			}
			Color[] colors = new Color[2] { characterAppearance.Color0, characterAppearance.Color1 };
			Texture themeTexture = null;
			if (gameID != GameID.Invalid)
			{
				GameDB gameDB = Main.Instance.GameDB;
				ItemDef item = gameDB.GetItem<ItemDef>(gameID);
				if (item != null)
				{
					themeTexture = item.ItemPortraitImage.GetObject<Texture>();
				}
			}
			Texture2D @object = humanoidDef.BodyPortrait.GetObject<Texture2D>();
			Texture2D object2 = humanoidDef.BodyPortraitMask.GetObject<Texture2D>();
			TextureModel s_textureModel = TextureModel.s_textureModel;
			s_textureModel.Clear();
			s_textureModel.Colors = colors;
			s_textureModel.MainTexture = mainTexture;
			s_textureModel.MaskingTexture = object2;
			s_textureModel.ForegroundTexture = @object;
			s_textureModel.OrgSize = humanoidDef.BodyPortraitOrgSize;
			s_textureModel.ThemeTexture = themeTexture;
			_itemTexture.SetMaterialData(s_textureModel);
		}

		public static void UpdatePortraitByColorDef(Assassin _assassin, UIPortraitTexture _portraitTexture, bool _fullSize = false, Vector4? _customUV = null)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _itemId = GameID.Invalid;
			bool _transparentAssassin;
			_assassin.GetAssasssinPortraitData(out _headId, out _humanoidId, out _colorId, out _itemId, out _transparentAssassin);
			UpdatePortraitByColorDef(_headId, _humanoidId, _colorId, _itemId, _portraitTexture, _fullSize, _customUV);
			_portraitTexture.alpha = ((!_transparentAssassin) ? 1f : 0.01f);
		}

		public static void UpdatePortraitByColorDef(HirelingAssassin _hireling, UIPortraitTexture _portraitTexture, bool _fullSize = false, Vector4? _customUV = null)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _insignia = GameID.Invalid;
			GetHirelingAssassinPortraitData(_hireling, out _headId, out _humanoidId, out _colorId, out _insignia);
			UpdatePortraitByColorDef(_headId, _humanoidId, _colorId, _insignia, _portraitTexture, _fullSize, _customUV);
		}

		public static void UpdatePortraitByColorDef(FriendAssassin _friendAssassin, UIPortraitTexture _portraitTexture, bool _fullSize = false, Vector4? _customUV = null)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _insignia = GameID.Invalid;
			GetFriendAssassinPortraitData(_friendAssassin, out _headId, out _humanoidId, out _colorId, out _insignia);
			UpdatePortraitByColorDef(_headId, _humanoidId, _colorId, _insignia, _portraitTexture, _fullSize, _customUV);
		}

		public static void UpdatePortraitByColorDef(GameID _headId, GameID _humanoidId, GameID _colorId, GameID _itemId, UIPortraitTexture _portraitTexture, bool _fullSize = false, Vector4? _customUV = null)
		{
			GameDB db = Main.Instance.GameDB;
			HumanoidDef humanoidDef = db.GetItem<HumanoidDef>(_humanoidId);
			humanoidDef.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success)
				{
					if (_itemId == GameID.Invalid)
					{
						ApplyUpdateToPortrait(humanoidDef, _headId, _colorId, _portraitTexture, null, _fullSize, _customUV);
					}
					else
					{
						ItemDef itemDef = db.GetItem<ItemDef>(_itemId);
						itemDef.AssetBundleInfo.Provide(delegate(bool _successItemDef)
						{
							if (_successItemDef)
							{
								ApplyUpdateToPortrait(humanoidDef, _headId, _colorId, _portraitTexture, itemDef, _fullSize, _customUV);
							}
						});
					}
				}
			});
		}

		private static void ApplyUpdateToPortrait(HumanoidDef _humanoidDef, GameID _headId, GameID _colorId, UIPortraitTexture _portraitTexture, ItemDef _itemDef, bool _fullSize = false, Vector4? _customUV = null)
		{
			GameDB gameDB = Main.Instance.GameDB;
			HeadDef item = gameDB.GetItem<HeadDef>(_headId);
			ColorDef item2 = gameDB.GetItem<ColorDef>(_colorId);
			Texture mainTexture = null;
			CharacterAppearance characterAppearance = new CharacterAppearance(_humanoidDef, item);
			HeadDef.HeadMaterials headMaterials = characterAppearance.HeadMaterials;
			if (headMaterials != null)
			{
				mainTexture = headMaterials.Portrait.GetObject<Texture2D>();
			}
			Texture @object = _humanoidDef.BodyPortrait.GetObject<Texture>();
			Texture object2 = _humanoidDef.BodyPortraitMask.GetObject<Texture>();
			Color[] colors = new Color[2] { item2.Color0, item2.Color1 };
			Texture themeTexture = null;
			if (_itemDef != null)
			{
				themeTexture = _itemDef.ItemPortraitImage.GetObject<Texture>();
			}
			TextureModel s_textureModel = TextureModel.s_textureModel;
			s_textureModel.Clear();
			s_textureModel.Colors = colors;
			s_textureModel.MainTexture = mainTexture;
			s_textureModel.MaskingTexture = object2;
			s_textureModel.ForegroundTexture = @object;
			s_textureModel.OrgSize = ((!_fullSize) ? _humanoidDef.BodyPortraitOrgSize : Vector2.zero);
			s_textureModel.UvCoord = _customUV;
			s_textureModel.ThemeTexture = themeTexture;
			_portraitTexture.SetMaterialData(s_textureModel);
		}

		public static void UpdateHeroicPortraitByAssassin(Assassin _assassin, int _heroicRank, UIHeroicPortraitTexture _texture, bool _fullSize = false)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _itemId = GameID.Invalid;
			bool _transparentAssassin;
			_assassin.GetAssasssinPortraitData(out _headId, out _humanoidId, out _colorId, out _itemId, out _transparentAssassin);
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(_heroicRank, out value);
			UpdateHeroicPortrait(_headId, _humanoidId, _colorId, _itemId, value.PortraitItem, _texture, _fullSize);
			_texture.alpha = ((!_transparentAssassin) ? 1f : 0.01f);
		}

		public static void UpdateHeroicPortraitByHirelingAssassin(HirelingAssassin _hireling, int _heroicRank, UIHeroicPortraitTexture _texture, bool _fullSize = false)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _insignia = GameID.Invalid;
			GetHirelingAssassinPortraitData(_hireling, out _headId, out _humanoidId, out _colorId, out _insignia);
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(_heroicRank, out value);
			UpdateHeroicPortrait(_headId, _humanoidId, _colorId, _insignia, value.PortraitItem, _texture, _fullSize);
		}

		public static void UpdateHeroicPortraitByFriendAssassin(FriendAssassin _friendAssassin, int _heroicRank, UIHeroicPortraitTexture _texture, bool _fullSize = false)
		{
			GameID _headId = GameID.Invalid;
			GameID _humanoidId = GameID.Invalid;
			GameID _colorId = GameID.Invalid;
			GameID _insignia = GameID.Invalid;
			GetFriendAssassinPortraitData(_friendAssassin, out _headId, out _humanoidId, out _colorId, out _insignia);
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(_heroicRank, out value);
			UpdateHeroicPortrait(_headId, _humanoidId, _colorId, _insignia, value.PortraitItem, _texture, _fullSize);
		}

		private static void UpdateHeroicPortrait(GameID _headId, GameID _humanoidId, GameID _colorId, GameID _itemId, GameID _heroicRankItemRankId, UIHeroicPortraitTexture _portraitTexture, bool _fullSize = false)
		{
			GameDB db = Main.Instance.GameDB;
			HumanoidDef humanoidDef = db.GetItem<HumanoidDef>(_humanoidId);
			humanoidDef.AssetBundleInfo.Provide(delegate(bool _success)
			{
				if (_success)
				{
					ItemDef heroicItemDef = db.GetItem<ItemDef>(_heroicRankItemRankId);
					heroicItemDef.AssetBundleInfo.Provide(delegate(bool _heroicSuccess)
					{
						if (_heroicSuccess)
						{
							if (_itemId == GameID.Invalid)
							{
								ApplyUpdateToHeroicPortrait(humanoidDef, _headId, _humanoidId, _colorId, _heroicRankItemRankId, heroicItemDef, _portraitTexture, null, _fullSize);
							}
							else
							{
								ItemDef itemDef = db.GetItem<ItemDef>(_itemId);
								itemDef.AssetBundleInfo.Provide(delegate(bool _successItemDef)
								{
									if (_successItemDef)
									{
										ApplyUpdateToHeroicPortrait(humanoidDef, _headId, _humanoidId, _colorId, _heroicRankItemRankId, heroicItemDef, _portraitTexture, itemDef, _fullSize);
									}
								});
							}
						}
					});
				}
			});
		}

		private static IEnumerator RetrieveItemTexture(ItemDef _itemDef, Action<Texture> _callback)
		{
			bool done = false;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			ItemDef _itemDef2 = default(ItemDef);
			Action<Texture> _callback2 = default(Action<Texture>);
			_itemDef.AssetBundleInfo.Provide(delegate(bool _successItemDef)
			{
				Texture obj = null;
				if (_successItemDef)
				{
					obj = _itemDef2.ItemImageMask.GetObject<Texture>();
				}
				if (_callback2 != null)
				{
					_callback2(obj);
				}
				done = true;
			});
			while (!done)
			{
				yield return wait;
			}
		}

		private static void ApplyUpdateToHeroicPortrait(HumanoidDef _humanoidDef, GameID _headId, GameID _humanoidId, GameID _colorId, GameID _heroicRankItemRankId, ItemDef _heroicItemDef, UIHeroicPortraitTexture _portraitTexture, ItemDef _itemDef, bool _fullSize = false)
		{
			GameDB gameDB = Main.Instance.GameDB;
			HeadDef item = gameDB.GetItem<HeadDef>(_headId);
			ColorDef item2 = gameDB.GetItem<ColorDef>(_colorId);
			Texture mainTexture = null;
			CharacterAppearance characterAppearance = new CharacterAppearance(_humanoidDef, item);
			HeadDef.HeadMaterials headMaterials = characterAppearance.HeadMaterials;
			if (headMaterials != null)
			{
				mainTexture = headMaterials.Portrait.GetObject<Texture2D>();
			}
			Color[] colors = new Color[2] { item2.Color0, item2.Color1 };
			Texture @object = _humanoidDef.BodyPortrait.GetObject<Texture>();
			Texture object2 = _humanoidDef.BodyPortraitMask.GetObject<Texture>();
			Texture object3 = _heroicItemDef.ItemImage.GetObject<Texture>();
			Texture object4 = _heroicItemDef.ItemImageMask.GetObject<Texture>();
			Texture themeTexture = null;
			if (_itemDef != null)
			{
				themeTexture = _itemDef.ItemPortraitImage.GetObject<Texture>();
			}
			HeroicTextureModel s_heroicTextureModel = HeroicTextureModel.s_heroicTextureModel;
			s_heroicTextureModel.Colors = colors;
			s_heroicTextureModel.MainTexture = mainTexture;
			s_heroicTextureModel.MaskingTexture = object2;
			s_heroicTextureModel.ForegroundTexture = @object;
			s_heroicTextureModel.HeroicFrameTexture = object3;
			s_heroicTextureModel.HeroicFrameStencilTexture = object4;
			s_heroicTextureModel.OrgSize = ((!_fullSize) ? _humanoidDef.BodyPortraitOrgSize : Vector2.zero);
			s_heroicTextureModel.ThemeTexture = themeTexture;
			_portraitTexture.SetMaterialData(s_heroicTextureModel);
		}

		public static void GetFriendAssassinPortraitData(FriendAssassin _friendAssassin, out GameID _headId, out GameID _humanoidId, out GameID _colorId, out GameID _insignia)
		{
			GetPortraitData(_friendAssassin.headGuid, _friendAssassin.humanoidGuid, _friendAssassin.colorSetting, _friendAssassin.insigniaGuid, out _headId, out _humanoidId, out _colorId, out _insignia);
		}

		public static void GetHirelingAssassinPortraitData(HirelingAssassin _hireling, out GameID _headId, out GameID _humanoidId, out GameID _colorId, out GameID _insignia)
		{
			GetPortraitData(_hireling.headGuid, _hireling.humanoidGuid, _hireling.colorSetting, _hireling.insigniaGuid, out _headId, out _humanoidId, out _colorId, out _insignia);
		}

		private static void GetPortraitData(string _headGuid, string _humanoidGuid, string _colorSetting, string _insigniaGuid, out GameID _headId, out GameID _humanoidId, out GameID _colorId, out GameID _insignia)
		{
			_headId = GameID.Invalid;
			_humanoidId = GameID.Invalid;
			_colorId = GameID.Invalid;
			_insignia = GameID.Invalid;
			UIGUID uIGUID = new UIGUID(_headGuid);
			UIGUID uIGUID2 = new UIGUID(_humanoidGuid);
			UIGUID uIGUID3 = new UIGUID(_colorSetting);
			if (uIGUID.Valid && uIGUID2.Valid && uIGUID3.Valid)
			{
				GameDB gameDB = Main.Instance.GameDB;
				_headId = gameDB.FindGameIdFromGuid(uIGUID);
				_humanoidId = gameDB.FindGameIdFromGuid(uIGUID2);
				_colorId = gameDB.FindGameIdFromGuid(uIGUID3);
				if (!string.IsNullOrEmpty(_insigniaGuid))
				{
					_insignia = gameDB.FindGameIdFromGuid(_insigniaGuid);
				}
			}
		}
	}
}
