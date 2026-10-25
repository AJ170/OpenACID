using AcierProtocol;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.Tracking
{
	public static class DeviceManager
	{
		public static string ClientIpAddress
		{
			get
			{
				return UnityEngine.Network.player.ipAddress;
			}
		}

		public static string ConnectivityType
		{
			get
			{
				return Application.internetReachability.ToString();
			}
		}

		public static string DeviceOS
		{
			get
			{
				return SystemInfo.operatingSystem;
			}
		}

		public static string DeviceModel
		{
			get
			{
				return SystemInfo.deviceModel;
			}
		}

		public static string DeviceLanguage
		{
			get
			{
				return Application.systemLanguage.ToString();
			}
		}

		public static string AppLanguage
		{
			get
			{
				return LocalizationManager.Instance.CurrentLanguage.ToString();
			}
		}

		public static string UserCountry { get; set; }

		public static string QualitySettingsName { get; set; }

		public static string StoreCountry
		{
			get
			{
				return NativeTools.StoreCountry;
			}
		}

		public static string CarrierName
		{
			get
			{
				return NativeTools.CarrierName;
			}
		}

		public static string RadioAccessTechnology
		{
			get
			{
				return NativeTools.RadioAccessTechnology;
			}
		}

		public static string BundleVersion
		{
			get
			{
				return NativeTools.BundleVersion;
			}
		}

		public static EQualityTextureType GetMostOptimalTextureType()
		{
			if (SystemInfo.SupportsTextureFormat(TextureFormat.DXT1))
			{
				return EQualityTextureType.DXT;
			}
			if (SystemInfo.SupportsTextureFormat(TextureFormat.ETC2_RGB))
			{
				return EQualityTextureType.ETC2;
			}
			return EQualityTextureType.Generic;
		}

		public static TrackingDeviceInfo CreateTrackingDeviceInfo()
		{
			TrackingDeviceInfo trackingDeviceInfo = new TrackingDeviceInfo();
			trackingDeviceInfo.device = DeviceModel;
			trackingDeviceInfo.os = DeviceOS;
			trackingDeviceInfo.deviceLanguage = DeviceLanguage;
			trackingDeviceInfo.graphicsDeviceName = SystemInfo.graphicsDeviceName;
			trackingDeviceInfo.systemMemorySize = SystemInfo.systemMemorySize;
			trackingDeviceInfo.processorType = SystemInfo.processorType;
			trackingDeviceInfo.textureType = GetMostOptimalTextureType();
			trackingDeviceInfo.platform = EPlatform.GoogleDevice;
			trackingDeviceInfo.tv = NativeTools.IsTVDevice;
			return trackingDeviceInfo;
		}

		public static TrackingAppInfo CreateTrackingAppInfo()
		{
			TrackingAppInfo trackingAppInfo = new TrackingAppInfo();
			trackingAppInfo.appLanguage = AppLanguage;
			trackingAppInfo.serverProtocolVersion = 59;
			trackingAppInfo.buildNumber = Main.Instance.BuildDefinition.BuildName;
			trackingAppInfo.bundleVersion = BundleVersion;
			trackingAppInfo.qualitySettingsName = QualitySettingsName ?? string.Empty;
			return trackingAppInfo;
		}
	}
}
