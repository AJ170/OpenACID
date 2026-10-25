using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.Screens.Popup.ThirdPartyAccountPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using MiniJSON;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

public class WeiboManager : MonoBehaviour
{
	public enum EWeiboLoginResult
	{
		LoginSuccessful = 0,
		AlreadyLoggedIn = 1,
		LinkedToNewProfile = 2,
		LoggedInSameAccount = 3,
		RecoveredAccount = 4,
		ResetAccount = 5,
		OldProfileOverriden = 6,
		FailedOrCancelled = 7,
		NotInitialized = 8,
		EmptyUserData = 9,
		NotLoggedIn = 10,
		ProfileNotLinked = 11,
		NotAvailable = 12
	}

	public enum EWeiboLogoutResult
	{
		Successful = 0,
		AlreadyLoggedOut = 1,
		NoAccessToken = 2,
		Failed = 3,
		NotAvailable = 4,
		NotInitialized = 5
	}

	public enum EWeiboShareItemFlowResult
	{
		Unavailable = 0,
		NotInitialized = 1,
		NotLoggedIn = 2,
		FailedOrCancelled = 3,
		Succeeded = 4
	}

	public delegate void WeiboAccountStatusCallback(bool hasProfile, UserProfile userProfile);

	public delegate void WeiboLoginFlowCallback(bool _success, EWeiboLoginResult _result);

	public delegate void WeiboLogoutFlowCallback(bool _success, EWeiboLogoutResult _result);

	public delegate void WeiboShareItemFlowCallback(bool _success, EWeiboShareItemFlowResult _result);

	private const string TEST_ONAUTHENTICATIONSUCCESSFUL_DATA = "{\"accessToken\":\"2.00dJV_1GSabxmB4cfb1b1c32YCFyuD\",\"userId\":\"5796971705\",\"playerName\":\"未通过审核应用\"}";

	private const string TEST_WEIBO_USER_ID = "5796971705";

	private const string AchievementPostCDNUrl = "http://static.cdn.ubi.com/0040/fb/";

	private static BBList<string> AchievementPostRankMappingSuffix = new BBList<string> { "_bronze", "_silver", "_gold" };

	private static WeiboManager m_mgrInstance = null;

	public static bool IsInitialized { get; set; }

	public static bool IsLoggedIn { get; private set; }

	public static bool IsAutoLoginInProgress { get; set; }

	public static bool OverrideIsWeiboAvailable { get; set; }

	public static bool IsEnabledForTesting { get; set; }

	public static bool CanShareAchievements
	{
		get
		{
			return IsWeiboAvailable && IsLoggedIn;
		}
	}

	public static bool IsWeiboAvailable
	{
		get
		{
			if (OverrideIsWeiboAvailable)
			{
				return IsEnabledForTesting;
			}
			if (!Main.Instance.WeiboEnabled)
			{
				return false;
			}
			return false;
		}
	}

