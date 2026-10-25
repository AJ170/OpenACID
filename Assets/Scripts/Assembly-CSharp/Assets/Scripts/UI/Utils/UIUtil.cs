using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UbiCore;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UIUtil
	{
		public enum ESide
		{
			Left = 0,
			Right = 1,
			Top = 2,
			Bottom = 3
		}

		private const string s_defaultQualityOverlaySpriteName = "aUI_fra_goods_fade";

		private const string s_swordQualityOverlaySpriteName = "aUI_fra_goodsWeapon_fade";

		public static readonly int ReferenceScreenHeight = 1536;

		public static readonly int ReferenceIconSize = 156;

		public static readonly int ReferenceScreenHeightStartUpscale = 768;

		public static readonly float ReferenceDpi = 264f;

		public static readonly float MAX_PHONE_INCHES = 5.5f;

		private static readonly float LOW_FACTOR_THRESHOLD = 0.7f;

		public static float CalculateRecommendedItemPOW(int _level, float _rank)
		{
			return Singleton<StateManager>.Instance.TemplateManager.Globals.missionLevelMapping.First((Globals.MissionLevelMapping x) => x.missionLevel == _level).powRecommendation;
		}

		public static int FormatPercentValue(float _factor)
		{
			int result = (int)(_factor * 100f + 0.5f);
			if (_factor < 0.01f && _factor > 0f)
			{
				result = 1;
			}
			else if (_factor > 0.99f && _factor < 1f)
			{
				result = 99;
			}
			return result;
		}

		public static float GetScreenInches()
		{
			return Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height) / GetLayoutDPI();
		}

		public static float GetLayoutDPI()
		{
			float num = Screen.dpi;
			if (MathHelper.Approximately(num, 0f))
			{
				RuntimePlatform platform = Application.platform;
				num = ((platform != RuntimePlatform.Android && platform != RuntimePlatform.IPhonePlayer) ? 96f : 300f);
			}
			return num;
		}

		public static float CalculateScaleFactorFromDpi()
		{
			float dpi = Screen.dpi;
			float num = 1f;
			if (!MathHelper.Approximately(dpi, 0f))
			{
				float num2 = dpi / ReferenceDpi;
				num = (float)Screen.height / (float)ReferenceScreenHeight * num2;
				if (GetScreenInches() > MAX_PHONE_INCHES && num2 <= LOW_FACTOR_THRESHOLD)
				{
					num *= 2f;
				}
			}
			else
			{
				float num3 = 1f;
				if ((float)Screen.height < (float)ReferenceScreenHeightStartUpscale)
				{
					num3 += 1f - (float)Screen.height / (float)ReferenceScreenHeightStartUpscale;
				}
				num = (float)Screen.height / (float)ReferenceScreenHeight * num3;
			}
			return num;
		}

		public static void ToggleLeftAndRightAnchorPoints(UIWidget _widget)
		{
			UIRect.AnchorPoint leftAnchor = _widget.leftAnchor;
			UIRect.AnchorPoint rightAnchor = _widget.rightAnchor;
			leftAnchor.relative = Mathf.Abs(leftAnchor.relative - 1f);
			rightAnchor.relative = Mathf.Abs(rightAnchor.relative - 1f);
			int absolute = leftAnchor.absolute;
			leftAnchor.absolute = rightAnchor.absolute * -1;
			rightAnchor.absolute = absolute * -1;
		}

		public static Vector3 GetWidgetCenter(UIWidget _widget)
		{
			Vector3 zero = Vector3.zero;
			zero.x = (float)_widget.width / 2f * ((0.5f - _widget.pivotOffset.x) / 0.5f);
			zero.y = (float)_widget.height / 2f * ((0.5f - _widget.pivotOffset.y) / 0.5f);
			return zero;
		}

		public static float GetOffscreenPositionX(UIWidget _widget, float _width, ESide _side)
		{
			_widget.Update();
			return GetOffscreenPositionX(_widget, _width, _widget.pivotOffset.x, _side);
		}

		public static float GetOffscreenPositionX(UIWidget _widget, ESide _side)
		{
			_widget.Update();
			float x = NGUIMath.CalculateRelativeWidgetBounds(_widget.transform, true).size.x;
			return GetOffscreenPositionX(_widget, x, _widget.pivotOffset.x, _side);
		}

		public static float GetOffscreenPositionX(UIWidget _widget, float _width, float _pivotOffsetX, ESide _side)
		{
			float result = _widget.transform.localPosition.x;
			Transform transform = _widget.transform;
			Camera cam = NGUITools.FindCameraForLayer(_widget.root.gameObject.layer);
			Vector3[] sides = cam.GetSides(transform);
			switch (_side)
			{
			case ESide.Left:
			{
				float num2 = _width * Mathf.Abs(_pivotOffsetX - 1f);
				result = Mathf.Floor(transform.localPosition.x + sides[0].x - num2);
				break;
			}
			case ESide.Right:
			{
				float num = _width * _pivotOffsetX;
				result = Mathf.Ceil(transform.localPosition.x + sides[2].x + num);
				break;
			}
			}
			return result;
		}

		public static float GetOffscreenPositionY(UIWidget _widget, float _height, ESide _side)
		{
			_widget.Update();
			return GetOffscreenPositionY(_widget, _height, _widget.pivotOffset.y, _side);
		}

		public static float GetOffscreenPositionY(UIWidget _widget, ESide _side)
		{
			_widget.Update();
			float y = NGUIMath.CalculateRelativeWidgetBounds(_widget.transform, true).size.y;
			return GetOffscreenPositionY(_widget, y, _widget.pivotOffset.y, _side);
		}

		public static float GetOffscreenPositionY(UIWidget _widget, float _height, float _pivotOffsetY, ESide _side)
		{
			float result = _widget.transform.localPosition.y;
			Transform transform = _widget.transform;
			Camera cam = NGUITools.FindCameraForLayer(_widget.root.gameObject.layer);
			Vector3[] sides = cam.GetSides(transform);
			switch (_side)
			{
			case ESide.Top:
			{
				float num2 = _height * Mathf.Abs(_pivotOffsetY - 1f);
				num2 *= ((!(sides[1].y < 0f)) ? 1f : (-1f));
				result = Mathf.Floor(transform.localPosition.y + sides[1].y + num2);
				break;
			}
			case ESide.Bottom:
			{
				float num = _height * _pivotOffsetY;
				num *= ((!(sides[3].y < 0f)) ? 1f : (-1f));
				result = Mathf.Ceil(transform.localPosition.y + sides[3].y + num);
				break;
			}
			}
			return result;
		}

		public static void UpdateLeftAndRightAnchorsForTarget(UIRect _rect, UIRect _anchorTarget)
		{
			_anchorTarget.Update();
			_rect.leftAnchor.target = _anchorTarget.transform;
			_rect.rightAnchor.target = _anchorTarget.transform;
			_rect.leftAnchor.Set(0f, 0f);
			_rect.rightAnchor.Set(1f, 0f);
			_rect.ResetAnchors();
			_rect.UpdateAnchors();
			_rect.Update();
		}

		public static int SortResourcesByType(Resource _resA, Resource _resB)
		{
			return GetSortOrderFromReosurceType(_resA.type).CompareTo(GetSortOrderFromReosurceType(_resB.type));
		}

		private static int GetSortOrderFromReosurceType(EResourceType _type)
		{
			switch (_type)
			{
			case EResourceType.HardCurrency:
				return 2;
			case EResourceType.SoftCurrency:
				return 0;
			case EResourceType.SocialCurrency:
				return 1;
			default:
				return 3;
			}
		}

		public static void SetupCurrencyWidgets(Resource _cost, UIExtendedLabel _label, UISprite _sprite, bool _largeIcon = false, bool _toggleSpriteParent = false)
		{
			long amount = _cost.amount;
			bool flag = _cost.amount <= 0;
			GameObject gameObject = ((!_toggleSpriteParent) ? _sprite.gameObject : _sprite.transform.parent.gameObject);
			CurrencyDescription value;
			if (!flag && UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)_cost.type, out value))
			{
				_sprite.spriteName = ((!_largeIcon) ? value.SmallIconName : value.IconName);
				gameObject.SetActive(true);
			}
			else
			{
				gameObject.SetActive(false);
			}
			if (flag)
			{
				_label.ClearFormatFlags();
				_label.text = LocalizationManager.Instance.Get(8023);
			}
			else
			{
				_label.SetAbbreviatedNumberIfNotFit(amount);
			}
		}

		public static void UpdateShopItemBannerQuality(ShopBundle _shopBundle, ref Color32 _qualityColor)
		{
			if (_shopBundle.IsHeroicBundle())
			{
				_qualityColor = UIDataMapper.UITextColors.HeroicShopGlowColor;
				return;
			}
			_qualityColor = _shopBundle.GetShopBundleItemColor();
			LootTemplate shopPreviewLootTemplate = _shopBundle.GetShopPreviewLootTemplate();
			bool flag = shopPreviewLootTemplate != null && shopPreviewLootTemplate.itemTemplate != null;
			if (shopPreviewLootTemplate == null || (_shopBundle.payout.lootTemplates.GetFirstModuleTemplate() != null && !flag))
			{
				return;
			}
			if (_shopBundle.ShowShopBundleQuility())
			{
				_qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
				if (_shopBundle.quality == Quality.EQuality.Perfect)
				{
					_qualityColor = UIItemUtil.GetItemTextQualityColor(_shopBundle.quality);
				}
				else if (_shopBundle.quality != Quality.EQuality.Mundane)
				{
					_qualityColor = UIItemUtil.GetItemQualityColor(_shopBundle.quality);
				}
			}
			else if (_shopBundle.payout.lootTemplates.Length == 1 && _shopBundle.payout.lootTemplates[0].mysteryBoxTemplate != null)
			{
				_qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
			}
		}

		public static void UpdateShopItemQuality(ShopBundle _shopBundle, ref Color32 _qualityColor, ref Color32 _textQualityColor, UISprite _overlayImage = null)
		{
			_qualityColor = _shopBundle.GetShopBundleItemColor();
			_textQualityColor = UIDataMapper.UITextColors.ShopMundaneOverideColor;
			LootTemplate shopPreviewLootTemplate = _shopBundle.GetShopPreviewLootTemplate();
			bool flag = shopPreviewLootTemplate != null && shopPreviewLootTemplate.itemTemplate != null;
			if (_overlayImage != null)
			{
				_overlayImage.gameObject.SetActive(false);
			}
			if (shopPreviewLootTemplate != null)
			{
				if (_shopBundle.payout.lootTemplates.GetFirstModuleTemplate() != null && !flag)
				{
					_qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
					Quality.EQuality moduleDisplayItemQuality = _shopBundle.GetModuleDisplayItemQuality();
					if (moduleDisplayItemQuality == Quality.EQuality.Mundane)
					{
						_textQualityColor = UIDataMapper.UITextColors.ShopMundaneOverideColor;
					}
					else if (_overlayImage != null)
					{
						_overlayImage.color = _qualityColor;
						_overlayImage.spriteName = "aUI_fra_goods_fade";
						_overlayImage.gameObject.SetActive(true);
					}
				}
				else if (_shopBundle.ShowShopBundleQuility())
				{
					_qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
					if (_shopBundle.quality == Quality.EQuality.Perfect)
					{
						_qualityColor = UIItemUtil.GetItemTextQualityColor(_shopBundle.quality);
					}
					else if (_shopBundle.quality != Quality.EQuality.Mundane)
					{
						_qualityColor = UIItemUtil.GetItemQualityColor(_shopBundle.quality);
					}
					_textQualityColor = UIItemUtil.GetItemTextQualityColor(_shopBundle.quality);
					if (_overlayImage != null)
					{
						_overlayImage.color = _qualityColor;
						_overlayImage.spriteName = ((!flag || shopPreviewLootTemplate.itemTemplate.category != Item.EItemCategory.Sword) ? "aUI_fra_goods_fade" : "aUI_fra_goodsWeapon_fade");
						_overlayImage.gameObject.SetActive(true);
					}
				}
				else if (_shopBundle.payout.lootTemplates.Length == 1 && _shopBundle.payout.lootTemplates[0].mysteryBoxTemplate != null)
				{
					_qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
					_textQualityColor = UIItemUtil.GetItemTextQualityColor(_shopBundle.quality);
				}
			}
			if (!_shopBundle.IsHeroicBundle())
			{
				return;
			}
			if (_shopBundle.IsCustomLootItemType(ECustomLootTemplateType.LockPick) || _shopBundle.IsSoloOutFit() || _shopBundle.IsMysteryBox() || _shopBundle.HasWalletPayoutResource())
			{
				_qualityColor = UIDataMapper.UITextColors.HeroicShopGlowColor;
			}
			else if (_shopBundle.quality == Quality.EQuality.Mundane && _shopBundle.IsBundleWithSoloItemType(Item.EItemCategory.Insignia))
			{
				_qualityColor = UIDataMapper.UITextColors.ShopMundaneInsigniaOverideColor;
				if (_overlayImage != null)
				{
					_overlayImage.color = UIDataMapper.UITextColors.ShopMundaneInsigniaOverlayOverideColor;
				}
			}
		}

		public static void PopUpGeneric(StringId _heading, StringId _message, StringMapper _headerParams = null, StringMapper _messageParams = null, EventDelegate.Callback _callback = null)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(new StringId(1780)),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (_callback != null)
						{
							_callback();
						}
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(_heading, _headerParams);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(_message, _messageParams)
			};
			popUpConfiguration.Buttons = buttons;
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}

		public static void UpdateColliderIfTimerPaused()
		{
			if (UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				Main.Instance.StartCoroutine(OnUpdateColiderIfTimerPaused());
			}
		}

		public static void QuitApplication()
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.Volume = 0f;
			}
			Application.Quit();
		}

		private static IEnumerator OnUpdateColiderIfTimerPaused()
		{
			if (UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				Main.Instance.enabled = false;
				float timeScale = Time.timeScale;
				Time.timeScale = 0.1f;
				yield return new WaitForFixedUpdate();
				Time.timeScale = timeScale;
				Main.Instance.enabled = true;
			}
		}
	}
}
