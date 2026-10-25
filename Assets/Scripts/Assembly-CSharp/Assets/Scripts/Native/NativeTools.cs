using System;
using System.Security.Cryptography;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Network.Support;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Native
{
	public static class NativeTools
	{
		private const string SHIPPING_BUNDLE_ID = "com.ubisoft.assassinscreed.identity";

		private static NativeToolsBase m_nativeToolsMgr;

		public static NativeToolsBase Instance
		{
			get
			{
				if (m_nativeToolsMgr == null)
				{
					// Only the Android backend survived in the rip; everything else (PS Vita,
					// editor) gets the generic one instead of null.
					m_nativeToolsMgr = (Application.platform == RuntimePlatform.Android) ? new NativeToolsAndroid() : new NativeToolsGeneric();
				}
				return m_nativeToolsMgr;
			}
		}

		public static bool ShouldShowRequestStoragePermissionRationale
		{
			get
			{
				return Instance.ShouldShowRequestStoragePermissionRationale();
			}
		}

		public static bool HaveStoragePermission
		{
			get
			{
				return Instance.HaveStoragePermission();
			}
		}

		public static int GetKeyboardHeight
		{
			get
			{
				return Instance.GetKeyboardHeight();
			}
		}

		public static int DaysFromInstallation
		{
			get
			{
				return Instance.DaysFromInstallation();
			}
		}

		public static bool IsLimitAdTrackingEnabled
		{
			get
			{
				return Instance.IsLimitAdTrackingEnabled();
			}
		}

		public static string AdvertisingID
		{
			get
			{
				return Instance.AdvertisingID();
			}
		}

		public static string BundleIdentifier
		{
			get
			{
				return Instance.BundleIdentifier();
			}
		}

		public static string BundleVersion
		{
			get
			{
				return Instance.BundleVersion();
			}
		}

		public static string CarrierName
		{
			get
			{
				return Instance.CarrierName();
			}
		}

		public static string RadioAccessTechnology
		{
			get
			{
				return Instance.RadioAccessTechnology();
			}
		}

		public static string StoreCountry
		{
			get
			{
				return Instance.StoreCountry();
			}
		}

		public static string GetAppReceipt
		{
			get
			{
				return Instance.GetAppReceipt();
			}
		}

		public static bool IsTVDevice
		{
			get
			{
				return Instance.IsTVDevice();
			}
		}

		public static string DeviceUniqueId
		{
			get
			{
				return GetUserIdMd5Sum();
			}
		}

		public static bool IsFirstCustomerSupportContact
		{
			get
			{
				return PlayerPrefs.GetInt("IsFirstCustomerSupportContact", 1) == 1;
			}
			set
			{
				PlayerPrefs.SetInt("IsFirstCustomerSupportContact", value ? 1 : 0);
			}
		}

		public static int ServerEnvironment
		{
			get
			{
				return PlayerPrefs.GetInt("ServerEnvironment", -1);
			}
			set
			{
				PlayerPrefs.GetInt("ServerEnvironment", value);
			}
		}

		public static void RequestStoragePermission()
		{
			Instance.RequestStoragePermission();
		}

		public static void OpenAppInDeviceSettings()
		{
			Instance.OpenAppInDeviceSettings();
		}

		public static void ShowPrivacyPolicy(string _url)
		{
			Instance.ShowPrivacyPolicy(_url);
		}

		public static string GetUserIdMd5Sum()
		{
			string empty = string.Empty;
			empty = ((Singleton<StateManager>.Instance.ProfileManager.Profile == null) ? AdvertisingID : Singleton<StateManager>.Instance.ProfileManager.Profile.id);
			if (string.IsNullOrEmpty(empty))
			{
				return string.Empty;
			}
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(empty);
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, '0');
			}
			return text.PadLeft(32, '0');
		}

		public static bool IsShipping()
		{
			return (BundleIdentifier == "com.ubisoft.assassinscreed.identity") ? true : false;
		}

		public static void CopyToClipboard(string _strToCopy)
		{
			Instance.CopyToClipboard(_strToCopy);
		}

		public static bool SetStringInKeyChain(string _key, string _value)
		{
			return Instance.SetStringInKeyChain(_key, _value);
		}

		public static string GetStringFromKeyChain(string _key)
		{
			return Instance.GetStringFromKeyChain(_key);
		}

		public static bool DeleteStringFromKeyChain(string _key)
		{
			return Instance.DeleteStringFromKeyChain(_key);
		}

		public static bool CanOpenUrl(string _url)
		{
			return Instance.CanOpenUrl(_url);
		}

		public static ELanguages GetPreferredLanguage()
		{
			string preferredLanguage = Instance.GetPreferredLanguage();
			if (preferredLanguage.StartsWith("en_US"))
			{
				return ELanguages.en_US;
			}
			if (preferredLanguage.StartsWith("es_ES"))
			{
				return ELanguages.es_ES;
			}
			if (preferredLanguage.StartsWith("it_IT"))
			{
				return ELanguages.it_IT;
			}
			if (preferredLanguage.StartsWith("ja_JP"))
			{
				return ELanguages.ja_JP;
			}
			if (preferredLanguage.StartsWith("ko_KR"))
			{
				return ELanguages.ko_KR;
			}
			if (preferredLanguage.StartsWith("pt_BR"))
			{
				return ELanguages.pt_BR;
			}
			if (preferredLanguage.StartsWith("ru_RU"))
			{
				return ELanguages.ru_RU;
			}
			if (preferredLanguage.StartsWith("fr_FR"))
			{
				return ELanguages.fr_FR;
			}
			if (preferredLanguage.StartsWith("en"))
			{
				return ELanguages.en_US;
			}
			if (preferredLanguage.StartsWith("de"))
			{
				return ELanguages.de_DE;
			}
			if (preferredLanguage.StartsWith("es"))
			{
				return ELanguages.es_ES;
			}
			if (preferredLanguage.StartsWith("it"))
			{
				return ELanguages.it_IT;
			}
			if (preferredLanguage.StartsWith("ja"))
			{
				return ELanguages.ja_JP;
			}
			if (preferredLanguage.StartsWith("ko"))
			{
				return ELanguages.ko_KR;
			}
			if (preferredLanguage.StartsWith("ru"))
			{
				return ELanguages.ru_RU;
			}
			if (preferredLanguage.StartsWith("fr"))
			{
				return ELanguages.fr_FR;
			}
			if (preferredLanguage.StartsWith("pt"))
			{
				return ELanguages.pt_BR;
			}
			return ELanguages.en_US;
		}

		public static void SendMail(ESupportQueue _queue = ESupportQueue.Other, string _subject = null, string _body = null)
		{
			if (Enum.IsDefined(typeof(ESupportQueue), _queue))
			{
				StringBuilder stringBuilder = new StringBuilder("mailto:");
				string value = _subject ?? " ";
				string value2 = _subject ?? " ";
				stringBuilder.Append(GetSupportEmail(_queue));
				stringBuilder.Append("?subject=X");
				stringBuilder.Append(value);
				stringBuilder.Append("&body=");
				stringBuilder.Append(value2);
				stringBuilder.Append("%0A%0A%0A");
				stringBuilder.Append("--- PLEASE DO NOT MODIFY ---%0A");
				stringBuilder.Append("--- BEGIN: META INFO ---%0A");
				stringBuilder.Append("Build Version : " + Main.Instance.BuildDefinition.BuildName + "%0A");
				stringBuilder.Append("Build Number : " + Main.Instance.BuildDefinition.BuildNumber + "%0A");
				stringBuilder.Append("Server Version : " + 59 + "%0A");
				stringBuilder.Append("User Id : " + Singleton<StateManager>.Instance.ProfileManager.Profile.id + "%0A");
				stringBuilder.Append("User Level : " + Singleton<StateManager>.Instance.ProfileManager.Profile.level + "%0A");
				long counterAmountByType = Singleton<StateManager>.Instance.ProfileManager.Profile.GetCounterAmountByType(ECounterType.PlayerPaymentValue);
				stringBuilder.Append("Tag : " + counterAmountByType + "%0A");
				stringBuilder.Append("Platform : " + DeviceManager.DeviceModel + "%0A");
				stringBuilder.Append("Operating System : " + DeviceManager.DeviceOS + "%0A");
				stringBuilder.Append("Language : " + DeviceManager.DeviceLanguage + "%0A");
				if (IsFirstCustomerSupportContact)
				{
					stringBuilder.Append("Fcs : true%0A");
					IsFirstCustomerSupportContact = false;
				}
				else
				{
					stringBuilder.Append("Fcs : false%0A");
				}
				stringBuilder.Append("--- END: META INFO ---%0A");
				Application.OpenURL(stringBuilder.ToString().Replace(" ", "%20"));
			}
		}

		private static string GetSupportEmail(ESupportQueue _type)
		{
			switch (GetPreferredLanguage())
			{
			case ELanguages.en_GB:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-EMEA-EN@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-EMEA-EN@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-EMEA-EN@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-EMEA-EN@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-EMEA-EN@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-EMEA-EN@assassinscreedidentity.com";
				}
				break;
			case ELanguages.de_DE:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-GER@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-GER@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-GER@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-GER@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-GER@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-GER@assassinscreedidentity.com";
				}
				break;
			case ELanguages.es_ES:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-EMEA-ES@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-EMEA-ES@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-EMEA-ES@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-EMEA-ES@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-EMEA-ES@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-EMEA-ES@assassinscreedidentity.com";
				}
				break;
			case ELanguages.it_IT:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-IT@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-IT@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-IT@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-IT@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-IT@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-IT@assassinscreedidentity.com";
				}
				break;
			case ELanguages.ja_JP:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-JP@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-JP@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-JP@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-JP@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-JP@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-JP@assassinscreedidentity.com";
				}
				break;
			case ELanguages.ko_KR:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-KOR@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-KOR@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-KOR@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-KOR@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-KOR@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-KOR@assassinscreedidentity.com";
				}
				break;
			case ELanguages.ru_RU:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-RUS@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-RUS@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-RUS@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-RUS@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-RUS@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-RUS@assassinscreedidentity.com";
				}
				break;
			case ELanguages.zh_CHS:
			case ELanguages.zh_CHT:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-CHIN-SING@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-CHIN-SING@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-CHIN-SING@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-CHIN-SING@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-CHIN-SING@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-CHIN-SING@assassinscreedidentity.com";
				}
				break;
			case ELanguages.fr_FR:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-FR@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-FR@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-FR@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-FR@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-FR@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-FR@assassinscreedidentity.com";
				}
				break;
			case ELanguages.pt_BR:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-BRA@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-BRA@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-BRA@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-BRA@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-BRA@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-BRA@assassinscreedidentity.com";
				}
				break;
			default:
				switch (_type)
				{
				case ESupportQueue.Account:
					return "aci-account-NCSA-EN@assassinscreedidentity.com";
				case ESupportQueue.Bug:
					return "aci-game-NCSA-EN@assassinscreedidentity.com";
				case ESupportQueue.Question:
					return "aci-feedback-NCSA-EN@assassinscreedidentity.com";
				case ESupportQueue.Payment:
					return "aci-payment-NCSA-EN@assassinscreedidentity.com";
				case ESupportQueue.Report:
					return "aci-player-NCSA-EN@assassinscreedidentity.com";
				case ESupportQueue.Other:
					return "aci-other-NCSA-EN@assassinscreedidentity.com";
				}
				break;
			}
			return string.Empty;
		}

		public static ulong GetFreeDiskspace()
		{
			return Instance.GetFreeDiskspace();
		}

		public static void ShowAlertView(string _title, string _message)
		{
			if (!IsShipping())
			{
			}
		}
	}
}
