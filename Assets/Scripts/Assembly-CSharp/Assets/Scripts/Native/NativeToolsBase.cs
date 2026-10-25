using System.IO;
using Assets.Scripts.Network;
using UnityEngine;

namespace Assets.Scripts.Native
{
	public class NativeToolsBase
	{
		public virtual int GetKeyboardHeight()
		{
			return 0;
		}

		public virtual int DaysFromInstallation()
		{
			return 0;
		}

		public virtual string AdvertisingID()
		{
			return string.Empty;
		}

		public virtual string BundleIdentifier()
		{
			return string.Empty;
		}

		public virtual string BundleVersion()
		{
			return string.Empty;
		}

		public virtual string CarrierName()
		{
			return string.Empty;
		}

		public virtual string GetPreferredLanguage()
		{
			return string.Empty;
		}

		public virtual string RadioAccessTechnology()
		{
			return string.Empty;
		}

		public virtual string StoreCountry()
		{
			return string.Empty;
		}

		public virtual void CopyToClipboard(string _strToCopy)
		{
		}

		public virtual bool IsLimitAdTrackingEnabled()
		{
			return false;
		}

		public virtual string GetAppReceipt()
		{
			return string.Empty;
		}

		public virtual bool IsTVDevice()
		{
			return false;
		}

		public virtual bool HaveStoragePermission()
		{
			return true;
		}

		public virtual bool ShouldShowRequestStoragePermissionRationale()
		{
			return true;
		}

		public virtual void RequestStoragePermission()
		{
		}

		public virtual void OpenAppInDeviceSettings()
		{
		}

		public virtual bool CanOpenUrl(string _url)
		{
			return true;
		}

		public virtual ulong GetFreeDiskspace()
		{
			return ulong.MaxValue;
		}

		public virtual string GetStringFromKeyChain(string _key)
		{
			if (string.IsNullOrEmpty(_key))
			{
				return null;
			}
			return GetStringFallback(_key);
		}

		public virtual bool SetStringInKeyChain(string _key, string _value)
		{
			if (string.IsNullOrEmpty(_key) || string.IsNullOrEmpty(_value))
			{
				return false;
			}
			return SetStringFallback(_key, _value);
		}

		public virtual bool DeleteStringFromKeyChain(string _key)
		{
			if (string.IsNullOrEmpty(_key))
			{
				return false;
			}
			return DeleteStringFallback(_key);
		}

		public virtual void ShowAlertView(string _title, string _message)
		{
			BBAlertViewMgr.ShowAlertView(_title, _message, true);
		}

		public virtual void ShowPrivacyPolicy(string _url)
		{
			Application.OpenURL(_url);
		}

		private string GetStringFallback(string _key)
		{
			string text = NetworkDataStore.s_NetworkCacheDir + "keychain/";
			if (File.Exists(text + _key + ".json"))
			{
				StreamReader streamReader = new StreamReader(text + _key + ".json");
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				return result;
			}
			return null;
		}

		private bool SetStringFallback(string _key, string _value)
		{
			string s_NetworkCacheDir = NetworkDataStore.s_NetworkCacheDir;
			if (!Directory.Exists(s_NetworkCacheDir))
			{
				Directory.CreateDirectory(s_NetworkCacheDir);
			}
			s_NetworkCacheDir = NetworkDataStore.s_NetworkCacheDir + "keychain/";
			if (!Directory.Exists(s_NetworkCacheDir))
			{
				Directory.CreateDirectory(s_NetworkCacheDir);
			}
			StreamWriter streamWriter = new StreamWriter(s_NetworkCacheDir + _key + ".json");
			streamWriter.WriteLine(_value);
			streamWriter.Flush();
			streamWriter.Close();
			return true;
		}

		private bool DeleteStringFallback(string _key)
		{
			string text = NetworkDataStore.s_NetworkCacheDir + "keychain/";
			if (File.Exists(text + _key + ".json"))
			{
				File.Delete(text + _key + ".json");
				return true;
			}
			return false;
		}
	}
}
