using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.Screens.Popup.UserConfirmationPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

public class AdColonyManager
{
	public enum ERewardType
	{
		MysteryBox = 0,
		Revive = 1
	}

	private const string m_appId = "app71d1f3448e2b41dc83";

	private const string m_zoneKey = "v4vce8607fd28cc14f17a6";

	private static bool s_isVideoAvailable;

	private static AdColonyManager s_instance;

	private string m_zoneId = "vz89269b1134594860b4";

	private readonly string _version = Application.version;

	private Dictionary<string, ADCVideoZone> _videoZones = new Dictionary<string, ADCVideoZone>();

	private ERewardType _rewardType;

	public static bool HasTriggeredAdColonyVideo { get; set; }

	public static bool IsVideoAvailable
	{
		get
		{
			return IsAvailable && s_isVideoAvailable;
		}
	}

	public static bool OverrideAdColonyAvailability { get; set; }

	public static bool EnableAdColonyOverride { get; set; }

	public static bool IsAvailable
	{
		get
		{
			if (!NativeTools.IsShipping() && OverrideAdColonyAvailability)
			{
				return EnableAdColonyOverride;
			}
			return false;
		}
	}

	public static AdColonyManager Instance
	{
		get
		{
			if (!IsAvailable)
			{
				return null;
			}
			return s_instance ?? (s_instance = new AdColonyManager());
		}
	}

	public event Action<string, int> CollectRewardsSuccessfulCallback;

	public event Action CollectRewardsFailedCallback;

	public event Action<bool> OnVideoFinishedCallback;

	public void Init()
	{
		HasTriggeredAdColonyVideo = false;
		if (IsAvailable)
		{
			if (!NativeTools.IsShipping())
			{
				m_zoneId = "vz4693be50bf014f2480";
			}
			ConfigureAdColonyPlugin();
			AddOnVideoStartedMethod(OnVideoStarted);
			AddOnVideoFinishedMethod(OnVideoFinished);
			AddOnV4VCResultMethod(OnV4VCResult);
			AddOnAdAvailabilityChangeMethod(OnAdAvailabilityChange);
			byte[] bytes = Encoding.UTF8.GetBytes(Singleton<StateManager>.Instance.ProfileManager.Profile.id);
			string customID = Convert.ToBase64String(bytes);
			AdColony.SetCustomID(customID);
		}
	}

	public void ShowAdColonyVideo(ERewardType _rewardType, bool _offerV4VcBeforePlay = false, bool _showPopUpAfter = false)
	{
		if (IsAvailable)
		{
			this._rewardType = _rewardType;
			if (_offerV4VcBeforePlay && IsVideoAvailableByZoneKey("v4vce8607fd28cc14f17a6"))
			{
				ShowAdPopup();
			}
			else
			{
				ShowVideoAdByZoneKey("v4vce8607fd28cc14f17a6", false, _showPopUpAfter);
			}
		}
	}

	public void ShowAdColonyVideoWithCallback(ERewardType _rewardType, bool _offerV4VcBeforePlay = false, bool _showPopupAfter = false)
	{
		if (IsAvailable)
		{
			RegisterToAdColonyCallbacks();
			ShowAdColonyVideo(_rewardType, _offerV4VcBeforePlay, _showPopupAfter);
		}
	}

