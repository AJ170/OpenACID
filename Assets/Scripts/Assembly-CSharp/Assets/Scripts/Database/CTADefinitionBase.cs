using AssetBundleData;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.CallToActionPopup;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Database
{
	[Database("CallToActionDefinition")]
	public abstract class CTADefinitionBase : DBEntry
	{
		public enum EResult
		{
			Fullfilled = 0,
			Proceed = 1,
			Abort = 2
		}

		public bool Enabled { get; set; }

		public int Priority { get; set; }

		public int MinMissions { get; set; }

		public string Entitlement { get; set; }

		public LocaKey HeaderLocaKey { get; set; }

		public LocaKey BodyLocaKey { get; set; }

		public LocaKey AcceptLocaKey { get; set; }

		public LocaKey CancelLocaKey { get; set; }

		public UnityResource Image { get; set; }

		public string Banner { get; set; }

		public string AcceptSpriteName { get; set; }

		public EScreenID RedirectScreen { get; set; }

		public string RedirectItem { get; set; }

		public bool CheckOnProfileUpdate { get; set; }

		protected CTADefinitionBase(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Enabled = true;
			CheckOnProfileUpdate = false;
			Priority = 0;
			HeaderLocaKey = new LocaKey();
			BodyLocaKey = new LocaKey();
			AcceptLocaKey = new LocaKey();
			CancelLocaKey = new LocaKey();
			Image = new UnityResource(_assetBundleInfo);
			RedirectScreen = EScreenID.Invalid;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Enabled = _s.SerializeBool("Enabled", Enabled);
			Priority = _s.SerializeInt("Priority", Priority);
			MinMissions = _s.SerializeInt("MinMissions", MinMissions);
			Entitlement = _s.SerializeString("Entitlement", Entitlement);
			HeaderLocaKey.Key = _s.SerializeInt("HeaderLocaKey", HeaderLocaKey.Key);
			BodyLocaKey.Key = _s.SerializeInt("BodyLocaKey", BodyLocaKey.Key);
			AcceptLocaKey.Key = _s.SerializeInt("AcceptLocaKey", AcceptLocaKey.Key);
			CancelLocaKey.Key = _s.SerializeInt("CancelLocaKey", CancelLocaKey.Key);
			Image.Serialize("Image", _s, base.AssetBundleInfo);
			Banner = _s.SerializeString("Banner", Banner);
			AcceptSpriteName = _s.SerializeString("AcceptSpriteName", AcceptSpriteName);
			RedirectScreen = (EScreenID)_s.SerializeEnum("RedirectScreen", typeof(EScreenID), (int)RedirectScreen);
			RedirectItem = _s.SerializeString("RedirectItem", RedirectItem);
		}

		public virtual EResult CheckTriggerBase()
		{
			if (!string.IsNullOrEmpty(Entitlement) && !Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(Entitlement))
			{
				return EResult.Proceed;
			}
			if ((RedirectScreen == EScreenID.AssassinSelection || RedirectScreen == EScreenID.AssassinSkillTree || RedirectScreen == EScreenID.InventoryScreen) && Singleton<StateManager>.Instance.ProfileManager.Profile.IsMysteryBoxOpen())
			{
				return EResult.Proceed;
			}
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			string key = "Mission" + GetType().Name;
			int num = instance.CTACounterGet(LocalStorage.CTACounterMission);
			if (!CheckOnProfileUpdate)
			{
				int num2 = instance.CTACounterGet(key, num - MinMissions);
				if (num - num2 < MinMissions)
				{
					return EResult.Proceed;
				}
			}
			EResult eResult = CheckTrigger();
			if (eResult == EResult.Fullfilled)
			{
				instance.CTACounterSet(key, num);
			}
			return eResult;
		}

		protected abstract EResult CheckTrigger();

		public virtual StringMapper GetBodyMapper()
		{
			return null;
		}

		public virtual void ShowPopup()
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(CancelLocaKey.Key),
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					SoundIntent = EButtonIntent.Cancel,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						CallToActionPopup.Proceed(this, false, false);
					}
				},
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(AcceptLocaKey.Key),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					SoundIntent = EButtonIntent.Accept,
					IconSpriteName = AcceptSpriteName,
					IconSpriteAtlas = UIScreenManager.EAtlasType.Animus,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						RedirectToScreen();
						CallToActionPopup.Proceed(this, true, false);
					},
					Data = true
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(HeaderLocaKey.Key);
			popUpConfiguration.Buttons = buttons;
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				CallToActionPopup.Proceed(this, false, true);
			};
			popUpConfiguration.Data = new CallToActionContent.CallToActionData
			{
				Definition = this
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_cta_popup);
		}

		public virtual void RedirectToScreen()
		{
			if (RedirectScreen == EScreenID.ShopScreen && !string.IsNullOrEmpty(RedirectItem))
			{
				UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
				ShopItemNode shopItemById = shopDetails.CategoryRootNode.GetShopItemById(RedirectItem);
				shopDetails.OpenAndSetFocusOnItem(shopItemById, true);
			}
			else if (RedirectScreen != EScreenID.Invalid)
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(RedirectScreen, new UITweenHandle(null, null));
			}
		}
	}
}
