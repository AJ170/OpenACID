using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
	public class AccountCreationScreen : MonoBehaviour
	{
		private enum EShowType
		{
			CreateOrLogin = 1,
			Create = 1,
			Login = 1,
			FacebookCreateOrLogin = 3,
			TypeName = 4,
			Confirm = 5
		}

		private enum ELoginCreateType
		{
			Default = 0,
			Facebook = 1
		}

		private enum EAccountIntent
		{
			Create = 0,
			Login = 1
		}

		[SerializeField]
		private UIToggle m_createOrLoginColliderToggle;

		[SerializeField]
		private UIToggle m_createColliderToggle;

		[SerializeField]
		private UIToggle m_loginColliderToggle;

		[SerializeField]
		private UIToggle m_createConfirmColliderToggle;

		[SerializeField]
		private UIToggle m_facebookCreateOrLoginColliderToggle;

		[SerializeField]
		private GameObject m_backButton;

		[SerializeField]
		private UIWidget m_textContainerWidget;

		[SerializeField]
		private UIWidget m_onFakeOnScreenKeyboard;

		[SerializeField]
		private UIAlignmentBox m_createProfileUsingContainer;

		[SerializeField]
		private UIAlignmentBox m_thirdPartyAlignmentBox;

		[SerializeField]
		private GameObject m_loginFacebookGameObject;

		[SerializeField]
		private GameObject m_loginWeiboGameObject;

		[SerializeField]
		private GameObject m_loginUplayGameObject;

		[SerializeField]
		private UILabel m_thirdPartyLoginPromptLabel;

		[SerializeField]
		private SubScreen m_subScreenCreateOrLogin;

		[SerializeField]
		private SubScreen m_subScreenCreate;

		[SerializeField]
		private SubScreen m_subScreenLogin;

		[SerializeField]
		private SubScreen m_subScreenFacebookCreateOrLogin;

		[SerializeField]
		private SubScreen m_subScreenTypeName;

		[SerializeField]
		private SubScreen m_subScreenConfirm;

		[SerializeField]
		private UIExtendedLabel m_lblTypeName;

		[SerializeField]
		private UILocalization m_lblTitle;

		[SerializeField]
		private UIWidget m_headerWidget;

		[SerializeField]
		private EShowType m_showType = EShowType.FacebookCreateOrLogin;

		[SerializeField]
		private UIExtendedButton m_facebookCreateContainerButton;

		[SerializeField]
		private UIButtonProxy m_facebookCreateButtonProxy;

		[SerializeField]
		private UIExtendedButton m_facebookLoginContainerButton;

		[SerializeField]
		private UIButtonProxy m_facebookLoginButtonProxy;

		private UIExtendedButton m_facebookCreateButton;

		private UIExtendedButton m_facebookLoginButton;

		private ELoginCreateType m_loginCreateType;

		private string m_userName = string.Empty;

		private bool m_facebookFailed;

		private TouchScreenKeyboard m_keyboard;

		private string m_oldKeyboardText;

		private readonly ExclusiveSubScreenManager m_subScreenMgr = new ExclusiveSubScreenManager();

		private DefaultHandler m_errorhandler;

		public string PrivacyPolicyUrl
		{
			get
			{
				return LocalizationManager.Instance.Get(3688);
			}
		}

		private EShowType GetMainScreenType()
		{
			return (!FacebookManager.IsAvailable && !WeiboManager.IsWeiboAvailable) ? EShowType.TypeName : EShowType.FacebookCreateOrLogin;
		}

		private void Awake()
		{
			m_subScreenMgr.Set(new List<SubScreen> { m_subScreenCreateOrLogin, m_subScreenCreate, m_subScreenLogin, m_subScreenFacebookCreateOrLogin, m_subScreenTypeName, m_subScreenConfirm });
		}

		private void Start()
		{
			if (WeiboManager.IsWeiboAvailable && WeiboManager.HasLoggedIntoWeiboKey)
			{
				WeiboManager.LogoutSilently();
			}
			m_facebookCreateButton = m_facebookCreateButtonProxy.Button;
			m_facebookLoginButton = m_facebookLoginButtonProxy.Button;
			if (WeiboManager.IsWeiboAvailable)
			{
				m_thirdPartyLoginPromptLabel.text = LocalizationManager.Instance.Get(8398);
			}
			else
			{
				m_thirdPartyLoginPromptLabel.text = LocalizationManager.Instance.Get(5836);
			}
			m_loginFacebookGameObject.gameObject.SetActive(FacebookManager.IsAvailable);
			if (!FacebookManager.IsAvailable)
			{
				m_loginWeiboGameObject.gameObject.SetActive(WeiboManager.IsWeiboAvailable);
			}
			m_loginUplayGameObject.gameObject.SetActive(false);
			m_thirdPartyAlignmentBox.RepositionNow = true;
			EventDelegate.Add(m_facebookCreateContainerButton.onClick, OnCreateAccountFacebookClick);
			EventDelegate.Add(m_facebookCreateButton.onClick, OnCreateAccountFacebookClick);
			EventDelegate.Add(m_facebookLoginContainerButton.onClick, OnLoginFacebookClick);
			EventDelegate.Add(m_facebookLoginButton.onClick, OnLoginFacebookClick);
		}

		private void OnDestroy()
		{
			EventDelegate.Remove(m_facebookCreateContainerButton.onClick, OnCreateAccountFacebookClick);
			EventDelegate.Remove(m_facebookCreateButton.onClick, OnCreateAccountFacebookClick);
			EventDelegate.Remove(m_facebookLoginContainerButton.onClick, OnLoginFacebookClick);
			EventDelegate.Remove(m_facebookLoginButton.onClick, OnLoginFacebookClick);
			m_facebookFailed = false;
		}

		private void OnEnable()
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.CurrentCategory.Category == ETutorialCategory.TutCatIntroMission && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(ETutorialCategory.TutCatIntroMission).CurrentFlowId == 1L)
			{
				SetShowType(GetMainScreenType(), true);
			}
		}

		private void OnDisable()
		{
			if (Singleton<UIScreenManager>.Instance != null && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.SetVisibility(false);
			}
		}

		private void Update()
		{
			if (m_keyboard != null)
			{
				if (m_oldKeyboardText != m_keyboard.text)
				{
					m_keyboard.text = ValidateUserName(m_keyboard.text);
					m_oldKeyboardText = m_keyboard.text;
				}
				if (m_keyboard.done)
				{
					if (m_keyboard.wasCanceled)
					{
						m_keyboard = null;
						OnScreenKeyboardCanceled(m_oldKeyboardText);
					}
					else
					{
						m_keyboard = null;
						OnScreenKeyboardDone(m_oldKeyboardText);
					}
				}
				else if (!m_keyboard.active)
				{
					m_keyboard.active = true;
					m_keyboard.text = m_oldKeyboardText;
				}
			}
			ELoginCreateType loginCreateType = m_loginCreateType;
			if (loginCreateType == ELoginCreateType.Facebook)
			{
				UpdateFacebook();
			}
		}

		[ContextMenu("SetShowType")]
		private void SetShowTypeFromInspector()
		{
			SetShowType(m_showType);
		}

		private void SetShowType(EShowType _showType)
		{
			SetShowType(_showType, false);
		}

		private void SetShowType(EShowType _showType, bool _instantCamera)
		{
			m_createOrLoginColliderToggle.value = false;
			m_createColliderToggle.value = false;
			m_loginColliderToggle.value = false;
			m_createConfirmColliderToggle.value = false;
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.SetVisibility(false);
			}
			switch (_showType)
			{
			case EShowType.CreateOrLogin:
				m_createColliderToggle.value = true;
				m_lblTitle.Key = 3385;
				m_facebookCreateButtonProxy.isEnabled = !m_facebookFailed;
				m_facebookLoginButtonProxy.isEnabled = !m_facebookFailed;
				m_facebookCreateContainerButton.isEnabled = !m_facebookFailed;
				m_facebookLoginContainerButton.isEnabled = !m_facebookFailed;
				break;
			case EShowType.FacebookCreateOrLogin:
				m_facebookCreateOrLoginColliderToggle.value = true;
				m_lblTitle.Key = 3385;
				break;
			case EShowType.TypeName:
				m_lblTitle.Key = 4309;
				OpenKeyboard(m_userName);
				m_createProfileUsingContainer.gameObject.SetActive(m_loginCreateType != ELoginCreateType.Default);
				m_createProfileUsingContainer.RepositionNow = true;
				break;
			case EShowType.Confirm:
				m_lblTitle.Key = 4310;
				m_createConfirmColliderToggle.value = true;
				if (Singleton<UIScreenManager>.Instance != null && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
				{
					Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.SetVisibility(true);
					Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Tween(UIPlayerCard.EPositionType.OffScreenRight, UIPlayerCard.EPositionType.CreateAccountAccept);
				}
				break;
			}
			m_subScreenMgr.Show((int)_showType);
			if (_instantCamera)
			{
				m_subScreenMgr.InstantCamera((int)_showType);
			}
			else
			{
				m_subScreenMgr.TweenCamera((int)_showType);
			}
			SetBackButtonVisibilityAccordingToShowType(_showType);
			m_createProfileUsingContainer.gameObject.SetActive(m_loginCreateType != ELoginCreateType.Default);
			m_onFakeOnScreenKeyboard.gameObject.SetActive(false);
			m_showType = _showType;
		}

		private void SetBackButtonVisibilityAccordingToShowType(EShowType _showType)
		{
			m_backButton.SetActive(false);
		}

		private void ShowConfirmPlayerCardDialog()
		{
			SetShowType(EShowType.Confirm);
		}

		private void OnScreenKeyboardDone(string _text)
		{
			string text = _text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				ShowInvalidProfileNamePpup(text);
				return;
			}
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_accept);
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Card.SetUserName(text);
			}
			if (m_errorhandler == null)
			{
				m_errorhandler = new DefaultHandler(OnModifyPlayerErrorPopupClosed);
			}
			string fbAccessToken = ((m_loginCreateType != ELoginCreateType.Facebook) ? null : FacebookManager.AccessToken);
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.ModifyPlayerProfileName(text, ESocialNetwork.Facebook, fbAccessToken, EModuleOrigin.Origin_CharacterCreation, false, _asyncResult);
			}, m_errorhandler, EndSetPlayerName);
		}

		private void OnScreenKeyboardCanceled(string _enteredText)
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Card.SetUserName(Singleton<StateManager>.Instance.ProfileManager.Profile.userName);
			}
			SetShowType(GetMainScreenType());
		}

		private void EndSetPlayerName(IAsyncResult _result)
		{
			if (!_result.IsError())
			{
				ShowConfirmPlayerCardDialog();
			}
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Card.SetUserName(Singleton<StateManager>.Instance.ProfileManager.Profile.userName);
			}
		}

		private string ValidateUserName(string _name)
		{
			return _name.Substring(0, (_name.Length <= 19) ? _name.Length : 19);
		}

		private void OnModifyPlayerErrorPopupClosed()
		{
			OnRetypeFaultyNameClicked(m_userName);
		}

		private void ShowInvalidProfileNamePpup(string _name)
		{
			PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.INVALID_PROFILENAME, ErrorCategory.SERVER_USERPROFILE);
			defaultPopUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			defaultPopUpConfiguration.ShowCloseButton = true;
			defaultPopUpConfiguration.OnCloseButtonClicked = delegate
			{
				OnRetypeFaultyNameClicked(_name);
			};
			defaultPopUpConfiguration.Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = OnRetypeFaultyNameClicked,
					Data = _name
				}
			};
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
		}

		private void OnRetypeFaultyNameClicked(object _name)
		{
			HideLoadingOverlay();
			SetShowType(EShowType.TypeName);
		}

		private void HandleLoggedInToExistingAccount()
		{
			if (!TutorialHelper.OwningFinishedEntitlements(ETutorialCategory.TutCatIntroMission))
			{
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
				{
					Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Card.SetUserName(profile.userName);
				}
				SetShowType(EShowType.Confirm);
				return;
			}
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.OnEnterAnimusAfterMission();
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.SetFirstAvailableStep())
			{
				Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.Mission, new UITweenHandle(null, null));
			}
			if (TutorialHelper.OwningFinishedEntitlements(ETutorialCategory.TutCatAssassinCreation))
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Show();
			}
		}

		private void LoginWithUplay()
		{
		}

		private void LoginWithWeibo()
		{
			WeiboManager.Logout(delegate
			{
				WeiboManager.Login(delegate(bool success, WeiboManager.EWeiboLoginResult result)
				{
					if (success && (result == WeiboManager.EWeiboLoginResult.LinkedToNewProfile || result == WeiboManager.EWeiboLoginResult.LoggedInSameAccount || result == WeiboManager.EWeiboLoginResult.OldProfileOverriden))
					{
						string currentLoggedInWeiboPlayerName = WeiboManager.CurrentLoggedInWeiboPlayerName;
						if (currentLoggedInWeiboPlayerName.Length <= 16)
						{
							m_userName = currentLoggedInWeiboPlayerName;
						}
						else
						{
							m_userName = currentLoggedInWeiboPlayerName.Substring(0, Math.Min(Math.Max(currentLoggedInWeiboPlayerName.IndexOf(" ", StringComparison.Ordinal), 4), 16));
						}
						m_lblTypeName.SetLocalizationKey(3620, new StringMapper { { "Social_Type", "Weibo" } });
						m_loginCreateType = ELoginCreateType.Facebook;
						SetShowType(EShowType.TypeName);
					}
				});
			});
		}

		public void OnShowCreateAccountContainer()
		{
			SetShowType(EShowType.CreateOrLogin);
		}

		public void OnShowLoginToExistingAccountContainer()
		{
			SetShowType(EShowType.CreateOrLogin);
		}

		public void OnBackClick()
		{
			SetShowType(GetMainScreenType());
		}

		public void OnCreateAccountClick()
		{
			m_loginCreateType = ELoginCreateType.Default;
			SetShowType(EShowType.TypeName);
			int height = Screen.height;
			int getKeyboardHeight = NativeTools.GetKeyboardHeight;
			int height2 = m_textContainerWidget.height;
			if (getKeyboardHeight != 0 && getKeyboardHeight != height && height - getKeyboardHeight >= height2)
			{
				int num = (height - getKeyboardHeight - height2) / 2;
				if (num > 0)
				{
					num *= -1;
				}
				m_textContainerWidget.topAnchor.absolute = num;
				m_textContainerWidget.ResetAnchors();
				m_textContainerWidget.UpdateAnchors();
				m_textContainerWidget.ForceUpdate(true);
			}
		}

		private void OpenKeyboard(string _name)
		{
			m_keyboard = TouchScreenKeyboard.Open(_name, TouchScreenKeyboardType.Default, false, false, false, false, LocalizationManager.Instance.Get(4309));
			m_onFakeOnScreenKeyboard.gameObject.SetActive(false);
		}

		public void OnCreateAccountFacebookClick()
		{
			LoginWithFacebook();
		}

		public void OnLoginFacebookClick()
		{
			LoginWithFacebook();
		}

		public void OnCreateAccountUplayClick()
		{
			LoginWithUplay();
		}

		public void OnLoginUplayClick()
		{
			LoginWithUplay();
		}

		public void OnCreateAccountWeiboClick()
		{
			LoginWithWeibo();
		}

		public void OnLoginWeiboClick()
		{
			LoginWithWeibo();
		}

		public void OnCreateNewConfirmClick()
		{
			if (Singleton<UIScreenManager>.Instance != null && Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.PlayerCard != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.SetVisibility(true);
				Singleton<UIScreenManager>.Instance.SceneData.PlayerCard.Tween(UIPlayerCard.EPositionType.CreateAccountAccept, UIPlayerCard.EPositionType.OffScreenLeft);
			}
			UITutorialManager tutorialManager = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager;
			tutorialManager.FinishCategory(ETutorialCategory.TutCatIntroMission, null);
		}

		public void OnCreateNewCancelClick()
		{
			SetShowType(GetMainScreenType());
		}

		public void OnScreenKeyboardFakeDone()
		{
			m_onFakeOnScreenKeyboard.gameObject.SetActive(false);
			OnScreenKeyboardDone("Ezio_Auditore");
		}

		public void OnPrivacyPolicyClick()
		{
			if (NativeTools.IsTVDevice || UIDownloadScreenDetails.s_useTvShowPolicy)
			{
				NativeTools.ShowPrivacyPolicy(PrivacyPolicyUrl);
			}
			else
			{
				Application.OpenURL(PrivacyPolicyUrl);
			}
		}

		public void OnBackKey()
		{
			SetShowType(EShowType.TypeName);
		}

		private void ShowLoadingInitOverlay()
		{
			Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
		}

		private void ShowLoadingFetchingUser()
		{
			Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
		}

		private void ShowLoadingFetchingFacebookName()
		{
			Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
		}

		private void HideLoadingOverlay()
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		}

		private void LoginWithFacebook()
		{
			FacebookManager.Logout(delegate(bool logoutSuccess, FacebookManager.EFBLogoutFlowResult logoutResult)
			{
				if (logoutSuccess)
				{
					FacebookManager.Login(delegate(bool loginSuccess, FacebookManager.EFBLoginFlowResult loginResult)
					{
						if (loginResult == FacebookManager.EFBLoginFlowResult.LinkedToNewProfile || loginResult == FacebookManager.EFBLoginFlowResult.LoggedInSameAccount)
						{
							string text = FacebookManager.Name;
							m_userName = text.Substring(0, Math.Min(Math.Max(text.IndexOf(" ", StringComparison.Ordinal), 4), 16));
							m_lblTypeName.SetLocalizationKey(3620, new StringMapper { { "Social_Type", "Facebook" } });
							m_loginCreateType = ELoginCreateType.Facebook;
							SetShowType(EShowType.TypeName);
						}
					});
				}
			});
		}

		private void UpdateFacebook()
		{
		}
	}
}
