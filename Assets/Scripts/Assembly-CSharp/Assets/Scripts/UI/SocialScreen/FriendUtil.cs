using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendUtil
	{
		public delegate void PopUpCallback(bool _success);

		public const long ActivityThreshold = 900000L;

		public const float ActivityUpdateInterval = 60f;

		public static bool IsFriendListFull()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			int num = (int)profile.GetCounterAmountByType(ECounterType.NumFriendsInList);
			return num >= profile.maxFriends;
		}

		public static void SetActivityLabel(UIExtendedLabel _label, long _lastActivity)
		{
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			DateTime value = ServerTimeManager.ToUtcTime(_lastActivity);
			long num = (long)utcNow.Subtract(value).TotalMilliseconds;
			num = ((num <= 0) ? 0 : num);
			if (num <= 900000)
			{
				_label.SetLocalizationKey(4557);
				return;
			}
			_label.SetLocalizationKey(4434, new StringMapper { 
			{
				"active_time",
				UIDuration.FromMilliSeconds(num, true)
			} });
		}

		public static void SetAssassinClassLabel(UIExtendedLabel _assassinClassLabel, Assassin.EClassTypeId _classType, Assassin.EClassTypeId _extendedClassType)
		{
			Dictionary<int, ClassDescription> assassinClassDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions;
			LocalizationManager instance = LocalizationManager.Instance;
			if (_extendedClassType == Assassin.EClassTypeId.None)
			{
				_assassinClassLabel.text = instance.Get(assassinClassDescriptions[(int)_classType].NameId);
				return;
			}
			_assassinClassLabel.text = instance.Get(2116, new StringMapper
			{
				{
					"Primary_Class_Name",
					instance.Get(assassinClassDescriptions[(int)_classType].NameId)
				},
				{
					"Secondary_Class_Name",
					instance.Get(assassinClassDescriptions[(int)_extendedClassType].NameId)
				}
			});
		}

		public static string ToHumanReadableUserHash(string _hash)
		{
			int num = _hash.LastIndexOf("::");
			if (num >= 0)
			{
				_hash = _hash.Substring(0, num);
			}
			_hash = _hash.ToLower();
			_hash = _hash.Insert(3, " ");
			_hash = _hash.Insert(_hash.Length - 3, " ");
			return _hash;
		}

		public static string FromHumanReadableUserHash(string _hash)
		{
			_hash = _hash.Replace(" ", string.Empty).ToUpper();
			if (_hash.Length != 10)
			{
				return null;
			}
			foreach (char c in _hash)
			{
				if ((c < '0' || c > '9') && (c < 'A' || c > 'F'))
				{
					return null;
				}
			}
			string id = Singleton<StateManager>.Instance.ProfileManager.Profile.id;
			int num = id.LastIndexOf("::");
			if (num < 0)
			{
				return null;
			}
			return _hash + id.Substring(num);
		}

		public static bool IsError(IAsyncResult _response)
		{
			AsyncErrorResult asyncErrorResult = _response as AsyncErrorResult;
			return asyncErrorResult != null && asyncErrorResult.Exception != null;
		}

		public static void NotificationGeneric(int _message, StringMapper _messageParams = null)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(_message, _messageParams));
		}

		public static void PopUpGeneric(int _heading, int _message, EventDelegate.Callback _callback)
		{
			PopUpGeneric(_heading, _message, null, null, _callback);
		}

		public static void PopUpGeneric(int _heading, int _message, StringMapper _headerParams = null, StringMapper _messageParams = null, EventDelegate.Callback _callback = null)
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

		public static void PopUpDeleteFriend(string _friendName, PopUpCallback _callback)
		{
			PopUpButtonConfiguration[] buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(new StringId(2245)),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						_callback(true);
					}
				},
				new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(new StringId(1781)),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						_callback(false);
					}
				}
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(4453);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.CloseButton;
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(4480, new StringMapper { { "player_name", _friendName } })
			};
			popUpConfiguration.Buttons = buttons;
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpScreen(config);
		}
	}
}
