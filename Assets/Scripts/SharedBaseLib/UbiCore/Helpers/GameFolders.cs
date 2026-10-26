using System;
using System.IO;
using UnityEngine;

namespace UbiCore.Helpers
{
	public class GameFolders
	{
		public enum EAssetBundlePlatform
		{
			Windows = 0,
			iOS = 1,
			Android_Generic = 2,
			Android_DXT = 3,
			Android_ETC2 = 4
		}

		public enum ETextureType
		{
			Generic = 0,
			DXT = 1,
			ETC2 = 2
		}

		private static EAssetBundlePlatform GetCurrentAssetbundlePlatform(ETextureType _textureType)
		{
			switch (Application.platform)
			{
			case RuntimePlatform.IPhonePlayer:
				return EAssetBundlePlatform.iOS;
			case RuntimePlatform.PSP2:
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.OSXEditor:
				// Keep the Android file names so data from the Android CDN/cache layout is found (the game only ever
				// shipped Android/iOS data; the editor uses the same files as the PS Vita).
				// The bundles themselves still have to be rebuilt for PSP2.
				return EAssetBundlePlatform.Android_Generic;
			case RuntimePlatform.Android:
				switch (_textureType)
				{
				case ETextureType.Generic:
					return EAssetBundlePlatform.Android_Generic;
				case ETextureType.ETC2:
					return EAssetBundlePlatform.Android_ETC2;
				case ETextureType.DXT:
					return EAssetBundlePlatform.Android_DXT;
				default:
					throw new Exception("GetCurrentAssetbundlePlatform RuntimePlatform.Android " + _textureType);
				}
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsPlayer:
			case (RuntimePlatform)5 /* WindowsWebPlayer */:
				return EAssetBundlePlatform.Windows;
			default:
				throw new Exception("GetCurrentAssetbundlePlatform Application.platform " + Application.platform);
			}
		}

		public static string GetTargetExtension(ETextureType _textureType)
		{
			switch (GetCurrentAssetbundlePlatform(_textureType))
			{
			case EAssetBundlePlatform.Windows:
				return "_Win";
			case EAssetBundlePlatform.iOS:
				return "_iOS";
			case EAssetBundlePlatform.Android_Generic:
				return "_AndroidGeneric";
			case EAssetBundlePlatform.Android_DXT:
				return "_AndroidDXT";
			case EAssetBundlePlatform.Android_ETC2:
				return "_AndroidETC2";
			default:
				throw new Exception("unknown AssetBundlePlatform " + GetCurrentAssetbundlePlatform(_textureType));
			}
		}

		public static string GetAcierStaticDataPath()
		{
			return Path.Combine(Application.streamingAssetsPath, "AcierData");
		}

		public static string GetAcierDataURL(string _baseURL, string _assetBundleName)
		{
			return FixURL(Path.Combine(_baseURL, _assetBundleName));
		}

		public static string GetRuntimeAssetsDictFilename()
		{
			return Path.Combine(Path.Combine(Application.streamingAssetsPath, "AcierData"), "RuntimeAssetDict.bin");
		}

		public static string GetGameDBFilename(ETextureType _textureType)
		{
			return "GameDB" + GetTargetExtension(_textureType) + ".bin";
		}

		public static string GetFirstContactFilename()
		{
			switch (Application.platform)
			{
			case RuntimePlatform.IPhonePlayer:
				return "FirstContact_iOS.xml";
			case RuntimePlatform.Android:
			case RuntimePlatform.PSP2:
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				return "FirstContact_Android.xml";
			case RuntimePlatform.OSXPlayer:
			case RuntimePlatform.WindowsPlayer:
			case (RuntimePlatform)5 /* WindowsWebPlayer */:
				return "FirstContact_Win.xml";
			default:
				throw new Exception("GetFirstContactFilename() unknown platform " + Application.platform);
			}
		}

		public static string GetEnvironmentDict(ETextureType _textureType)
		{
			return "AssetBundleDict" + GetTargetExtension(_textureType) + ".xml";
		}

		public static string GetEnvironmentDictURL(string _baseURL, ETextureType _textureType)
		{
			return GetAcierDataURL(_baseURL, GetEnvironmentDict(_textureType));
		}

		public static string FixSlashes(string _url, bool forceSlash)
		{
			RuntimePlatform platform = Application.platform;
			if (platform != RuntimePlatform.OSXPlayer)
			{
				_url = ((!forceSlash && platform != 0 && platform != RuntimePlatform.IPhonePlayer && platform != RuntimePlatform.Android && platform != RuntimePlatform.PSP2) ? _url.Replace("/", "\\") : _url.Replace("\\", "/"));
			}
			else
			{
				string newValue = Path.DirectorySeparatorChar.ToString();
				_url = _url.Replace("\\", newValue);
				_url = _url.Replace("/", newValue);
			}
			return _url;
		}

		public static string FixURL(string _url)
		{
			string text;
			bool forceSlash;
			if (_url.StartsWith("file://"))
			{
				text = "file://";
				forceSlash = false;
			}
			else if (_url.StartsWith("http://"))
			{
				text = "http://";
				forceSlash = true;
			}
			else if (_url.StartsWith("https://"))
			{
				text = "https://";
				forceSlash = true;
			}
			else
			{
				if (!_url.StartsWith("jar:file://"))
				{
					return _url;
				}
				text = "jar:file://";
				forceSlash = true;
			}
			_url = _url.Replace(text, "");
			_url = FixSlashes(_url, forceSlash);
			_url = _url.Replace(" ", "%20");
			return text + _url;
		}
	}
}