	public static bool IsProfileLinkedToWeibo
	{
		get
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			string linkedGameUserId = LinkedGameUserId;
			string text = ((profile == null) ? null : profile.userIdHash);
			return linkedGameUserId != null && linkedGameUserId == text;
		}
	}

	public static bool HasLoggedIntoWeiboKey
	{
		get
		{
			return PlayerPrefsExtension.GetBool("HasLoggedIntoWeiboKey");
		}
		set
		{
			PlayerPrefsExtension.SetBool("HasLoggedIntoWeiboKey", value);
		}
	}

	public static string LoggedInWeiboUserId
	{
		get
		{
			return PlayerPrefs.GetString("LoggedInWeiboUserId", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("LoggedInWeiboUserId", value);
		}
	}

	public static string LinkedWeiboUserId
	{
		get
		{
			return PlayerPrefs.GetString("LinkedWeiboUserId", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("LinkedWeiboUserId", value);
		}
	}

	public static string LinkedGameUserId
	{
		get
		{
			return PlayerPrefs.GetString("LinkedWeiboGameUserId", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("LinkedWeiboGameUserId", value);
		}
	}

	public static string NameWeiboProfile
	{
		get
		{
			return PlayerPrefs.GetString("NameWeiboProfile", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("NameWeiboProfile", value);
		}
	}

	public static string LoggedInWeiboAT
	{
		get
		{
			return PlayerPrefs.GetString("WbAT", string.Empty);
		}
		set
		{
			PlayerPrefs.SetString("WbAT", value);
		}
	}

	public static string CurrentLoggedInWeiboUserId { get; private set; }

	public static string CurrentLoggedInWeiboAccessToken { get; private set; }

	public static string CurrentLoggedInWeiboPlayerName { get; private set; }

	public static WeiboManager Instance
	{
		get
		{
			if (m_mgrInstance == null)
			{
				Init();
			}
			return m_mgrInstance;
		}
	}

	public static event WeiboLoginFlowCallback OnWeiboLoginFlowCallback;

	public static event WeiboLogoutFlowCallback OnWeiboLogoutFlowCallback;

	private void Awake()
	{
		CurrentLoggedInWeiboUserId = LinkedWeiboUserId;
		CurrentLoggedInWeiboPlayerName = NameWeiboProfile;
	}

	private static void InitializeInstance()
	{
		if (m_mgrInstance == null)
		{
			GameObject gameObject = new GameObject("WeiboGameObject");
			m_mgrInstance = gameObject.AddComponent<WeiboManager>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	public static void Init()
	{
		if (IsWeiboAvailable)
		{
			InitializeInstance();
		}
	}

	public static void AutoLogin(WeiboLoginFlowCallback callback = null)
	{
		IsAutoLoginInProgress = true;
		if (!IsWeiboAvailable)
		{
			CompleteLoginFlow(false, EWeiboLoginResult.NotAvailable, callback);
		}
		else if (!IsProfileLinkedToWeibo)
		{
			CompleteLoginFlow(false, EWeiboLoginResult.ProfileNotLinked, callback);
		}
		else if (!HasLoggedIntoWeiboKey)
		{
			CompleteLoginFlow(false, EWeiboLoginResult.NotLoggedIn, callback);
		}
		else
		{
			Login(callback);
		}
	}

	public static void Login(WeiboLoginFlowCallback callback = null)
	{
		if (!IsWeiboAvailable || Instance == null)
		{
			CompleteLoginFlow(false, EWeiboLoginResult.NotAvailable, callback);
			return;
		}
		if (HasLoggedIntoWeiboKey && IsProfileLinkedToWeibo)
		{
			CompleteLoginFlow(true, EWeiboLoginResult.AlreadyLoggedIn, callback);
			return;
		}
		ShowLoadingInitOverlay();
		CompleteLoginFlow(false, EWeiboLoginResult.NotAvailable, callback);
	}

	public static void Logout(WeiboLogoutFlowCallback callback = null)
	{
		if (!IsWeiboAvailable || Instance == null)
		{
			CompleteLogoutFlow(false, EWeiboLogoutResult.NotAvailable, callback);
		}
		else if (!HasLoggedIntoWeiboKey)
		{
			CompleteLogoutFlow(true, EWeiboLogoutResult.AlreadyLoggedOut, callback);
		}
		else if (string.IsNullOrEmpty(LoggedInWeiboAT))
		{
			CompleteLogoutFlow(false, EWeiboLogoutResult.NoAccessToken, callback);
		}
		else if (Instance != null)
		{
			Instance.OnLogoutSuccessful("TEST driven");
		}
	}

	public static void LogoutSilently(WeiboLogoutFlowCallback callback = null)
	{
		if (IsWeiboAvailable && HasLoggedIntoWeiboKey && !string.IsNullOrEmpty(LoggedInWeiboAT) && Instance != null)
		{
			Instance.OnLogoutSuccessful("TEST driven");
		}
	}

	private static void FetchWeiboAccountStatus(WeiboAccountStatusCallback registeredCallback, WeiboLoginFlowCallback callback)
	{
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.QueryFbStatus(CurrentLoggedInWeiboAccessToken, ESocialNetwork.Weibo, asyncResult);
		}, new DefaultHandler(delegate
		{
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
			PopUpConfiguration defaultPopUpConfiguration2 = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration2);
		}), delegate(IAsyncResult response)
		{
			if (response.IsError())
			{
				CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
				PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
			}
			else
			{
				FBStatusResponse fBStatusResponse = ((AsyncDataResult<FBStatusResponse>)response).Result();
				EFBStatus status = fBStatusResponse.status;
				UserProfile userProfile = fBStatusResponse.userProfile;
				bool hasProfile = status == EFBStatus.Registered;
				if (registeredCallback != null)
				{
					registeredCallback(hasProfile, userProfile);
				}
			}
		});
	}

	private static void FetchWeiboAccountStatusById(string userId, WeiboAccountStatusCallback registeredCallback, WeiboLoginFlowCallback callback)
	{
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.QueryFbStatusById(userId, ESocialNetwork.Weibo, _asyncResult);
		}, new DefaultHandler(delegate
		{
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
			PopUpConfiguration defaultPopUpConfiguration2 = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration2);
		}), delegate(IAsyncResult _response)
		{
			if (_response.IsError())
			{
				CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
				PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
			}
			else
			{
				FBStatusByIdResponse fBStatusByIdResponse = ((AsyncDataResult<FBStatusByIdResponse>)_response).Result();
				EFBStatus status = fBStatusByIdResponse.status;
				UserProfile userProfile = fBStatusByIdResponse.userProfile;
				bool hasProfile = status == EFBStatus.Registered;
				if (registeredCallback != null)
				{
					registeredCallback(hasProfile, userProfile);
				}
			}
		});
	}

	private static void LinkCurrentProfileToWeibo(WeiboLoginFlowCallback callback)
	{
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.ModifyPlayerProfileName(null, ESocialNetwork.Weibo, CurrentLoggedInWeiboAccessToken, EModuleOrigin.Origin_SocialScreen, false, asyncResult);
		}, new DefaultHandler(delegate
		{
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
			PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
		}), delegate(IAsyncResult response)
		{
			if (!response.IsError())
			{
				PopUpConfiguration config = new PopUpConfiguration
				{
					ShowCloseButton = false,
					Aligntarget = UIPopup.EContentAlignTarget.Heading,
					Heading = LocalizationManager.Instance.Get(8387),
					Buttons = new PopUpButtonConfiguration[1]
					{
						new PopUpButtonConfiguration
						{
							Label = LocalizationManager.Instance.Get(1780),
							OnButtonPressed = delegate
							{
								HideLoadingOverlay();
								CompleteLoginFlow(true, EWeiboLoginResult.LinkedToNewProfile, callback);
							}
						}
					},
					Data = new ErrorContent.ErrorData
					{
						Description = LocalizationManager.Instance.Get(8388, new StringMapper { { "name", CurrentLoggedInWeiboPlayerName } })
					}
				};
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			}
		});
	}

	private static void ShowLoadProfilePopup(UserProfile userProfile, WeiboLoginFlowCallback callback)
	{
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			HideLoadingOverlay();
			LogoutSilently();
			SilentCancel(callback);
		};
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(8386);
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
		{
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(1781),
				ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					LogoutSilently();
					SilentCancel(callback);
				}
			},
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(5823),
				ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					RecoverProfile(callback, (userProfile == null) ? string.Empty : userProfile.userIdHash);
				}
			}
		};
		popUpConfiguration.Data = new ThirdPartyAccountContent.ThirdPartyAccountData
		{
			ShowOnlyLoadProfile = true,
			ThirdPartyType = ThirdPartyAccountContent.ThirdPartyAccountData.EThirdPartyType.Weibo,
			ThirdPartyName = CurrentLoggedInWeiboPlayerName,
			CurrentProfile = Singleton<StateManager>.Instance.ProfileManager.Profile,
			ThirdPartyProfile = userProfile
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private static void ShowKeepCurrentAndLoadProfilePopup(UserProfile userProfile, WeiboLoginFlowCallback _callback)
	{
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			HideLoadingOverlay();
			LogoutSilently();
			SilentCancel(_callback);
		};
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(8386);
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
		{
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(8020),
				ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					WarnOverrideOldProfile(_callback, (userProfile == null) ? string.Empty : userProfile.userIdHash);
				}
			},
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(5823),
				ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					WarnRecoverProfile(_callback, (userProfile == null) ? string.Empty : userProfile.userIdHash);
				}
			}
		};
		popUpConfiguration.Data = new ThirdPartyAccountContent.ThirdPartyAccountData
		{
			ShowOnlyLoadProfile = false,
			ThirdPartyType = ThirdPartyAccountContent.ThirdPartyAccountData.EThirdPartyType.Weibo,
			ThirdPartyName = CurrentLoggedInWeiboPlayerName,
			CurrentProfile = Singleton<StateManager>.Instance.ProfileManager.Profile,
			ThirdPartyProfile = userProfile
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private static void WarnOverrideOldProfile(WeiboLoginFlowCallback callback, string requestedIdHash)
	{
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			HideLoadingOverlay();
			LogoutSilently();
			SilentCancel(callback);
		};
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(8021);
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
		{
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(1198),
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					OverrideOldProfile(callback, requestedIdHash);
				}
			},
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(1781),
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					LogoutSilently();
					SilentCancel(callback);
				}
			}
		};
		popUpConfiguration.Data = new ErrorContent.ErrorData
		{
			Description = LocalizationManager.Instance.Get(8389)
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private static void WarnRecoverProfile(WeiboLoginFlowCallback callback, string requestedIdHash)
	{
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			HideLoadingOverlay();
			LogoutSilently();
			SilentCancel(callback);
		};
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(5828);
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
		{
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(1198),
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					RecoverProfile(callback, requestedIdHash);
				}
			},
			new PopUpButtonConfiguration
			{
				Label = LocalizationManager.Instance.Get(1781),
				OnButtonPressed = delegate
				{
					HideLoadingOverlay();
					LogoutSilently();
					SilentCancel(callback);
				}
			}
		};
		popUpConfiguration.Data = new ErrorContent.ErrorData
		{
			Description = LocalizationManager.Instance.Get(8390)
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private static void RecoverProfile(WeiboLoginFlowCallback callback, string requestedIdHash)
	{
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.ModifyPlayerProfileName(null, ESocialNetwork.Weibo, CurrentLoggedInWeiboAccessToken, EModuleOrigin.Origin_SocialScreen, false, asyncResult);
		}, new DefaultHandler(delegate
		{
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
			PopUpConfiguration defaultPopUpConfiguration2 = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration2);
		}), delegate(IAsyncResult response)
		{
			if (!response.IsError())
			{
				if (Singleton<StateManager>.Instance.ProfileManager.Profile.userIdHash != requestedIdHash)
				{
					CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
					PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
				}
				else
				{
					if (FacebookManager.IsAvailable && FacebookManager.IsProfileLinkedToFacebook)
					{
						FacebookManager.LogoutSilently(delegate
						{
						});
					}
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(new PopUpConfiguration
					{
						Data = new ErrorContent.ErrorData
						{
							Description = LocalizationManager.Instance.Get(5831, new StringMapper { { "name", CurrentLoggedInWeiboPlayerName } })
						},
						Heading = LocalizationManager.Instance.Get(5830),
						Buttons = new PopUpButtonConfiguration[1]
						{
							new PopUpButtonConfiguration
							{
								Label = LocalizationManager.Instance.Get(1780),
								OnButtonPressed = delegate
								{
									HideLoadingOverlay();
									CompleteLoginFlow(true, EWeiboLoginResult.RecoveredAccount, callback);
									UIUtil.QuitApplication();
								}
							}
						}
					});
				}
			}
		});
	}

	private static void ResetProfile(WeiboLoginFlowCallback callback = null)
	{
		LoggedInWeiboUserId = string.Empty;
		LinkedWeiboUserId = string.Empty;
		LinkedGameUserId = string.Empty;
		NameWeiboProfile = string.Empty;
		UbiCore.Utils.Singleton<LocalStorage>.Instance.Clear();
		MailCache.Clear();
		if (!Singleton<StateManager>.Instance.ProfileManager.AbandonUserProfile())
		{
			PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
		}
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(new PopUpConfiguration
		{
			Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(5819)
			},
			Heading = LocalizationManager.Instance.Get(5817),
			Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = delegate
					{
						HideLoadingOverlay();
						CompleteLoginFlow(true, EWeiboLoginResult.ResetAccount, callback);
						UIUtil.QuitApplication();
					}
				}
			}
		});
	}

	private static void OverrideOldProfile(WeiboLoginFlowCallback callback, string requestedIdHash)
	{
		UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback asyncResult)
		{
			Singleton<StateManager>.Instance.ProfileManager.ModifyPlayerProfileName(null, ESocialNetwork.Weibo, CurrentLoggedInWeiboAccessToken, EModuleOrigin.Origin_SocialScreen, true, asyncResult);
		}, new DefaultHandler(delegate
		{
			CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
			PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.UnknownCode, ErrorCategory.SERVER_USERPROFILE);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
		}), delegate(IAsyncResult response)
		{
			if (!response.IsError())
			{
				PopUpConfiguration config = new PopUpConfiguration
				{
					ShowCloseButton = false,
					Aligntarget = UIPopup.EContentAlignTarget.Heading,
					Heading = LocalizationManager.Instance.Get(8386),
					Buttons = new PopUpButtonConfiguration[1]
					{
						new PopUpButtonConfiguration
						{
							Label = LocalizationManager.Instance.Get(1780),
							OnButtonPressed = delegate
							{
								HideLoadingOverlay();
								CompleteLoginFlow(true, EWeiboLoginResult.OldProfileOverriden, callback);
							}
						}
					},
					Data = new ErrorContent.ErrorData
					{
						Description = LocalizationManager.Instance.Get(8388, new StringMapper { { "name", CurrentLoggedInWeiboPlayerName } })
					}
				};
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
			}
		});
	}

	private static void CompleteLoginFlow(bool success, EWeiboLoginResult result, WeiboLoginFlowCallback callback)
	{
		HideLoadingOverlay();
		IsLoggedIn = success;
		switch (result)
		{
		case EWeiboLoginResult.LoginSuccessful:
			if (!IsProfileLinkedToWeibo)
			{
				FetchWeiboAccountStatus(delegate(bool registered, UserProfile userProfile)
				{
					if (string.IsNullOrEmpty(CurrentLoggedInWeiboPlayerName))
					{
						CurrentLoggedInWeiboPlayerName = ((userProfile == null) ? CurrentLoggedInWeiboUserId : userProfile.userName);
					}
					if (!registered)
					{
						LinkCurrentProfileToWeibo(callback);
					}
					else
					{
						bool flag2 = false;
						UserProfile profile2 = Singleton<StateManager>.Instance.ProfileManager.Profile;
						if (profile2 != null && userProfile != null && profile2.id == userProfile.id)
						{
							flag2 = true;
						}
						if (flag2)
						{
							LinkCurrentProfileToWeibo(callback);
						}
						else if (FacebookManager.IsAvailable && FacebookManager.IsProfileLinkedToFacebook)
						{
							ShowLoadProfilePopup(userProfile, callback);
						}
						else
						{
							ShowKeepCurrentAndLoadProfilePopup(userProfile, callback);
						}
					}
				}, callback);
				break;
			}
			FetchWeiboAccountStatus(delegate(bool registered, UserProfile userProfile)
			{
				bool flag = LinkedWeiboUserId == CurrentLoggedInWeiboUserId && userProfile != null && LinkedGameUserId == userProfile.userIdHash;
				if (string.IsNullOrEmpty(CurrentLoggedInWeiboPlayerName))
				{
					CurrentLoggedInWeiboPlayerName = ((userProfile == null) ? CurrentLoggedInWeiboUserId : userProfile.userName);
				}
				if (flag)
				{
					CompleteLoginFlow(true, EWeiboLoginResult.LoggedInSameAccount, callback);
				}
				else if (registered)
				{
					RecoverProfile(callback, (userProfile == null) ? string.Empty : userProfile.userIdHash);
				}
				else
				{
					PopUpConfiguration config = new PopUpConfiguration
					{
						ShowCloseButton = true,
						OnCloseButtonClicked = delegate
						{
							HideLoadingOverlay();
							SilentCancel(callback);
						},
						Aligntarget = UIPopup.EContentAlignTarget.CloseButton,
						Heading = LocalizationManager.Instance.Get(8386),
						Buttons = new PopUpButtonConfiguration[2]
						{
							new PopUpButtonConfiguration
							{
								Label = LocalizationManager.Instance.Get(5825),
								OnButtonPressed = delegate
								{
									HideLoadingOverlay();
									ResetProfile(callback);
								}
							},
							new PopUpButtonConfiguration
							{
								Label = LocalizationManager.Instance.Get(1781),
								OnButtonPressed = delegate
								{
									HideLoadingOverlay();
									Logout();
									SilentCancel(callback);
								}
							}
						},
						Data = new ErrorContent.ErrorData
						{
							Description = LocalizationManager.Instance.Get(8382, new StringMapper { { "name", CurrentLoggedInWeiboPlayerName } })
						}
					};
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
				}
			}, callback);
			break;
		case EWeiboLoginResult.AlreadyLoggedIn:
			FetchWeiboAccountStatusById(LinkedWeiboUserId, delegate(bool hasProfile, UserProfile userProfile)
			{
				if (userProfile != null && LinkedGameUserId == userProfile.userIdHash)
				{
					if (string.IsNullOrEmpty(NameWeiboProfile))
					{
						NameWeiboProfile = userProfile.userName;
					}
					CompleteLoginFlow(true, EWeiboLoginResult.LoggedInSameAccount, callback);
				}
				else if (hasProfile)
				{
					RecoverProfile(callback, (userProfile == null) ? string.Empty : userProfile.userIdHash);
				}
			}, callback);
			break;
		case EWeiboLoginResult.LoggedInSameAccount:
		case EWeiboLoginResult.ResetAccount:
			if (callback != null)
			{
				callback(true, result);
			}
			break;
		default:
			if (success)
			{
				UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
				LinkedGameUserId = ((profile == null) ? string.Empty : profile.userIdHash);
				LinkedWeiboUserId = CurrentLoggedInWeiboUserId;
				NameWeiboProfile = ((!string.IsNullOrEmpty(CurrentLoggedInWeiboPlayerName)) ? CurrentLoggedInWeiboPlayerName : ((profile == null) ? string.Empty : profile.userIdHash));
			}
			else if (HasLoggedIntoWeiboKey)
			{
				LogoutSilently();
			}
			if (callback != null)
			{
				callback(success, result);
			}
			break;
		}
	}

	private static void CompleteLogoutFlow(bool success, EWeiboLogoutResult result, WeiboLogoutFlowCallback callback)
	{
		if (success)
		{
			HasLoggedIntoWeiboKey = false;
			LoggedInWeiboUserId = string.Empty;
		}
		if (callback != null)
		{
			callback(success, result);
		}
	}

	private static void CompleteShareItemFlow(bool _success, EWeiboShareItemFlowResult _result, WeiboShareItemFlowCallback _callback)
	{
		HideLoadingOverlay();
		if (_callback != null)
		{
			_callback(_success, _result);
		}
	}

	private static void SilentCancel(WeiboLoginFlowCallback callback)
	{
		CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, callback);
	}

	public static void ShowLoadingInitOverlay()
	{
		Singleton<UIScreenManager>.Instance.ShowLoadingOverlay(LoadingOverlayConfiguration.Default);
	}

	public static void HideLoadingOverlay()
	{
		Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
	}

	public static void ShareAchievement(AchievementMapping.Definition _achievementMapping, int _rankIndex, WeiboShareItemFlowCallback _callback = null)
	{
		if (!IsWeiboAvailable)
		{
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.Unavailable, _callback);
			return;
		}
		if (!IsLoggedIn)
		{
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.NotLoggedIn, _callback);
			return;
		}
		AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
		Achievement value = null;
		AchievementCategoryData achievementCategoryData = achievementsManager.GetAchievementCategoryData(_achievementMapping.categoryId);
		if (!achievementCategoryData.IdToAchievements.TryGetValue(_achievementMapping.id, out value))
		{
			Debug.LogError("Could not find achievement for mapping ID " + _achievementMapping.id);
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.FailedOrCancelled, _callback);
			return;
		}
		if (_rankIndex <= 0 || _rankIndex > AchievementPostRankMappingSuffix.Count)
		{
			Debug.LogError("Achievement rank is incorrect for mapping ID " + _achievementMapping.id);
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.FailedOrCancelled, _callback);
			return;
		}
		AchievementDefinition achievementDefinition = achievementsManager.GetAchievementDefinition(_achievementMapping);
		if (achievementDefinition == null)
		{
			Debug.LogError("Achievement definition not found for mapping ID " + _achievementMapping.id);
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.FailedOrCancelled, _callback);
			return;
		}
		AchievementMapping.RankDefinition rankDefinitionByIdx = _achievementMapping.GetRankDefinitionByIdx(_rankIndex);
		if (rankDefinitionByIdx == null)
		{
			Debug.LogError("Achievement rank definition not found for mapping ID " + _achievementMapping.id);
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.FailedOrCancelled, _callback);
			return;
		}
		long value2 = 1L;
		ECounterType counterType = ECounterType.InvalidCounter;
		if (rankDefinitionByIdx.conditions.Length > 0)
		{
			AchievementMapping.Condition condition = rankDefinitionByIdx.conditions[0];
			value2 = condition.goal;
			counterType = condition.counterType;
		}
		string text = LocalizationManager.Instance.Get(achievementDefinition.NameLocaKey.Key);
		UIAchievementGoal value3 = new UIAchievementGoal(value2, counterType);
		string text2 = LocalizationManager.Instance.Get(achievementDefinition.DescriptionLocaKey.Key, new StringMapper { { "goal", value3 } });
		string arg = AchievementPostRankMappingSuffix[_rankIndex - 1];
		if (_achievementMapping.ranks.Length == 1)
		{
			arg = AchievementPostRankMappingSuffix[2];
		}
		string text3 = string.Format("{0}/achievement_{1}{2}.png", "http://static.cdn.ubi.com/0040/fb/", (uint)achievementDefinition.GameID.Guid, arg);
		string text4 = LocalizationManager.Instance.Get(6737) + " " + text + ": " + text2;
		CompleteShareItemFlow(false, EWeiboShareItemFlowResult.Unavailable, _callback);
	}

	public void OnInitializeResponse(string message)
	{
		IsInitialized = true;
	}

	public void OnAuthenticationSuccessful(string message)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(message) as Dictionary<string, object>;
		string text = (CurrentLoggedInWeiboPlayerName = string.Empty);
		text = (LoggedInWeiboAT = text);
		LoggedInWeiboUserId = text;
		if (dictionary.ContainsKey("userId"))
		{
			text = (CurrentLoggedInWeiboUserId = dictionary["userId"] as string);
			LoggedInWeiboUserId = text;
		}
		if (dictionary.ContainsKey("accessToken"))
		{
			text = (CurrentLoggedInWeiboAccessToken = dictionary["accessToken"] as string);
			LoggedInWeiboAT = text;
		}
		if (dictionary.ContainsKey("playerName"))
		{
			CurrentLoggedInWeiboPlayerName = dictionary["playerName"] as string;
		}
		if (LoggedInWeiboUserId != string.Empty && LoggedInWeiboAT != string.Empty)
		{
			HasLoggedIntoWeiboKey = true;
			CompleteLoginFlow(true, EWeiboLoginResult.LoginSuccessful, WeiboManager.OnWeiboLoginFlowCallback);
		}
		else
		{
			CompleteLoginFlow(false, EWeiboLoginResult.EmptyUserData, WeiboManager.OnWeiboLoginFlowCallback);
		}
	}

	public void OnAuthenticationFailed(string message)
	{
		CompleteLoginFlow(false, EWeiboLoginResult.FailedOrCancelled, WeiboManager.OnWeiboLoginFlowCallback);
	}

	public void OnLogoutSuccessful(string message)
	{
		HasLoggedIntoWeiboKey = false;
		string text = (CurrentLoggedInWeiboPlayerName = string.Empty);
		text = (CurrentLoggedInWeiboAccessToken = text);
		CurrentLoggedInWeiboUserId = text;
		CompleteLogoutFlow(true, EWeiboLogoutResult.Successful, WeiboManager.OnWeiboLogoutFlowCallback);
	}

	public void OnLogoutFailed(string message)
	{
		CompleteLogoutFlow(false, EWeiboLogoutResult.Failed, WeiboManager.OnWeiboLogoutFlowCallback);
	}

	public void OnShareAchievementCallback(string message)
	{
		if (message.Equals("SUCCESSFUL"))
		{
			CompleteShareItemFlow(true, EWeiboShareItemFlowResult.Succeeded, null);
		}
		else
		{
			CompleteShareItemFlow(false, EWeiboShareItemFlowResult.FailedOrCancelled, null);
		}
	}
}
