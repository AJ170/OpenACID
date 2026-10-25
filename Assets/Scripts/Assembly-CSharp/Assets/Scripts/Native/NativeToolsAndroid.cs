using InControl;
using UnityEngine;

namespace Assets.Scripts.Native
{
	public class NativeToolsAndroid : NativeToolsBase
	{
		private static AndroidJavaObject m_nativeToolsObject = new AndroidJavaObject("com.acid.plugins.nativetools.NativeTools");

		public override bool HaveStoragePermission()
		{
			return m_nativeToolsObject.Call<bool>("_HaveStoragePermission", new object[0]);
		}

		public override bool ShouldShowRequestStoragePermissionRationale()
		{
			return m_nativeToolsObject.Call<bool>("_ShouldShowRequestStoragePermissionRationale", new object[0]);
		}

		public override void RequestStoragePermission()
		{
			m_nativeToolsObject.Call("_RequestStoragePermission");
		}

		public override void OpenAppInDeviceSettings()
		{
			m_nativeToolsObject.Call("_OpenAppInDeviceSettings");
		}

		public override void ShowPrivacyPolicy(string _url)
		{
			m_nativeToolsObject.CallStatic("_ShowPrivacyPolicyOnAndroidTv", _url);
			InputManager.ClearInputState();
		}

		public override int GetKeyboardHeight()
		{
			return m_nativeToolsObject.CallStatic<int>("_GetVirtualKeyboardHeight", new object[0]);
		}

		public override string AdvertisingID()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetAdvertisingID", new object[0]);
		}

		public override bool IsLimitAdTrackingEnabled()
		{
			return m_nativeToolsObject.CallStatic<bool>("_IsLimitAdTrackingEnabled", new object[0]);
		}

		public override string BundleIdentifier()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetBundleIdentifier", new object[0]);
		}

		public override string BundleVersion()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetBundleVersion", new object[0]);
		}

		public override string CarrierName()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetDataCarrierName", new object[0]);
		}

		public override int DaysFromInstallation()
		{
			return m_nativeToolsObject.CallStatic<int>("_DaysFromInstallation", new object[0]);
		}

		public override string GetPreferredLanguage()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetDeviceLanguage", new object[0]);
		}

		public override string RadioAccessTechnology()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetNetworkType", new object[0]);
		}

		public override string StoreCountry()
		{
			return m_nativeToolsObject.CallStatic<string>("_GetUserCountry", new object[0]);
		}

		public override string GetAppReceipt()
		{
			return string.Empty;
		}

		public override void CopyToClipboard(string _strToCopy)
		{
			m_nativeToolsObject.CallStatic("_CopyToClipboard", _strToCopy);
		}

		public override bool CanOpenUrl(string _url)
		{
			return true;
		}

		public override ulong GetFreeDiskspace()
		{
			return (ulong)m_nativeToolsObject.CallStatic<long>("_GetAvailableSpace", new object[0]);
		}

		public override bool SetStringInKeyChain(string _key, string _value)
		{
			return base.SetStringInKeyChain(_key, _value);
		}

		public override string GetStringFromKeyChain(string _key)
		{
			return base.GetStringFromKeyChain(_key);
		}

		public override bool DeleteStringFromKeyChain(string _key)
		{
			return base.DeleteStringFromKeyChain(_key);
		}

		public override void ShowAlertView(string _title, string _message)
		{
			m_nativeToolsObject.CallStatic("_DisplayAlertMessage", _title, _message);
		}

		public override bool IsTVDevice()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.content.Context");
			AndroidJavaObject static2 = androidJavaClass2.GetStatic<AndroidJavaObject>("UI_MODE_SERVICE");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getSystemService", new object[1] { static2 });
			int num = androidJavaObject.Call<int>("getCurrentModeType", new object[0]);
			AndroidJavaClass androidJavaClass3 = new AndroidJavaClass("android.content.res.Configuration");
			int static3 = androidJavaClass3.GetStatic<int>("UI_MODE_TYPE_TELEVISION");
			return static3 == num;
		}
	}
}