	public void AddOnVideoStartedMethod(AdColony.VideoStartedDelegate onVideoStarted)
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Combine(AdColony.OnVideoStarted, onVideoStarted);
	}

	public void RemoveOnVideoStartedMethod(AdColony.VideoStartedDelegate onVideoStarted)
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Remove(AdColony.OnVideoStarted, onVideoStarted);
	}

	public void AddOnVideoFinishedMethod(AdColony.VideoFinishedDelegate onVideoFinished)
	{
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Combine(AdColony.OnVideoFinished, onVideoFinished);
	}

	public void RemoveOnVideoFinishedMethod(AdColony.VideoFinishedDelegate onVideoFinished)
	{
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Remove(AdColony.OnVideoFinished, onVideoFinished);
	}

	private void AddOnV4VCResultMethod(AdColony.V4VCResultDelegate onV4VCResult)
	{
		AdColony.OnV4VCResult = (AdColony.V4VCResultDelegate)Delegate.Combine(AdColony.OnV4VCResult, onV4VCResult);
	}

	private void AddOnAdAvailabilityChangeMethod(AdColony.AdAvailabilityChangeDelegate onAdAvailabilityChange)
	{
		AdColony.OnAdAvailabilityChange = (AdColony.AdAvailabilityChangeDelegate)Delegate.Combine(AdColony.OnAdAvailabilityChange, onAdAvailabilityChange);
	}

	private void OnVideoStarted()
	{
		HasTriggeredAdColonyVideo = true;
	}

	private void OnVideoFinished(bool _adWasShown)
	{
		if (this.OnVideoFinishedCallback != null)
		{
			this.OnVideoFinishedCallback(_adWasShown);
		}
		HasTriggeredAdColonyVideo = false;
	}

	private void OnV4VCResult(bool _success, string _name, int _amount)
	{
		UbiCore.Utils.Singleton<LocalStorage>.Instance.CTACounterSetTimestamp(LocalStorage.CTACounterLastAdColony);
		if (_rewardType == ERewardType.Revive)
		{
			return;
		}
		if (_success)
		{
			UINetworkAdaptor.BlockedNetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.CollectAdRewards(_asyncResult);
			}, null, delegate(IAsyncResult _response)
			{
				if (_response.IsError())
				{
					PopUpConfiguration defaultPopUpConfiguration2 = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED, ErrorCategory.ADCOLONY);
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration2);
					if (this.CollectRewardsFailedCallback != null)
					{
						this.CollectRewardsFailedCallback();
					}
				}
				else if (this.CollectRewardsSuccessfulCallback != null)
				{
					this.CollectRewardsSuccessfulCallback(_name, _amount);
				}
			});
		}
		else
		{
			if (this.CollectRewardsFailedCallback != null)
			{
				this.CollectRewardsFailedCallback();
			}
			PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.ADCOLONY_V4VC_TRANSACTION_FAILED, ErrorCategory.ADCOLONY);
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
		}
	}

	private void OnAdAvailabilityChange(bool _avail, string _zoneId)
	{
		s_isVideoAvailable = _avail;
	}

	private void ShowAdPopup()
	{
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(2341);
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
		popUpConfiguration.ShowCloseButton = false;
		popUpConfiguration.Data = new UserConfirmationContent.ConfirmationData
		{
			Description = LocalizationManager.Instance.Get(7227)
		};
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
		{
			new PopUpButtonConfiguration
			{
				ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
				Label = LocalizationManager.Instance.Get(3904),
				OnButtonPressed = delegate
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				}
			},
			new PopUpButtonConfiguration
			{
				ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
				Label = LocalizationManager.Instance.Get(3948),
				OnButtonPressed = delegate
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					ShowAdColonyVideo(ERewardType.MysteryBox);
				}
			}
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private void ShowVideoAdByZoneKey(string _zoneIdKey, bool _offerV4VcBeforePlay = false, bool _showPopUpAfter = false)
	{
		ADCVideoZone videoZoneObjectByKey = GetVideoZoneObjectByKey(_zoneIdKey);
		string zoneIdByKey = GetZoneIdByKey(_zoneIdKey);
		if (IsVideoAvailableByZoneKey(_zoneIdKey))
		{
			if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.Interstitial)
			{
				AdColony.ShowVideoAd(zoneIdByKey);
			}
			else if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.V4VC)
			{
				if (_offerV4VcBeforePlay)
				{
					AdColony.OfferV4VC(_showPopUpAfter, zoneIdByKey);
				}
				else
				{
					AdColony.ShowV4VC(_showPopUpAfter, zoneIdByKey);
				}
			}
			else
			{
				PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.ADCOLONY_ZONE_UNAVAILABLE, ErrorCategory.ADCOLONY);
				Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
			}
		}
		else
		{
			Main.Instance.StartCoroutine(DetermineError());
		}
	}

	private IEnumerator DetermineError()
	{
		ErrorState errorState = null;
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			errorState = new ErrorState
			{
				ErrorCode = ErrorCode.NoNetworkConnection,
				ErrorGroup = ErrorCategory.Reachability
			};
		}
		if (errorState != null)
		{
			ScreenExtensions.ShowPopUpScreen(_config: UIErrorHelper.GetDefaultPopUpConfiguration(errorState.ErrorCode, errorState.ErrorGroup), _manager: Singleton<UIScreenManager>.Instance);
			yield break;
		}
		ScreenExtensions.ShowPopUpScreen(_config: new PopUpConfiguration
		{
			Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(7231)
			},
			OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			},
			Aligntarget = UIPopup.EContentAlignTarget.Top,
			Heading = LocalizationManager.Instance.Get(5715),
			Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
					}
				}
			}
		}, _manager: Singleton<UIScreenManager>.Instance);
	}

	private void RegisterToAdColonyCallbacks()
	{
		this.CollectRewardsSuccessfulCallback = (Action<string, int>)Delegate.Combine(this.CollectRewardsSuccessfulCallback, new Action<string, int>(OnAdWatchedSuccessful));
		this.CollectRewardsFailedCallback = (Action)Delegate.Combine(this.CollectRewardsFailedCallback, new Action(OnAdWatchedFailed));
	}

	private void DeregisterToAdColonyCallbacks()
	{
		this.CollectRewardsSuccessfulCallback = (Action<string, int>)Delegate.Remove(this.CollectRewardsSuccessfulCallback, new Action<string, int>(OnAdWatchedSuccessful));
		this.CollectRewardsFailedCallback = (Action)Delegate.Remove(this.CollectRewardsFailedCallback, new Action(OnAdWatchedFailed));
	}

	private void OnAdWatchedSuccessful(string _type, int _amount)
	{
		DeregisterToAdColonyCallbacks();
		PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
		popUpConfiguration.ShowCloseButton = true;
		popUpConfiguration.OnCloseButtonClicked = delegate
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
		};
		popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
		popUpConfiguration.Heading = LocalizationManager.Instance.Get(4312);
		popUpConfiguration.Buttons = new PopUpButtonConfiguration[1]
		{
			new PopUpButtonConfiguration
			{
				ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
				Label = LocalizationManager.Instance.Get(1780),
				OnButtonPressed = delegate
				{
					Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
				}
			}
		};
		popUpConfiguration.Data = new ErrorContent.ErrorData
		{
			Description = LocalizationManager.Instance.Get(7229, new StringMapper { { "NumItems", _amount } })
		};
		PopUpConfiguration config = popUpConfiguration;
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
	}

	private void OnAdWatchedFailed()
	{
		DeregisterToAdColonyCallbacks();
		PopUpConfiguration defaultPopUpConfiguration = UIErrorHelper.GetDefaultPopUpConfiguration(ErrorCode.ADCOLONY_REWARDS_SERVER_CONSUMPTION_FAILED, ErrorCategory.ADCOLONY);
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(defaultPopUpConfiguration);
	}

	private void ConfigureAdColonyPlugin()
	{
		AddZoneToManager("v4vce8607fd28cc14f17a6", m_zoneId, ADCVideoZoneType.V4VC);
		AdColony.Configure(_version, "app71d1f3448e2b41dc83", GetVideoZoneIdsAsStringArray());
	}

	private void AddZoneToManager(string _zoneKey, string _zoneId, ADCVideoZoneType _videoZoneType)
	{
		_zoneKey = _zoneKey.ToLower();
		if (!ContainsZoneKey(_zoneKey))
		{
			_videoZones.Add(_zoneKey, new ADCVideoZone(_zoneId, _videoZoneType));
		}
	}

	private bool IsVideoAvailableByZoneKey(string _zoneKey)
	{
		bool result = false;
		if (ContainsZoneKey(_zoneKey))
		{
			ADCVideoZone videoZoneObjectByKey = GetVideoZoneObjectByKey(_zoneKey);
			if (videoZoneObjectByKey != null)
			{
				if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.Interstitial)
				{
					if (AdColony.IsVideoAvailable(videoZoneObjectByKey.zoneId))
					{
						result = true;
					}
				}
				else if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.V4VC && AdColony.IsV4VCAvailable(videoZoneObjectByKey.zoneId))
				{
					result = true;
				}
			}
		}
		return result;
	}

	private ADCVideoZone GetVideoZoneObjectByKey(string _key)
	{
		_key = _key.ToLower();
		return (!ContainsZoneKey(_key)) ? null : _videoZones[_key];
	}

	private string GetZoneIdByKey(string _key)
	{
		_key = _key.ToLower();
		return (!ContainsZoneKey(_key)) ? string.Empty : _videoZones[_key].zoneId;
	}

	private bool ContainsZoneKey(string _key)
	{
		_key = _key.ToLower();
		return _videoZones.ContainsKey(_key);
	}

	private string[] GetVideoZoneIdsAsStringArray()
	{
		Dictionary<string, ADCVideoZone> videoZones = _videoZones;
		string[] array = new string[_videoZones.Count];
		int num = 0;
		foreach (KeyValuePair<string, ADCVideoZone> item in videoZones)
		{
			array[num] = item.Value.zoneId;
			num++;
		}
		return array;
	}
}
