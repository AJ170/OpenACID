using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Native;
using Assets.Scripts.Network.Error;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.NewsFeedScreen.Data;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Network
{
	public static class UIErrorHelper
	{
		private static readonly StringId s_FallbackId = new StringId(3819);

		private static readonly StringId s_ErrorHeadingId = new StringId(3955);

		private static readonly ErrorCode[] s_twitterCodes = new ErrorCode[2]
		{
			ErrorCode.MAINTENANCE_MODE,
			ErrorCode.ACClientOutdated
		};

		private static readonly ErrorCategory[] s_twitterCategoryCodes = new ErrorCategory[1] { ErrorCategory.Reachability };

		public static string AppStoreUrl
		{
			get
			{
				return "http://market.android.com/details?id=" + NativeTools.BundleIdentifier;
			}
		}

		private static StringMapper GetTranslationContext(ErrorCode _code, ErrorCategory _category)
		{
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("ErrorCode", Convert.ChangeType(_code, _code.GetTypeCode()));
			stringMapper.Add("ErrorCategory", Convert.ChangeType(_category, _category.GetTypeCode()));
			StringMapper stringMapper2 = stringMapper;
			if (Singleton<StateManager>.Instance.TemplateManager.Globals != null)
			{
				stringMapper2.Add("MailSubject_max_char_count", Singleton<StateManager>.Instance.TemplateManager.Globals.mailSubjectCharLimit);
				stringMapper2.Add("MailBody_max_char_count", Singleton<StateManager>.Instance.TemplateManager.Globals.mailBodyCharLimit);
			}
			return stringMapper2;
		}

		private static StringId GetStringId(ErrorCode _code, ErrorCategory _category)
		{
			ErrorDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ErrorDescriptions.TryGetValue((int)_code, out value))
			{
				ErrorGroupDescription value2;
				if (value.DescriptionId == s_FallbackId && UbiCore.Utils.Singleton<UIDataMapper>.Instance.ErrorGroupDescriptions.TryGetValue((int)_category, out value2))
				{
					return value2.DescriptionId;
				}
				return value.DescriptionId;
			}
			return s_FallbackId;
		}

		private static string GetErrorDescription(ErrorCode _code, ErrorCategory _category)
		{
			return LocalizationManager.Instance.Get(GetStringId(_code, _category), GetTranslationContext(_code, _category));
		}

		public static bool ShowTwitterHint(ErrorCode _code, ErrorCategory _category)
		{
			for (int i = 0; i < s_twitterCodes.Length; i++)
			{
				if (s_twitterCodes[i] == _code)
				{
					return true;
				}
			}
			for (int j = 0; j < s_twitterCategoryCodes.Length; j++)
			{
				if (s_twitterCategoryCodes[j] == _category)
				{
					return true;
				}
			}
			return false;
		}

		public static PopUpConfiguration GetDefaultErrorBody(AcierErrorException _error)
		{
			return GetDefaultErrorBody(_error.Error.ErrorCode, _error.Error.ErrorCategory);
		}

		public static PopUpConfiguration GetDefaultErrorBody(ErrorCode _code, ErrorCategory _category)
		{
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(s_ErrorHeadingId);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = GetErrorDescription(_code, _category)
			};
			return popUpConfiguration;
		}

		public static PopUpConfiguration GetDefaultPopUpConfiguration(AcierErrorException _error, EventDelegate.Callback _callback = null)
		{
			return GetDefaultPopUpConfiguration(_error.Error.ErrorCode, _error.Error.ErrorCategory, _callback);
		}

		public static PopUpConfiguration GetDefaultPopUpConfiguration(ErrorCode _code, ErrorCategory _category, EventDelegate.Callback _callback = null)
		{
			string text = string.Empty;
			List<PopUpButtonConfiguration> list = new List<PopUpButtonConfiguration>();
			if (_category == ErrorCategory.LICENSE_CHECK)
			{
				text = LocalizationManager.Instance.Get(8053);
				list.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Data = true,
					Label = LocalizationManager.Instance.Get(7272),
					OnButtonPressed = delegate
					{
						UIUtil.QuitApplication();
					}
				});
				string label = string.Empty;
				PopUpButtonConfiguration.ButtonPressHandler buttonPressHandler = null;
				if (_code != ErrorCode.LICENSE_CHECK_FAILED_CANNOT_CONNECT)
				{
					text = LocalizationManager.Instance.Get(9773);
					label = LocalizationManager.Instance.Get(8055);
					buttonPressHandler = delegate
					{
						string appStoreUrl = AppStoreUrl;
						if (!string.IsNullOrEmpty(appStoreUrl))
						{
							Application.OpenURL(appStoreUrl);
						}
					};
				}
				else if (_callback != null)
				{
					label = LocalizationManager.Instance.Get(3947);
					buttonPressHandler = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						_callback();
					};
				}
				if (buttonPressHandler != null)
				{
					list.Add(new PopUpButtonConfiguration
					{
						ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
						Data = true,
						Label = label,
						OnButtonPressed = buttonPressHandler
					});
				}
			}
			else
			{
				List<PopUpButtonConfiguration> list2 = new List<PopUpButtonConfiguration>();
				list2.Add(new PopUpButtonConfiguration
				{
					ButtonType = PopUpButtonConfiguration.EButtonType.eNormal,
					Label = LocalizationManager.Instance.Get(new StringId(3948)),
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (_callback != null)
						{
							_callback();
						}
					}
				});
				list = list2;
				if (ShowTwitterHint(_code, _category))
				{
					list.Add(new PopUpButtonConfiguration
					{
						Data = true,
						Label = LocalizationManager.Instance.Get(4933),
						OnButtonPressed = delegate
						{
							string getTwitterAccountUrl = NewsFeedTwitter.GetTwitterAccountUrl;
							if (!string.IsNullOrEmpty(getTwitterAccountUrl))
							{
								Application.OpenURL(getTwitterAccountUrl);
							}
						}
					});
				}
			}
			PopUpConfiguration defaultErrorBody = GetDefaultErrorBody(_code, _category);
			if (!string.IsNullOrEmpty(text))
			{
				defaultErrorBody.Heading = text;
			}
			defaultErrorBody.DeactivateButtonsOnTap = false;
			defaultErrorBody.Buttons = list.ToArray();
			return defaultErrorBody;
		}

		public static void ShowSupiciousAppVersionCheck(EventDelegate.Callback _callback = null)
		{
			PopUpButtonConfiguration.ButtonPressHandler onButtonPressed = delegate
			{
				if (_callback != null)
				{
					_callback();
				}
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.Heading = LocalizationManager.Instance.Get(9771);
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Heading;
			popUpConfiguration.ShowCloseButton = false;
			popUpConfiguration.DeactivateButtonsOnTap = false;
			popUpConfiguration.Data = new ErrorContent.ErrorData
			{
				Description = LocalizationManager.Instance.Get(9772)
			};
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[2]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(1780),
					OnButtonPressed = onButtonPressed
				},
				new PopUpButtonConfiguration
				{
					Data = true,
					Label = LocalizationManager.Instance.Get(8055),
					OnButtonPressed = delegate
					{
						string appStoreUrl = AppStoreUrl;
						if (!string.IsNullOrEmpty(appStoreUrl))
						{
							Application.OpenURL(appStoreUrl);
						}
					}
				}
			};
			PopUpConfiguration config = popUpConfiguration;
			Singleton<UIScreenManager>.Instance.ShowPopUpBareScreen(config);
		}
	}
}
